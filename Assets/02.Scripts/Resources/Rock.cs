using System.Numerics;
using UnityEngine.AI;

public class Rock : AResource
{
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
    private NavMeshObstacle _navMeshObstacle;
    private UnityEngine.Vector2 centerPos;
    private void Start()
    {
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
        centerPos = transform.position + _navMeshObstacle.center;

        CheckWithInRange();
    }
    public void CheckWithInRange()
    {
        if (IsWithInFireRange != FireManager.Instance.IsWithInFireRange(centerPos))
        {
            IsWithInFireRange = !IsWithInFireRange;
        }
    }
    public override void TakeDamage(int amount, bool isHeal)
    {
        base.TakeDamage(amount, isHeal);
        HitCount++;
        if (HitCount >= AmountToHit)
        {
            HitCount = 0;
            InventoryResourceManager.Instance.TryAddCurrentResourceCount(OutputType, OutputAmountPerExtract);
        }
        if (CanInteract) return;

        gameObject.SetActive(false);
    }
}
