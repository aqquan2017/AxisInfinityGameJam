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
        SceneController.Instance.ChangeScene(1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitToGameMenu();
        }
    }

    private bool canChangeScene = true;
    public void ExitToGameMenu()
    {
        if (SceneController.Instance.CurrentScene > 1 && canChangeScene)
        {
            canChangeScene = false;
            CircleTransition.Instance.FadeIn(onMidFadeIn:() => SoundManager.Instance.Play(Sounds.FadeIn),
                onEndFadeIn:() =>
                {
                    canChangeScene = true;
                    SceneController.Instance.ChangeScene(1);
                });
        }
    }

    private void OnChangeScene(int sceneId)
    {
        CurrentPlayer = GameObject.FindGameObjectWithTag("Player");
        if (CurrentPlayer)
        {
            CircleTransition.Instance._playerPos = CurrentPlayer.transform;
        }
        if(sceneId > 1)
            CircleTransition.Instance.FadeOut(onMidFadeOut:() => SoundManager.Instance.Play(Sounds.FadeOut));
    }

    public override void Init()
    {
        
    }
}
