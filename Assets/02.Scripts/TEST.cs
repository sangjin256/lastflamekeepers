using UnityEngine;
using System.Threading.Tasks;

public class TEST : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _LoadStat();
    }

    private async void _LoadStat()
    {
        Task _init = Task.Factory.StartNew(() =>
        {
            for (; ; )
            {
                if (DataTable.Instance.GetToolDataList() != null) break;
            }
        });

        await _init;

        ToolData data = DataTable.Instance.GetToolData(10000);
        Debug.Log(data.ToolName);
    }
}
