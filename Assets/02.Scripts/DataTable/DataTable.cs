// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public partial class DataTable
{
    #region RandomStat
    private ReadOnlyList<RandomStatData> RandomStatList = null;
    private ReadOnlyDictionary<int, RandomStatData> RandomStatTable = null;

    public ReadOnlyList<RandomStatData> GetRandomStatDataList()
    {
        return RandomStatList;
    }

    public RandomStatData GetRandomStatData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (RandomStatTable.TryGetValue(key, out RandomStatData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of RandomStatData: <{key}>");
            return null;
        }
    }
    #endregion
    #region GoodFeature
    private ReadOnlyList<GoodFeatureData> GoodFeatureList = null;
    private ReadOnlyDictionary<int, GoodFeatureData> GoodFeatureTable = null;

    public ReadOnlyList<GoodFeatureData> GetGoodFeatureDataList()
    {
        return GoodFeatureList;
    }

    public GoodFeatureData GetGoodFeatureData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (GoodFeatureTable.TryGetValue(key, out GoodFeatureData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of GoodFeatureData: <{key}>");
            return null;
        }
    }
    #endregion
    #region BadFeature
    private ReadOnlyList<BadFeatureData> BadFeatureList = null;
    private ReadOnlyDictionary<int, BadFeatureData> BadFeatureTable = null;

    public ReadOnlyList<BadFeatureData> GetBadFeatureDataList()
    {
        return BadFeatureList;
    }

    public BadFeatureData GetBadFeatureData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (BadFeatureTable.TryGetValue(key, out BadFeatureData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of BadFeatureData: <{key}>");
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
        GetBytes_FromResources("RandomStat", (bytes) =>
        {
            LoadRandomStatData(bytes);
            loadedCount++;
        });
        allCount++;
        GetBytes_FromResources("GoodFeature", (bytes) =>
        {
            LoadGoodFeatureData(bytes);
            loadedCount++;
        });
        allCount++;
        GetBytes_FromResources("BadFeature", (bytes) =>
        {
            LoadBadFeatureData(bytes);
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
        byte[] randomStatBytes = GetBytes_ForEditor("RandomStatData");
        LoadRandomStatData(randomStatBytes);
        byte[] goodFeatureBytes = GetBytes_ForEditor("GoodFeatureData");
        LoadGoodFeatureData(goodFeatureBytes);
        byte[] badFeatureBytes = GetBytes_ForEditor("BadFeatureData");
        LoadBadFeatureData(badFeatureBytes);
        byte[] toolBytes = GetBytes_ForEditor("ToolData");
        LoadToolData(toolBytes);
        byte[] buildBytes = GetBytes_ForEditor("BuildData");
        LoadBuildData(buildBytes);
        byte[] waveBytes = GetBytes_ForEditor("WaveData");
        LoadWaveData(waveBytes);
    }

    private void LoadRandomStatData(byte[] bytes)
    {
        List<RandomStatData> randomStatList = new List<RandomStatData>();
        Dictionary<int, RandomStatData> randomStatTable = new Dictionary<int, RandomStatData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            RandomStatData data = new RandomStatData(Reader);
            if (randomStatTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in RandomStat");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in RandomStat");
                continue;
            }

            randomStatList.Add(data);
            randomStatTable.Add(data.TID, data);
        }

        Reader.Close();

        RandomStatList = new ReadOnlyList<RandomStatData>(randomStatList);
        RandomStatTable = new ReadOnlyDictionary<int, RandomStatData>(randomStatTable);
    }

    private void LoadGoodFeatureData(byte[] bytes)
    {
        List<GoodFeatureData> goodFeatureList = new List<GoodFeatureData>();
        Dictionary<int, GoodFeatureData> goodFeatureTable = new Dictionary<int, GoodFeatureData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            GoodFeatureData data = new GoodFeatureData(Reader);
            if (goodFeatureTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in GoodFeature");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in GoodFeature");
                continue;
            }

            goodFeatureList.Add(data);
            goodFeatureTable.Add(data.TID, data);
        }

        Reader.Close();

        GoodFeatureList = new ReadOnlyList<GoodFeatureData>(goodFeatureList);
        GoodFeatureTable = new ReadOnlyDictionary<int, GoodFeatureData>(goodFeatureTable);
    }

    private void LoadBadFeatureData(byte[] bytes)
    {
        List<BadFeatureData> badFeatureList = new List<BadFeatureData>();
        Dictionary<int, BadFeatureData> badFeatureTable = new Dictionary<int, BadFeatureData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            BadFeatureData data = new BadFeatureData(Reader);
            if (badFeatureTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in BadFeature");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in BadFeature");
                continue;
            }

            badFeatureList.Add(data);
            badFeatureTable.Add(data.TID, data);
        }

        Reader.Close();

        BadFeatureList = new ReadOnlyList<BadFeatureData>(badFeatureList);
        BadFeatureTable = new ReadOnlyDictionary<int, BadFeatureData>(badFeatureTable);
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
