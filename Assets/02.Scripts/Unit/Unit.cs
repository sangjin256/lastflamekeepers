using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    //테스트용
    //public GameObject _target;
    private CircleCollider2D _interactCollider;
    private CircleCollider2D _searchCollider;

    private Animator _animator;
    private Animator _toolAnimator;
    public Animator ToolAnimator => _toolAnimator;


    private bool IsFacingRight = true;
    public bool MissTarget = false;
    public bool Runaway = false;
    public Action<Unit> OnDamaged;

    public Slider HPBar;
    public Canvas canvas;

    public Renderer Renderer;
    public MaterialPropertyBlock _materialPropertyBlock;
    private void Awake()
    {
        _unitStat = GetComponent<UnitStat>();
        _unitTool = GetComponentInChildren<UnitTool>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _interactCollider = transform.GetChild(0).GetComponent<CircleCollider2D>();
        _searchCollider = transform.GetChild(1).GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();
        _toolAnimator = transform.GetChild(2).GetComponent<Animator>();

        Renderer = GetComponent<Renderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
    }

    private void Start()
    {
        _interactType = InteractType.Unit;
        _navMeshAgent.speed = _unitStat.MoveSpeed.Value / 10f;
        _animator.SetFloat("MoveSpeed", _unitStat.MoveSpeed.Value/10f);
        Health = _unitStat.MaxHealth.Value;
        _navMeshAgent.enabled = false;
        _navMeshAgent.enabled = true;

        HPBar.maxValue = _unitStat.MaxHealth.Value;
        HPBar.value = Health;
    }
    private void Update()
    {
        if (!CanInteract)
        {
            return;
        }
        if (IsWithInFireRange != FireManager.Instance.IsUnitWithInFireRange(transform.position))
        {
            IsWithInFireRange = !IsWithInFireRange;
        }

        if (_target == null || !_target.isActiveAndEnabled)
        {
            ToolAnimator.SetBool("IsInteracting", false);
        }
        else
        {
            if (_target.CanInteract)
            {
                if (_target.IsWithInFireRange)
                {
                    _navMeshAgent.SetDestination(_target.transform.position);
                    //_rigidbody.linearVelocity = _navMeshAgent.desiredVelocity;
                    _navMeshAgent.nextPosition = transform.position;

                    
                }
                else
                {
                    _navMeshAgent.ResetPath();
                    _animator.SetBool("IsRunning", false);
                }

                if (transform.position.x < _target.transform.position.x ^ IsFacingRight)
                {
                    if (Mathf.Abs(transform.position.x - _target.transform.position.x) > 0.1f)
                    {
                        Flip();
                    }
                }
            }
        }

        if(_navMeshAgent.hasPath && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.avoidancePriority = 60;
            _navMeshAgent.ResetPath();
        }
        if (_target == null || !_target.isActiveAndEnabled)
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

        //땅바닥 찍었을 경우
        //if (_target == null && _navMeshAgent.desiredVelocity.x != 0 && (_navMeshAgent.desiredVelocity.x > 0 ^ IsFacingRight))
        //{
        //    Flip();
        //}
        if (!IsWithInFireRange)
        {
            if (!Runaway)
            {
                Runaway = true;
                _navMeshAgent.ResetPath();
                _navMeshAgent.SetDestination(Vector2.zero);
            }
            else
            {
                _navMeshAgent.SetDestination(Vector2.zero);
            }
        }
        else
        {
            if (Runaway)
            {
                Runaway = false;
                _navMeshAgent.ResetPath();
            }
        }



    }

    public void Flip()
    {
        IsFacingRight = !IsFacingRight;
        transform.Rotate(0, 180, 0);
        canvas.transform.Rotate(0, 180, 0);
    }

    public void SetTarget(AInteractableEntity interactable)
    {
        _navMeshAgent.avoidancePriority = 50;
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
        }
    }

    // TODO: 인원 수 전달해서 그에 따른 랜덤 도착 범위 설정
    public void SetTarget(Vector2 point)
    {
        _navMeshAgent.avoidancePriority = 50;

        if (!CanInteract)
        {
            return;
        }
       

        Vector2 randomPoint = point + UnityEngine.Random.insideUnitCircle / 2;
        _navMeshAgent.SetDestination(randomPoint);
        _target = null;

        if(transform.position.x < randomPoint.x ^ IsFacingRight)
        {
            Flip();
        }

    }

    public void SetTarget(Transform point)
    {
        _navMeshAgent.avoidancePriority = 50;

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
        float minDistance = float.MaxValue;
        Collider2D minDistanceCollider = null;
        foreach (Collider2D collider in colliders)
        {
            if (collider.transform == transform)
            {
                continue;
            }
            if(collider == null)
            {
                return;
            }
            if(!collider.GetComponent<AInteractableEntity>().IsWithInFireRange && collider.GetComponent<AInteractableEntity>().InteractType != InteractType.Enemy)
            {
                continue;
            }
            //if (!_tool.IsInteractable(collider.GetComponent<AInteractableEntity>().InteractType))
            if (!_unitTool.CurrentTool.IsInteractable(collider.GetComponent<AInteractableEntity>().InteractType))
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
        if(Health >= _unitStat.MaxHealth.Value)
        {
            Health = _unitStat.MaxHealth.Value;
        }
        if (!isHeal)
        {
            CancelInvoke(nameof(Recover));
            _materialPropertyBlock.SetFloat("_HitEffectBlend", 1);
            Renderer.SetPropertyBlock(_materialPropertyBlock);
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
        _target = null;
        _toolAnimator.SetBool("IsInteracting", false);
        _animator.SetTrigger("Die");
        _navMeshAgent.enabled = false;
        ToolManager.Instance.RemoveCurrentToolCount(_unitTool.CurrentTool.ToolType, 1);

        //시체 사라진 후가 아닌 쓰러졌을 때 바로?
        UnitSelectionManager.Instance.Deselect(this);
    }

    public void Recover()
    {
        _materialPropertyBlock.SetFloat("_HitEffectBlend", 0);
        Renderer.SetPropertyBlock(_materialPropertyBlock);
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
            _navMeshAgent.ResetPath();
            if (_unitTool.CurrentTool.ToolType == ToolType.Sword)
            {
                FindTarget();
            }
        }
    }

    public void SetTool(int tool)
    {
        _unitTool.SetTool(ToolManager.Instance.GetTool((ToolType)tool));
        SetTargetNull();
        _navMeshAgent.ResetPath();
        if (_unitTool.CurrentTool.ToolType == ToolType.Sword)
        {
            FindTarget();
        }
    }

    public void DestroyThis()
    {
        UnitManager.Instance.DestroyUnit(this);
    }
}