using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeMovement : MonoBehaviour
{
    private void Start()
    {
        RandomPosition();
    }

    public void RandomPosition()
    {
        this.enabled = true;
        transform.position = new Vector3(Random.Range(-9f, 9f), Random.Range(-3f, 3f), 0);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.DOMove(transform.position + Vector3.up, 0.2f);
            TestManager.Instance.PlaySound( TestManager.Instance._moveSound);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.DOMove(transform.position + Vector3.left, 0.2f);
            TestManager.Instance.PlaySound( TestManager.Instance._moveSound);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.DOMove(transform.position + Vector3.down, 0.2f);
            TestManager.Instance.PlaySound(TestManager.Instance._moveSound);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.DOMove(transform.position + Vector3.right, 0.2f);
            TestManager.Instance.PlaySound(TestManager.Instance._moveSound);
        }
    }

    
}
