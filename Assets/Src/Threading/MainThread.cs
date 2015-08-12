namespace GlobalPlay.Threading
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal class MainThreadObject
    {
        private float defaultDeltaTime;

        private float deltaTime;

        private volatile bool once;

        public Action<object[]> CallBack { get; set; }

        public object[] Args { get; set; }

        public string Tag { get; set; }

        public float DeltaTime
        {
            get { return this.deltaTime; }

            set
            {
                this.deltaTime = value;
                this.defaultDeltaTime = value;
            }
        }

        public bool Once
        {
            get { return this.once; }

            set { this.once = value; }
        }

        public void DecrementDeltaTime(float time)
        {
            this.deltaTime -= time;
        }

        public void ResetTime()
        {
            this.deltaTime = this.defaultDeltaTime;
        }
    }

    public static class MainThread
    {
        private static readonly List<MainThreadObject> mainThreadObjects = new List<MainThreadObject>();

        private static bool updateBreak;

        internal static void Update()
        {
            MainThreadObject obj;
            for (int index = 0; index < mainThreadObjects.Count; index++)
            {
                lock (mainThreadObjects)
                {
                    if (index < mainThreadObjects.Count)
                    {
                        obj = mainThreadObjects[index];
                    }
                    else
                    {
                        break;
                    }
                }

                obj.DecrementDeltaTime(Time.deltaTime);

                if (obj.CallBack != null && obj.DeltaTime <= 0)
                {
                    try
                    {
                        obj.CallBack(obj.Args);
                        obj.ResetTime();
                    }
                    finally
                    {
                        if (obj.Once)
                        {
                            lock (mainThreadObjects)
                            {
                                mainThreadObjects.Remove(obj);
                                index--;
                            }
                        }
                    }
                }
            }
        }

        public static void Clear()
        {
            lock (mainThreadObjects)
            {
                mainThreadObjects.Clear();
            }
        }

        public static void Add(Action<object[]> callBack, float deltaTime = 0, string tag = null, params object[] args)
        {
            if (callBack == null)
            {
                return;
            }

            int index = IndexOfCallBack(callBack);

            if (index == -1)
            {
                lock (mainThreadObjects)
                {
                    mainThreadObjects.Add(
                        new MainThreadObject {CallBack = callBack, Args = args, DeltaTime = deltaTime, Tag = tag});
                }
            }
        }

        public static void AddOnce(
            Action<object[]> callBack,
            float deltaTime = 0,
            string tag = null,
            params object[] args)
        {
            if (callBack == null)
            {
                return;
            }

            //int index = IndexOfCallBack(callBack);

            // if (index == -1)
            // {
            lock (mainThreadObjects)
            {
                mainThreadObjects.Add(
                    new MainThreadObject
                    {
                        CallBack = callBack,
                        Args = args,
                        Once = true,
                        DeltaTime = deltaTime,
                        Tag = tag
                    });
            }

            // }
        }

        public static void AddOnce(Action callBack, float deltaTime = 0, string tag = null)
        {
            if (callBack == null)
            {
                return;
            }

            var wrappedCallback = new Action<object[]>(objects => callBack());

            //int index = IndexOfCallBack(wrappedCallback);

            // if (index == -1)
            // {
            lock (mainThreadObjects)
            {
                mainThreadObjects.Add(
                    new MainThreadObject
                    {
                        CallBack = wrappedCallback,
                        Args = null,
                        Once = true,
                        DeltaTime = deltaTime,
                        Tag = tag
                    });
            }

            // }
        }

        public static void Remove(Action<object[]> callBack)
        {
            // Make sure that index of callback is not changed during remove
            lock (mainThreadObjects)
            {
                int index = IndexOfCallBack(callBack);

                if (index != -1)
                {
                    mainThreadObjects.RemoveAt(index);
                }
            }
        }

        public static void Remove(string tag)
        {
            // Make sure that index of callback is not changed during remove
            lock (mainThreadObjects)
            {
                int index = IndexOfCallBack(tag);

                if (index != -1)
                {
                    mainThreadObjects.RemoveAt(index);
                }
            }
        }

        private static int IndexOfCallBack(Action<object[]> callBack)
        {
            lock (mainThreadObjects)
            {
                for (int i = 0; i < mainThreadObjects.Count; i++)
                {
                    if (mainThreadObjects[i].CallBack.Equals(callBack))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        private static int IndexOfCallBack(string tag)
        {
            lock (mainThreadObjects)
            {
                for (int i = 0; i < mainThreadObjects.Count; i++)
                {
                    if (mainThreadObjects[i].Tag.Equals(tag))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        public static void Call(params object[] methods)
        {
            try
            {
                foreach (Action method in methods)
                {
                    try
                    {
                        method();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}