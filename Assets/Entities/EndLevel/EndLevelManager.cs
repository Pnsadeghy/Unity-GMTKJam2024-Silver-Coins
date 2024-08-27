using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelManager : MonoBehaviour
{
    public static int Step = 0;
    public Transform endBossReveal;
    public AudioSource endBossRevealSound;
    public AudioSource endBossStartSound;
    public AudioSource endBossShitSound;
    public AudioClip endBossTheme;
    public PlayerController player;
    public EndBossController boss;

    private void Start()
    {
        switch (Step)
        {
            case 1:
                player.OnDisabled();
                Destroy(endBossReveal.gameObject);
                player.transform.position = new Vector3(-0.7499757f, -7f, 0f);
                StartCoroutine(BossStart1());
                break;
        }
    }

    public void Trigger1DarkSideCall()
    {
        player.OnDisabled();
        PersistentAudioManager.instance.StopMusic();
        endBossRevealSound.Play();
        StartCoroutine(BossStart());
    }

    IEnumerator BossStart()
    {
        yield return new WaitForSeconds(5f);
        endBossStartSound.Play();
        yield return new WaitForSeconds(2f);
        PersistentAudioManager.instance.ChangeMusic(endBossTheme);
        player.OnEnabled();
        boss.StartMoving();
        EndLevelManager.Step = 1;
    }

    IEnumerator BossStart1()
    {
        yield return new WaitForSeconds(1f);
        player.OnEnabled();
        boss.StartMoving();
    }

    public void ReachEnd()
    {
        PersistentAudioManager.instance.StopMusic();
        player.OnDisabled();
        player.transform.rotation = Quaternion.Euler(0, 180f, 0);
        boss.Stop();
        
        StartCoroutine(ReachEndAction());
    }

    IEnumerator ReachEndAction()
    {
        yield return new WaitForSeconds(1f);
        endBossShitSound.Play();
        yield return new WaitForSeconds(1f);
        boss.StartRunning();
        yield return new WaitForSeconds(4f);
        boss.Stop();
        MenuController.isEnded = true;
        LevelManager.Instance.EndGame();
    }
}
