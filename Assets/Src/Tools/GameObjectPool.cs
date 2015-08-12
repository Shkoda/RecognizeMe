namespace GlobalPlay.Tools
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;
    using UnityEngine;
    using UnityObject = UnityEngine.Object;

    public class GameObjectPool : MonoBehaviour
    {
        private readonly Dictionary<GameObject, List<GameObject>> poolDictionary =
            new Dictionary<GameObject, List<GameObject>>();

        private readonly Dictionary<GameObject, GameObject> prefabDictionary = new Dictionary<GameObject, GameObject>();

        private static GameObjectPool Instance { get; set; }

        public static GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
        {
//            Debug.Log("Getting " + prefab.GetHashCode());
            List<GameObject> list;
            if (!Instance.poolDictionary.TryGetValue(prefab, out list))
            {
                list = new List<GameObject>();
                Instance.poolDictionary[prefab] = list;
            }

            if (list.Any())
            {
                var obj = list[0];
                list.RemoveAt(0);
                obj.SetActive(true);
//                Instance.prefabDictionary[obj] = prefab;
//                Debug.Log("Return from pool " + obj.name + " # " + obj.GetInstanceID());
                return obj;
            }

            var result = Instantiate(prefab, position, rotation) as GameObject;
            result.transform.parent = Instance.gameObject.transform;
            Instance.prefabDictionary[result] = prefab;
//            Debug.Log("Create new " + prefab.GetHashCode());
            return result;
        }

        public static GameObject Get(GameObject prefab)
        {
            return Get(prefab, Vector3.zero, Quaternion.identity);
        }

        public static void Recycle(GameObject obj)
        {
//            Debug.Log("Recycle " + obj.name + " # " + obj.GetInstanceID());
            GameObject prefab;
            if (Instance.prefabDictionary.TryGetValue(obj, out prefab))
            {
                obj.SetActive(false);
//                Debug.Log("Back To List " + obj.GetHashCode());
                Instance.poolDictionary[prefab].Add(obj);
            }
        }

        // todo replace with scheduling
        public static void Recycle(GameObject obj, float delay)
        {
            Instance.StartCoroutine(Instance.RecycleWithDelay(obj, delay));
        }

        private IEnumerator RecycleWithDelay(GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            Recycle(obj);
        }

        [UsedImplicitly]
        private void Awake()
        {
            Instance = this;
        }
    }
}