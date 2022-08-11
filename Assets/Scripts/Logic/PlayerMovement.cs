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
    public bool _canMove = true;
    
    void Start()
    {
        transform.position = _spawnPos;
    }

    private void Update()
    {
        if (!_canMove)
            return;
        
        if (Input.GetKeyDown(KeyCode.W) && CanMoveWithoutObstacle(Vector2.up))
        {
            _canMove = false;
            transform.DOMove(transform.position + Vector3.up, 0.1f).OnComplete(() => _canMove = true);
            _axieFigure.SetAnimation("action/move-forward"); 
        }
        if (Input.GetKeyDown(KeyCode.S) && CanMoveWithoutObstacle(Vector2.down))
        {
            _canMove = false;
            transform.DOMove(transform.position + Vector3.down, 0.1f).OnComplete(() => _canMove = true);
            _axieFigure.SetAnimation("action/move-forward"); 
        }
        if (Input.GetKeyDown(KeyCode.A) && CanMoveWithoutObstacle(Vector2.left) )
        {
            _canMove = false;
            transform.DOMove(transform.position + Vector3.left, 0.1f).OnComplete(() => _canMove = true);
            _axieFigure.SetAnimation("action/move-forward");
            _axieFigure.FlipX = false;
        }
        if (Input.GetKeyDown(KeyCode.D) && CanMoveWithoutObstacle(Vector2.right))
        {
            _canMove = false;
            transform.DOMove(transform.position + Vector3.right, 0.1f).OnComplete(() => _canMove = true);
            _axieFigure.SetAnimation("action/move-forward"); 
            _axieFigure.FlipX = true;
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
}
