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
    //Å×½ºÆ®¿ë
    //public GameObject _target;
    private CircleCollider2D _interactCollider;
    private CircleCollider2D _searchCollider;
    private Rigidbody2D _rigidbody;

    private Animator _animator;
    private Animator _toolAnimator;
    public Animator ToolAnimator => _toolAnimator;


    private bool IsMoving = true;
    private bool IsFacingRight = true;
    public bool MissTarget = false;

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
        if (!CanInteract)
        {
            return;
        }
        //if (MissTarget == false)
        //{
        //    if (_target == null)
        //    {
        //        MissTarget = true;
        //    }
        //}
        //else
        //{
        //    if (MissTarget)
        //    {
                //FindTarget();
                //Debug.Log("DDDDDD");
            //}
        //}
        if (_target == null)
        {
            if (_navMeshAgent.desiredVelocity != Vector3.zero)
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

        if (_target == null)
        {
            if (_navMeshAgent.desiredVelocity == Vector3.zero)
            {
                _animator.SetBool("IsRunning", false);
            }
        }
        else if (_unitTool.IsInteracting)
        {
            _animator.SetBool("IsRunning", false);
        }
        else if(!_unitTool.IsInteracting)
        {
            _animator.SetBool("IsRunning", true);
        }

        //¶¥¹Ù´Ú Âï¾úÀ» °æ¿ì
        if (_target == null && _navMeshAgent.desiredVelocity.x != 0 && (_navMeshAgent.desiredVelocity.x > 0 ^ IsFacingRight))
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
        if (!CanInteract)
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
        }
    }

    public void SetTarget(Vector2 point)
    {
        if (!CanInteract)
        {
            return;
        }

        _navMeshAgent.SetDestination(point);
        _target = null;
    }

    public void SetTarget(Transform point)
    {
        if (!CanInteract)
        {
            return;
        }

        _navMeshAgent.SetDestination(point.position);
        _target = null;
    }

    public void SetTargetNull()
    {
        _target = null;
    }

    public void FindTarget()
    {
        if (!CanInteract)
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
            if (collider.transform == transform)
            {
                continue;
            }
            //if (!_tool.IsInteractable(collider.GetComponent<AInteractableEntity>().InteractType))
            if (collider.CompareTag("Enemy"))
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
        MissTarget = false;
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

    public override void TakeDamage(int amount, bool isHeal)
    {
        base.TakeDamage(amount, isHeal);
        Debug.Log($"{transform.name}: damage({amount}) health({Health})");

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
        if (_target != null && !_unitTool.Tool.IsInteractable(_target.InteractType))
        {
            SetTargetNull();
            SetTarget(transform.position);
        }
    }

    public void SetTool(int tool)
    {
        _unitTool.SetTool(ToolManager.Instance.GetTool((ToolType)tool));
        SetTargetNull();
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
        UnitManager.Instance.DestroyUnit(this);
    }
}