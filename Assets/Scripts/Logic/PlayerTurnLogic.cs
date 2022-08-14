using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnLogic : MonoBehaviour
{
    [SerializeField] int _turnCanGo = 10;
    [SerializeField] private TextMesh _textMesh;
    [SerializeField] private PlayerMovement _playerMovement;
    
    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        UpdateTextMesh(_turnCanGo.ToString());
    }

    void UpdateTextMesh(string text)
    {
        _textMesh.text = text;
    }

    public int TurnCanGo => _turnCanGo;

    public bool DecreaseTurn()
    {
        _turnCanGo--;
        UpdateTextMesh(_turnCanGo.ToString());
        
        if (_playerMovement.GameOver)
            return false;
        
        if (_turnCanGo <= 0)
        {
            //Game Over
            GameStatic.Instance.OnLoseGame();
            return false;
        }

        return true;
    }
    
}
