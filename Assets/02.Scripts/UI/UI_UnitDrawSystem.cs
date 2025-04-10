using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitDrawSystem : MonoBehaviour
{
    public List<UI_UnitData> UnitChoiceList;
    private ToggleGroup _toggleGroup;
    private Toggle[] _toggleArray;
    public Button AcceptButton;
    public Button DeclineButton;
    public GameObject[] SelectedList;
    private int _selectedIndex;

    List<Unit> randomUnitList = new List<Unit>();



    private void Awake()
    {
        _toggleArray = GetComponentsInChildren<Toggle>();
        _toggleGroup = GetComponent<ToggleGroup>();
    }
    private void OnEnable()
    {
        Debug.Log("ZDFZSG");
        for (int i = 0; i < 3; i++)
        {
            randomUnitList.Add(UnitManager.Instance.GenerateRandomUnit());
            UnitChoiceList[i].Initialize(randomUnitList[i]);
        }

        foreach (GameObject selected in SelectedList)
        {
            selected.SetActive(false);
        }
    }

    
    private void Update()
    {
        UpdateButtonByToggle();
    }

    public void UpdateButtonByToggle()
    {
        for (int i = 0; i < _toggleArray.Length; i++)
        {
            if (_toggleArray[i].isOn)
            {
                _selectedIndex = i;
                Debug.Log("zzzss");
                break;
            }
            _selectedIndex = -1;
        }

        AcceptButton.gameObject.SetActive(_selectedIndex >= 0);
        DeclineButton.gameObject.SetActive(_selectedIndex < 0);
    }
    public void SelectUnit()
    {
        if(_selectedIndex < 0)
        {
            return;
        }
        UnitManager.Instance.SpawnUnit(randomUnitList[_selectedIndex], Vector2.zero);
        randomUnitList.Remove(randomUnitList[_selectedIndex]);
        
        for(int i = randomUnitList.Count - 1; i>= 0; i--)
        {
            UnitManager.Instance.DestroyUnit(randomUnitList[i]);
        }

        randomUnitList.Clear();
        _selectedIndex = -1;
        _toggleGroup.SetAllTogglesOff();
        _toggleGroup.gameObject.SetActive(false);
    }

    public void Decline()
    {
        if(_selectedIndex >= 0)
        {
            return;
        }
        for (int i = randomUnitList.Count - 1; i >= 0; i--)
        {
            UnitManager.Instance.DestroyUnit(randomUnitList[i]);
        }

        randomUnitList.Clear();
        _selectedIndex = -1;

        _toggleGroup.gameObject.SetActive(false);
    }


}
