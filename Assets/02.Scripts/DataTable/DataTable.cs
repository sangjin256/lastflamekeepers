// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public partial class DataTable
{
    #region Stat
    private ReadOnlyList<StatData> StatList = null;
    private ReadOnlyDictionary<int, StatData> StatTable = null;

    public ReadOnlyList<StatData> GetStatDataList()
    {
        return StatList;
    }

    public StatData GetStatData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (StatTable.TryGetValue(key, out StatData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of StatData: <{key}>");
            return null;
        }
    }
    #endregion
    #region Tool
    private ReadOnlyList<ToolData> ToolList = null;
    private ReadOnlyDictionary<int, ToolData> ToolTable = null;

    public ReadOnlyList<ToolData> GetToolDataList()
    {
        return ToolList;
    }

    public ToolData GetToolData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (ToolTable.TryGetValue(key, out ToolData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of ToolData: <{key}>");
            return null;
        }
    }
    #endregion
    #region Build
    private ReadOnlyList<BuildData> BuildList = null;
    private ReadOnlyDictionary<int, BuildData> BuildTable = null;

    public ReadOnlyList<BuildData> GetBuildDataList()
    {
        return BuildList;
    }

    public BuildData GetBuildData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (BuildTable.TryGetValue(key, out BuildData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of BuildData: <{key}>");
            return null;
        }
    }
    #endregion
    #region Wave
    private ReadOnlyList<WaveData> WaveList = null;
    private ReadOnlyDictionary<int, WaveData> WaveTable = null;

    public ReadOnlyList<WaveData> GetWaveDataList()
    {
        return WaveList;
    }

    public WaveData GetWaveData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (WaveTable.TryGetValue(key, out WaveData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of WaveData: <{key}>");
            return null;
        }
    }
    #endregion

    public IEnumerator LoadRoutine()
    {
        int allCount = 0;
        int loadedCount = 0;

        allCount++;
        GetBytes_FromResources("Stat", (bytes) =>
        {
            LoadStatData(bytes);
            loadedCount++;
        });
        allCount++;
        GetBytes_FromResources("Tool", (bytes) =>
        {
            LoadToolData(bytes);
            loadedCount++;
        });
        allCount++;
        GetBytes_FromResources("Build", (bytes) =>
        {
            LoadBuildData(bytes);
            loadedCount++;
        });
        allCount++;
        GetBytes_FromResources("Wave", (bytes) =>
        {
            LoadWaveData(bytes);
            loadedCount++;
        });

        yield return new WaitUntil(() => allCount == loadedCount);
    }

    public void LoadForEditor()
    {
        byte[] statBytes = GetBytes_ForEditor("StatData");
        LoadStatData(statBytes);
        byte[] toolBytes = GetBytes_ForEditor("ToolData");
        LoadToolData(toolBytes);
        byte[] buildBytes = GetBytes_ForEditor("BuildData");
        LoadBuildData(buildBytes);
        byte[] waveBytes = GetBytes_ForEditor("WaveData");
        LoadWaveData(waveBytes);
    }

    private void LoadStatData(byte[] bytes)
    {
        List<StatData> statList = new List<StatData>();
        Dictionary<int, StatData> statTable = new Dictionary<int, StatData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            StatData data = new StatData(Reader);
            if (statTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in Stat");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in Stat");
                continue;
            }

            statList.Add(data);
            statTable.Add(data.TID, data);
        }

        Reader.Close();

        StatList = new ReadOnlyList<StatData>(statList);
        StatTable = new ReadOnlyDictionary<int, StatData>(statTable);
    }

    private void LoadToolData(byte[] bytes)
    {
        List<ToolData> toolList = new List<ToolData>();
        Dictionary<int, ToolData> toolTable = new Dictionary<int, ToolData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            ToolData data = new ToolData(Reader);
            if (toolTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in Tool");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in Tool");
                continue;
            }

            toolList.Add(data);
            toolTable.Add(data.TID, data);
        }

        Reader.Close();

        ToolList = new ReadOnlyList<ToolData>(toolList);
        ToolTable = new ReadOnlyDictionary<int, ToolData>(toolTable);
    }

    private void LoadBuildData(byte[] bytes)
    {
        List<BuildData> buildList = new List<BuildData>();
        Dictionary<int, BuildData> buildTable = new Dictionary<int, BuildData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            BuildData data = new BuildData(Reader);
            if (buildTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in Build");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in Build");
                continue;
            }

            buildList.Add(data);
            buildTable.Add(data.TID, data);
        }

        Reader.Close();

        BuildList = new ReadOnlyList<BuildData>(buildList);
        BuildTable = new ReadOnlyDictionary<int, BuildData>(buildTable);
    }

    private void LoadWaveData(byte[] bytes)
    {
        List<WaveData> waveList = new List<WaveData>();
        Dictionary<int, WaveData> waveTable = new Dictionary<int, WaveData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            WaveData data = new WaveData(Reader);
            if (waveTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in Wave");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in Wave");
                continue;
            }

            waveList.Add(data);
            waveTable.Add(data.TID, data);
        }

        Reader.Close();

        WaveList = new ReadOnlyList<WaveData>(waveList);
        WaveTable = new ReadOnlyDictionary<int, WaveData>(waveTable);
    }

}
