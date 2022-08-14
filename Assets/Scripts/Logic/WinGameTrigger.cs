using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public interface ITriggerObject
{
    void OnTrigger(GameObject triggerObj);
}

public class WinGameTrigger : MonoBehaviour, ITriggerObject
{
    public void OnTrigger(GameObject triggerObj)
    {
        if (triggerObj.TryGetComponent(out PlayerTurnLogic playerTurnLogic))
        {
            //win game
            //TODO : WIN GAME LOGIC ,Cicle Transition and sound,vfx
            string animName = Random.value > 0.5f ? "battle/get-buff" : "activity/evolve";
            triggerObj.transform.GetComponent<PlayerMovement>()._axieFigure.SetAnimation(animName, 1.5f, false);
            GameStatic.Instance.OnWinGame();
            Destroy(gameObject);
        }
    }
}
