using System.Collections.Generic;
using UnityEngine;

public abstract class ABaseBuilding : MonoBehaviour
{
    // 빌딩 타입
    private BuildingType _buildingType;
    public BuildingType BuildingType => _buildingType;
    protected abstract BuildingType DefineType();

    // 업그레이드 별 건물 이미지
    [Header("건물 레벨별 이미지")]
    [SerializeField] private List<Sprite> _levelBuildingImage;

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

        // 기본 이미지 설정
        _spriteRenderer.sprite = _levelBuildingImage[0];
    }

    public void ChangeColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    public void OnClikcUpgradeButton()
    {
        UpgradeBuilding();
    }

    public virtual bool UpgradeBuilding()
    {
        if (_level >= _maxLevel)
        {
            Debug.Log("MaxLevel");
            return false;
        }

        Debug.Log($"필요 자원 나무 : {_requiredResourcesList[_level][0]}, 돌 : {_requiredResourcesList[_level][0]}");

        if (!InventoryResourceManager.Instance.TryRemoveCurrentResourceCount(InventoryResourceType.Wood, _requiredResourcesList[_level][0]))
        {
            Debug.Log("나무 자원 부족");
            return false;
        }

        if (!InventoryResourceManager.Instance.TryRemoveCurrentResourceCount(InventoryResourceType.Stone, _requiredResourcesList[_level][1]))
        {
            Debug.Log("돌 자원 부족");
            return false;
        }
        _level += 1;

        BuildManager.Instance.UpgradeBuilding(this);

        if (_level < _levelBuildingImage.Count)
        {
            _spriteRenderer.sprite = _levelBuildingImage[_level];
        }

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
