using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CF2InputController : MonoBehaviour
{
    void Start()
    {
        if(Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            Destroy(gameObject);
        
        SceneManager.sceneLoaded += OnSceneLoad;
        CircleTransition.Instance.OnGlobalFadeIn += OnFadeIn;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
        CircleTransition.Instance.OnGlobalFadeIn -= OnFadeIn;
    }

    void OnFadeIn()
    {
        gameObject.SetActive(false);
    }

    void OnSceneLoad(Scene arg0, LoadSceneMode loadSceneMode)
    {
        if (arg0.buildIndex < 2)
        {
            gameObject.SetActive(false);
            return;
        }
        
        TimerManager.Instance.AddTimer(1f , () => gameObject.SetActive(true));
    }
}
