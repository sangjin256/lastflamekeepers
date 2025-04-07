public abstract class AInvenResource
{
    public InventoryResourceType InventoryResourceType;
    public string Name;
    public int MaxCount;
    public string Description;

    public AInvenResource(InventoryResourceType inventoryResourceType, string name, int maxCount, string description)
    {
        InventoryResourceType = inventoryResourceType;
        Name = name;
        MaxCount = maxCount;
        Description = description;
    }
}
