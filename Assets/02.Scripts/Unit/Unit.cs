using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Unit : AInteractableEntity
//:IInteractable
{
    [SerializeField] private UnitStat _unitStat;
    private UnitTool _unitTool;
    public UnitTool UnitTool => _unitTool;

    private NavMeshAgent _agent;
    public NavMeshAgent Agent => _agent;

    public AInteractableEntity _target;
    public AInteractableEntity Target => _target;

    private CircleCollider2D _interactCollider;
    private CircleCollider2D _searchCollider;

    private Animator _animator;
    private Animator _toolAnimator;
    public Animator ToolAnimator => _toolAnimator;


    private bool _isFacingRight = true;
    private bool _isAvoidingDark = false;
    private bool _shouldFlip;

    public Action<Unit> OnDamaged;

    public Slider HPBar;
    public Canvas canvas;

    private Renderer _renderer;
    public Renderer Renderer => _renderer;
    private MaterialPropertyBlock _materialPropertyBlock;

    private void Awake()
    {
        _unitStat = GetComponent<UnitStat>();
        _unitTool = GetComponentInChildren<UnitTool>();
        _agent = GetComponent<NavMeshAgent>();

        _interactCollider = transform.GetChild(0).GetComponent<CircleCollider2D>();
        _searchCollider = transform.GetChild(1).GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();
        _toolAnimator = transform.GetChild(2).GetComponent<Animator>();

        _renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        _interactType = InteractType.Unit;
        SetMoveSpeed(_unitStat.MoveSpeed.Value);
        Health = _unitStat.MaxHealth.Value;

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        //실제 위치와 NavMesh 시스템 동기화
        _agent.enabled = false;
        _agent.enabled = true;

        HPBar.maxValue = _unitStat.MaxHealth.Value;
        HPBar.value = Health;

        _materialPropertyBlock = new MaterialPropertyBlock();
    }
    private void Update()
    {
        if (!_agent.enabled || !CanInteract)
        {
            return;
        }

        // When Unit has no target
        if (_target == null || !_target.isActiveAndEnabled || !_target.CanInteract)
        {
            ToolAnimator.SetBool("IsInteracting", false);
        }

        // When Unit has a target in FireRange
        else if (_target.IsWithInFireRange)
        {
            if (!_unitTool.IsInteracting && (_target.InteractType == InteractType.Enemy || _target.InteractType == InteractType.Unit))
            {
                _agent.SetDestination(_target.transform.position);
            }
            //Flip according to target direction
            if (transform.position.x < _target.transform.position.x ^ _isFacingRight && !_unitTool.IsInteracting)
            {
                if (Mathf.Abs(transform.position.x - _target.transform.position.x) > 0.1f)
                {
                    Flip();
                }
            }
        }
        // When target disappears into the dark
        else
        {
            _agent.ResetPath();
            SetTargetNull();
            _animator.SetBool("IsRunning", false);
        }

        //Flip after interaction is complete
        if (_shouldFlip == true && !_unitTool.IsInteracting)
        {
            _shouldFlip = false;
            Flip();
        }

        SetRunningAnimation();
        RunAwayFromDark();
    }

    public void Flip()
    {
        _isFacingRight = !_isFacingRight;
        transform.Rotate(0, 180, 0);
        canvas.transform.Rotate(0, 180, 0);
    }

    public void SetMoveSpeed(float speed)
    {
        _agent.speed = speed / 10f;
        _animator.SetFloat("MoveSpeed", speed / 10f);
    }

    public void RunAwayFromDark()
    {
        //Check if unit is out of Fire Range
        if (IsWithInFireRange != FireManager.Instance.IsUnitWithInFireRange(transform.position))
        {
            IsWithInFireRange = !IsWithInFireRange;
        }

        if (!IsWithInFireRange)
        {
            if (!_isAvoidingDark)
            {
                _isAvoidingDark = true;
                _agent.ResetPath();
                SetTarget(Vector2.zero);
            }
            else
            {
                _agent.SetDestination(Vector2.zero);
            }
        }
        else
        {
            if (_isAvoidingDark)
            {
                _isAvoidingDark = false;
                _agent.ResetPath();
            }
        }
    }

    public void SetRunningAnimation()
    {
        if (_target == null && _agent.hasPath && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _agent.avoidancePriority = 60;
            _agent.ResetPath();
        }
        if (_target == null || !_target.isActiveAndEnabled)
        {
            if (_agent.desiredVelocity == Vector3.zero)
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
        else if (!_unitTool.IsInteracting)
        {
            _animator.SetBool("IsRunning", true);
        }
    }

    public void SetTarget(AInteractableEntity interactable)
    {
        if (!_agent.enabled)
        {
            return;
        }
        _agent.ResetPath();
        _toolAnimator.SetBool("IsInteracting", false);
        _agent.avoidancePriority = 50;
        if (!CanInteract)
        {
            return;
        }
        if (!interactable.IsWithInFireRange)
        {
            return;
        }
        if (!_unitTool.CurrentTool.IsInteractable(interactable.InteractType))
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
            _agent.SetDestination(_target.transform.position);
        }
    }

    // TODO: 인원 수 전달해서 그에 따른 랜덤 도착 범위 설정
    public void SetTarget(Vector2 point)
    {
        if (!_agent.enabled)
        {
            return;
        }
        _agent.avoidancePriority = 50;

        if (!CanInteract)
        {
            return;
        }

        Vector2 randomPoint = point + UnityEngine.Random.insideUnitCircle / 2;
        _agent.SetDestination(randomPoint);
        _target = null;


        if (transform.position.x < randomPoint.x ^ _isFacingRight)
        {
            _shouldFlip = true;
            //Flip();
        }

    }

    public void SetTargetNull()
    {
        _target = null;
    }

    public void FindTarget()
    {
        if (!_agent.enabled)
        {
            return;
        }
        if (!CanInteract)
        {
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _searchCollider.radius, LayerMask.GetMask("Interactable"));
        if (colliders.Length == 0)
        {
            return;
        }
        float minDistance = float.MaxValue;
        Collider2D minDistanceCollider = null;
        foreach (Collider2D collider in colliders)
        {
            if (collider.transform == transform)
            {
                continue;
            }
            if (collider == null)
            {
                return;
            }
            if (!collider.GetComponent<AInteractableEntity>().IsWithInFireRange && collider.GetComponent<AInteractableEntity>().InteractType != InteractType.Enemy)
            {
                continue;
            }
            //if (!_tool.IsInteractable(collider.GetComponent<AInteractableEntity>().InteractType))
            if (!_unitTool.CurrentTool.IsInteractable(collider.GetComponent<AInteractableEntity>().InteractType))
            {
                continue;
            }
            float distance = Vector2.Distance(collider.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistanceCollider = collider;
                minDistance = distance;
            }
        }

        if (minDistanceCollider == null)
        {
            return;
        }
        Debug.Log(minDistanceCollider.gameObject);

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
        if (_agent.enabled)
        {
            _agent.isStopped = true;
        }
    }
    public void ResumeNavMeshAgent()
    {
        if (_agent.enabled)
        {
            _agent.isStopped = false;
        }
    }

    public override void TakeDamage(int amount, bool isHeal)
    {
        base.TakeDamage(amount, isHeal);
        if (Health >= _unitStat.MaxHealth.Value)
        {
            Health = _unitStat.MaxHealth.Value;
        }
        if (!isHeal)
        {
            CancelInvoke(nameof(Recover));
            _materialPropertyBlock.SetFloat("_HitEffectBlend", 1);
            _renderer.SetPropertyBlock(_materialPropertyBlock);
            Invoke(nameof(Recover), 0.2f);
        }
        HPBar.value = Health;
        HPBar.gameObject.SetActive(true);
        CancelInvoke(nameof(HideHPBar));
        Invoke(nameof(HideHPBar), 3);

        OnDamaged?.Invoke(this);
        if (CanInteract)
        {
            return;
        }
        GetComponent<CircleCollider2D>().enabled = false;
        _agent.enabled = false;
        _target = null;
        //_toolAnimator.SetBool("IsInteracting", false);
        _toolAnimator.Play("Idle");
        _animator.SetTrigger("Die");
        ToolManager.Instance.RemoveCurrentToolCount(_unitTool.CurrentTool.ToolType, 1);

        //시체 사라진 후가 아닌 쓰러졌을 때 바로?
        UnitSelectionManager.Instance.Deselect(this);
    }

    public void Recover()
    {
        _materialPropertyBlock.SetFloat("_HitEffectBlend", 0);
        _renderer.SetPropertyBlock(_materialPropertyBlock);
    }
    public void HideHPBar()
    {
        HPBar.gameObject.SetActive(false);
    }

    public void SetTool(ATool tool)
    {
        _unitTool.SetTool(tool);
        if (_target != null && !_unitTool.CurrentTool.IsInteractable(_target.InteractType))
        {
            SetTargetNull();
            _agent.ResetPath();
        }
        if (_unitTool.CurrentTool.ToolType == ToolType.Sword)
        {
            Debug.Log($"셋툴: {_unitTool.CurrentTool.ToolType}");
            FindTarget();
        }
    }

    //Animation Event
    public void DestroyThis()
    {
        UnitManager.Instance.DestroyUnit(this);
    }
}