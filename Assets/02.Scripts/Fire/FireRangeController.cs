using UnityEngine;

public class FireRangeController : MonoBehaviour
{
    [SerializeField]
    private float amount = 10f;

    void Start()
    {
        // 이벤트 구독 (불 범위가 바뀔 때마다 로그 출력)
        FireManager.Instance.SubscribeToRangeChange(OnFireRangeChanged);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FireManager.Instance.ExpandFire(amount);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            FireManager.Instance.ReduceFire(amount);
        }
    }

    private void OnFireRangeChanged(float newRange)
    {
        Debug.Log($"[Fire] 불의 범위가 변경됨 → 현재 반지름: {newRange}");
    }
}
