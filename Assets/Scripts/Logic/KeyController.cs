using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour, ITriggerObject
{
    public ParticleSystem _playerHit;
    [SerializeField] private Transform _playerHitVfxSpawn;

    public void OnTrigger(GameObject triggerObj)
    {
        if (triggerObj.TryGetComponent(out PlayerKeyLock playerKeyLock))
        { 
            string animName = Random.value > 0.5f ? "battle/get-debuff" : "defense/hit-by-normal-crit";
            triggerObj.transform.GetComponent<PlayerMovement>()._axieFigure.SetAnimation(animName, 1.5f, false);
            SoundManager.Instance.Play(Sounds.Buy);
            
            playerKeyLock.SetHaveKey();
            
            var playerHitVFX = Instantiate(_playerHit, _playerHitVfxSpawn.position, _playerHitVfxSpawn.rotation);
            playerHitVFX.Play();
            
            Destroy(gameObject);
        }
    }
    
}