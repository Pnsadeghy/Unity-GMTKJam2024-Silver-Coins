using System;
using System.Collections;
using System.Collections.Generic;
using EasyTransition;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public TransitionSettings transition;
    public TransitionSettings fadeTransition;
    private bool _onSceneChange;

    public AudioSource coinAudio;
    public AudioSource hurtAudio;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ResetScene();
    }

    public void ResetScene(float delay = 0)
    {
        this.LoadScene(SceneManager.GetActiveScene().name, delay);
    }

    public void LoadScene(string name, float delay = 0)
    {
        if (_onSceneChange) return;
        _onSceneChange = true;
        TransitionManager.Instance().Transition(name, transition, delay);
    }

    public void GetCoin()
    {
        coinAudio.Play();
    }

    public void EnemyDie()
    {
        hurtAudio.Play();
    }

    public void EndGame()
    {
        _onSceneChange = true;
        TransitionManager.Instance().Transition("End", fadeTransition, 0f);
    }

    public void BackToMenu()
    {
        _onSceneChange = true;
        TransitionManager.Instance().Transition("Menu", fadeTransition, 0f);
    }
}
