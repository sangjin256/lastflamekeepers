public class Tree : AResource
{
    public Tree(ResourceType resourceType, string name, int durability, int amountToHit,
                int outputAmountPerExtract, InventoryResourceType outputType, float respawnTime)
        : base(resourceType, name, durability, amountToHit, outputAmountPerExtract, outputType, respawnTime)
    {
        // Constructor logic can be added here if needed
    }
}
