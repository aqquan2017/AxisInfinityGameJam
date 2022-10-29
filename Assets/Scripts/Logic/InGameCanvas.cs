using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameCanvas : BasePanel
{
    public Text _levelText;
    public Button _howToPlayBtn;
    public Button _rewindBtn;
    public Button _exitToMainMenuBtn;
    public Button _resetGameBtn;

    void Start()
    {
        _levelText.text = "Level " + (SceneController.Instance.CurrentScene - 1).ToString();
        _howToPlayBtn.onClick.AddListener(OnHowToPlay);
        _rewindBtn.onClick.AddListener(OnRewind);
        _exitToMainMenuBtn.onClick.AddListener(OnExitToMainMenu);
        _resetGameBtn.onClick.AddListener(OnResetLevel);
    }

    private void OnDestroy()
    {
        _howToPlayBtn.onClick.RemoveListener(OnHowToPlay);
    }

    void OnHowToPlay()
    {
        UIManager.Instance.GetPanel<TextPopupPanel>().SetInfo("GUIDE" ,
            "<color=#ff0000ff>Swipe</color> to move Axie around." +
            "\n\n Please help poor Axie ^^"); 
            
        UIManager.Instance.ShowPanelWithDG(typeof(TextPopupPanel));
        SoundManager.Instance.Play(Sounds.UI_POPUP);
    }
    void OnRewind()
    {
        SoundManager.Instance.Play(Sounds.UI_POPUP);
    }
    
    void OnResetLevel()
    {
        GameStatic.Instance.ResetGame();    
        SoundManager.Instance.Play(Sounds.UI_POPUP);
    }
    
    void OnExitToMainMenu()
    {
        UIManager.Instance.ShowPanelWithDG(typeof(ExitToMainMenuPanel));
        SoundManager.Instance.Play(Sounds.UI_POPUP);
    }

    public override void OverrideText()
    {
        throw new NotImplementedException();
    }
}
