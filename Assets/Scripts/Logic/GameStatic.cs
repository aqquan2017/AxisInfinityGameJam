using System;
using System.Collections;
using System.Collections.Generic;
using AxieMixer.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatic : BaseManager<GameStatic>
{
    public int _curLevel = 0;
    public GameObject CurrentPlayer;
    
    private void Awake()
    {
        Mixer.Init();
    }

    public void OnWinGame()
    {
        CurrentPlayer.GetComponent<PlayerMovement>().PlayerFrozen();
        CircleTransition.Instance.FadeIn();
    }

    public void OnLoseGame()
    {
        CurrentPlayer.GetComponent<PlayerMovement>().PlayerFrozen();
        CircleTransition.Instance.FadeIn(onEndFadeIn:() => SceneController.Instance.ReloadScene());
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);        
        SceneController.Instance.OnChangeScene += OnChangeScene;
        SceneController.Instance.ChangeScene(2);
    }

    private void OnChangeScene(int sceneName)
    {
        CurrentPlayer = GameObject.FindGameObjectWithTag("Player");
        if (CurrentPlayer)
        {
            CircleTransition.Instance._playerPos = CurrentPlayer.transform;
        }
        CircleTransition.Instance.FadeOut();
    }

    public override void Init()
    {
        
    }
}
