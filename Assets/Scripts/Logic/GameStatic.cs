using System;
using System.Collections;
using System.Collections.Generic;
using AxieMixer.Unity;
using UnityEngine;

public class GameStatic : BaseManager<GameStatic>
{
    public int _curLevel = 0;
    
    private void Awake()
    {
        Mixer.Init();
    }

    public void OnWinGame()
    {
        CircleTransition.Instance.FadeIn();
    }

    void Start()
    {
        OnChangeScene(0);
        SceneController.Instance.OnChangeScene += OnChangeScene;
        DontDestroyOnLoad(gameObject);        
    }

    private void OnChangeScene(int sceneName)
    {
        CircleTransition.Instance.FadeOut();
    }

    public override void Init()
    {
        
    }
}
