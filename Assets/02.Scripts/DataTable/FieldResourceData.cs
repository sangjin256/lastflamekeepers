// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections.Generic;
using System.IO;
using System.Text;

public class FieldResourceData
{
    ///<summary>TID</summary>
    public readonly int TID;

    ///<summary>자원 타입</summary>
    public readonly ResourceType ResourceType;

    ///<summary>자원 이름</summary>
    public readonly string ResourceName;

    ///<summary>내구도</summary>
    public readonly int Durability;

    ///<summary>1번 얻을때까지 때리는 횟수</summary>
    public readonly int AmountToHit;

    public FieldResourceData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        ResourceType = (ResourceType)reader.ReadInt32();
        int resourcename = reader.ReadInt32();
        ResourceName = Encoding.UTF8.GetString(reader.ReadBytes(resourcename));
        Durability = reader.ReadInt32();
        AmountToHit = reader.ReadInt32();
    }
}
