using System.Collections.Generic;
using UnityEngine;

public abstract class ABaseBuilding : MonoBehaviour
{
    // 빌딩 타입
    private BuildingType _buildingType;
    public BuildingType BuildingType => _buildingType;
    protected abstract BuildingType DefineType();

    // 빌딩 데이터
    protected BuildData _buildData;

    // 빌딩 활성화 여부
    protected bool _isActive = true;

    // 레벨
    private int _maxLevel;
    private int _level = 0;
    public int Level => _level;

    // 업그레이드에 필요한 자원
    private List<List<int>> _requiredResourcesList;

    // 색상 변경
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _buildingType = DefineType();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Initialize(BuildData buildData)
    {
        _buildData = buildData;
        _isActive = true;

        _requiredResourcesList = new List<List<int>>();
        _maxLevel = buildData.Upgrade_AddValueList.Count;
        for (int i = 0; i < _maxLevel; i++)
        {
            _requiredResourcesList.Add(new List<int> { buildData.Upgrade_WoodCountList[i], buildData.Upgrade_StoneCountList[i] });
        }
    }

    public void ChangeColor(Color color)
    {
        _spriteRenderer.color = color;
    }
    
    public virtual bool UpgradeBuilding()
    {
        if (_level >= _maxLevel - 1)
        {
            Debug.Log("MaxLevel");
            return false;
        }

        if (InventoryResourceManager.Instance.TryAddCurrentResourceCount(InventoryResourceType.Wood, _requiredResourcesList[_level][0]))
        {
            Debug.Log("나무 자원 부족");
            return false;
        }

        if (InventoryResourceManager.Instance.TryAddCurrentResourceCount(InventoryResourceType.Stone, _requiredResourcesList[_level][1]))
        {
            Debug.Log("돌 자원 부족");
            return false;
        }
        _level += 1;

        BuildManager.Instance.UpgradeBuilding(this);

        return true;
    }

    public List<int> GetCurrentRequiredResourcesList()
    {
        if (_level > _maxLevel)
        {
            return null;
        }

        return _requiredResourcesList[_level];
    }

    public abstract void SetActive(bool active);
}
