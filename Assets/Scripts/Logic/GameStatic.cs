using System.Collections;
using System.Collections.Generic;
using AxieMixer.Unity;
using UnityEngine;

public class GameStatic : MonoBehaviour
{
    
    void Start()
    {
        Mixer.Init();
        DontDestroyOnLoad(gameObject);        
    }
}
