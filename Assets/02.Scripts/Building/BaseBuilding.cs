using UnityEngine;

public abstract class BaseBuilding : MonoBehaviour
{
    // 빌딩 타입
    private BuildingType _buildingType;
    public BuildingType BuildingType => _buildingType;
    protected abstract BuildingType DefineType();

    // 빌딩 데이터
    protected BuildData _buildData;

    // 레벨 
    private int _level = 0;
    public int Level => _level;

    // 색상 변경
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _buildingType = DefineType();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void SetBuildData(BuildData buildData)
    {
        _buildData = buildData;
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
}
