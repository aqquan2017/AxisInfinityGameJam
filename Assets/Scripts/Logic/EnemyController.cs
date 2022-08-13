using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyController : MonoBehaviour, IInteractObject
{
    bool CanMoveWithoutObstacle(Vector2 direction)
    {
        //little bit tricky one, do this to avoid check collider itself
        if (Physics2D.Raycast((Vector2)transform.position + direction, direction, 0.3f))
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
            transform.DOMove((Vector2)transform.position + direction, 0.1f);
        }
        else
        {
            //destroy box
            //TODO : VFX, Sound
            
            Destroy(transform.gameObject);
        }
    }
}
