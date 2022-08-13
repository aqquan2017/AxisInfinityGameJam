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
        SoundManager.Instance.Play(Sounds.WIN_LV);
        CircleTransition.Instance.FadeIn(onMidFadeIn:() => SoundManager.Instance.Play(Sounds.FadeIn)
        ,onEndFadeIn:() =>
        {
            SceneController.Instance.NextScene();
        });
    }

    public void OnLoseGame()
    {
        CurrentPlayer.GetComponent<PlayerMovement>().PlayerFrozen();
        SoundManager.Instance.Play(Sounds.LOSE_LV);
        CircleTransition.Instance.FadeIn(onMidFadeIn:() => SoundManager.Instance.Play(Sounds.FadeIn)
            , onEndFadeIn:() => SceneController.Instance.ReloadScene());
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);        
        SceneController.Instance.OnChangeScene += OnChangeScene;
        SoundManager.Instance.Play(Sounds.LOSE_LV);
        SceneController.Instance.ChangeScene(2);
    }

    private void OnChangeScene(int sceneName)
    {
        CurrentPlayer = GameObject.FindGameObjectWithTag("Player");
        if (CurrentPlayer)
        {
            CircleTransition.Instance._playerPos = CurrentPlayer.transform;
        }
        CircleTransition.Instance.FadeOut(onMidFadeOut:() => SoundManager.Instance.Play(Sounds.FadeOut));
    }

    public override void Init()
    {
        
    }
}
