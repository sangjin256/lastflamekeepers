using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FireVisual : MonoBehaviour
{
    //private CircleCollider2D fireLightCollider;
    //private Light2D fireLight;

    //[SerializeField] private float lightRadiusMultiplier = 1f; // 불 범위 대비 라이트 크기 계수

    //private void Start()
    //{
    //    fireLightCollider = GetComponent<CircleCollider2D>();
    //    fireLight = GetComponentInChildren<Light2D>();

    //    FireManager.Instance.OnFireRangeChanged += ChangeFireRange;
    //}

    //private void ChangeFireRange(float newRange)
    //{
    //    // 반지름을 크기로 사용 (적절히 스케일 조절 계수 곱해도 됨)
    //    transform.localScale = Vector3.one * newRange;

    //    // Collider 갱신
    //    if (fireLightCollider != null)
    //    {
    //        fireLightCollider.radius = newRange;
    //    }

    //    // 라이트 범위
    //    if (fireLight != null)
    //    {
    //        fireLight.pointLightOuterRadius = newRange * lightRadiusMultiplier;
    //    }
    //}
}
