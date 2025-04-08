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

    ///<summary>설명</summary>
    public readonly string Description;

    ///<summary>기본값</summary>
    public readonly int Value;

    ///<summary>업그레이드1</summary>
    private readonly int Upgrade1_AddValue;

    ///<summary>업그레이드2</summary>
    private readonly int Upgrade2_AddValue;

    ///<summary>업그레이드3</summary>
    private readonly int Upgrade3_AddValue;

    ///<summary>업그레이드 1 나무 추가 개수</summary>
    private readonly int Upgrade1_WoodCount;

    ///<summary>업그레이드 1 돌 추가 개수</summary>
    private readonly int Upgrade1_StoneCount;

    ///<summary>업그레이드 2 나무 추가 개수</summary>
    private readonly int Upgrade2_WoodCount;

    ///<summary>업그레이드 2 돌 추가 개수</summary>
    private readonly int Upgrade2_StoneCount;

    ///<summary>업그레이드 3 나무 추가 개수</summary>
    private readonly int Upgrade3_WoodCount;

    ///<summary>업그레이드 3 돌 추가 개수</summary>
    private readonly int Upgrade3_StoneCount;

    ///<summary>최대 개수</summary>
    public readonly int MaxCount;

    ///<summary>도구 종류</summary>
    public readonly ToolType ToolType;

    ///<summary>Upgrade_AddValue 리스트</summary>
    public readonly List<int> Upgrade_AddValueList = new List<int>();

    ///<summary>Upgrade_WoodCount 리스트</summary>
    public readonly List<int> Upgrade_WoodCountList = new List<int>();

    ///<summary>Upgrade_StoneCount 리스트</summary>
    public readonly List<int> Upgrade_StoneCountList = new List<int>();
    public ToolData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        int toolname = reader.ReadInt32();
        ToolName = Encoding.UTF8.GetString(reader.ReadBytes(toolname));
        int description = reader.ReadInt32();
        Description = Encoding.UTF8.GetString(reader.ReadBytes(description));
        Value = reader.ReadInt32();
        Upgrade1_AddValue = reader.ReadInt32();
        Upgrade2_AddValue = reader.ReadInt32();
        Upgrade3_AddValue = reader.ReadInt32();
        Upgrade1_WoodCount = reader.ReadInt32();
        Upgrade1_StoneCount = reader.ReadInt32();
        Upgrade2_WoodCount = reader.ReadInt32();
        Upgrade2_StoneCount = reader.ReadInt32();
        Upgrade3_WoodCount = reader.ReadInt32();
        Upgrade3_StoneCount = reader.ReadInt32();
        MaxCount = reader.ReadInt32();
        ToolType = (ToolType)reader.ReadInt32();

        LinkTable();
    }

    public void LinkTable()
    {
        Upgrade_AddValueList.Add(Upgrade1_AddValue);
        Upgrade_AddValueList.Add(Upgrade2_AddValue);
        Upgrade_AddValueList.Add(Upgrade3_AddValue);
        Upgrade_WoodCountList.Add(Upgrade1_WoodCount);
        Upgrade_StoneCountList.Add(Upgrade1_StoneCount);
        Upgrade_WoodCountList.Add(Upgrade2_WoodCount);
        Upgrade_StoneCountList.Add(Upgrade2_StoneCount);
        Upgrade_WoodCountList.Add(Upgrade3_WoodCount);
        Upgrade_StoneCountList.Add(Upgrade3_StoneCount);
    }
}
