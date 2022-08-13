using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CircleTransition : MonoBehaviour
{
    public static CircleTransition Instance;
    
    [SerializeField] Canvas    _canvas;
    [SerializeField] Image     _image;
    [SerializeField] public Transform _playerPos;

    [SerializeField] private float _stopRadius = 0.15f;
    [SerializeField] private float _delayTime = 0.3f;
    [SerializeField] private bool _isStop1Time = true;

    private void Awake()
    {
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if(_canvas == null)
            _canvas = GetComponent<Canvas>();
        if(_image == null)
            _image = GetComponentInChildren<Image>();
        DrawBlackCircle();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        // //test input
        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     FadeIn();
        // }
        //
        // if (Input.GetKeyDown(KeyCode.E))
        // {
        //     FadeOut();
        // }
    }

    void DrawBlackCircle()
    {
        if (_playerPos == null)
            return;
        //draw a transparent circle to the black screen
        var canvasRect   = _canvas.GetComponent<RectTransform>();
        var canvasHeight = canvasRect.rect.height;
        var canvasWidth  = canvasRect.rect.width;

        //Set a target
        var playerScreenPos = Camera.main.WorldToScreenPoint(_playerPos.position);
            
        //Convert player pos to canvas screen (because canvas is different screen size, so we need to adjust playerScreenPos
        var playerCanvasPos = new Vector2
                              {
                                  //calculate the corresponding playerCanvasPos to big canvas
                                  x = (canvasWidth / Screen.width) * playerScreenPos.x,
                                  y = (canvasHeight / Screen.height) * playerScreenPos.y,
                              };
        
        //compress to a square
        float squareSize = 0f;
        if (canvasWidth < canvasHeight)
        {
            //landscape
            squareSize = canvasHeight;
            playerCanvasPos.x += (canvasHeight - canvasWidth) * 0.5f;
        }
        else
        {
            //portrait
            squareSize = canvasWidth;
            playerCanvasPos.y += (canvasWidth - canvasHeight) * 0.5f;
        }

        playerCanvasPos /= squareSize;
        _image.rectTransform.sizeDelta = new Vector2(squareSize, squareSize);
        
        _image.material.SetVector("_Center" , playerCanvasPos);
    }
    
    [ContextMenu("Fade out")]
    public void FadeOut(Action onStartFadeOut = null, Action onMidFadeOut = null, Action onEndFadeOut = null)
    {
        onStartFadeOut?.Invoke();
        DrawBlackCircle();
        float startVal = 0;

        if (_isStop1Time)
        {
            DOTween.To( () => startVal , (x) => _image.material.SetFloat("_Radius" , x), _stopRadius, 0.3f).SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                    onMidFadeOut?.Invoke();
                    DOTween.To(() => _stopRadius, (x) => _image.material.SetFloat("_Radius", x), 1, 1f).SetEase(Ease.OutBack).SetDelay(_delayTime)
                        .OnComplete(() => onEndFadeOut?.Invoke());
                });
        }
        else
        {
            DOTween.To(() => startVal, (x) => _image.material.SetFloat("_Radius", x), 1, 1f).SetEase(Ease.OutSine);
        }
    }
    
    
    [ContextMenu("Fade in")]
    public void FadeIn(Action onStartFadeIn = null, Action onMidFadeIn = null, Action onEndFadeIn = null)
    {
        onStartFadeIn?.Invoke();
        DrawBlackCircle();
        float startVal = 1f;
        if (_isStop1Time)
        {
            DOTween.To( () => startVal , (x) => _image.material.SetFloat("_Radius" , x), _stopRadius, 0.7f).SetEase(Ease.InSine)
                .OnComplete(() =>
                {
                    onMidFadeIn?.Invoke();
                    DOTween.To(() => _stopRadius, (x) => _image.material.SetFloat("_Radius", x), 0, 0.3f).SetEase(Ease.InBack).SetDelay(_delayTime)
                        .OnComplete(() => onEndFadeIn?.Invoke());
                        
                });
        }
        else
        {
            DOTween.To(() => startVal, (x) => _image.material.SetFloat("_Radius", x), 0, 1f)
                .SetEase(Ease.InSine);
        }
    }
}
