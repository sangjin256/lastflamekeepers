using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public List<Button> UpgradeButtonList;

    [Header("Level 텍스트")]
    public List<TextMeshProUGUI> ToolLevelTextList;

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

        RefreshUI();
    }

    private void RefreshUI()
    {
        for (int i = 0; i < UpgradeResourceWoodTextList.Count; i++)
        {
            int level = ToolManager.Instance.GetCurrentLevel((ToolType)i + 1);

            ToolLevelTextList[i].text = $"Lv. {level}";

            int woodCount = ToolManager.Instance.GetWoodCountToUpgrade((ToolType)i + 1);
            if (woodCount == -1)
            {
                ToolLevelTextList[i].text = "Lv.Max";
                UpgradeResourceWoodTextList[i].text = $"";
                UpgradeResourceStoneTextList[i].text = $"";
                UpgradeButtonList[i].gameObject.SetActive(false);
                continue;
            }
            int stoneCount = ToolManager.Instance.GetStoneCountToUpgrade((ToolType)i + 1);
            

            UpgradeResourceWoodTextList[i].text = $"X {woodCount}";
            UpgradeResourceStoneTextList[i].text = $"X {stoneCount}";
            UpgradeButtonList[i].gameObject.SetActive(true);
        }
    }
}
