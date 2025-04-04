using System;
using UnityEngine;

//  불의 범위를 관리하고 범위가 변경될 때 이벤트를 발생시키는 클래스


public class FireManager : BehaviourSingleton<FireManager>
{

    [Header("불 범위 설정")]
    [SerializeField] float MaxRange = 20f;
    [SerializeField] float MinRange = 0f;
    [SerializeField] float CurrentRange = 0f;
    [SerializeField] float CurrentSquareRange => CurrentRange * CurrentRange;

    // 불의 범위가 변경될 때 발생하는 이벤트
    public Action<float> OnFireRangeChanged;

    private void SetFireRange(float range)
    {
        // 범위를 설정하고 범위가 변경될 때 이벤트를 발생시킴
        CurrentRange = Mathf.Clamp(range, MinRange, MaxRange);

        // 범위 변경 이벤트 발생
        OnFireRangeChanged?.Invoke(CurrentRange);
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
