using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitData : MonoBehaviour
{
    public Image UnitImage;
    public TextMeshProUGUI UnitNameText;
    public TextMeshProUGUI UnitMaxHealthText;
    public TextMeshProUGUI UnitDamageText;
    public TextMeshProUGUI UnitAttackSpeedText;
    public TextMeshProUGUI UnitMoveSpeedText;
    public TextMeshProUGUI UnitWoodSpeedText;
    public TextMeshProUGUI UnitRockSpeedText;
    public GameObject UnitPositiveFeature;
    public GameObject UnitNegativeFeature;

    public List<TextMeshProUGUI> _unitPositiveFeatureTextList;
    public List<TextMeshProUGUI> _unitNegativeFeatureTextList;

    public void Initialize(Unit unit)
    {

        UnitStat unitStat = unit.GetComponent<UnitStat>();
        UnitImage.sprite = unit.GetComponent<SpriteRenderer>().sprite;
        UnitNameText.text = unitStat.Name;

        UnitMaxHealthText.text = unitStat.MaxHealth.AddedValue != 0
            ? unitStat.MaxHealth.AddedValue > 0
            ? $"{unitStat.MaxHealth.Value} <color=#57AC4B>({unitStat.MaxHealth.AddedValue})</color>"
            : $"{unitStat.MaxHealth.Value} <color=#B13E38>({unitStat.MaxHealth.AddedValue})</color>"
            : $"{unitStat.MaxHealth.Value}";
        UnitDamageText.text = unitStat.Damage.AddedValue != 0
            ? unitStat.Damage.AddedValue > 0
            ? $"{unitStat.Damage.Value} <color=#57AC4B>({unitStat.Damage.AddedValue})</color>"
            : $"{unitStat.Damage.Value} <color=#B13E38>({unitStat.Damage.AddedValue})</color>"
            : $"{unitStat.Damage.Value}";
        UnitAttackSpeedText.text = unitStat.AttackSpeed.AddedValue != 0
            ? unitStat.AttackSpeed.AddedValue > 0
            ? $"{unitStat.AttackSpeed.Value} <color=#57AC4B>({unitStat.AttackSpeed.AddedValue})</color>"
            : $"{unitStat.AttackSpeed.Value} <color=#B13E38>({unitStat.AttackSpeed.AddedValue})</color>"
            : $"{unitStat.AttackSpeed.Value}";
        UnitMoveSpeedText.text = unitStat.MoveSpeed.AddedValue != 0
            ? unitStat.MoveSpeed.AddedValue > 0
            ? $"{unitStat.MoveSpeed.Value} <color=#57AC4B>({unitStat.MoveSpeed.AddedValue})</color>"
            : $"{unitStat.MoveSpeed.Value} <color=#B13E38>({unitStat.MoveSpeed.AddedValue})</color>"
            : $"{unitStat.MoveSpeed.Value}";
        UnitWoodSpeedText.text = unitStat.WoodSpeed.AddedValue != 0
            ? unitStat.WoodSpeed.AddedValue > 0
            ? $"{unitStat.WoodSpeed.Value} <color=#57AC4B>({unitStat.WoodSpeed.AddedValue})</color>"
            : $"{unitStat.WoodSpeed.Value} <color=#B13E38>({unitStat.WoodSpeed.AddedValue})</color>"
            : $"{unitStat.WoodSpeed.Value}";
        UnitRockSpeedText.text = unitStat.RockSpeed.AddedValue != 0
            ? unitStat.RockSpeed.AddedValue > 0
            ? $"{unitStat.RockSpeed.Value} <color=#57AC4B>({unitStat.RockSpeed.AddedValue})</color>"
            : $"{unitStat.RockSpeed.Value} <color=#B13E38>({unitStat.RockSpeed.AddedValue})</color>"
            : $"{unitStat.RockSpeed.Value}";

        _unitPositiveFeatureTextList = new List<TextMeshProUGUI>(UnitPositiveFeature.GetComponentsInChildren<TextMeshProUGUI>());
        _unitNegativeFeatureTextList = new List<TextMeshProUGUI>(UnitNegativeFeature.GetComponentsInChildren<TextMeshProUGUI>());

        int i = 0;
        for (i = 0; i< unitStat.PositiveFeatureList.Count; i++)
        {
            _unitPositiveFeatureTextList[i].text = unitStat.PositiveFeatureList[i].FeatureName;
        }
        for (; i < _unitPositiveFeatureTextList.Count; i++)
        {
            _unitPositiveFeatureTextList[i].text = string.Empty;
        }

        i = 0;
        for (i = 0; i < unitStat.NegativeFeatureList.Count; i++)
        {
            _unitNegativeFeatureTextList[i].text = unitStat.NegativeFeatureList[i].FeatureName;
        }
        for (; i < _unitNegativeFeatureTextList.Count; i++)
        {
            _unitNegativeFeatureTextList[i].text = string.Empty;
        }
    }


}
