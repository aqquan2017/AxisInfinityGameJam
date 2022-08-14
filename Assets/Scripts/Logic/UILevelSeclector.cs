using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILevelSeclector : UICarouselView
{

    [Space(10)]
    [SerializeField] bool allowSelectingSelectedItem;

    [Header("UI")]
    [SerializeField] Transform itemsParent;
    [SerializeField] UIObjectSelectorItem itemSamplePrefab;

    [SerializeField] Sprite lockSprite;
    [SerializeField] Sprite unlockSprite;

    int selectedIndex;
    public System.Action<UnlockableItemData> onObjectSelected;


    protected override void Start()
    {
        //Add new prefab to img
        //images = new RectTransform[LevelData.Instance.totalLevel];
        for(int i=2; i< SceneManager.sceneCountInBuildSettings; i++)
        {
            var lvObj = Instantiate(itemSamplePrefab, itemsParent);

            var objUI = lvObj.GetComponent<UIObjectSelectorItem>();
            int lv = i - 1;

            objUI.SetLvText("Level " + lv);
            objUI.gameObject.SetActive(true);

            if(lv <= DataManager.Instance.UnlockLv)
            {
                int temp = i;
                objUI.Init(unlockSprite, false, () => SceneController.Instance.ChangeScene(temp));
            }
            else
            {
                objUI.Init(lockSprite, true, null);
            }


            images.Add(lvObj.GetComponent<RectTransform>());
        }


        base.Start();

        base.OnIndexChanged += OnIndexChange;

        

        //go to current lv
        GoToIndex(DataManager.Instance.CurrentLv - 1);
        

    }

    protected override void OnSwipeComplete()
    {
        base.OnSwipeComplete();
    }

    protected override void OnSelectObject()
    {
        selectedIndex = CurrentIndex;
        OnIndexChange(selectedIndex);

        //load new lv
        SoundManager.Instance.Play(Sounds.UI_POPUP);
        SceneController.Instance.ChangeScene(selectedIndex +2);
    }

    void OnIndexChange(int idx)
    {
        bool isLocked = images[idx].GetComponent<UIObjectSelectorItem>().IsLocked;
        selectButton.interactable = !isLocked && selectedIndex != idx || !isLocked && allowSelectingSelectedItem;
    }

    public void UnlockItem(int index)
    {
        images[index].GetComponent<UIObjectSelectorItem>().IsLocked = false;
        OnIndexChange(index);
    }

    public void LockItem(int index)
    {
        images[index].GetComponent<UIObjectSelectorItem>().IsLocked = true;
        OnIndexChange(index);
    }

}
