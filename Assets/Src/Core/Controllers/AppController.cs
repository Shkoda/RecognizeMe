using System;
using System.Collections;
using JetBrains.Annotations;
using Shkoda.RecognizeMe.Core.Game;
using UnityEngine;
using Graphics = Shkoda.RecognizeMe.Core.Graphics.Graphics;

[UsedImplicitly]
public class AppController : MonoBehaviour
{
    public GameObject RecognizeMePrefab;

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
        Graphics.Instance.GameChosen += this.OnGameChosen;
        Graphics.Instance.GameClosed += this.OnGameClosed;
    }


    private void OnGameChosen(Game newGame)
    {
        this.fieldObject = Instantiate(this.RecognizeMePrefab) as GameObject;


        game = newGame;
//        game.Player = player;
//        player.StartGame(game);

        game.Start();
    }

    private void OnGameClosed()
    {
        this.CleanUp();
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