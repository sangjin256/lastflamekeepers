using UnityEngine;
using DG.Tweening;

public class UI_SlideMove : MonoBehaviour
{
    public Vector2 OriginPosition;
    public Vector2 TargetPosition;
    public float Duration;

    public void SetMove(bool isMove)
    {
        if (isMove)
        {
            transform.DOMove(TargetPosition, Duration);
        }
        else
        {
            transform.DOMove(OriginPosition, Duration);
        }
    }
}
