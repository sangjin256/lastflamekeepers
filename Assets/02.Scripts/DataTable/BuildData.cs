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

    ///<summary>업그레이드1</summary>
    private readonly int Upgrade1_Value;

    ///<summary>업그레이드2</summary>
    private readonly int Upgrade2_Value;

    ///<summary>업그레이드3</summary>
    private readonly int Upgrade3_Value;

    ///<summary>업그레이드4</summary>
    private readonly int Upgrade4_Value;

    ///<summary>건물 타입</summary>
    public readonly BuildingType BuildingType;

    ///<summary>Upgrade_Value 리스트</summary>
    public readonly List<int> Upgrade_ValueList = new List<int>();
    public BuildData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        int building = reader.ReadInt32();
        Building = Encoding.UTF8.GetString(reader.ReadBytes(building));
        int description = reader.ReadInt32();
        Description = Encoding.UTF8.GetString(reader.ReadBytes(description));
        Upgrade1_Value = reader.ReadInt32();
        Upgrade2_Value = reader.ReadInt32();
        Upgrade3_Value = reader.ReadInt32();
        Upgrade4_Value = reader.ReadInt32();
        BuildingType = (BuildingType)reader.ReadInt32();

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
