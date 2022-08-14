using AxieMixer.Unity;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Game
{
    public class AxieFigure : MonoBehaviour
    {
        private SkeletonAnimation skeletonAnimation;
        
        
        [SerializeField] private bool _flipX = false;
        public bool FlipX
        {
            get
            {
                return _flipX;
            }
            set
            {
                _flipX = value;
                if (skeletonAnimation != null)
                {
                    skeletonAnimation.skeleton.ScaleX = (_flipX ? -1 : 1) * Mathf.Abs(skeletonAnimation.skeleton.ScaleX);
                }
            }
        }

        private void Awake()
        {
            skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>();
        }

        public void SetGenes(string id, string genes)
        {
            if (string.IsNullOrEmpty(genes)) return;

            if (skeletonAnimation != null && skeletonAnimation.state != null)
            {
                skeletonAnimation.state.End -= SpineEndHandler;
            }
            Mixer.SpawnSkeletonAnimation(skeletonAnimation, id, genes);

            skeletonAnimation.transform.SetParent(transform, false);
            skeletonAnimation.skeleton.ScaleX = (_flipX ? -1 : 1) * Mathf.Abs(skeletonAnimation.skeleton.ScaleX);
            skeletonAnimation.skeleton.FindSlot("shadow").Attachment = null;
            skeletonAnimation.timeScale = 0.5f;
            skeletonAnimation.state.SetAnimation(0, "activity/appear", true);
            skeletonAnimation.state.End += SpineEndHandler;
        }

        public void SetAnimation(string animName, float timeScale, bool loop = false)
        {
            skeletonAnimation.timeScale = timeScale;
            skeletonAnimation.state.ClearTrack(0);
            skeletonAnimation.state.SetAnimation(0, animName, loop);
        }

        private void OnDestroy()
        {
            if (skeletonAnimation != null  && skeletonAnimation.state != null)
            {
                skeletonAnimation.state.End -= SpineEndHandler;
            }
        }

        private void SpineEndHandler(TrackEntry trackEntry)
        {
            string animation = trackEntry.Animation.Name;
            if (animation == "activity/appear"
                || animation == "attack/melee/multi-attack"
                || animation == "attack/ranged/cast-high")
            {
                skeletonAnimation.state.SetAnimation(0, "action/idle/normal", true);
                skeletonAnimation.timeScale = 0.5f;
            }
        }
    }
}