// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public partial class DataTable
{
    #region Test
    private ReadOnlyList<TestData> TestList = null;
    private ReadOnlyDictionary<int, TestData> TestTable = null;

    public ReadOnlyList<TestData> GetTestDataList()
    {
        return TestList;
    }

    public TestData GetTestData(int key)
    {
        if (key == 0)
        {
            return null;
        }

        if (TestTable.TryGetValue(key, out TestData retVal) == true)
        {
            return retVal;
        }
        else
        {
            Debug.LogError($"Can not find UniqueID of TestData: <{key}>");
            return null;
        }
    }
    #endregion

    public IEnumerator LoadRoutine()
    {
        int allCount = 0;
        int loadedCount = 0;

        allCount++;
        GetBytes_FromResources("Test", (bytes) =>
        {
            LoadTestData(bytes);
            loadedCount++;
        });

        yield return new WaitUntil(() => allCount == loadedCount);
    }

    public void LoadForEditor()
    {
        byte[] testBytes = GetBytes_ForEditor("TestData");
        LoadTestData(testBytes);
    }

    private void LoadTestData(byte[] bytes)
    {
        List<TestData> testList = new List<TestData>();
        Dictionary<int, TestData> testTable = new Dictionary<int, TestData>();

        Reader = new BinaryReader(new MemoryStream(bytes));

        while (Reader.BaseStream.Position < bytes.Length)
        {
            TestData data = new TestData(Reader);
            if (testTable.ContainsKey(data.TID) == true)
            {
                Debug.LogError("The duplicate TID: " + data.TID + " in Test");
                continue;
            }
            else if (data.TID == 0)
            {
                Debug.LogError("TID is 0 in Test");
                continue;
            }

            testList.Add(data);
            testTable.Add(data.TID, data);
        }

        Reader.Close();

        TestList = new ReadOnlyList<TestData>(testList);
        TestTable = new ReadOnlyDictionary<int, TestData>(testTable);
    }

}
