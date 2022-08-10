using System.Collections;
using System.Collections.Generic;
using AxieMixer.Unity;
using Spine.Unity;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    [SerializeField] private GameObject _axieTest;
    
    void Start()
    {
        TimerManager.Instance.Init();
        UIManager.Instance.Init();
        SceneController.Instance.Init();
        SoundManager.Instance.Init();
        
        Mixer.Init();
        
    }
}
