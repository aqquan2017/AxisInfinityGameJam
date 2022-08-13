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
        CircleTransition.Instance.OnEndFadeIn += () => SceneController.Instance.NextScene();
    }

    public void OnLoseGame()
    {
        
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);        
        SceneController.Instance.OnChangeScene += OnChangeScene;
        SceneController.Instance.ChangeScene(2);
    }

    private void OnChangeScene(int sceneName)
    {
        var pos = GameObject.FindGameObjectWithTag("Player");
        if (pos)
        {
            CircleTransition.Instance._playerPos = pos.transform;
        }
        CircleTransition.Instance.FadeOut();
    }

    public override void Init()
    {
        
    }
}
