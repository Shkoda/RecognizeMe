namespace Shkoda.RecognizeMe.Core.Graphics
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    using UnityEngine;

    using Debug = UnityEngine.Debug;

    public static class Pointer
    {
        private static float pointerDownTime;

        private static Vector2 pointerDownPosition;

        private static bool wasPointerPressedDown;

        public static bool IsDown { get; private set; }

        /// <summary>
        /// Pointer is down for more time that is needed to tap
        /// </summary>
        public static bool PointerDown { get; private set; }

        public static bool PointerDownRaw { get; private set; }

        public static bool PointerUp { get; private set; }

        /// <summary>
        /// Player quickly pressed down and up, not moving the pointer
        /// </summary>
        public static bool Tap { get; private set; }

        public static Ray PointerRayInWorldspace { get; private set; }

        public static float TimeAfterLastPointerDown
        {
            get
            {
                return Time.time - pointerDownTime;
            }
        }

        public static void Update()
        {
#if UNITY_EDITOR
            EditorUpdate();
#else
            AppleUpdate();
#endif
        }

        private static int mainTouchId = -1;

        private static void AppleUpdate()
        {
             PointerDown = false;
             PointerUp = false;
             Tap = false;
             PointerDownRaw = false;

            // Something is pressed
            if (Input.touchCount != 0)
            {
                var mainTouch = new Touch();

                // There was no touches on previous frames 
                if ( mainTouchId == -1)
                {
                    // Same as mouse button down
                    try
                    {
                        Debug.Log(string.Format("Touches count: {0}", Input.touchCount));

                        if (Input.touchCount == 1)
                        {
                            mainTouch = Input.touches[0];
                        }
                        else
                        {
                            mainTouch = Input.touches.First(touch => touch.phase == TouchPhase.Began);
                        }

                         mainTouchId = mainTouch.fingerId;
                        
                         wasPointerPressedDown = true;
                         pointerDownPosition = mainTouch.position;
                         pointerDownTime = Time.time;

                         PointerDownRaw = true;
                         PointerRayInWorldspace = Camera.main.ScreenPointToRay(mainTouch.position);
                    }
                    catch (Exception e)
                    {
                        // No touch began on this frame
                        Debug.Log("Many touches without touchphase.began");
                    }
                }
                else
                {
                    bool found = false;
                    foreach (var touch in Input.touches)
                    {
                        if (touch.fingerId ==  mainTouchId)
                        {
                            mainTouch = touch;
                            found = true;
                        }
                    }

                    if (!found)
                    {
                        Debug.Log("No touch with saved touch id found");
                        mainTouchId = -1;
                        return;
                    }

                    float slideMagnitude = ( pointerDownPosition - new Vector2(Input.mousePosition.x, Input.mousePosition.y)).magnitude;
                    int MaxTapSlideMagnitude = Screen.dpi < 200 ? 15 : 30;
                    const float MaxTapTime = 0.3f;
                    
                    // Same as mouse button up
                    if (mainTouch.phase == TouchPhase.Canceled || mainTouch.phase == TouchPhase.Ended)
                    {
                        Debug.Log(string.Format("Touch ended. Slide mag: {0}, time: {1}", slideMagnitude,  TimeAfterLastPointerDown));
                         PointerUp = true;
                         wasPointerPressedDown = false;
                         mainTouchId = -1;

                        if ( TimeAfterLastPointerDown <= MaxTapTime &&
                            slideMagnitude < MaxTapSlideMagnitude)
                        {
                             Tap = true;
                            Debug.Log("Touch tap");
                        }
                    }

                    if ( wasPointerPressedDown &&
                        ( TimeAfterLastPointerDown > MaxTapTime || slideMagnitude > MaxTapSlideMagnitude))
                    {

                        Debug.Log(string.Format("Long select. Slide mag: {0}, time: {1}", slideMagnitude,  TimeAfterLastPointerDown));
                        Debug.Log("Touch select");

                         PointerDown = true;
                         wasPointerPressedDown = false;
                    }

                     PointerRayInWorldspace = Camera.main.ScreenPointToRay(mainTouch.position);
                }
            }
        }

        private static void EditorUpdate()
        {
             PointerDown = false;
             PointerUp = false;
             Tap = false;
             PointerDownRaw = false;

             IsDown = Input.GetMouseButton(0);

            if (Input.GetMouseButtonDown(0))
            {
                 wasPointerPressedDown = true;
                 pointerDownPosition = Input.mousePosition;
                 pointerDownTime = Time.time;
                 PointerDownRaw = true;
            }

            float slideMagnitude = ( pointerDownPosition - new Vector2(Input.mousePosition.x, Input.mousePosition.y)).magnitude;
            const int MaxTapSlideMagnitude = 10;
            const float MaxTapTime = 0.3f;
            
            if (Input.GetMouseButtonUp(0))
            {
                 PointerUp = true;
                 wasPointerPressedDown = false;

                if ( TimeAfterLastPointerDown <= MaxTapTime &&
                    slideMagnitude < MaxTapSlideMagnitude)
                {
                     Tap = true;
                }
            }

            if ( wasPointerPressedDown && ( TimeAfterLastPointerDown > MaxTapTime || slideMagnitude > MaxTapSlideMagnitude))
            {
                 PointerDown = true;
                 wasPointerPressedDown = false;
            }

             PointerRayInWorldspace = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}