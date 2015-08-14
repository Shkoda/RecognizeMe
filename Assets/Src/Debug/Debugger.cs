//#define NOLOG

#region imports

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using GlobalPlay.Threading;
using GlobalPlay.Tools;
using UnityEngine;
using Random = UnityEngine.Random;

#endregion

public partial class Debugger : MonoBehaviour
{
    public float ButtonHeight;

    public float ButtonWidth;

    public float AnimateSpeed = 30;

    private string ButtonText = "Debug";

    public string ScriptName = "Debugger";

    public int DebugWindowID = 5555;

    public static bool ConsoleModeOn { get; private set; }

    public static bool InterceptDebugMessages
    {
        get { return interceptDebugMessages; }

        set
        {
            interceptDebugMessages = value;
            if (value)
            {
                Application.RegisterLogCallback(LogCallback);
            }
            else
            {
                Application.RegisterLogCallback(null);
            }
        }
    }

    private static bool interceptDebugMessages;

    public static bool WriteToDebugger { get; set; }

    public static bool WriteDebugToServer { get; set; }

    public static bool WriteDebugToConsole { get; set; }

    private float buttonYPosition;

    private Rect windowRect;

    private const float Margin = 2f;

    private bool isDragging;

    private bool isMinimizing;

    private bool isMaximizing;

    private bool isMaximized;

    private bool isFixed;

    private GUIStyle buttonStyle;

    private GUIStyle textStyle;

    private Vector2 scrollViewVector;

    private float deltaY;

    private static readonly List<Message> logList;

    private static readonly List<Message> logListFiltered;

    private static readonly Dictionary<int, int> logListIndices = new Dictionary<int, int>();

    private static readonly Dictionary<int, int> logListFilteredIndices = new Dictionary<int, int>();

    static Debugger()
    {
        logList = new List<Message>();
        logListFiltered = new List<Message>();

        WriteToDebugger = true;

        SetFilter((DebugType) 0x7FFFFFFF);
    }

    #region Updates, GUI

