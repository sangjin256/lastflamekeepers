using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using TMPro;

public class ClickSystem : MonoBehaviour
{
    public LayerMask Clilckable;
    public LayerMask Ground;

    private GameObject ClickedObject;

    private void Start()
    {
        
    }

    private void Update()
    {
        LeftMouseAction();
        RightMouseAction();
        MiddleMouseAction();
    }

    public void LeftMouseAction()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            List<RaycastHit2D> hitList = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1f, Clilckable).ToList();
            if (hitList.Count != 0)
            {
                #region Unit 클릭
                RaycastHit2D unitObject = hitList.Find(x => x.collider.transform.parent.CompareTag("Unit"));
                if (unitObject)
                {
                    Unit unit = unitObject.collider.transform.GetComponentInParent<Unit>();
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        UnitSelectionManager.Instance.ShiftClickSelect(unit);
                    }
                    else
                    {
                        if (UnitSelectionManager.Instance.SelectedUnitList.Contains(unit))
                        {
                            // 유닛 더블클릭
                            CameraManager.Instance.MoveToEntity(unit.transform);
                            UIManager.Instance.OpenUnitDetail(unit);
                        }
                        UnitSelectionManager.Instance.ClickSelect(unit);
                    }
                }
                #endregion
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    if (UnitSelectionManager.Instance.GetIsDragging() == false) UnitSelectionManager.Instance.DeselectAll();
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject() && UnitSelectionManager.Instance.GetIsDragging() == false)
        {
            List<RaycastHit2D> hitList = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1f, Clilckable).ToList();
            if (hitList.Count != 0)
            {
                #region 불 클릭
                RaycastHit2D FireObject = hitList.Find(x => x.collider.transform.parent.CompareTag("Fire"));
                if (FireObject)
                {
                    if (ClickedObject != null && ClickedObject.transform.parent.GetInstanceID() == FireObject.collider.transform.parent.GetInstanceID())
                    {
                        ClickedObject = null;
                        CameraManager.Instance.MoveToEntity(FireObject.collider.transform.parent);
                    }
                    else
                    {
                        ClickedObject = FireObject.collider.gameObject;
                        UIManager.Instance.OnClickFire();
                    }
                }
                #endregion
                else
                {
                    if (BuildManager.Instance.IsCreateMode == false)
                    {
                        #region 건물 클릭
                        RaycastHit2D BuildObject = hitList.Find(x => x.collider.transform.parent.CompareTag("Building"));
                        if (BuildObject)
                        {
                            if (ClickedObject != null && ClickedObject.transform.parent.GetInstanceID() == BuildObject.collider.transform.parent.GetInstanceID())
                            {
                                ClickedObject = null;
                                CameraManager.Instance.MoveToEntity(BuildObject.collider.transform.parent);
                            }
                            else
                            {
                                ClickedObject = BuildObject.collider.gameObject;
                                Debug.Log("빌딩 창");
                                ABaseBuilding building = ClickedObject.GetComponentInParent<ABaseBuilding>();

                                UIManager.Instance.OpenBuildingDetail(building, false);
                            }
                        }
                        #endregion
                    }
                }
            }
        }
    }

    public void RightMouseAction()
    {
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (UnitSelectionManager.Instance.SelectedUnitList.Count != 0)
            {
                Vector3 WorldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (FireManager.Instance.IsWithInFireRange(WorldMousePosition) == false)
                {
                    return;
                }

                RaycastHit2D hit = Physics2D.Raycast(WorldMousePosition, Vector2.zero, 1f, Clilckable);
                // 땅이면 좌표 전달
                if (hit.collider == null || hit.collider.transform.parent.CompareTag("Fire"))
                {
                    UnitSelectionManager.Instance.SelectedUnitList?.ForEach(x => x.SetTarget(WorldMousePosition));
                }
                else if (hit.collider.transform.parent.CompareTag("Enemy"))
                {
                    UnitSelectionManager.Instance.SelectedUnitList?.ForEach(x => x.SetTarget(hit.collider.GetComponentInParent<AInteractableEntity>()));
                    Debug.Log("여기 채워야됨");
                }
                else if (hit.collider.transform.parent.CompareTag("Resource"))
                {
                    UnitSelectionManager.Instance.SelectedUnitList?.ForEach(x => x.SetTarget(hit.collider.GetComponentInParent<AInteractableEntity>()));
                }
                else if (hit.collider.transform.parent.CompareTag("Unit"))
                {
                    UnitSelectionManager.Instance.SelectedUnitList?.ForEach(x => x.SetTarget(hit.collider.GetComponentInParent<AInteractableEntity>()));
                }
            }
        }
    }

    public void MiddleMouseAction()
    {

    }
}
