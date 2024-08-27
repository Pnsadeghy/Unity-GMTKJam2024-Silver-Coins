using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndController : MonoBehaviour
{
   public string nextScene;

   private AudioSource _audioSource;
   private bool _isTrigger;

   private void Awake()
   {
      TryGetComponent(out _audioSource);
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (_isTrigger) return;
      if (other.CompareTag("Player"))
      {
         _isTrigger = true;
         _audioSource.Play();
         other.GetComponent<PlayerController>().OnDisabled();
         LevelManager.Instance.LoadScene(nextScene);
      }
   }
}
