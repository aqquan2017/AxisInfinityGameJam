using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine;

public class SceneController : BaseManager<SceneController>
{
    private int _currentScene = 0;
    public int CurrentScene => _currentScene;
    public event Action<int, int> OnChangeScene;

    public override void Init()
    {
        SceneManager.activeSceneChanged += OnLoadScene;
    }

    void OnLoadScene(Scene cur, Scene next)
    {
        Debug.Log("SCENE" + " cur " + "START");
        OnChangeScene?.Invoke(cur.buildIndex, next.buildIndex);
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnLoadScene;
    }

    public void ChangeScene(int sceneId, float timeWait =1f)
    {
        if(sceneId >= SceneManager.sceneCountInBuildSettings)
        {
            LogSystem.LogError("SCENE LOAD OUT OF BOUND");
            return;
        }
        
        SceneManager.LoadScene(sceneId);
        _currentScene = sceneId;
    }

    public void NextScene(float timeWait = 1)
    {
        if (_currentScene + 1 >= SceneManager.sceneCountInBuildSettings)
        {
            LogSystem.LogError("SCENE LOAD OUT OF BOUND");
            return;
        }

        _currentScene++;
        SceneManager.LoadScene(_currentScene);
    }

    public void ReloadScene(float timeWait = 1)
    {
        SceneManager.LoadScene(_currentScene);
    }
}