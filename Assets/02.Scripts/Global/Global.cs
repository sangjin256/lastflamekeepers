using UnityEngine;
using System;
using System.Collections;

public class Global : BehaviourSingleton<Global>
{
    public Action OnDataLoaded;
    public Vector2 MapSize = new(60f, 60f);
    public Vector2 TileBlockSize = new Vector2(20f, 20f);

    private IEnumerator Start()
    {
        yield return DataTable.Instance.Load_Routine();
        OnDataLoaded?.Invoke();
    }

    public bool BoundaryCheck(Vector3 position)
    {
        float MaxX = MapSize.x / 2;
        float MinX = -MapSize.x / 2;
        float MaxY = MapSize.y / 2;
        float MinY = -MapSize.y / 2;

        if (position.x < MaxX && position.x > MinX && position.y < MaxY && position.y > MinY) return true;
        return false;
    }
}
