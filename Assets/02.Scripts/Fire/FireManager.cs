using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

//  불의 범위를 관리하고 범위가 변경될 때 이벤트를 발생시키는 클래스


public class FireManager : BehaviourSingleton<FireManager>
{
    private Light2D _outerLight;

    [Header("불 범위 설정")]
    [SerializeField] float MaxRange = 20f;
    [SerializeField] float MinRange = 0f;
    [SerializeField] float CurrentRange = 5f;

    private Light2D _fireLight;
    private float _innerRange;

    [SerializeField] float CurrentSquareRange => CurrentRange * CurrentRange;

    // 불의 범위가 변경될 때 발생하는 이벤트
    public Action<float> OnFireRangeChanged;

    private void Start()
    {
        _fireLight = GetComponent<Light2D>();
        _outerLight = transform.GetChild(0).GetComponent<Light2D>();
    }

    private void SetFireRange(float range)
    {
        // 범위를 설정하고 범위가 변경될 때 이벤트를 발생시킴
        CurrentRange = Mathf.Clamp(range, MinRange, MaxRange);
        _innerRange = Mathf.Clamp(CurrentRange - 2, MinRange, MaxRange);

        _fireLight.pointLightInnerRadius = _innerRange;
        _fireLight.pointLightOuterRadius = CurrentRange;

        _outerLight.pointLightInnerRadius = _innerRange;
        _outerLight.pointLightOuterRadius = CurrentRange + 2f;

        // 범위 변경 이벤트 발생
        OnFireRangeChanged?.Invoke(CurrentRange);
    }

    public float GetRange()
    {
        return CurrentRange;
    }

    public void ExpandFire(float amount)
    {
        // 범위를 증가시키고 이벤트 발생
        SetFireRange(CurrentRange + amount);
    }

    public void ReduceFire(float amount)
    {
        // 범위를 감소시키고 이벤트 발생
        SetFireRange(CurrentRange - amount);
    }

    public bool IsWithInFireRange(Vector2 position)
    {
        // 주어진 거리가 현재 범위 내에 있는지 확인
        float distance = Vector2.SqrMagnitude(position);
        return distance <= CurrentSquareRange;
    }

    public bool IsWithInFireRange(float squareDistance)
    {

        return squareDistance <= CurrentSquareRange;
    }
}
