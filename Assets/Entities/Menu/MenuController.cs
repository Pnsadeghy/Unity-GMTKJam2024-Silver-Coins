using System;
using System.Collections;
using System.Collections.Generic;
using EasyTransition;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static bool isEnded;
    public AudioClip mainMusic;
    public TransitionSettings transition;
    public GameObject mainImage;
    public GameObject darkImage;

    private void Start()
    {
        PersistentAudioManager.instance.PlayMusic(mainMusic);
        if (isEnded)
        {
            mainImage.SetActive(false);
            darkImage.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            TransitionManager.Instance().Transition("Level00", transition, 0);
    }
}
