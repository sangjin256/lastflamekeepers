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
        for (int i = 0; i < buildData.Upgrade_AddValueList.Count; i++)
        {
            _requiredResourcesList[i] = new List<int>();
        }
    }

    public void ChangeColor(Color color)
    {
        _spriteRenderer.color = color;
    }
    
    public virtual void UpgradeBuilding()
    {
        if (_level >= _buildData.Upgrade_AddValueList.Count - 1)
        {
            return;
        }

        _level += 1;
    }

    public abstract void SetActive(bool active);
}
