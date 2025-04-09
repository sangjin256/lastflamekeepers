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
            ? $"{unitStat.MaxHealth.BaisicValue}{unitStat.MaxHealth.AddedValue.ToString("(+#);(-#)")}"
            : $"{unitStat.MaxHealth.BaisicValue}";
        UnitDamageText.text = unitStat.Damage.AddedValue != 0
            ? $"{unitStat.Damage.BaisicValue}{unitStat.Damage.AddedValue.ToString("(+#);(-#)")}"
            : $"{unitStat.Damage.BaisicValue}";
        UnitAttackSpeedText.text = unitStat.AttackSpeed.AddedValue != 0 
            ? $"{unitStat.AttackSpeed.BaisicValue}{unitStat.AttackSpeed.AddedValue.ToString("(+#);(-#)")}" 
            : $"{unitStat.AttackSpeed.BaisicValue}";
        UnitMoveSpeedText.text = unitStat.MoveSpeed.AddedValue != 0 
            ? $"{unitStat.MoveSpeed.BaisicValue}{unitStat.MoveSpeed.AddedValue.ToString("(+#);(-#)")}" 
            : $"{unitStat.MoveSpeed.BaisicValue  }";
        UnitWoodSpeedText.text = unitStat.WoodSpeed.AddedValue != 0 
            ? $"{unitStat.WoodSpeed.BaisicValue}{unitStat.WoodSpeed.AddedValue.ToString("(+#);(-#)")}" 
            : $"{unitStat.WoodSpeed.BaisicValue}";
        UnitRockSpeedText.text = unitStat.RockSpeed.AddedValue != 0 
            ? $"{unitStat.RockSpeed.BaisicValue}{unitStat.RockSpeed.AddedValue.ToString("(+#);(-#)")}" 
            : $"{unitStat.RockSpeed.BaisicValue}";

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
