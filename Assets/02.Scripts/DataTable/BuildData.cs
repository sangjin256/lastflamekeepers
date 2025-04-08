// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections.Generic;
using System.IO;
using System.Text;

public class BuildData
{
    ///<summary>TID</summary>
    public readonly int TID;

    ///<summary>건물 이름</summary>
    public readonly string Building;

    ///<summary>건물 설명</summary>
    public readonly string Description;

    ///<summary>기본값</summary>
    public readonly int Value;

    ///<summary>업그레이드1</summary>
    private readonly int Upgrade1_AddValue;

    ///<summary>업그레이드2</summary>
    private readonly int Upgrade2_AddValue;

    ///<summary>필요 나무 개수</summary>
    public readonly int WoodCount;

    ///<summary>필요 돌 개수</summary>
    public readonly int StoneCount;

    ///<summary>업그레이드 1 나무 추가 개수</summary>
    private readonly int Upgrade1_WoodCount;

    ///<summary>업그레이드 1 돌 추가 개수</summary>
    private readonly int Upgrade1_StoneCount;

    ///<summary>업그레이드 2 나무 추가 개수</summary>
    private readonly int Upgrade2_WoodCount;

    ///<summary>업그레이드 2 돌 추가 개수</summary>
    private readonly int Upgrade2_StoneCount;

    ///<summary>건물 타입</summary>
    public readonly BuildingType BuildingType;

    ///<summary>Upgrade_AddValue 리스트</summary>
    public readonly List<int> Upgrade_AddValueList = new List<int>();

    ///<summary>Upgrade_WoodCount 리스트</summary>
    public readonly List<int> Upgrade_WoodCountList = new List<int>();

    ///<summary>Upgrade_StoneCount 리스트</summary>
    public readonly List<int> Upgrade_StoneCountList = new List<int>();
    public BuildData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        int building = reader.ReadInt32();
        Building = Encoding.UTF8.GetString(reader.ReadBytes(building));
        int description = reader.ReadInt32();
        Description = Encoding.UTF8.GetString(reader.ReadBytes(description));
        Value = reader.ReadInt32();
        Upgrade1_AddValue = reader.ReadInt32();
        Upgrade2_AddValue = reader.ReadInt32();
        WoodCount = reader.ReadInt32();
        StoneCount = reader.ReadInt32();
        Upgrade1_WoodCount = reader.ReadInt32();
        Upgrade1_StoneCount = reader.ReadInt32();
        Upgrade2_WoodCount = reader.ReadInt32();
        Upgrade2_StoneCount = reader.ReadInt32();
        BuildingType = (BuildingType)reader.ReadInt32();

        LinkTable();
    }

    public void LinkTable()
    {
        Upgrade_AddValueList.Add(Upgrade1_AddValue);
        Upgrade_AddValueList.Add(Upgrade2_AddValue);
        Upgrade_WoodCountList.Add(Upgrade1_WoodCount);
        Upgrade_StoneCountList.Add(Upgrade1_StoneCount);
        Upgrade_WoodCountList.Add(Upgrade2_WoodCount);
        Upgrade_StoneCountList.Add(Upgrade2_StoneCount);
    }
}
