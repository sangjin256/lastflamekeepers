using System.Collections.Generic;

// 게임 내 소모 자원 관리
// 필드 자원 : 목재(Wood), 돌 자원(Stone), 잿가루(Ash)

public class InventoryResourceManager : BehaviourSingleton<InventoryResourceManager>
{
    private Dictionary<InventoryResourceType, int> _currentResource;
    private Dictionary<InventoryResourceType, int> _maxResource;

    public bool TryAdd(InventoryResourceType type, int amount)
    {
        if (amount < 0)
            return false;
        if (_currentResource[type] + amount > _maxResource[type])
            return false;
        _currentResource[type] += amount;
        return true;
    }
    public bool TryConsume(InventoryResourceType type, int amount)
    {
        if (amount < 0)
            return false;
        if (_currentResource[type] - amount < 0)
            return false;
        _currentResource[type] -= amount;
        return true;
    }
}
