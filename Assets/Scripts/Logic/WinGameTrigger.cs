using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = System.Numerics.Vector2;

public interface ITriggerObject
{
    void OnTrigger(GameObject triggerObj);
}

public class WinGameTrigger : MonoBehaviour, ITriggerObject
{
    public ParticleSystem _winVFX;
    public Transform _spawnVfx; 
    
    public void OnTrigger(GameObject triggerObj)
    {
        if (triggerObj.TryGetComponent(out PlayerTurnLogic playerTurnLogic))
        {
            //win game
            //TODO : WIN GAME LOGIC ,Cicle Transition and sound,vfx
            string animName = "activity/evolve";
            triggerObj.transform.GetComponent<PlayerMovement>()._axieFigure.SetAnimation(animName, 1.5f, true);
            
            
            var playerHitVFX = Pooling.Instantiate(_winVFX, _spawnVfx.position + Vector3.up * 0.8f, _spawnVfx.rotation);
            playerHitVFX.Play();
            var playerHitVFX1 = Pooling.Instantiate(_winVFX, _spawnVfx.position + Vector3.down * 0.8f, _spawnVfx.rotation);
            playerHitVFX1.Play();
            var playerHitVFX2 = Pooling.Instantiate(_winVFX, _spawnVfx.position + Vector3.left * 0.8f, _spawnVfx.rotation);
            playerHitVFX2.Play();
            var playerHitVFX3 = Pooling.Instantiate(_winVFX, _spawnVfx.position + Vector3.right * 0.8f, _spawnVfx.rotation);
            playerHitVFX3.Play();
            GameStatic.Instance.OnWinGame();
            Pooling.Destroy(gameObject);
        }
    }
}
