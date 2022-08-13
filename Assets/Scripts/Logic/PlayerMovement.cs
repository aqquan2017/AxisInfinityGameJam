using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector3 _spawnPos;
    public AxieFigure _axieFigure;
    public Vector2 playerDirectionFacing = Vector2.right;
    public bool _canMove = true;
    
    void Start()
    {
        transform.position = _spawnPos;
    }

    private void Update()
    {
        if (!_canMove)
            return;
        
        if (Input.GetKeyDown(KeyCode.W) )
        {
            playerDirectionFacing = Vector2.up;
            if (CanMoveWithoutObstacle(Vector2.up))
            {
                _canMove = false;
                transform.DOMove(transform.position + Vector3.up, 0.1f).OnComplete(() => _canMove = true);
                _axieFigure.SetAnimation("action/move-forward"); 
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            playerDirectionFacing = Vector2.down;
            if (CanMoveWithoutObstacle(Vector2.down))
            {
                _canMove = false;
                transform.DOMove(transform.position + Vector3.down, 0.1f).OnComplete(() => _canMove = true);
                _axieFigure.SetAnimation("action/move-forward"); 
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            playerDirectionFacing = Vector2.left;
            if (CanMoveWithoutObstacle(Vector2.left))
            {
                transform.DOMove(transform.position + Vector3.left, 0.1f).OnComplete(() => _canMove = true);
                _canMove = false;
                _axieFigure.SetAnimation("action/move-forward");
                _axieFigure.FlipX = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            playerDirectionFacing = Vector2.right;
            if (CanMoveWithoutObstacle(Vector2.right))
            {
                _canMove = false;
                transform.DOMove(transform.position + Vector3.right, 0.1f).OnComplete(() => _canMove = true);
                _axieFigure.SetAnimation("action/move-forward"); 
                _axieFigure.FlipX = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //kick obstacle or enemy
            AttackObject(playerDirectionFacing);
        }
    }

    bool CanMoveWithoutObstacle(Vector2 direction)
    {
        if (Physics2D.Raycast(transform.position, direction, 1))
        {
            return false;
        }
        return true;
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
