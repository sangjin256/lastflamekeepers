using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitListElement : MonoBehaviour
{
    private Unit _unit;
    public Unit Unit
    {
        get
        {
            return _unit;
        }
        set
        {
            _unit = value;
        }
    }

    public Image UnitImage;
    public Image ToolImage;
    public TextMeshProUGUI UnitName;
    public Slider UnitHealthSlider;

    public Sprite[] NoneSpriteArray;
    public Sprite[] SwordSpriteArray;
    public Sprite[] AxeSpriteArray;
    public Sprite[] PickAxeSpriteArray;
    public Sprite[] CrossSpriteArray;



    private Sprite[][] ToolSpriteArray;
    //체력바
    public void Initialize()
    {
        ToolSpriteArray = new Sprite[][] {NoneSpriteArray, SwordSpriteArray, AxeSpriteArray, PickAxeSpriteArray, CrossSpriteArray};
        UnitImage.sprite = Unit.GetComponent<SpriteRenderer>().sprite;
        ToolImage.sprite = ToolSpriteArray[(int)_unit.UnitTool.CurrentTool.ToolType][(int)_unit.UnitTool.CurrentTool.UpgradeLevel];
        UnitName.text = Unit.GetComponent<UnitStat>().Name;
        UnitHealthSlider.maxValue = Unit.GetComponent<UnitStat>().MaxHealth.Value;
        UnitHealthSlider.value = UnitHealthSlider.maxValue;
    }
    public void Refresh()
    {
        Debug.Log("리프레시");
        ToolImage.sprite = ToolSpriteArray[(int)_unit.UnitTool.CurrentTool.ToolType][(int)_unit.UnitTool.CurrentTool.UpgradeLevel];
        UnitHealthSlider.value = _unit.Health;
        if(_unit.Health < 0)
        {
            Destroy(this);
        }
    }

}
