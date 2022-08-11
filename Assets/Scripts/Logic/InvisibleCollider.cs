using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleCollider : MonoBehaviour
{
    void Start()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position , Vector3.one);
    }
}
