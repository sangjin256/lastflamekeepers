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
    #region Feature
    private ReadOnlyList<FeatureData> FeatureList = null;
    private ReadOnlyDictionary<int, FeatureData> FeatureTable = null;

    public ReadOnlyList<FeatureData> GetFeatureDataList()
    {
        return FeatureList;
    }

    public FeatureData GetFeatureData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (FeatureTable.TryGetValue(key, out FeatureData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of FeatureData: <{key}>");
            return null;
        }
    }
    #endregion
    #region UnitName
    private ReadOnlyList<UnitNameData> UnitNameList = null;
    private ReadOnlyDictionary<int, UnitNameData> UnitNameTable = null;

    public ReadOnlyList<UnitNameData> GetUnitNameDataList()
    {
        return UnitNameList;
    }

    public UnitNameData GetUnitNameData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (UnitNameTable.TryGetValue(key, out UnitNameData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of UnitNameData: <{key}>");
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
    #region FieldResource
    private ReadOnlyList<FieldResourceData> FieldResourceList = null;
    private ReadOnlyDictionary<int, FieldResourceData> FieldResourceTable = null;

    public ReadOnlyList<FieldResourceData> GetFieldResourceDataList()
    {
        return FieldResourceList;
    }

    public FieldResourceData GetFieldResourceData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (FieldResourceTable.TryGetValue(key, out FieldResourceData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of FieldResourceData: <{key}>");
            return null;
        }
    }
    #endregion
    #region InventoryResource
    private ReadOnlyList<InventoryResourceData> InventoryResourceList = null;
    private ReadOnlyDictionary<int, InventoryResourceData> InventoryResourceTable = null;

    public ReadOnlyList<InventoryResourceData> GetInventoryResourceDataList()
    {
        return InventoryResourceList;
    }

    public InventoryResourceData GetInventoryResourceData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (InventoryResourceTable.TryGetValue(key, out InventoryResourceData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of InventoryResourceData: <{key}>");
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
        GetBytes_FromResources("Feature", (bytes) =>
        {
            LoadFeatureData(bytes);
            loadedCount++;
        });
        allCount++;
        GetBytes_FromResources("UnitName", (bytes) =>
        {
            LoadUnitNameData(bytes);
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
        allCount++;
        GetBytes_FromResources("FieldResource", (bytes) =>
        {
            LoadFieldResourceData(bytes);
            loadedCount++;
        });
        allCount++;
        GetBytes_FromResources("InventoryResource", (bytes) =>
        {
            LoadInventoryResourceData(bytes);
            loadedCount++;
        });

        yield return new WaitUntil(() => allCount == loadedCount);
    }

    public void LoadForEditor()
    {
        byte[] randomStatBytes = GetBytes_ForEditor("RandomStatData");
        LoadRandomStatData(randomStatBytes);
        byte[] featureBytes = GetBytes_ForEditor("FeatureData");
        LoadFeatureData(featureBytes);
        byte[] unitNameBytes = GetBytes_ForEditor("UnitNameData");
        LoadUnitNameData(unitNameBytes);
        byte[] toolBytes = GetBytes_ForEditor("ToolData");
        LoadToolData(toolBytes);
        byte[] buildBytes = GetBytes_ForEditor("BuildData");
        LoadBuildData(buildBytes);
        byte[] waveBytes = GetBytes_ForEditor("WaveData");
        LoadWaveData(waveBytes);
        byte[] fieldResourceBytes = GetBytes_ForEditor("FieldResourceData");
        LoadFieldResourceData(fieldResourceBytes);
        byte[] inventoryResourceBytes = GetBytes_ForEditor("InventoryResourceData");
        LoadInventoryResourceData(inventoryResourceBytes);
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

    private void LoadFeatureData(byte[] bytes)
    {
        List<FeatureData> featureList = new List<FeatureData>();
        Dictionary<int, FeatureData> featureTable = new Dictionary<int, FeatureData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            FeatureData data = new FeatureData(Reader);
            if (featureTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in Feature");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in Feature");
                continue;
            }

            featureList.Add(data);
            featureTable.Add(data.TID, data);
        }

        Reader.Close();

        FeatureList = new ReadOnlyList<FeatureData>(featureList);
        FeatureTable = new ReadOnlyDictionary<int, FeatureData>(featureTable);
    }

    private void LoadUnitNameData(byte[] bytes)
    {
        List<UnitNameData> unitNameList = new List<UnitNameData>();
        Dictionary<int, UnitNameData> unitNameTable = new Dictionary<int, UnitNameData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            UnitNameData data = new UnitNameData(Reader);
            if (unitNameTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in UnitName");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in UnitName");
                continue;
            }

            unitNameList.Add(data);
            unitNameTable.Add(data.TID, data);
        }

        Reader.Close();

        UnitNameList = new ReadOnlyList<UnitNameData>(unitNameList);
        UnitNameTable = new ReadOnlyDictionary<int, UnitNameData>(unitNameTable);
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

    private void LoadFieldResourceData(byte[] bytes)
    {
        List<FieldResourceData> fieldResourceList = new List<FieldResourceData>();
        Dictionary<int, FieldResourceData> fieldResourceTable = new Dictionary<int, FieldResourceData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            FieldResourceData data = new FieldResourceData(Reader);
            if (fieldResourceTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in FieldResource");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in FieldResource");
                continue;
            }

            fieldResourceList.Add(data);
            fieldResourceTable.Add(data.TID, data);
        }

        Reader.Close();

        FieldResourceList = new ReadOnlyList<FieldResourceData>(fieldResourceList);
        FieldResourceTable = new ReadOnlyDictionary<int, FieldResourceData>(fieldResourceTable);
    }

    private void LoadInventoryResourceData(byte[] bytes)
    {
        List<InventoryResourceData> inventoryResourceList = new List<InventoryResourceData>();
        Dictionary<int, InventoryResourceData> inventoryResourceTable = new Dictionary<int, InventoryResourceData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            InventoryResourceData data = new InventoryResourceData(Reader);
            if (inventoryResourceTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in InventoryResource");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in InventoryResource");
                continue;
            }

            inventoryResourceList.Add(data);
            inventoryResourceTable.Add(data.TID, data);
        }

        Reader.Close();

        InventoryResourceList = new ReadOnlyList<InventoryResourceData>(inventoryResourceList);
        InventoryResourceTable = new ReadOnlyDictionary<int, InventoryResourceData>(inventoryResourceTable);
    }

}
