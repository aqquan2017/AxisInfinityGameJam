using DG.Tweening;
using Game;
using UnityEngine;
using System;
using MoreMountains.Feedbacks;
using MoreMountains.FeedbacksForThirdParty;
using Spine.Unity;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    private PlayerTurnLogic _playerTurnLogic;
    private PlayerKeyLock _playerKeyLock;
    public AxieFigure _axieFigure;
    public bool _canMove = true;
    private bool _gameOver = false;
    public MMF_Player _shakeFeedback;
    public ParticleSystem _moveParticle;
    public Transform _vfxSpawn;
    public event Action OnMoveAction;

    public bool GameOver => _gameOver;
    
    void Start()
    {
        _playerTurnLogic = GetComponent<PlayerTurnLogic>();
        _playerKeyLock = GetComponent<PlayerKeyLock>();
    }

    private void Update()
    {
        if (!_canMove || _gameOver || _axieFigure == null)
            return;
#if UNITY_EDITOR
        Movement(GetInput());
#endif
    }

       
    public void PlayerFrozen()
    {
        _gameOver = true;
    }

    public void MoveLeft()
    {
        Movement(Vector2.left);
    }
    public void MoveRight()
    {
        Movement(Vector2.right);
    }
    public void MoveUp()
    {
        Movement(Vector2.up);
    }
    public void MoveDown()
    {
        Movement(Vector2.down);
    }

    void Movement(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            SoundManager.Instance.Play(Sounds.UI_POPUP);

            //rotate VFX transform to play VFX
            float rotationVal = direction == Vector2.left ? 0
                : direction == Vector2.down ? 90
                : direction == Vector2.right ? 180
                : 270;
            _vfxSpawn.eulerAngles = Vector3.forward * rotationVal;
            
            //flip character
            if (direction == Vector2.right)
            {
                _axieFigure.FlipX = true;
            }
            else if (direction == Vector2.left)
            {
                _axieFigure.FlipX = false;
            }
            

            Action OnDoLater = null;
            if (CanMove(direction, ref OnDoLater))
            {
                _canMove = false;
                var moveVFX = Pooling.Instantiate(_moveParticle, _vfxSpawn.position, _vfxSpawn.rotation);
                moveVFX.Play();
                
                transform.DOMove((Vector2)transform.position + direction, 0.1f).OnComplete(() =>
                {
                    OnDoLater?.Invoke();
                    _playerTurnLogic.DecreaseTurn();
                    _canMove = true;
                });
                _axieFigure.SetAnimation("action/move-forward", 2f, true);
                OnMoveAction?.Invoke();
                return;
            }

            
            if (CanAttack(direction))
            {
                AttackObject(direction);
                string animAttack = Random.value > 0.5f ? "attack/melee/multi-attack" : "attack/ranged/cast-high";
                SoundManager.Instance.Play(Sounds.ENEMY_HIT);
                _axieFigure.SetAnimation(animAttack, 2f, false);
                
                
                _playerTurnLogic.DecreaseTurn();
                CheckHurtItSelf();
                OnMoveAction?.Invoke();
            }
            
            var cameraShaker = _shakeFeedback.GetFeedbackOfType<MMF_CameraShake>();
            cameraShaker.CameraShakeProperties.AmplitudeX = direction.x * 0.6f;
            cameraShaker.CameraShakeProperties.AmplitudeY = direction.y * 0.6f;
            _shakeFeedback?.PlayFeedbacks();
        }

    }

    bool CanMove(Vector2 direction, ref Action OnDoLater)
    {
        var interfaceInteract = Physics2D.RaycastAll((Vector2)transform.position + direction, direction, 0.1f);
        if (interfaceInteract.Length > 0)
        {
            for (int i = 0; i < interfaceInteract.Length; i++)
            {
                if (interfaceInteract[i].transform.TryGetComponent(out IInteractObject interactObject)
                    || interfaceInteract[i].transform.TryGetComponent(out IWallCollider wallCollider))
                {
                    return false;
                }
                
                if (interfaceInteract[i].transform.TryGetComponent(out ITriggerObject triggerObject))
                {
                    if (interfaceInteract[i].transform.TryGetComponent(out ILockMechanic lockMechanic) && !_playerKeyLock.HaveKey)
                    {
                        return false;
                    }
                    
                    OnDoLater = () => triggerObject.OnTrigger(gameObject);
                }
            }
        }
        return true;
    }

    bool CanAttack(Vector2 direction)
    {
        var interfaceInteract = Physics2D.RaycastAll((Vector2)transform.position + direction, direction, 0.1f);
        if (interfaceInteract.Length > 0)
        {
            for (int i = 0; i < interfaceInteract.Length; i++)
            {
                if (interfaceInteract[i].transform.TryGetComponent(out IInteractObject interactObject))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void AttackObject(Vector2 direction)
    {
        RaycastHit2D[] raycastHit2D = Physics2D.RaycastAll((Vector2)transform.position + direction, direction, 0.1f);
        if (raycastHit2D.Length > 0)
        {
            for (int i = 0; i < raycastHit2D.Length; i++)
            {
                if (raycastHit2D[i].transform.TryGetComponent(out IInteractObject interactObject))
                {
                    interactObject.OnImpact(direction);
                }
            }
        }
    }

    //used for when player attack - still in a trap
    void CheckHurtItSelf()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2)transform.position, Vector2.down, 0.01f);
        if (raycastHit2D)
        {
            if (raycastHit2D.transform.TryGetComponent(out ITriggerObject interactObject))
            {
                interactObject.OnTrigger(gameObject);
            }
        }
    }

    Vector2 GetInput()
    {
        if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.W) || ControlFreak2.CF2Input.GetKeyDown(KeyCode.UpArrow))
        {
            return Vector2.up;
        }
        if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.A) || ControlFreak2.CF2Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return Vector2.left;
        }
        if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.S) || ControlFreak2.CF2Input.GetKeyDown(KeyCode.DownArrow))
        {
            return Vector2.down;
        }
        if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.D) || ControlFreak2.CF2Input.GetKeyDown(KeyCode.RightArrow))
        {
            return Vector2.right;
        }
        return Vector2.zero;
    }
}
