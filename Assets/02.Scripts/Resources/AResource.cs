public abstract class AResource
{
    public ResourceType ResourceType;
    public string Name;
    public int Durability;
    public int AmountToHit;

    public abstract void Interact();
}
