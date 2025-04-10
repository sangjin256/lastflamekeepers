using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LaboratoryDetail : MonoBehaviour
{
    protected Laboratory _laboratory;

    [Header("건물 정보")]
    public Image BuildingImage;
    public TextMeshProUGUI BuildingDescriptionText;

    [Header("업그레이드 텍스트")]
    public List<TextMeshProUGUI> UpgradeResourceWoodTextList;
    public List<TextMeshProUGUI> UpgradeResourceStoneTextList;

    [Header("업그레이드 버튼")]
    public List<Button> UpgradeButton;

    public void Initialize(ABaseBuilding building)
    {
        _laboratory = (Laboratory)building;

        if (_laboratory == null )
        {
            Debug.Log("이 건물은 연구소가 아닙니다.");
            return;
        }

        BuildingImage.sprite = building.GetCurrentLevelSprite();

        BuildingDescriptionText.text = $"{building.GetBuildingDescription()}";

        RefreshUI();
    }

    public void OnClickUpgradeButton(int toolTypeNumber)
    {
        _laboratory.UpgradeTool(toolTypeNumber);
    }

    private void RefreshUI()
    {
        
    }
}
