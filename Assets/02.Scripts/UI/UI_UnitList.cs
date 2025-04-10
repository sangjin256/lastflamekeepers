using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_UnitList : MonoBehaviour
{
    [Header("프리팹들의 부모 오브젝트")]
    public GameObject UnitPrefab_Layout;

    [Header("빌딩 리스트 엘리먼트 프리팹")]
    public GameObject UnitListUnitPrefab;
    private Dictionary<ToolType, List<UI_UnitListElement>> _unitElements;

    private void Start()
    {
        Debug.Log("ㅇㅇ");

        UnitManager.Instance.OnUnitListChanged += Refresh;
        ToolManager.Instance.OnToolChanged += ReRocate;
        _unitElements = new Dictionary<ToolType, List<UI_UnitListElement>>();
        for (int i = 0; i <= (int)ToolType.Medicine; i++)
        {
            _unitElements[(ToolType)i] = new List<UI_UnitListElement>();
        }
    }

    private void Refresh(Unit unit)
    {
        Debug.Log("ㄴㄴㄴ");

        ToolType unitToolType = unit.UnitTool.CurrentTool.ToolType;
        List<UI_UnitListElement> unitElementList = _unitElements[unitToolType];
        for (int i = 0; i < unitElementList.Count; i++)
        {
            //갱신
            //Equals?
            if (unitElementList[i].Unit == unit)
            {
                unitElementList[i].Refresh();
                return;
            }
        }
        Debug.Log("ㅋㄹㄹ");

        //추가
        GameObject unitElementObject = Instantiate(UnitListUnitPrefab);
        UI_UnitListElement unitElement = unitElementObject.GetComponent<UI_UnitListElement>();

        if (unitElement == null)
        {
            return;
        }

        Debug.Log("들어옴");
        unitElement.transform.SetParent(UnitPrefab_Layout.transform);
        unitElement.transform.localScale = Vector3.one;
        unitElement.transform.position = Vector3.zero;

        unitElement.Unit = unit;
        unitElementList.Add(unitElement);
        unit.OnDamaged += Refresh;

        unitElement.Initialize();
    }

    public void ReRocate(Unit unit)
    {
        ToolType prevUnitToolType = unit.gameObject.GetComponentInChildren<UnitTool>().PrevTool.ToolType;
        ToolType currentUnitToolType = unit.gameObject.GetComponentInChildren<UnitTool>().CurrentTool.ToolType;

        if (prevUnitToolType == currentUnitToolType)
        {
            return;
        }
        List<UI_UnitListElement> currentList = _unitElements[prevUnitToolType];
        List<UI_UnitListElement> destinationList = _unitElements[currentUnitToolType];


        for (int i = 0; i < currentList.Count; i++)
        {
            //Equals?
            if (currentList[i].Unit == unit)
            {
                //currentList[i].transform.SetParent(UnitPrefab_Layout.transform);
                currentList[i].Refresh();
                destinationList.Add(currentList[i]);
                currentList.RemoveAt(i);
                
                return;
            }
            Debug.LogError("도구 전환 but 재배치 실패");
        }
    }

}
