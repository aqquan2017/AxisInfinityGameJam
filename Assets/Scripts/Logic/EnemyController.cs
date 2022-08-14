using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyController : MonoBehaviour, IInteractObject
{
    [SerializeField] Animator anim;

    private string idleName = "Enemy_Idle";
    private string hurtName = "Enemy_Hurt";
    private string dieName = "Enemy_Die";

    void Start()
    {
        anim.Play("Enemy_Idle");
    }

    bool CanMoveWithoutObstacle(Vector2 direction)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) transform.position + direction, direction, 0.1f);
        if (raycastHit2D)
        {
            if (raycastHit2D.transform.TryGetComponent(out IWallCollider triggerObject))
            {
                return false;
            }
        }
        return true;
    }

    bool HaveTriggerInDirection(ref Action OnTrigger, Vector2 direction)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) transform.position + direction, direction, 0.1f);
        if (raycastHit2D)
        {
            if (raycastHit2D.transform.TryGetComponent(out ITriggerObject triggerObject))
            {
                OnTrigger = () => triggerObject.OnTrigger(gameObject);
                return true;
            }
        }
        return false;
    }

    public void OnImpact(Vector2 direction)
    {
        if (CanMoveWithoutObstacle(direction))
        {
            //move box
            //TODO : VFX, Sound
            anim.SetBool("IsHurting", true);

            Vector2 originPos = transform.position;
            Action OnTrigger = null;
            bool haveTrigger = HaveTriggerInDirection(ref OnTrigger, direction);
            transform.DOMove((Vector2)transform.position + direction, 0.1f).OnComplete(() =>
            {
                anim.SetBool("IsHurting", false);
                if (haveTrigger)
                {
                    OnTrigger?.Invoke();
                }
            });
        }
        else
        {
            Die();
        }
    }

    public void Die()
    {
        //destroy box
        //TODO : VFX, Sound
        anim.SetTrigger("Die");
        Destroy(transform.gameObject, 1);
    }
}
