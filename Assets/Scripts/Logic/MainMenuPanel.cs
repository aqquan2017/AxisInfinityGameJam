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
            //UIManager.Instance.ShowPanelWithDG(typeof(InfoPanel));
            SoundManager.Instance.Play(Sounds.UI_POPUP);
        }
}