    // Use this for initialization
    private void Start()
    {
        InterceptDebugMessages = true;

        windowRect = new Rect(0, 0, Screen.width, 0);

        ButtonHeight = Screen.height*0.04f;
        ButtonWidth = ButtonHeight*3;

        buttonYPosition = Screen.height - ButtonHeight;

        AddConsoleCommands();

        var reports = CrashReport.reports;

        if (reports != null)
        {
            foreach (var crashReport in reports)
            {
                Log(
                    string.Format("Crash report from {0}:\n{1}", crashReport.time, crashReport.text),
                    DebugType.Exception,
                    sendToServer: true,
                    writeToUnityConsole: true);
                crashReport.Remove();
            }
        }
        else
        {
            Log("No crash reports found");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateConsole();
#if UNITY_FLASH

    // No touchscreen at all
        if (isDragging && Input.GetMouseButtonUp(0))        {

// mouse
            isDragging = false;
            if (Input.mousePosition.y > Screen.height * 0.1 && Input.mousePosition.y < Screen.height * 0.94)
            {
                isFixed = true;
                isMaximized = false;
            }
            else if (Input.mousePosition.y > Screen.height * 0.1)
            {
                isMaximizing = true;
            }
            else
            {
                isMinimizing = true;
            }
        }
#else

        // Who knows, maybe someone wants to play on android without touchscreen
        if (isDragging && Input.GetMouseButtonUp(0))
        {
            // mouse
            isDragging = false;
            if (Input.mousePosition.y > Screen.height*0.1 && Input.mousePosition.y < Screen.height*0.94)
            {
                isFixed = true;
                isMaximized = false;
            }
            else if (Input.mousePosition.y > Screen.height*0.1)
            {
                isMaximizing = true;
            }
            else
            {
                isMinimizing = true;
            }
        }
        else if (isDragging && Input.multiTouchEnabled
                 && (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            // touchscreen
            var touch = Input.GetTouch(0);
            isDragging = false;
            if (touch.position.y > Screen.height*0.1 && touch.position.x < Screen.height*0.9)
            {
                isFixed = true;
                isMaximized = false;
            }
            else if (touch.position.y > Screen.height*0.1)
            {
                isMaximizing = true;
            }
            else
            {
                isMinimizing = true;
            }
        }

#endif
    }

    public static readonly Dictionary<string, Action<string>> consoleCommands = new Dictionary<string, Action<string>>();

    private void UpdateConsole()
    {
#if UNITY_IPHONE || UNITY_ANDROID
        if (consoleKeyboard != null && consoleKeyboard.done &&
            (Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.OSXEditor))
        {
            if (consoleKeyboard.text == "q" || consoleKeyboard.text == "Q" || consoleKeyboard.text == string.Empty)
            {
                SwitchConsoleMode();
            }
            else
            {
                var lower = consoleKeyboard.text.ToLower();
                var key = lower.Split(' ')[0];
                if (consoleCommands.ContainsKey(key))
                {
                    consoleCommands[key](lower);
                    consoleKeyboard = TouchScreenKeyboard.Open("");
                }
                else
                {
                    AddConsoleRequest(consoleKeyboard.text);

// new GSRequestEnvelope(consoleKeyboard.text).Send();
                    consoleKeyboard = TouchScreenKeyboard.Open("");
                }
            }
        }
#endif
    }

    private static void LogCallback(string logMessage, string stackTrace, LogType type)
    {
        if (WriteDebugToConsole)
        {
            return;
        }

        if (WriteToDebugger)
        {
            if (type == LogType.Exception)
            {
                _Log(logMessage + "\n" + stackTrace, DebugType.Exception);
            }
            else
            {
                _Log(logMessage, DebugType.UnityConsole);
            }
        }

        if (WriteDebugToServer)
        {
            // if (type == LogType.Exception || type == LogType.Error)
            // {
            // new SDebugMessageEnvelope(
            // string.Format(
            // "Type: {0}.\r\nMessage: {1}.\r\nStackTrace: {2}.\r\nUTC time: {3}\r\n\r\n",
            // type,
            // logMessage,
            // stackTrace,
            // DateTime.UtcNow)).Send();
            // }
        }

#if UNITY_EDITOR
        if (logMessage.Equals("The compiler this script was imported with is not available anymore."))
        {
            Debug.LogWarning("Aborting player!");

            // EditorApplication.isPlaying = false;
        }

#endif
    }

    private string sendLogsName = "Send Logs";

    private void OnGUI()
    {
#if UNITY_EDITOR
        if (GUI.Button(new Rect(Screen.width - 80, Screen.height - 60, 80, 30), "Save Logs"))
        {
            SaveAllLogs();
        }

#endif

        if (buttonStyle == null)
        {
            buttonStyle = GUI.skin.button;
            textStyle = GUI.skin.box;

            var fontSize = 0;
            buttonStyle.fontSize = fontSize;
            textStyle.fontSize = fontSize;

            textHeight = textStyle.fontSize == 0 ? 25f : textStyle.fontSize*1.5f;
            textHeightWithMargin = textHeight + Margin;

            textStyle.alignment = TextAnchor.MiddleLeft;
        }

        if (
            GUI.RepeatButton(
                new Rect(
                    Screen.width*0.5f - ButtonWidth*0.5f,
                    buttonYPosition + 2*Margin,
                    ButtonWidth,
                    ButtonHeight),
                new GUIContent(ButtonText),
                buttonStyle))
        {
            if (!isDragging)
            {
#if UNITY_FLASH
                deltaY = buttonYPosition + Input.mousePosition.y;
#else
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

#endif
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
            if (buttonYPosition < 0)
            {
                buttonYPosition = 0;
                isMaximized = true;
                isMaximizing = false;
            }
        }

        if (isMinimizing)
        {
            buttonYPosition += AnimateSpeed;
            if (buttonYPosition + ButtonHeight > Screen.height)
            {
                buttonYPosition = Screen.height - ButtonHeight;
                isMinimizing = false;
            }
        }

        if (isDragging)
        {
            buttonYPosition = -Input.mousePosition.y + deltaY;
            if (buttonYPosition < 0)
            {
                buttonYPosition = 0;
            }

            if (buttonYPosition + ButtonHeight > Screen.height)
            {
                buttonYPosition = Screen.height - ButtonHeight;
            }
        }

        windowRect.Set(windowRect.x, buttonYPosition, Screen.width, Screen.height - buttonYPosition);
        if (isFixed || isDragging || isMaximizing || isMinimizing || isMaximized)
        {
            GUI.Window(DebugWindowID, windowRect, OnDebugWindow, string.Empty);
        }
    }

    private void SaveAllLogs()
    {
        if (!Directory.Exists(@"C:\Logs\Slototherapy\"))
        {
            Directory.CreateDirectory(@"C:\Logs\Slototherapy\");
        }

        using (
            var file =
                new FileStream(
                    @"C:\Logs\Slototherapy\" + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year
                    + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".log",
                    FileMode.Create))
        {
            using (var writer = new StreamWriter(file))
            {
                var res = PackLogs();

                writer.Write(res);
            }
        }
    }

    private static string PackLogs()
    {
        var res = string.Empty;
        foreach (var message in logList)
        {
            res += message.ToString();
            res += "\r\n";
        }

        return res;
    }

    public string SendAllLogsToServer()
    {
        var log = PackLogs();
        var rnd = Random.Range(0, 10000000);
        var name = rnd.ToString("D6");

        return name;
    }

    private static float _totalTextHeight;

    private void OnDebugWindow(int windowId)
    {
        // return;
        // var textHeight = textStyle.fontSize == 0 ? 25f : textStyle.fontSize * 1.5f;
        const float windowHeaderHeight = 17f;

        var scrollViewHeight = windowRect.height - Margin*2 - windowHeaderHeight - ButtonHeight;

        if (scrollViewHeight > 0)
        {
            scrollViewVector =
                GUI.BeginScrollView(
                    new Rect(
                        Margin*2,
                        ButtonHeight + windowHeaderHeight + Margin,
                        windowRect.width - 6,
                        scrollViewHeight),
                    scrollViewVector,
                    new Rect(0, 0, windowRect.width - 22, _totalTextHeight + 100 + Margin));

            for (var i = 0; i < logListFiltered.Count; i++)
            {
                if (logListFiltered[i].Top > scrollViewVector.y + scrollViewHeight)
                {
                    break;
                }

                if (logListFiltered[i].Top + logListFiltered[i].Height > scrollViewVector.y)
                {
                    GUI.Box(
                        new Rect(
                            0,
                            logListFiltered[i].Top + Margin,
                            windowRect.width - 21 - Margin,
                            logListFiltered[i].Height - Margin),
                        logListFiltered[i].ToString(),
                        textStyle);
                }
            }

            GUI.EndScrollView();

            if (GUI.Button(
                new Rect(2*Margin, 2*Margin, ButtonWidth, ButtonHeight),
                new GUIContent("clear"),
                buttonStyle))
            {
                Clear();
            }

            if (GUI.Button(
                new Rect(3*Margin + ButtonWidth, 2*Margin, ButtonWidth, ButtonHeight),
                new GUIContent("console"),
                buttonStyle))
            {
                SwitchConsoleMode();
            }
        }
    }

#if UNITY_IPHONE || UNITY_ANDROID
    private TouchScreenKeyboard consoleKeyboard;
#endif

    private void SwitchConsoleMode()
    {
        if (!ConsoleModeOn)
        {
            _backupFilter = _filter;
            SetFilter(DebugType.Console);
#if UNITY_IPHONE || UNITY_ANDROID
            consoleKeyboard = TouchScreenKeyboard.Open(string.Empty);
#endif
            ConsoleModeOn = true;
        }
        else
        {
            ConsoleModeOn = false;
            SetFilter(_backupFilter);
#if UNITY_IPHONE || UNITY_ANDROID
            consoleKeyboard = null;
#endif
        }
    }

    public static void AddConsoleRequest(string command)
    {
        ConsoleLog(command, false);
    }

    public static void AddConsoleResponse(string command)
    {
        ConsoleLog(command, true);
    }

    private static void ConsoleLog(string message, bool isResponse)
    {
        if (message == null)
        {
            return;
        }

        var msg = new ConsoleMessage(
            message,
            isResponse,
            message.Split('\n').Length*lineHeight + tableHeight,
            _totalTextHeight + Margin);

        if (WriteToDebugger)
        {
            logList.Add(msg);

            // logListIndices.Add(msg.GetHashCode(), logList.Count);
        }

        if ((_filter & DebugType.Console) != 0 || _filter == 0)
        {
            logListFiltered.Add(msg);

            // logListFilteredIndices.Add(msg.GetHashCode(), logList.Count);
            _totalTextHeight += msg.Height;
        }
    }

    #endregion

    private static DebugType _backupFilter;

    private static DebugType _filter;

    private static float lineHeight = 13f;

    private static float tableHeight = 15f;

    private static float textHeight = 25f;

    private static float textHeightWithMargin = 27f;

    public static void SetFilter(DebugType filter)
    {
        if (ConsoleModeOn)
        {
            _backupFilter = filter;
            return;
        }

        _filter = filter;

        // Update filtered list
        logListFiltered.Clear();
        logListFilteredIndices.Clear();
        _totalTextHeight = 0;

        foreach (var message in logList)
        {
            if ((message.DebugType & filter) != 0)
            {
                logListFiltered.Add(message);
                if (!logListFilteredIndices.ContainsKey(message.GetHashCode()))
                {
                    logListFilteredIndices.Add(message.GetHashCode(), logListFiltered.Count);
                }

                _totalTextHeight += message.Height;
            }
        }
    }

    public static void Watch(int watchId, string msg, DebugType debugType = DebugType.Main, bool sendToServer = false)
    {
#if NOLOG
        return;
#endif
        var message = new WatchMessage(
            watchId,
            msg,
            debugType,
            msg.Split('\n').Length*lineHeight + tableHeight,
            _totalTextHeight + Margin);

        // var index = logList.IndexOf(message);
        int index;
        logListIndices.TryGetValue(message.GetHashCode(), out index);

        // Normalize index
        index--;

        if (index != -1)
        {
            // Manual lock
            // Just "watch" here, so if anyone wants to write while it's locked - bad for him, he won't get a chance.
            var lockAcquired = false;
            try
            {
                lockAcquired = Monitor.TryEnter(logList);
                if (lockAcquired)
                {
                    ((WatchMessage) logList[index]).UpdateMessage(msg);
                }
            }
            finally
            {
                if (lockAcquired)
                {
                    Monitor.Exit(logList);
                }
            }
        }
        else
        {
            // if (sendToServer || WriteDebugToServer)
            // {
            // new SDebugMessageEnvelope(
            // string.Format(
            // "D.Watch(). Message: {0}.\r\nWatchId: {1}.\r\nUTC time: {2}\r\n\r\n",
            // msg,
            // watchId,
            // DateTime.UtcNow)).Send();
            // }
            lock (logList)
            {
                logList.Add(message);
            }
        }

        if ((_filter & debugType) != 0 || _filter == 0)
        {
            int index2;
            logListFilteredIndices.TryGetValue(message.GetHashCode(), out index2);

            // Normalize index
            index2--;

            if (index2 != -1)
            {
                // Manual lock (see above)
                var lockAcquired = false;
                try
                {
                    lockAcquired = Monitor.TryEnter(logListFiltered);
                    if (lockAcquired)
                    {
                        ((WatchMessage) logListFiltered[index2]).UpdateMessage(msg);
                    }
                }
                finally
                {
                    if (lockAcquired)
                    {
                        Monitor.Exit(logListFiltered);
                    }
                }
            }
            else
            {
                lock (logListFiltered)
                {
                    logListFiltered.Add(message);
                    logListFilteredIndices.Add(message.GetHashCode(), logListFiltered.Count);
                    _totalTextHeight += message.Height;
                }
            }
        }
    }

    public static void Log(
        object message,
        DebugType debugType = DebugType.Main,
        bool writeToUnityConsole = false,
        bool sendToServer = false,
        string stackTrace = "")
    {
#if NOLOG
        return;
#endif
        if (message == null)
        {
            message = "Message sent to debugger is null";
        }

        _Log(message.ToString(), debugType);

        if (writeToUnityConsole || WriteDebugToConsole)
        {
            if (ThreadManager.IsRunningInMainThread)
            {
                Debug.Log(message + stackTrace);
            }
            else
            {
                MainThread.AddOnce(nothing => Debug.Log(message.ToString() + stackTrace));
            }
        }

        // if (sendToServer || WriteDebugToServer)
        // {
        // new SDebugMessageEnvelope(
        // string.Format(
        // "D.Log(). Message: {0}.\r\nStack trace: {1}.\r\nUTC time: {2}\r\n\r\n",
        // message,
        // stackTrace,
        // DateTime.UtcNow)).Send();
        // }
    }

    private static void _Log(string message, DebugType debugType)
    {
        if (message == null)
        {
            return;
        }

        var msg = new Message(
            message,
            debugType,
            message.Split('\n').Length*lineHeight + tableHeight,
            _totalTextHeight + 2*Margin);

        if (WriteToDebugger)
        {
            int index;
            logListIndices.TryGetValue(message.GetHashCode(), out index);
            index--;
            index = -1;

            if (index != -1)
            {
                lock (logList)
                {
                    logList[index].count++;
                }
            }
            else
            {
                lock (logList)
                {
                    logList.Add(msg);
                    logListIndices.AddOrReplace(msg.GetHashCode(), logList.Count);
                }
            }
        }

        if ((_filter & debugType) != 0 || _filter == 0)
        {
            int index;
            logListFilteredIndices.TryGetValue(message.GetHashCode(), out index);
            index--;

            index = -1;

            if (index == -1)
            {
                lock (logListFiltered)
                {
                    logListFiltered.Add(msg);
                    logListFilteredIndices.AddOrReplace(msg.GetHashCode(), logListFiltered.Count);
                    _totalTextHeight += msg.Height;
                }
            }
        }
    }

    /// <summary>
    ///     Clear all messages.
    /// </summary>
    public static void Clear()
    {
#if NOLOG
        return;
#endif
        lock (logList)
        {
            lock (logListFiltered)
            {
                logListFiltered.Clear();
                logList.Clear();

                logListFilteredIndices.Clear();
                logListIndices.Clear();
                _totalTextHeight = 0;
            }
        }
    }
}

[Flags]
public enum DebugType
{
    Main = 0x01,

    UnityConsole = 0x02,

    Exception = 0x04,

    NetworkHandler = 0x10,

    NetworkInterface = 0x20,

    Console = 0x400,

    Threading = 0x4000
}