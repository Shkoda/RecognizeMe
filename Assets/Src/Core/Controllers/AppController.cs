﻿#region imports

using System.Collections;
using JetBrains.Annotations;
using Shkoda.RecognizeMe.Core.Game;
using UnityEngine;
using Graphics = Shkoda.RecognizeMe.Core.Graphics.Graphics;

#endregion

[UsedImplicitly]
public class AppController : MonoBehaviour
{
    private static Game game;
    private static AppController instance;
    private GameObject fieldObject;

    public static Game Game
    {
        get { return game; }
    }

    [UsedImplicitly]
    private void Awake()
    {
        instance = this;
        LeanTween.init(2000);
    }

    [UsedImplicitly]
    private void Start()
    {
        Graphics.Instance.GameChosen += OnGameChosen;
        Graphics.Instance.GameClosed += OnGameClosed;
    }

    private void OnGameChosen(Game newGame)
    {
//        Debug.Log("AppController.OnGameChosen");
        game = newGame;
//        game.Player = player;
//        player.StartGame(game);

        game.Start();
    }

    private void OnGameClosed()
    {
        CleanUp();
//        player.FinishGame();
    }

    public static Coroutine StartRoutine(IEnumerator routine)
    {
        return instance.StartCoroutine(routine);
    }

    public static void StopRoutine(IEnumerator routine)
    {
        if (instance != null)
        {
            instance.StopCoroutine(routine);
        }
    }

    public void CleanUp()
    {
        if (game != null)
        {
            game.CleanUp();
            game = null;
        }

        if (fieldObject != null && fieldObject)
        {
            Debug.Log("destroying...");
            DestroyImmediate(fieldObject);
            fieldObject = null;
        }
    }
}