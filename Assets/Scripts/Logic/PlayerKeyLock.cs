using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyLock : MonoBehaviour
{
    private bool _haveKey = false;

    public bool HaveKey => _haveKey;

    public void SetHaveKey()
    {
        _haveKey = true;
    }
    
    
}
