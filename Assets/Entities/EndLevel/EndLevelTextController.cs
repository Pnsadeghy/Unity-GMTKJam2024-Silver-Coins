using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelTextController : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(End());
    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(10f);
        LevelManager.Instance.BackToMenu();
    }
}
