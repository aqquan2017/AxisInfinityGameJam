using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AxieRescuePanel : BasePanel, IPointerDownHandler
{
    [SerializeField] private Text header;
    [SerializeField] private Text des;
    [SerializeField] private Image axieImg;

    private Action OnClosePanel = null;

    public void SetInfo(string head, string desTxt, Sprite axieSprite, Action OnWorkWhileClose = null)
    {
        axieImg.sprite = axieSprite;
        header.text = head;
        des.text = desTxt;
        OnClosePanel = OnWorkWhileClose;
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
