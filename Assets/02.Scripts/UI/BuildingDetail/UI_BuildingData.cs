using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuildingData : MonoBehaviour
{
    protected ABaseBuilding _building;

    [Header("건물 정보")]
    public Image BuildingImage;
    public TextMeshProUGUI BuildingLevelText;
    public TextMeshProUGUI BuildingDescriptionText;

    [Header("업그레이드 텍스트")]
    public TextMeshProUGUI UpgradeResourceWoodText;
    public TextMeshProUGUI UpgradeResourceStoneText;

    [Header("업그레이드 버튼")]
    public Button UpgradeButton;

    public virtual void Initialize(ABaseBuilding building)
    {
        _building = building;

        BuildingImage.sprite = building.GetCurrentLevelSprite();

        BuildingDescriptionText.text = $"{building.GetBuildingDescription()}";

        RefreshUI();
    }

    public void OnClickUpgradeButton()
    {
        bool upgradeSuccess = _building.UpgradeBuilding();

        if (upgradeSuccess)
        {
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        BuildingLevelText.text = $"Level {_building.Level}";
        
        if (_building.IsMaxLevel)
        {
            UpgradeButton.gameObject.SetActive(false);
            UpgradeResourceWoodText.text = $"";
            UpgradeResourceStoneText.text = $"";
            return;
        }

        List<int> requiredResources = _building.GetCurrentRequiredResourcesList();
        UpgradeResourceWoodText.text = $"X {requiredResources[0]}";
        UpgradeResourceStoneText.text = $"X {requiredResources[1]}";
    }
}
