using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBehaviour : MonoBehaviour, ITriggerObject
{

    public void OnTrigger(GameObject triggerObj)
    {
        if (triggerObj.TryGetComponent(out PlayerTurnLogic playerTurnLogic))
        {
            playerTurnLogic.DecreaseTurn();
            string animName = Random.value > 0.5f ? "battle/get-debuff" : "defense/hit-by-normal-crit";
            triggerObj.transform.GetComponent<PlayerMovement>()._axieFigure.SetAnimation(animName, 1.5f, false);
        }
        //TODO : VFX HIT DAME, -1 TURN IN UI
    }
}
