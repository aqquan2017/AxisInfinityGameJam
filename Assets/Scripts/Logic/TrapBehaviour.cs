using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using DG.Tweening;

public class TrapBehaviour : MonoBehaviour, ITriggerObject
{
    public ParticleSystem _playerHit;
    [SerializeField] private Transform _playerHitVfxSpawn;
    [SerializeField] private List<GameObject> _trapGraphic;

    [SerializeField] private bool _isSpriteUp_Down;
    [SerializeField] private bool _initSpriteUp_Down;
    private bool _canBeHurt = true;
    private bool isDown = false;
    private PlayerMovement _playerMovement;
    
    private void Start()
    {
        _playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
        if (_isSpriteUp_Down)
        {
            _playerMovement.OnMoveAction += OnPlayerMove;
        }
        foreach (var trap in _trapGraphic)
        {
            //trap.enabled = _initSpriteUp_Down;
            if (_initSpriteUp_Down)
                trap.transform.localPosition = new Vector2(trap.transform.localPosition.x, 0);
            else
                trap.transform.localPosition = new Vector2(trap.transform.localPosition.x, -.45f);
        }

        isDown = _initSpriteUp_Down;
        _canBeHurt = isDown;
    }

    private void OnDestroy()
    {
        if (_isSpriteUp_Down)
        {
            _playerMovement.OnMoveAction -= OnPlayerMove;
        }
    }

    void OnPlayerMove()
    {
        foreach (var trap in _trapGraphic)
        {
            //trap.enabled = !trap.enabled;
            trap.transform.DOKill();
            if (isDown)
                trap.transform.DOLocalMoveY(-.45f, .1f).SetEase(Ease.OutQuad);
            else
                trap.transform.DOLocalMoveY(0, .1f).SetEase(Ease.OutQuad);
            
        }
        isDown = !isDown;
        _canBeHurt = !_canBeHurt;
    }
    
    public void OnTrigger(GameObject triggerObj)
    {
        if(!_canBeHurt)
            return;

        if (triggerObj.TryGetComponent(out PlayerTurnLogic playerTurnLogic))
        { 
            //TODO : VFX HIT DAME
            playerTurnLogic.DecreaseTurn();
            string animName = Random.value > 0.5f ? "battle/get-debuff" : "defense/hit-by-normal-crit";
            triggerObj.transform.GetComponent<PlayerMovement>()._axieFigure.SetAnimation(animName, 1.5f, false);
            SoundManager.Instance.Play(Sounds.PlayerTakeDamage);
            var playerHitVFX = Pooling.Instantiate(_playerHit, _playerHitVfxSpawn.position, _playerHitVfxSpawn.rotation);
            playerHitVFX.Play();
        }

        if (triggerObj.TryGetComponent(out EnemyController enemyController))
        {
            //make it dead
            var playerHitVFX = Pooling.Instantiate(_playerHit, _playerHitVfxSpawn.position, _playerHitVfxSpawn.rotation);
            playerHitVFX.Play();
            enemyController.Die();
        }

    }
}
