using DG.Tweening;
using Game;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    private PlayerTurnLogic _playerTurnLogic;
    public Vector3 _spawnPos;
    public AxieFigure _axieFigure;
    public bool _canMove = true;
    public bool _gameOver = false;

    void Start()
    {
        _playerTurnLogic = GetComponent<PlayerTurnLogic>();
        transform.position = _spawnPos;
    }

    private void Update()
    {
        if (!_canMove || _gameOver)
            return;
        Movement();
    }

    public void PlayerFrozen()
    {
        _gameOver = true;
    }

    void Movement()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A)
                                        || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S))
        {
            Vector2 direction = Input.GetKeyDown(KeyCode.W) ? Vector2.up
                : Input.GetKeyDown(KeyCode.S) ? Vector2.down
                : Input.GetKeyDown(KeyCode.A) ? Vector2.left
                : Vector2.right;

            SoundManager.Instance.Play(Sounds.UI_POPUP);
            
            CheckHurtItSelf();

            //flip character
            if (direction == Vector2.right)
            {
                _axieFigure.FlipX = true;
            }
            else if (direction == Vector2.left)
            {
                _axieFigure.FlipX = false;
            }

            Action OnDoLater = null;
            if (CanMove(direction, ref OnDoLater))
            {
                _canMove = false;
                transform.DOMove((Vector2)transform.position + direction, 0.1f).OnComplete(() =>
                {
                    _playerTurnLogic.DecreaseTurn();
                    OnDoLater?.Invoke();
                    _canMove = true;
                });
                _axieFigure.SetAnimation("action/move-forward", 2f, false);
                
                return;
            }
            
            if (CanAttack(direction))
            {
                AttackObject(direction);
                string animAttack = Random.value > 0.5f ? "attack/melee/multi-attack" : "attack/ranged/cast-high";
                SoundManager.Instance.Play(Sounds.ENEMY_HIT);
                _axieFigure.SetAnimation(animAttack, 2f, false);
                
                _playerTurnLogic.DecreaseTurn();
                return;
            }
        }

    }

    bool CanMove(Vector2 direction, ref Action OnDoLater)
    {
        var interfaceInteract = Physics2D.Raycast((Vector2)transform.position + direction, direction, 0.1f);
        if (interfaceInteract)
        {
            if (interfaceInteract.transform.TryGetComponent(out ITriggerObject triggerObject))
            {
                OnDoLater = () => triggerObject.OnTrigger(gameObject);
                return true;
            }
            
            return false;
            // if (interfaceInteract.transform.TryGetComponent(out IInteractObject interactObject)
            //     || interfaceInteract.transform.TryGetComponent(out IWallCollider wallCollider))
            // {
            // }
            
        }
        return true;
    }

    bool CanAttack(Vector2 direction)
    {
        var interfaceInteract = Physics2D.Raycast((Vector2)transform.position + direction, direction, 0.1f);
        if (interfaceInteract)
        {
            if (interfaceInteract.transform.TryGetComponent(out IInteractObject interactObject))
            {
                return true;
            }
        }
        return false;
    }

    void AttackObject(Vector2 direction)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2)transform.position + direction, direction, 0.1f);
        if (raycastHit2D)
        {
            if (raycastHit2D.transform.TryGetComponent(out IInteractObject interactObject))
            {
                interactObject.OnImpact(direction);
            }
        }
    }

    //used for when player attack - still in a trap
    void CheckHurtItSelf()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2)transform.position, Vector2.down, 0.01f);
        if (raycastHit2D)
        {
            if (raycastHit2D.transform.TryGetComponent(out ITriggerObject interactObject))
            {
                interactObject.OnTrigger(gameObject);
            }
        }
    }
}
