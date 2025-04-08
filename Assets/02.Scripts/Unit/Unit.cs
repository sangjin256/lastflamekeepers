using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : AInteractableEntity
//:IInteractable
{
    [SerializeField] private UnitStat _unitStat;
    private UnitTool _unitTool;
    public UnitTool UnitTool => _unitTool;
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
        Health = _unitStat.MaxHealth.Value;

        _navMeshAgent.SetDestination(Vector3.zero);
    }
    private void Update()
    {
        if (!CanInteract)
        {
            return;
        }

        if (_target == null)
        {
            ToolAnimator.SetBool("IsInteracting", false);
        }
        else
        {
            if (_target.CanInteract)
            {


                _navMeshAgent.SetDestination(_target.transform.position);
                //_rigidbody.linearVelocity = _navMeshAgent.desiredVelocity;
                _navMeshAgent.nextPosition = transform.position;

                if (transform.position.x < _target.transform.position.x ^ IsFacingRight)
                {
                    if (Mathf.Abs(transform.position.x - _target.transform.position.x) > 0.1f)
                    {
                        Flip();
                    }
                }
            }
        }

        if (_target == null)
        {
            if (_navMeshAgent.desiredVelocity == Vector3.zero)
            {
                _animator.SetBool("IsRunning", false);
            }
            else
            {
                _animator.SetBool("IsRunning", true);
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
        //if (_target == null && _navMeshAgent.desiredVelocity.x != 0 && (_navMeshAgent.desiredVelocity.x > 0 ^ IsFacingRight))
        //{
        //    Flip();
        //}




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
        else
        {
            ResumeNavMeshAgent();
        }
    }

    public void SetTarget(Vector2 point)
    {
        if (!CanInteract)
        {
            return;
        }

        Vector2 randomPoint = point + Random.insideUnitCircle / 2;
        _navMeshAgent.SetDestination(randomPoint);
        _target = null;

        if(transform.position.x < randomPoint.x ^ IsFacingRight)
        {
            Flip();
        }

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
            if (!collider.CompareTag("Enemy"))
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
            Debug.Log($"{gameObject.name}: zz");
            return;
        }
        Debug.Log(minDistanceCollider.gameObject);

        SetTarget(minDistanceCollider.GetComponent<AInteractableEntity>());
        Debug.Log("¾Æ´ÏÁö");
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
        FindTarget();
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
        UnitManager.Instance.DestroyUnit(this);
    }


    public void ResetPathTest()
    {
        _navMeshAgent.ResetPath();
        ResumeNavMeshAgent();
    }
}