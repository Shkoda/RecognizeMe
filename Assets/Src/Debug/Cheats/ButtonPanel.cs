#region imports

using System;
using System.Collections.Generic;
using Assets.Src.Debug.Cheats;
using UnityEngine;

#endregion

public class ButtonPanel : MonoBehaviour
{
    public List<Performable> ActionList;

    public List<MonoBehaviour> ScriptList;

    public float AnimateSpeed = 30;

    public string TabName = "Cheats";

    [Range(0, 1)] public float RelationPositionX;

    private const float WindowHeaderHeight = 17f;

    private const float Margin = 2f;

    private const float LineHeight = 13f;

    private const float TableHeight = 15f;

    private int debugWindowId = 5556;

    private float buttonWidth, buttonHeight;

    private float boundTopY;

    private float buttonYPosition, ButtonXPosition;

    private Rect windowRect;

    private bool isDragging, isMinimizing, isMaximizing;

    private bool isMaximized, isFixed;

    private GUIStyle buttonStyle;

    private GUIStyle textStyle;

    private Vector2 scrollViewVector;

    private float deltaY;

    private float totalTextHeight;

#if UNITY_IPHONE || UNITY_ANDROID
    private TouchScreenKeyboard consoleKeyboard;
#endif

    private List<ConsoleButton> buttonList = new List<ConsoleButton>();

    public int DebugWindowId
    {
        get { return debugWindowId; }

        set { debugWindowId = value; }
    }

    #region Updates, GUI

    // Use this for initialization
    public void Start()
    {
        windowRect = new Rect(0, 0, Screen.width, 0);

        buttonHeight = Screen.height*0.04f;
        buttonWidth = buttonHeight*3;

        ButtonXPosition = Math.Max(Screen.width*RelationPositionX - buttonWidth, 0);
        buttonYPosition = Screen.height - buttonHeight;

        if (ActionList == null)
        {
            ActionList = new List<Performable>();
        }

        if (ScriptList == null)
        {
            ScriptList = new List<MonoBehaviour>();
        }

        foreach (var performable in ActionList)
        {
            if (performable != null)
            {
                var cheat = new CheatButton(performable, ElementHeight(performable.Name), totalTextHeight + 2*Margin);
                buttonList.Add(cheat);
                totalTextHeight += cheat.Height;
            }
        }

        foreach (var script in ScriptList)
        {
            if (script != null)
            {
                var scriptButton = new ScriptButton(
                    script,
                    ElementHeight(script.GetType().ToString()),
                    totalTextHeight + 2*Margin);
                buttonList.Add(scriptButton);
                totalTextHeight += scriptButton.Height;
            }
        }


        boundTopY = Math.Min(totalTextHeight + 100 + Margin, Screen.height);
    }

    public void AddSript(MonoBehaviour script)
    {
        if (ScriptList == null)
        {
            ScriptList = new List<MonoBehaviour>();
        }

        ScriptList.Add(script);
    }

    public void AddPerformable(Performable script)
    {
        if (ActionList == null)
        {
            ActionList = new List<Performable>();
        }

        ActionList.Add(script);
    }

    private float ElementHeight(string elementText)
    {
        return elementText.Split('\n').Length*LineHeight + TableHeight;
    }

    // Update is called once per frame
    private void Update()
    {
        // Who knows, maybe someone wants to play on android without touchscreen
        var isDraggingWithMouse = isDragging && Input.GetMouseButtonUp(0);

        // touchscreen
        var isDraggingWithMultiTouch = isDragging && Input.multiTouchEnabled
                                       && (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended);

        if (isDraggingWithMouse || isDraggingWithMultiTouch)
        {
            isDragging = false;
            var y = isDraggingWithMouse ? Input.mousePosition.y : Input.GetTouch(0).position.y;

            if (y > Screen.height*0.1 && y < boundTopY*0.9)
            {
                isFixed = true;
                isMaximized = false;
            }
            else if (y > Screen.height*0.1)
            {
                isMaximizing = true;
            }
            else
            {
                isMinimizing = true;
            }
        }
    }

    private void SetDefaultStyle()
    {
        buttonStyle = GUI.skin.button;
        textStyle = GUI.skin.button;
    }

    private bool ControlTabPressed()
    {
        return
            GUI.RepeatButton(
                new Rect(ButtonXPosition, buttonYPosition + 2*Margin, buttonWidth, buttonHeight),
                new GUIContent(TabName),
                buttonStyle);
    }

    private void OnGUI()
    {
        if (buttonStyle == null)
        {
            SetDefaultStyle();
        }

        if (ControlTabPressed())
        {
            if (!isDragging)
            {
                if (Input.multiTouchEnabled && Input.touchCount == 1)
                {
                    // touch 
                    deltaY = buttonYPosition + Input.GetTouch(0).position.y;
                }
                else if (Input.GetMouseButton(0))
                {
                    // mouse
                    deltaY = buttonYPosition + Input.mousePosition.y;
                }
            }

            isMaximizing = false;
            isMinimizing = false;
            isMaximized = false;
            isFixed = false;
            isDragging = true;
        }

        if (isMaximizing)
        {
            buttonYPosition -= AnimateSpeed;
            if (buttonYPosition < Screen.height - boundTopY)
            {
                buttonYPosition = Screen.height - boundTopY;
                isMaximized = true;
                isMaximizing = false;
            }
        }

        if (isMinimizing)
        {
            buttonYPosition += AnimateSpeed;
            if (buttonYPosition + buttonHeight > Screen.height)
            {
                buttonYPosition = Screen.height - buttonHeight;
                isMinimizing = false;
            }
        }

        if (isDragging)
        {
            buttonYPosition = Clamp(-Input.mousePosition.y + deltaY, 0, Screen.height - buttonHeight);
        }

        windowRect.Set(windowRect.x, buttonYPosition, Screen.width, Screen.height - buttonYPosition);

        if (isFixed || isDragging || isMaximizing || isMinimizing || isMaximized)
        {
            GUI.Window(debugWindowId, windowRect, OnPanelIsVisible, string.Empty);
        }
    }

    private float Clamp(float value, float minInclusive, float maxInclusive)
    {
        return Math.Min(Math.Max(value, minInclusive), maxInclusive);
    }

    private Vector2 ScrollViewVector(float scrollViewHeight)
    {
        return
            GUI.BeginScrollView(
                new Rect(
                    Margin*2,
                    buttonHeight + WindowHeaderHeight + Margin,
                    windowRect.width - 6,
                    scrollViewHeight),
                scrollViewVector,
                new Rect(0, 0, windowRect.width - 22, totalTextHeight + 100 + Margin));
    }

    private bool ActionPressed(ConsoleButton cheat)
    {
        return GUI.Button(
            new Rect(0, cheat.Offset + Margin, windowRect.width - 21 - Margin, cheat.Height - Margin),
            "[Perform Action] " + cheat.Name,
            textStyle);
    }

    private void OnPanelIsVisible(int windowId)
    {
        var scrollViewHeight = windowRect.height - Margin*2 - WindowHeaderHeight - buttonHeight;

        if (scrollViewHeight > 0)
        {
            scrollViewVector = ScrollViewVector(scrollViewHeight);

            foreach (var action in buttonList)
            {
                if (action.Offset > scrollViewVector.y + scrollViewHeight)
                {
                    break;
                }

                if (action.Offset + action.Height > scrollViewVector.y)
                {
                    if (ActionPressed(action))
                    {
                        action.Perform();
                    }
                }
            }

            GUI.EndScrollView();
        }
    }

    #endregion
}