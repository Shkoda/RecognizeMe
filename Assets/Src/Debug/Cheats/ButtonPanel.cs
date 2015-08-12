using System;
using System.Collections.Generic;
using Assets.Src.Debug.Cheats;
using UnityEngine;

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
        get { return this.debugWindowId; }

        set { this.debugWindowId = value; }
    }

    #region Updates, GUI

    // Use this for initialization
    public void Start()
    {
        this.windowRect = new Rect(0, 0, Screen.width, 0);

        this.buttonHeight = Screen.height*0.04f;
        this.buttonWidth = this.buttonHeight*3;

        this.ButtonXPosition = Math.Max(Screen.width*this.RelationPositionX - this.buttonWidth, 0);
        this.buttonYPosition = Screen.height - this.buttonHeight;

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
                this.buttonList.Add(cheat);
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
                this.buttonList.Add(scriptButton);
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

        this.ScriptList.Add(script);
    }

    public void AddPerformable(Performable script)
    {
        if (ActionList == null)
        {
            ActionList = new List<Performable>();
        }

        this.ActionList.Add(script);
    }

    private float ElementHeight(string elementText)
    {
        return elementText.Split('\n').Length*LineHeight + TableHeight;
    }

    // Update is called once per frame
    private void Update()
    {
        // Who knows, maybe someone wants to play on android without touchscreen
        bool isDraggingWithMouse = this.isDragging && Input.GetMouseButtonUp(0);

        // touchscreen
        bool isDraggingWithMultiTouch = this.isDragging && Input.multiTouchEnabled
                                        && (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended);

        if (isDraggingWithMouse || isDraggingWithMultiTouch)
        {
            isDragging = false;
            float y = isDraggingWithMouse ? Input.mousePosition.y : Input.GetTouch(0).position.y;

            if (y > Screen.height*0.1 && y < boundTopY*0.9)
            {
                this.isFixed = true;
                this.isMaximized = false;
            }
            else if (y > Screen.height*0.1)
            {
                this.isMaximizing = true;
            }
            else
            {
                this.isMinimizing = true;
            }
        }
    }

    private void SetDefaultStyle()
    {
        this.buttonStyle = GUI.skin.button;
        this.textStyle = GUI.skin.button;
    }

    private bool ControlTabPressed()
    {
        return
            GUI.RepeatButton(
                new Rect(this.ButtonXPosition, this.buttonYPosition + 2*Margin, this.buttonWidth, this.buttonHeight),
                new GUIContent(this.TabName),
                this.buttonStyle);
    }

    private void OnGUI()
    {
        if (this.buttonStyle == null)
        {
            this.SetDefaultStyle();
        }

        if (this.ControlTabPressed())
        {
            if (!this.isDragging)
            {
                if (Input.multiTouchEnabled && Input.touchCount == 1)
                {
                    // touch 
                    this.deltaY = this.buttonYPosition + Input.GetTouch(0).position.y;
                }
                else if (Input.GetMouseButton(0))
                {
                    // mouse
                    this.deltaY = this.buttonYPosition + Input.mousePosition.y;
                }
            }

            this.isMaximizing = false;
            this.isMinimizing = false;
            this.isMaximized = false;
            this.isFixed = false;
            this.isDragging = true;
        }

        if (this.isMaximizing)
        {
            this.buttonYPosition -= this.AnimateSpeed;
            if (this.buttonYPosition < Screen.height - boundTopY)
            {
                this.buttonYPosition = Screen.height - boundTopY;
                this.isMaximized = true;
                this.isMaximizing = false;
            }
        }

        if (this.isMinimizing)
        {
            this.buttonYPosition += this.AnimateSpeed;
            if (this.buttonYPosition + this.buttonHeight > Screen.height)
            {
                this.buttonYPosition = Screen.height - this.buttonHeight;
                this.isMinimizing = false;
            }
        }

        if (this.isDragging)
        {
            buttonYPosition = this.Clamp(-Input.mousePosition.y + this.deltaY, 0, Screen.height - this.buttonHeight);
        }

        this.windowRect.Set(this.windowRect.x, this.buttonYPosition, Screen.width, Screen.height - this.buttonYPosition);

        if (this.isFixed || this.isDragging || this.isMaximizing || this.isMinimizing || this.isMaximized)
        {
            GUI.Window(this.debugWindowId, this.windowRect, this.OnPanelIsVisible, string.Empty);
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
                    this.buttonHeight + WindowHeaderHeight + Margin,
                    this.windowRect.width - 6,
                    scrollViewHeight),
                this.scrollViewVector,
                new Rect(0, 0, this.windowRect.width - 22, totalTextHeight + 100 + Margin));
    }

    private bool ActionPressed(ConsoleButton cheat)
    {
        return GUI.Button(
            new Rect(0, cheat.Offset + Margin, this.windowRect.width - 21 - Margin, cheat.Height - Margin),
            "[Perform Action] " + cheat.Name,
            this.textStyle);
    }

    private void OnPanelIsVisible(int windowId)
    {
        float scrollViewHeight = this.windowRect.height - Margin*2 - WindowHeaderHeight - this.buttonHeight;

        if (scrollViewHeight > 0)
        {
            this.scrollViewVector = this.ScrollViewVector(scrollViewHeight);

            foreach (ConsoleButton action in this.buttonList)
            {
                if (action.Offset > this.scrollViewVector.y + scrollViewHeight)
                {
                    break;
                }

                if (action.Offset + action.Height > this.scrollViewVector.y)
                {
                    if (this.ActionPressed(action))
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