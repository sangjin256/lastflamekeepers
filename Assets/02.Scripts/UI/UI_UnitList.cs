using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class UI_UnitList : MonoBehaviour
{
    [Header("프리팹들의 부모 오브젝트")]
    public GameObject UnitPrefab_Layout;

    [Header("빌딩 리스트 엘리먼트 프리팹")]
    public GameObject UnitListUnitPrefab;
    private Dictionary<ToolType, List<UI_UnitListElement>> _unitElements;

    [Header("필터링 버튼")]
    [SerializeField] private List<Button> _filterButtonList;

    public UI_UnitData UnitDetail;
    public void Initialize()
    {
        UnitManager.Instance.OnUnitListChanged += Refresh;
        ToolManager.Instance.OnToolChanged += ReRocate;
        _unitElements = new Dictionary<ToolType, List<UI_UnitListElement>>();
        for (int i = 0; i <= (int)ToolType.Medicine; i++)
        {
            _unitElements[(ToolType)i] = new List<UI_UnitListElement>();
        }

        _filterButtonList[0].onClick.AddListener(() => UpdateFilter(-1));
        //if (_filterButtonList.Count != (int)(ToolType.Medicine) + 2)
        //{
        //    Debug.Log("UI_BuildingList의 버튼을 할당했는지 체크하시오");
        //    return;
        //}

        for (int i = 0; i <= (int)ToolType.Medicine; i++)
        {
            int index = i;
            _filterButtonList[i + 1].onClick.AddListener(() => UpdateFilter(index));
        }
    }
    private void OnEnable()
    {
        if (_unitElements == null)
        {
            return;
        }
        UpdateFilter(-1);
    }
    private void Refresh(Unit unit)
    {
        ToolType unitToolType = unit.UnitTool.CurrentTool.ToolType;
        List<UI_UnitListElement> unitElementList = _unitElements[unitToolType];
        for (int i = 0; i < unitElementList.Count; i++)
        {
            //갱신
            //Equals?
            if (unitElementList[i].Unit == unit)
            {
                unitElementList[i].Refresh();
                if (unitElementList[i].UnitHealthSlider.value <= 0)
                {
                    unit.OnDamaged -= Refresh;
                    unitElementList.RemoveAt(i);
                }
                return;
            }
        }

        //추가
        GameObject unitElementObject = Instantiate(UnitListUnitPrefab);
        UI_UnitListElement unitElement = unitElementObject.GetComponent<UI_UnitListElement>();
        //unitElementObject.GetComponent<Button>().onClick.AddListener(() => UnitDetail.Initialize(unit));
        //unitElementObject.GetComponent<Button>().onClick.AddListener(() => UnitDetail.transform.parent.gameObject.SetActive(true));
        unitElementObject.GetComponent<Button>().onClick.AddListener(() => UIManager.Instance.OpenUnitDetail(unit, true));

        if (unitElement == null)
        {
            return;
        }

        unitElement.transform.SetParent(UnitPrefab_Layout.transform);
        unitElement.transform.localScale = Vector3.one;
        unitElement.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

        unitElement.Unit = unit;
        unitElementList.Add(unitElement);
        unit.OnDamaged += Refresh;
        ToolManager.Instance.OnToolChanged += Refresh;
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

        Debug.Log(prevUnitToolType + " " + currentUnitToolType);

        for (int i = 0; i < currentList.Count; i++)
        {
            //Equals?
            if (currentList[i].Unit == unit)
            {
                Debug.Log(prevUnitToolType + " " + currentUnitToolType);

                //currentList[i].transform.SetParent(UnitPrefab_Layout.transform);
                currentList[i].Refresh();
                destinationList.Add(currentList[i]);
                currentList.RemoveAt(i);
                
                return;
            }
        }
        Debug.LogError("도구 전환 but 재배치 실패");
    }

    public void UpdateFilter(int toolTypeNumber)
    {
        for (int i = 0; i <= (int)ToolType.Medicine; i++)
        {
            bool isActive = false;
            if (toolTypeNumber == -1 || toolTypeNumber == i)
            {
                isActive = true;
            }

            foreach (UI_UnitListElement unitNode in _unitElements[(ToolType)i])
            {
                unitNode.gameObject.SetActive(isActive);
            }
        }
    }
}
