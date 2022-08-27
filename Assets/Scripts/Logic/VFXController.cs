using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System;

public class VFXController : MonoBehaviour
{
    [SerializeField] SkeletonAnimation vfxSke;
    [SerializeField] AutoDisable autoDisable;

    private void OnEnable()
    {
        SetAnimation("play", false, 1, autoDisable.DisableObj);
    }

    void SetAnimation(string name, bool loop, float timeScale = 1, Action callback = null)
    {
        vfxSke.AnimationState.SetAnimation(0, name, loop);
        vfxSke.timeScale = timeScale;
        if (callback != null)
        {
            vfxSke.AnimationState.Complete += delegate (Spine.TrackEntry entry)
            {
                callback.Invoke();
            };
        }
    }
}
