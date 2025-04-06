using UnityEngine;

public class BaseBuilding : MonoBehaviour
{
    public BuildingType BuildingType;
    private BuildData _buildData;
    private int _level = 0;
    public int Level => _level;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetBuildData(BuildData buildData)
    {
        _buildData = buildData;
    }

    public void ChangeColor(Color color)
    {
        _spriteRenderer.color = color;
    }
    
    public void UpgradeBuilding()
    {
        if (_level >= _buildData.Upgrade_AddValueList.Count - 1)
        {
            return;
        }

        _level += 1;
    }
}
