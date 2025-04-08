using UnityEngine;
using UnityEngine.AI;

public class Unit : AInteractableEntityTest
//:IInteractable
{
    [SerializeField] private UnitStat _unitStat;
    private ATool _tool;

    private NavMeshAgent _navMeshAgent;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public AInteractableEntityTest _target;
    public AInteractableEntityTest Target => _target;
    //테스트용
    //public GameObject _target;
    private CircleCollider2D _interactCollider;
    private CircleCollider2D _searchCollider;
    private Rigidbody2D _rigidbody;

    private Animator _animator;
    private Animator _toolAnimator;
    public Animator ToolAnimator => _toolAnimator;


    private bool IsFacingRight = true;
    private void Awake()
    {
        _unitStat = GetComponent<UnitStat>();
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
        _navMeshAgent.speed = _unitStat.MoveSpeed.Value / 10f;
    }
    private void Update()
    {
        if(_target == null)
        {
            Debug.Log(_navMeshAgent.desiredVelocity.x);
            Debug.Log(_navMeshAgent.desiredVelocity.y);
            Debug.Log(_navMeshAgent.desiredVelocity.z);

            if (_navMeshAgent.desiredVelocity == Vector3.zero)
            {
                FindTarget();
            }
        }
        else
        {
            _navMeshAgent.SetDestination(_target.transform.position);
            _rigidbody.linearVelocity = _navMeshAgent.desiredVelocity;
            _navMeshAgent.nextPosition = transform.position;

            //땅바닥 찍었을 경우
            if (transform.position.x < _target.transform.position.x ^ IsFacingRight)
            {
                Flip();
            }
        }

        if (_navMeshAgent.velocity == Vector3.zero)
        {
            _animator.SetBool("IsRunning", false);
        }
        else
        {
            _animator.SetBool("IsRunning", true);
        }

        if (_navMeshAgent.desiredVelocity.x != 0 && (_navMeshAgent.desiredVelocity.x > 0 ^ IsFacingRight))
        {
            Flip();
        }




    }

    public void Flip()
    {
        IsFacingRight = !IsFacingRight;
        transform.Rotate(0, 180, 0);
    }

    public void SetTarget(AInteractableEntityTest interactable)
    {
        //if (!_tool.IsInteractable(interactable.InteractType))
        //{
        //    return;
        //}
        _target = interactable;
        if (CheckTargetInInteractTrigger())
        {
            StopNavMeshAgent();
            Interact(_target);
        }
    }

    public void SetTarget(Vector3 point)
    {
        _navMeshAgent.SetDestination(point);
        _target = null;
    }

    // TODO: 마우스 클릭으로 도착했을때도 FindTarget 실행하도록
    public void FindTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _searchCollider.radius, LayerMask.GetMask("Interactable"));
        float minDistance = _searchCollider.radius + 1;
        Collider2D minCollider = null;
        foreach (Collider2D collider in colliders)
        {
            if(collider.transform == transform)
            {
                continue;
            }
            //if (!_tool.IsInteractable(collider.GetComponent<AInteractableEntityTest>().InteractType))
            //{
            //    continue;
            //}
            if (Vector2.Distance(collider.transform.position, transform.position) < minDistance)
            {
                minCollider = collider;
            }
        }

        if (minCollider == null)
        {
            return;
        }
        SetTarget(minCollider.GetComponent<AInteractableEntityTest>());
    }

    public bool CheckTargetInInteractTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _interactCollider.radius, LayerMask.GetMask("Interactable"));

        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<AInteractableEntityTest>() == _target)
            {
                return true;
            }
        }
        return false;
    }


    public void StopNavMeshAgent()
    {
        _navMeshAgent.isStopped = true;
    }
    public void ResumeNavMeshAgent()
    {
        _navMeshAgent.isStopped = false;
    }

    public void Interact(IInteractable interactable)
    {
        //if (_tool.IsInteractable(interactable))
        if (true)
        {
            // TODO: NAVMESH 타겟지정
            _toolAnimator.SetBool("IsInteracting", true);

        }
    }



    public override void TakeDamage(int amount, bool isHeal)
    {

    }

}