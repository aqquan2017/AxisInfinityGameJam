using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;

public interface IInteractObject
{
    void OnImpact(Vector2 direction);
}

public class BoxObstacle : MonoBehaviour, IInteractObject
{
    public MMF_Player _hitStrech;
    
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
        var stretch = _hitStrech.GetFeedbackOfType<MMF_SquashAndStretch>();
        stretch.Axis = direction.x != 0
            ? MMF_SquashAndStretch.PossibleAxis.YtoX
            : MMF_SquashAndStretch.PossibleAxis.XtoY;
        _hitStrech?.PlayFeedbacks();

        if (CanMoveWithoutObstacle(direction))
        {
            //move box
            //TODO : VFX, Sound
            transform.DOMove((Vector2)transform.position + direction, 0.1f);
        }
        else
        {
            //box stay the same
            //TODO : VFX, Sound
            
        }
    }
}
