using System.Collections.Generic;
using UnityEngine;

// 게임 내 소모 자원 관리
// 필드 자원 : 목재(Wood), 돌 자원(Stone), 잿가루(Ash)

public class InventoryResourceManager : BehaviourSingleton<InventoryResourceManager>
{
    private Dictionary<InventoryResourceType, AInvenResource> _invenresourceDataDict;
    private Dictionary<InventoryResourceType, int> _currentResource;
    private Dictionary<InventoryResourceType, int> _maxResource;

    private void Awake()
    {
        Global.Instance.OnDataLoaded += LoadInvenResourceDefinitions;
    }

    public bool IsResourceFull(InteractType type)
    {
        if(type == InteractType.Tree)
        {
            if (_currentResource[InventoryResourceType.Wood] >= _maxResource[InventoryResourceType.Wood]) return true;
            else return false;
        }
        else if(type == InteractType.Rock)
        {
            if (_currentResource[InventoryResourceType.Stone] >= _maxResource[InventoryResourceType.Stone]) return true;
            else return false;
        }
        return true;
    }

    private void LoadInvenResourceDefinitions()
    {
        _invenresourceDataDict = new Dictionary<InventoryResourceType, AInvenResource>();
        _currentResource = new Dictionary<InventoryResourceType, int>();
        _maxResource = new Dictionary<InventoryResourceType, int>();

        var resourceList = DataTable.Instance.GetInventoryResourceDataList();
        foreach (var data in resourceList)
        {
            _currentResource[data.InventoryResourceType] = 0;
            _maxResource[data.InventoryResourceType] = data.MaxCount;
        }

        Debug.Log("Inventory Resource Loaded");
    }

    // 소모 자원 타입 조회
    public AInvenResource GetResource(InventoryResourceType type)
    {
        if (_invenresourceDataDict.TryGetValue(type, out var resource))
            return resource;

        Debug.LogWarning($"[InvenResourceManager] 정의되지 않은 ResourceType: {type}");
        return null;
    }

    // 현재 자원 수 조회
    public int GetCurrentResourceCount(InventoryResourceType type)
    {
        return _currentResource.TryGetValue(type, out int value) ? value : 0;
    }

    // 최대 자원 수 조회
    public int GetMaxResourceCount(InventoryResourceType type)
    {
        return _maxResource.TryGetValue(type, out int value) ? value : 0;
    }

    // 자원 수 추가 및 제거
    public bool TryAddCurrentResourceCount(InventoryResourceType type, int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning($"[InvenResourceManager] 현재 자원 수 추가량이 음수입니다: {type}");
            return false;
        }
        if (_currentResource.TryGetValue(type, out int currentAmount))
        {
            if (currentAmount + amount > _maxResource[type])
            {
                Debug.LogWarning($"[InvenResourceManager] 최대 자원 수 초과: {type}");
                return false;
            }
            _currentResource[type] += amount;
            return true;
        }
        Debug.LogWarning($"[InvenResourceManager] 정의되지 않은 ResourceType: {type}");
        return false;
    }

    public bool TryRemoveCurrentResourceCount(InventoryResourceType type, int amount)
    {
        Debug.Log($"현재 자원 수 : {_currentResource[InventoryResourceType.Wood]}, {_currentResource[InventoryResourceType.Wood]}");
        if (_currentResource.TryGetValue(type, out int currentAmount))
        {
            if (currentAmount - amount < 0)
            {
                Debug.LogWarning($"[InvenResourceManager] 현재 자원 수 부족: {type}");
                return false;
            }
            _currentResource[type] -= amount;
            return true;
        }
        Debug.LogWarning($"[InvenResourceManager] 정의되지 않은 ResourceType: {type}");
        return false;
    }

    public bool TryAddMaxResourceCount(InventoryResourceType type, int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning($"[InvenResourceManager] 최대 자원 수 추가량이 음수입니다: {type}");
            return false;
        }
        if (_maxResource.TryGetValue(type, out int currentMax))
        {
            _maxResource[type] += amount;
            return true;
        }
        Debug.LogWarning($"[InvenResourceManager] 정의되지 않은 ResourceType: {type}");
        return false;
    }

    public bool TryRemoveMaxResourceCount(InventoryResourceType type, int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning($"[InvenResourceManager] 최대 자원 수 감소량이 음수입니다: {type}");
            return false;
        }

        if (_maxResource.TryGetValue(type, out int currentMaxAmount))
        {
            if (currentMaxAmount - amount < 0)
            {
                Debug.LogWarning($"[InvenResourceManager] 자원 수 부족: {type}");
                return false;
            }

            _maxResource[type] = Mathf.Max(currentMaxAmount - amount, 0);
            return true;
        }

        Debug.LogWarning($"[InvenResourceManager] 정의되지 않은 ResourceType: {type}");
        return false;
    }

}
