public class Rock : AResource
{
    public override void Initialize(ResourceType resourceType)
    {
        FieldResourceData resource = ResourceManager.Instance.GetResource(resourceType);
        ResourceType = resource.ResourceType;
        Name = resource.Name;
        Durability = resource.Durability;
        AmountToHit = resource.AmountToHit;
        OutputAmountPerExtract = resource.OutputAmountPerExtract;
        OutputType = resource.OutputType;
        RespawnTime = resource.RespawnTime;
        _health = Durability;

        _interactType = InteractType.Rock;
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
