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
        //little bit tricky one, do this to avoid check collider itself
        if (Physics2D.Raycast((Vector2)transform.position + direction, direction, 0.1f))
        {
            return false;
        }
        return true;
    }

    public void OnImpact(Vector2 direction)
    {
        if (CanMoveWithoutObstacle(direction))
        {
            //move box
            //TODO : VFX, Sound
            anim.SetBool("IsHurting", true);
            transform.DOMove((Vector2)transform.position + direction, 0.1f).OnComplete(() => anim.SetBool("IsHurting", false));
        }
        else
        {
            //destroy box
            //TODO : VFX, Sound
            anim.SetTrigger("Die");
            Destroy(transform.gameObject, 1);
        }
    }
}
