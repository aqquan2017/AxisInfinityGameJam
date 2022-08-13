using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChangeScene : MonoBehaviour
{
    void Start()
    {
        RandomPosition();
    }

    public void RandomPosition()
    {
        gameObject.SetActive(true);
        transform.position = new Vector3(Random.Range(-9f, 9f), Random.Range(-3f, 3f), 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        //change scene
        CircleTransition.Instance.FadeIn();
        TestManager.Instance.PlaySound( TestManager.Instance._winSound);
        gameObject.SetActive(false);
    }
}
