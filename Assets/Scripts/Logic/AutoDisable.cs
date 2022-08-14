using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    public void DisableObj()
    {
        Pooling.DestroyObject(this.gameObject);
    }

    public void DisableParent()
    {
        Pooling.DestroyObject(this.transform.parent.gameObject);
    }
}
