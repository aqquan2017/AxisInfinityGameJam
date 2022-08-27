using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameCanvas : MonoBehaviour
{
    public Text _levelText;
    public Button _howToPlayBtn;
    void Start()
    {
        _levelText.text = "Level " + (SceneController.Instance.CurrentScene - 1).ToString();
        _howToPlayBtn.onClick.AddListener(OnHowToPlay);
    }

    private void OnDestroy()
    {
        _howToPlayBtn.onClick.RemoveListener(OnHowToPlay);
    }

    void OnHowToPlay()
    {
        UIManager.Instance.GetPanel<TextPopupPanel>().SetInfo("GUIDE" ,
            "<color=#ff0000ff>W/A/S/D</color> or <color=#ff0000ff>ARROW KEY</color> to move axie around." +
            "\n<color=#ff0000ff>SPACE</color> to reset level." +
            "\n<color=#ff0000ff>ESCAPE</color> to go back the main menu." +
            "\n\n Please help poor Axie ^^"); 
            
        UIManager.Instance.ShowPanelWithDG(typeof(TextPopupPanel));
        SoundManager.Instance.Play(Sounds.UI_POPUP);
    }
}
