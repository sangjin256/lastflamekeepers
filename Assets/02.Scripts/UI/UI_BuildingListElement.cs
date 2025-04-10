using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuildingListElement : MonoBehaviour
{
    private ABaseBuilding _building;
    public ABaseBuilding Building => _building;

    [Header("빌딩 이미지")]
    public Image BuildingImage;

    // 레벨 텍스트
    private TextMeshProUGUI _levelText;

    // 팝업 용 버튼
    private Button _popUpButton;

    private void Awake()
    {
        _levelText = GetComponentInChildren<TextMeshProUGUI>();
        _popUpButton = GetComponentInChildren<Button>();
    }

    public void SetBuilding(ABaseBuilding building)
    {
        _building = building;

        BuildingImage.sprite = building.GetCurrentLevelSprite();
        _levelText.text = $"Level {building.Level + 1}";

        _popUpButton.onClick.AddListener(() => OpenBuildingDetailPopUp());
    }

    private void OpenBuildingDetailPopUp()
    {
        UIManager.Instance.OpenBuildingDetail(_building, true);
    }

    public void Refresh()
    {
        BuildingImage.sprite = _building.GetCurrentLevelSprite();
        _levelText.text = $"Level {_building.Level + 1}";

    }
}