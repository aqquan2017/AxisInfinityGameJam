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
    
    public ParticleSystem deadVFX;
    public GameObject hitVFX;
    public Transform spawnVfx;

    private bool isDead = false;
    private BoxCollider2D collider2D;

    void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();

        anim.Play("Enemy_Idle");
    }

    bool CanMoveWithoutObstacle(Vector2 direction)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) transform.position + direction, direction, 0.1f);
        if (raycastHit2D)
        {
            if (raycastHit2D.transform.TryGetComponent(out IWallCollider triggerObject)
                || raycastHit2D.transform.TryGetComponent(out IInteractObject interactObject))
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
        if (isDead) return;

        if (CanMoveWithoutObstacle(direction))
        {
            anim.SetBool("IsHurting", true);
            // var hitFx = Instantiate(hitVFX, spawnVfx.position, spawnVfx.rotation);
            // hitFx.Play();
            float angle = direction == Vector2.up ? 90
                : direction == Vector2.down ? -90
                : direction == Vector2.right ? 0
                : 180;
            Pooling.InstantiateObject(hitVFX, transform.position - (Vector3)direction * .5f, Quaternion.Euler(0, 0, angle));

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
        isDead = true;
        collider2D.enabled = false;
        anim.SetTrigger("Die");
        var deadFX = Instantiate(deadVFX, spawnVfx.position, spawnVfx.rotation);
        TimerManager.Instance.AddTimer(0.9f, () => deadFX.Play());
        SoundManager.Instance.Play(Sounds.ENEMY_DEAD);
        Destroy(transform.gameObject, 1);
    }
}
