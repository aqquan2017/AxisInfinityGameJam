using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerTurnLogic _playerTurnLogic;
    public Vector3 _spawnPos;
    public AxieFigure _axieFigure;
    public bool _canMove = true;

    void Start()
    {
        _playerTurnLogic = GetComponent<PlayerTurnLogic>();
        transform.position = _spawnPos;
    }

    private void Update()
    {
        if (!_canMove)
            return;
        Movement();
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

            //flip character
            if (direction == Vector2.right)
            {
                _axieFigure.FlipX = true;
            }
            else if (direction == Vector2.left)
            {
                _axieFigure.FlipX = false;
            }

            if (CanMoveWithoutObstacle(direction))
            {
                _canMove = false;
                transform.DOMove((Vector2)transform.position + direction, 0.1f).OnComplete(() => _canMove = true);
                _axieFigure.SetAnimation("action/move-forward");
                _playerTurnLogic.DecreaseTurn();
                return;
            }
            
            if (CanAttack(direction))
            {
                AttackObject(direction);
                _playerTurnLogic.DecreaseTurn();
                return;
            }
        }

    }

    bool CanMoveWithoutObstacle(Vector2 direction)
    {
        var interfaceInteract = Physics2D.Raycast((Vector2)transform.position + direction, direction, 0.1f);
        if (interfaceInteract)
        {
            if (interfaceInteract.transform.TryGetComponent(out IInteractObject interactObject)
                || interfaceInteract.transform.TryGetComponent(out IWallCollider wallCollider))
            {
                return false;
            }
            
            if (interfaceInteract.transform.TryGetComponent(out ITriggerObject triggerObject))
            {
                triggerObject.OnTrigger(gameObject);
                return true;
            }
        }
        return true;
    }

    bool CanAttack(Vector2 direction)
    {
        var interfaceInteract = Physics2D.Raycast(transform.position, direction, 1);
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
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, direction, 1);
        if (raycastHit2D)
        {
            if (raycastHit2D.transform.TryGetComponent(out IInteractObject interactObject))
            {
                interactObject.OnImpact(direction);
            }
        }
    }
}
