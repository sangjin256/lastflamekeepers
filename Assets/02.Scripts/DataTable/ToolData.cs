// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections.Generic;
using System.IO;
using System.Text;

public class ToolData
{
    ///<summary>TID</summary>
    public readonly int TID;

    ///<summary>도구 이름</summary>
    public readonly string ToolName;

    ///<summary>업그레이드1</summary>
    private readonly int Upgrade1_Value;

    ///<summary>업그레이드2</summary>
    private readonly int Upgrade2_Value;

    ///<summary>업그레이드3</summary>
    private readonly int Upgrade3_Value;

    ///<summary>업그레이드4</summary>
    private readonly int Upgrade4_Value;

    ///<summary>도구 종류</summary>
    public readonly ToolType ToolType;

    ///<summary>Upgrade_Value 리스트</summary>
    public readonly List<int> Upgrade_ValueList = new List<int>();
    public ToolData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        int toolname = reader.ReadInt32();
        ToolName = Encoding.UTF8.GetString(reader.ReadBytes(toolname));
        Upgrade1_Value = reader.ReadInt32();
        Upgrade2_Value = reader.ReadInt32();
        Upgrade3_Value = reader.ReadInt32();
        Upgrade4_Value = reader.ReadInt32();
        ToolType = (ToolType)reader.ReadInt32();

        LinkTable();
    }

    public void LinkTable()
    {
        Upgrade_ValueList.Add(Upgrade1_Value);
        Upgrade_ValueList.Add(Upgrade2_Value);
        Upgrade_ValueList.Add(Upgrade3_Value);
        Upgrade_ValueList.Add(Upgrade4_Value);
    }
}
