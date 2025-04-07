using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : AInteractableEntityTest
{
    private EnemyStat _enemyStat;

    public Transform Target;
    private Rigidbody2D _rigidbody;
    public float MoveSpeed;
    public bool IsAttacking;
    public CircleCollider2D _interactCollider;
    public CircleCollider2D _searchCollider;

    private Animator _animator;
    public Animator Animator => _animator;

    public Transform Center;
    public bool IsFacingRight = true;
    private void Awake()
    {
        _interactCollider = transform.GetChild(0).GetComponent<CircleCollider2D>();
        _searchCollider = transform.GetChild(1).GetComponent<CircleCollider2D>();

        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _rigidbody.linearVelocity = Vector2.zero;
        if ( Target == null)
        {
            Target = Center;
            return;
        }

        if (transform.position.x < Target.transform.position.x ^ IsFacingRight)
        {
            Flip();
        }

        if (IsAttacking)
        {

            return;
        }
  

        Vector2 direction = (Target.position - transform.position).normalized;

        _rigidbody.MovePosition(transform.position + (Vector3)direction * Time.deltaTime * MoveSpeed);
    }

    public void FindTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _searchCollider.radius, LayerMask.GetMask("Interactable"));
        float minDistance = _searchCollider.radius + 1;
        Collider2D minCollider = null;
        foreach (Collider2D collider in colliders)
        {
            if (collider.transform == transform)
            {
                continue;
            }
            if (Vector2.Distance(collider.transform.position, transform.position) < minDistance)
            {
                minCollider = collider;
            }
        }

        if (minCollider == null)
        {
            return;
        }
        if (CheckInteractTrigger())
        {
            IsAttacking = true;
            Animator.SetBool("IsAttacking", IsAttacking);
        }
    }

    public bool CheckInteractTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _interactCollider.radius, LayerMask.GetMask("Interactable"));

        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<AInteractableEntityTest>() == Target)
            {
                return true;
            }
        }
        return false;
    }
    public void Flip()
    {
        IsFacingRight = !IsFacingRight;
        transform.Rotate(0, 180, 0);
    }

    public override void TakeDamage(int amount, bool isHeal)
    {
        base.TakeDamage(amount, isHeal);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
