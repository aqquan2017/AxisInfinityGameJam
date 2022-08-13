using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerObject
{
    void OnTrigger();
}

public class WinGameTrigger : MonoBehaviour, ITriggerObject
{
    public void OnTrigger()
    {
        //win game
        //TODO : WIN GAME LOGIC ,Cicle Transition and sound,vfx
        GameStatic.Instance.OnWinGame();
        Destroy(gameObject);
    }
}
