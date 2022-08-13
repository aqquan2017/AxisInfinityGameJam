using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerObject
{
    void OnTrigger(GameObject triggerObj);
}

public class WinGameTrigger : MonoBehaviour, ITriggerObject
{
    public void OnTrigger(GameObject triggerObj)
    {
        //win game
        //TODO : WIN GAME LOGIC ,Cicle Transition and sound,vfx
        GameStatic.Instance.OnWinGame();
        Destroy(gameObject);
    }
}
