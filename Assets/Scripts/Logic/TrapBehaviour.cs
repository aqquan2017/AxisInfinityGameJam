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
        }
        //TODO : VFX HIT DAME, -1 TURN IN UI
    }
}
