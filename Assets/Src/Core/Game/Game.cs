using System;
using System.Collections;
using System.Diagnostics;
using Shkoda.Rec.Core.Controllers;
using Shkoda.RecognizeMe.Core.Game.Achievements;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Graphics = Shkoda.RecognizeMe.Core.Graphics.Graphics;

public abstract class Game
{
    private readonly Graphics graphics = Graphics.Instance;
    private readonly Stopwatch timer = new Stopwatch();
    private readonly IEnumerator timerRoutine;

    protected Game()
    {
        timerRoutine = UpdateTimerInHud();
    }

    public RecognizeController Controller { get; protected set; }
    public event Action<GameFinishedEventArgs> GameFinished = delegate { };

    public void Start()
    {
        Controller.Subscribe();
        Controller.StartGame();
        Controller.TilesInitialized += GameInitialized;
        Controller.GameFinished += OnGameFinished;
    }

    private IEnumerator UpdateTimerInHud()
    {
        while (true)
        {
            Graphics.Instance.UpdateTimer(timer.ElapsedMilliseconds);
            yield return new WaitForSeconds(1);
        }
    }

    private void GameInitialized()
    {
        timer.Start();
        AppController.StartRoutine(timerRoutine);
    }

    private void OnGameFinished(GameFinishedEventArgs args)
    {
        Debug.Log("Game Finishing...");
        timer.Stop();
        args.Time = Math.Max(1, timer.ElapsedMilliseconds/1000);
//        var timePoints = Math.Min(200, (int)(100000 / args.Time));
//        PointsEarned(timePoints);
//        args.Points += timePoints;
        GameFinished(args);
//        graphics.ShowWin(args);
    }

    public void CleanUp()
    {
        Controller.CleanUp();
#if UNITY_EDITOR
        Controller.OnApplicationQuit();
#endif
        UnSubscribe();
        AppController.StopRoutine(timerRoutine);
        Graphics.Instance.CleanUpGameHud();
    }

    private void UnSubscribe()
    {
        Controller.TilesInitialized -= GameInitialized;
        Controller.GameFinished -= OnGameFinished;
    }
}