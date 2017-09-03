﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
    [SerializeField] private int _CurrentLevel = 1;
    [SerializeField] private float _FadeSpeed = .5f;

    private Image _FadePanel = null;
    [SerializeField] private GameStates _CurrentGameState = GameStates.Menu;
    private LevelManager _LevelManager = null;

    private void Awake ()
    {
        AssignReferences ();
    }

    private void AssignReferences ()
    {
        _LevelManager = GetComponent<LevelManager> ();
        _FadePanel = GameObject.Find ("Fade Panel").GetComponent<Image> ();

        EventManager.OnStateChanged += StateChanged;
    }

    private void Start ()
    {
        _FadePanel.gameObject.SetActive (true);
    }

    private void StateChanged (GameStates state)
    {
        _CurrentGameState = state;

        switch (state)
        {
            case GameStates.Menu:
                break;
            case GameStates.Options:
                break;
            case GameStates.Credits:
                break;
            case GameStates.LevelSelect:
                break;
            case GameStates.InGame:
                break;
            case GameStates.LevelComplete:
                break;
            case GameStates.InGameOptions:
                break;
        }
    }

    private void Update ()
    {
        if (_CurrentGameState == GameStates.InGame || _CurrentGameState == GameStates.InGameOptions)
            if (Input.GetKeyDown (KeyCode.Escape) || Input.GetButtonDown ("Pause"))
                PauseGame ();
    }

    private void PauseGame ()
    {
        switch (_CurrentGameState)
        {
            case GameStates.InGame:
                EventManager.ChangeState (GameStates.InGameOptions);
                break;
            case GameStates.InGameOptions:
                EventManager.ChangeState (GameStates.InGame);
                break;
        }
    }

    private IEnumerator FadeToNextLevel (bool isFading)
    {
        float time = 0f;

        while (time < _FadeSpeed)
        {
            if (isFading)
                _FadePanel.color = Color.Lerp (Color.clear, Color.black, time / _FadeSpeed);
            else
                _FadePanel.color = Color.Lerp (Color.black, Color.clear, time / _FadeSpeed);

            time += Time.deltaTime;
            yield return new WaitForEndOfFrame ();
        }

        if (isFading)
            _FadePanel.color = Color.black;
        else
            _FadePanel.color = Color.clear;
    }

    private IEnumerator FadeIn ()
    {
        float time = 0f;

        while (time < _FadeSpeed)
        {
            _FadePanel.color = Color.Lerp (Color.clear, Color.black, time / _FadeSpeed);

            time += Time.deltaTime;
            yield return new WaitForEndOfFrame ();
        }

        _FadePanel.color = Color.black;
    }

    private IEnumerator FadeOut ()
    {
        float time = 0f;

        while (time < _FadeSpeed)
        {
            _FadePanel.color = Color.Lerp (Color.black, Color.clear, time / _FadeSpeed);

            time += Time.deltaTime;
            yield return new WaitForEndOfFrame ();
        }

        _FadePanel.color = Color.clear;
    }

    public IEnumerator NextLevel ()
    {
        yield return StartCoroutine (FadeToNextLevel (true));
        _LevelManager.NewLevel (_CurrentLevel += 1);
        yield return StartCoroutine (FadeToNextLevel (false));
    }

    public IEnumerator LastLevel ()
    {
        yield return StartCoroutine (FadeToNextLevel (true));
        _LevelManager.NewLevel (_CurrentLevel -= 1);
        yield return StartCoroutine (FadeToNextLevel (false));
    }

    private void OnDestroy ()
    {
        EventManager.OnStateChanged += StateChanged;
    }
}
