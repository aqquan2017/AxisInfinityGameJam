using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
        [SerializeField] private Button soundSettingBtn;
        [SerializeField] private Button infoSettingBtn;
        [SerializeField] private Image soundSettingImg;
    
        [SerializeField] private AudioSource bgmMainMenu;
    
        [SerializeField] private Color colorOn;
        [SerializeField] private Color colorOff;
    
        private void Awake()
        {
            soundSettingBtn.onClick.AddListener(OnSoundSetting);
            infoSettingBtn.onClick.AddListener(OnInfoSetting);
    
        }
    
        private void Start()
        {
            soundSettingImg.color = DataManager.Instance.SoundOn ? colorOn : colorOff;
    
            SoundManager.Instance.StopAll(true);
            SoundManager.Instance.GLOBAL_ON = DataManager.Instance.SoundOn;
            bgmMainMenu.enabled = DataManager.Instance.SoundOn;
        }



        private void OnSoundSetting()
        {
            DataManager.Instance.SoundOn = !DataManager.Instance.SoundOn;
            soundSettingImg.color = DataManager.Instance.SoundOn ? colorOn : colorOff;
    
            bgmMainMenu.enabled = DataManager.Instance.SoundOn;
            SoundManager.Instance.GLOBAL_ON = DataManager.Instance.SoundOn;
            SoundManager.Instance.Play(Sounds.UI_POPUP);
        }
    
        private void OnInfoSetting()
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
