using UnityEngine;
using UnityEngine.AI;

public class Enemy : AInteractableEntity
{
    private EnemyStat _enemyStat;

    private NavMeshAgent _navMeshAgent;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public AInteractableEntity _target;
    public AInteractableEntity Target => _target;

    public float MoveSpeed;
    public CircleCollider2D _interactCollider;
    public CircleCollider2D _searchCollider;
    private Rigidbody2D _rigidbody;

    private Animator _animator;
    public Animator Animator => _animator;

    public Transform Center;
    public bool IsFacingRight = true;
    public bool MissTarget = false;
    private void Awake()
    {
        _enemyStat = GetComponent<EnemyStat>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _interactCollider = transform.GetChild(0).GetComponent<CircleCollider2D>();
        _searchCollider = transform.GetChild(1).GetComponent<CircleCollider2D>();

        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        _interactType = InteractType.Enemy;
    }
    private void Update()
    {
     
        if (!CanInteract)
        {
            return;
        }
        if (IsWithInFireRange != FireManager.Instance.IsWithInFireRange(transform.position))
        {
            IsWithInFireRange = !IsWithInFireRange;
        }
        if (_target == null)
        {
            //Target = Center;
            Animator.SetBool("IsAttacking", false);

            SetTarget(FireManager.Instance.GetFire());
            return;
        }

        _navMeshAgent.SetDestination(_target.transform.position);
        //_rigidbody.linearVelocity = _navMeshAgent.desiredVelocity;
        _navMeshAgent.nextPosition = transform.position;

        if (transform.position.x < _target.transform.position.x ^ IsFacingRight)
        {
            Flip();
        }

        //if (_navMeshAgent.velocity == Vector3.zero)
        //{
        //    _animator.SetBool("IsRunning", false);
        //}
        //else
        //{
        //    _animator.SetBool("IsRunning", true);
        //}
    }
    public void Flip()
    {
        IsFacingRight = !IsFacingRight;
        transform.Rotate(0, 180, 0);
    }

    public void SetTarget(AInteractableEntity interactable)
    {
        _target = interactable;
        if (CheckTargetInInteractTrigger())
        {
            StopNavMeshAgent();
            Animator.SetBool("IsAttacking", true);
        }
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
            if (!collider.CompareTag("Unit") && !collider.CompareTag("Fire"))
            {
                continue;
            }
            if (!collider.GetComponent<AInteractableEntity>().CanInteract)
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
        //MissTarget = false;
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

    //AnimationEvent
    public void Interact()
    {
        _target.TakeDamage(_enemyStat.Damage, false);
    }

    public void StopNavMeshAgent()
    {
        _navMeshAgent.isStopped = true;
    }
    public void ResumeNavMeshAgent()
    {
        _navMeshAgent.isStopped = false;
    }

    public override void TakeDamage(int amount, bool isHeal)
    {
        base.TakeDamage(amount, isHeal);
        //Debug.Log($"{transform.name}: damage({amount}) health({Health})");

        if (CanInteract)
        {
            return;
        }
        WaveManager.Instance.OnEnemyDeath();
        InventoryResourceManager.Instance.TryAddCurrentResourceCount(InventoryResourceType.Ash, _enemyStat.DropAshCount);

        GetComponent<CircleCollider2D>().enabled = false;
        _target = null;
        _animator.SetTrigger("Die");
        _navMeshAgent.enabled = false;
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    public void FindTargetRepaeat()
    {
        if (_target == null)
        {
            FindTarget();
        }
    }
}
