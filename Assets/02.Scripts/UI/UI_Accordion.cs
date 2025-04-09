using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_Accordion : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private float distanceFormToggle = 130f;
    [SerializeField] private float distanceBetweenButton = 110f;
    [SerializeField] private float duration = 1f;
    
    public void OpenButtons(bool isOn)
    {
        if (isOn)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].transform.DOLocalMoveX(distanceFormToggle + distanceBetweenButton * i, duration);
            }
        }
        else
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].transform.DOLocalMoveX(0, duration);
            }
        }
    }
}
