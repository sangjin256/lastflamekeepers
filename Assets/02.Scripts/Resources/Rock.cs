using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

public class Rock : AResource
{
    public Renderer Renderer;
    public MaterialPropertyBlock _materialPropertyBlock;
    private NavMeshObstacle _navMeshObstacle;
    private UnityEngine.Vector2 centerPos;
    public override void Initialize(ResourceType resourceType)
    {
        FieldResourceData resource = ResourceManager.Instance.GetResource(resourceType);
        ResourceType = resource.ResourceType;
        Name = resource.Name;
        Durability = resource.Durability;
        Health = Durability;
        AmountToHit = resource.AmountToHit;
        OutputAmountPerExtract = resource.OutputAmountPerExtract;
        OutputType = resource.OutputType;
        RespawnTime = resource.RespawnTime;
        _health = Durability;

        _interactType = InteractType.Rock;
        IsWithInFireRange = true;
        FireManager.Instance.OnFireRangeChanged += CheckWithInRange;


    }

    private void Start()
    {
        Renderer = GetComponent<Renderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
        centerPos = transform.position + _navMeshObstacle.center;

        CheckWithInRange();
    }
    public void CheckWithInRange()
    {
        if (IsWithInFireRange != FireManager.Instance.IsUnitWithInFireRange(centerPos))
        {
            IsWithInFireRange = !IsWithInFireRange;

            //_navMeshObstacle.carving = IsWithInFireRange;
            _navMeshObstacle.enabled = IsWithInFireRange;
        }
    }
    public override void TakeDamage(int amount, bool isHeal)
    {
        base.TakeDamage(amount, isHeal);
        _materialPropertyBlock.SetFloat("_ShakeUvX", 0.3f);
        CancelInvoke(nameof(Recover));
        Renderer.SetPropertyBlock(_materialPropertyBlock);
        Invoke(nameof(Recover), 0.2f);
        HitCount++;
        if (HitCount >= AmountToHit)
        {
            HitCount = 0;
            InventoryResourceManager.Instance.TryAddCurrentResourceCount(OutputType, OutputAmountPerExtract);
        }
        if (CanInteract)
        {

            AudioManager.Instance.PlayResourceAudio(ResourceType, transform.position, false);
            return;
        }

        AudioManager.Instance.PlayResourceAudio(ResourceType, transform.position, true);
        gameObject.SetActive(false);
    }

    public void Recover()
    {
        _materialPropertyBlock.SetFloat("_ShakeUvX", 0);
        Renderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
