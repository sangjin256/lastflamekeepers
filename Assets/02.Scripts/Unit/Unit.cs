using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : AInteractableEntity
//:IInteractable
{
    [SerializeField] private UnitStat _unitStat;
    private UnitTool _unitTool;

    private NavMeshAgent _navMeshAgent;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public AInteractableEntity _target;
    public AInteractableEntity Target => _target;
    //≈◊Ω∫∆ÆøÎ
    //public GameObject _target;
    private CircleCollider2D _interactCollider;
    private CircleCollider2D _searchCollider;
    private Rigidbody2D _rigidbody;

    private Animator _animator;
    private Animator _toolAnimator;
    public Animator ToolAnimator => _toolAnimator;


    private bool IsMoving = true;
    private bool IsFacingRight = true;
    public bool IsInteracting = false;
    public bool IsDead = false;
    //¿”Ω√
    public List<RuntimeAnimatorController> ToolAnimatorList;
    private void Awake()
    {
        _unitStat = GetComponent<UnitStat>();
        _unitTool = GetComponentInChildren<UnitTool>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _interactCollider = transform.GetChild(0).GetComponent<CircleCollider2D>();
        _searchCollider = transform.GetChild(1).GetComponent<CircleCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _toolAnimator = transform.GetChild(2).GetComponent<Animator>();
    }

    private void Start()
    {
        _interactType = InteractType.Unit;
        _navMeshAgent.speed = _unitStat.MoveSpeed.Value / 10f;
    }
    private void Update()
    {
        if (IsDead)
        {
            return;
        }
        if(_target == null)
        {
            if(_navMeshAgent.desiredVelocity != Vector3.zero)
            {
                IsMoving = true;
            }
            else
            {
                if (IsMoving)
                {
                    IsMoving = false;
                    FindTarget();
                }
            }
        }
        else
        {
            _navMeshAgent.SetDestination(_target.transform.position);
            _rigidbody.linearVelocity = _navMeshAgent.desiredVelocity;
            _navMeshAgent.nextPosition = transform.position;

            if (transform.position.x < _target.transform.position.x ^ IsFacingRight)
            {
                if (Mathf.Abs(transform.position.x - _target.transform.position.x) > 0.1f)
                {
                    Flip();
                }
            }
        }

        if (IsInteracting || _target == null)
        {
            _animator.SetBool("IsRunning", false);
        }

        else
        {
            _animator.SetBool("IsRunning", true);
        }

        //∂•πŸ¥⁄ ¬Ôæ˙¿ª ∞ÊøÏ
        if (_target == null &&_navMeshAgent.desiredVelocity.x != 0 && (_navMeshAgent.desiredVelocity.x > 0 ^ IsFacingRight))
        {
            Flip();
        }




    }

    public void Flip()
    {
        IsFacingRight = !IsFacingRight;
        transform.Rotate(0, 180, 0);
    }

    public void SetTarget(AInteractableEntity interactable)
    {
        if (IsDead)
        {
            return;
        }

        if (!_unitTool.Tool.IsInteractable(interactable.InteractType))
        {
            return;
        }
        _target = interactable;
        if (CheckTargetInInteractTrigger())
        {
            StopNavMeshAgent();
            _toolAnimator.SetBool("IsInteracting", true);
            
            IsInteracting = true;
        }
    }

    public void SetTarget(Transform point)
    {
        if (IsDead)
        {
            return;
        }

        _navMeshAgent.SetDestination(point.position);
        _target = null;
    }

    public void FindTarget()
    {
        if (IsDead)
        {
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _searchCollider.radius, LayerMask.GetMask("Interactable"));
        if (colliders.Length == 0)
        {
            return;
        }
        float minDistance = _searchCollider.radius + 1;
        Collider2D minDistanceCollider = null;
        foreach (Collider2D collider in colliders)
        {
            if(collider.transform == transform)
            {
                continue;
            }
            //if (!_tool.IsInteractable(collider.GetComponent<AInteractableEntity>().InteractType))
            if(collider.CompareTag("Enemy"))
            {
                continue;
            }
            if (Vector2.Distance(collider.transform.position, transform.position) < minDistance)
            {
                minDistanceCollider = collider;
            }
        }

        if (minDistanceCollider == null)
        {
            return;
        }
        SetTarget(minDistanceCollider.GetComponent<AInteractableEntity>());
    }

    public bool CheckTargetInInteractTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _interactCollider.radius, LayerMask.GetMask("Interactable"));

        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<AInteractableEntity>() == _target)
            {
                return true;
            }
        }
        return false;
    }


    public void StopNavMeshAgent()
    {
        if (_navMeshAgent.enabled)
        {
            _navMeshAgent.isStopped = true;
        }
    }
    public void ResumeNavMeshAgent()
    {
        if (_navMeshAgent.enabled)
        {
            _navMeshAgent.isStopped = false;
        }
    }

    //Animation event




    public override void TakeDamage(int amount, bool isHeal)
    {
        base.TakeDamage(amount, isHeal);
        if (CanInteract)
        {
            return;
        }
        GetComponent<CircleCollider2D>().enabled = false;
        _target = null;
        _toolAnimator.SetBool("IsInteracting", false);
        _animator.SetTrigger("Die");
        _navMeshAgent.enabled = false;
    }


    public void SetTool(ATool tool)
    {
        _unitTool.SetTool(tool);
    }

    public void DestroyThis()
    {
        gameObject.SetActive(false);
    }
}