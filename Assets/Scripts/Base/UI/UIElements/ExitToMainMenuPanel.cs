using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExitToMainMenuPanel : BasePanel, IPointerDownHandler
{
    [SerializeField] private Button _exitBtn;

    private Action OnClosePanel = null;

    private void Start()
    {
        _exitBtn.onClick.AddListener(OnExitToMainMenu);
    }

    private void OnDestroy()
    {
        _exitBtn.onClick.RemoveAllListeners();
    }

    void OnExitToMainMenu()
    {
        ClosePanel();
        GameStatic.Instance.ExitToGameMenu();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ClosePanel();
    }

    public void ClosePanel()
    {
        OnClosePanel?.Invoke();
        OnClosePanel = null;
        SoundManager.Instance.Play(Sounds.UI_POPUP);
        HideWithDG();
    }

    public override void OverrideText()
    {
        throw new System.NotImplementedException();
    }
}
