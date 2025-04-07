// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections.Generic;
using System.IO;
using System.Text;

public class FieldResourceData
{
    ///<summary>TID</summary>
    public readonly int TID;

    ///<summary>필드 자원 타입</summary>
    public readonly ResourceType ResourceType;

    ///<summary>필드 자원 이름</summary>
    public readonly string Name;

    ///<summary>내구도</summary>
    public readonly int Durability;

    ///<summary>1번 얻을때까지 때리는 횟수</summary>
    public readonly int AmountToHit;

    ///<summary>채집 1회 당 나오는 소모 자원 개수</summary>
    public readonly int OutputAmountPerExtract;

    ///<summary>채집 시 생성되는 소모 자원 타입</summary>
    public readonly InventoryResourceType OutputType;

    ///<summary>리젠되는 시간</summary>
    public readonly float RespawnTime;

    public FieldResourceData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        ResourceType = (ResourceType)reader.ReadInt32();
        int name = reader.ReadInt32();
        Name = Encoding.UTF8.GetString(reader.ReadBytes(name));
        Durability = reader.ReadInt32();
        AmountToHit = reader.ReadInt32();
        OutputAmountPerExtract = reader.ReadInt32();
        OutputType = (InventoryResourceType)reader.ReadInt32();
        RespawnTime = reader.ReadSingle();
    }
}
