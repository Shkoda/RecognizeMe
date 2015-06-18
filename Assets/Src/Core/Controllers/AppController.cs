using System;
using System.Collections;
using JetBrains.Annotations;

using UnityEngine;


[UsedImplicitly]
public class AppController : MonoBehaviour
{
    private GameObject RecognizeMePrefab;

    private static Game game;

    private static AppController instance;

    private GameObject fieldObject;

    public static Game Game
    {
        get
        {
            return game;
        }
    }

    [UsedImplicitly]
    private void Awake()
    {
        instance = this;

        LeanTween.init(2000);
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