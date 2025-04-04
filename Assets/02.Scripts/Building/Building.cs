using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingType BuildingType;
    private int _level = 0;
    public int Level => _level;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeColor(Color color)
    {
        _spriteRenderer.color = color;
    }
}
