using DG.Tweening;
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

    [Header("건물 비활성화 시 색")]
    public Color DeActiveColor = new Color(1f, 1f, 1f, 0.4f);
    public float RedColorDuration = 0.1f;

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
        if (!_building.IsActive)
        {
            Color originalColor = _popUpButton.image.color;

            // 붉은색으로 깜빡이는 애니메이션
            _popUpButton.image.DOColor(Color.red, RedColorDuration)
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(() => _popUpButton.image.color = originalColor)
                .SetUpdate(true);

            return;
        }
        UIManager.Instance.OpenBuildingDetail(_building, true);
    }

    public void Refresh()
    {
        Color targetColor = _building.IsActive ? Color.white : DeActiveColor;
        _popUpButton.image.color = targetColor;

        BuildingImage.sprite = _building.GetCurrentLevelSprite();
        BuildingImage.color = targetColor;

        _levelText.text = $"Level {_building.Level + 1}";
        _levelText.color = targetColor;
        
    }
}