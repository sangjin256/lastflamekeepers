using UnityEngine;
using System;
using System.Collections;

public class Global : BehaviourSingleton<Global>
{
    public Action OnDataLoaded;
    public Vector2 MapSize = new(120f, 120f);
    public Vector2 TileBlockSize = new Vector2(40f, 40f);

    private IEnumerator Start()
    {
        yield return DataTable.Instance.Load_Routine();
        OnDataLoaded?.Invoke();
    }
}
