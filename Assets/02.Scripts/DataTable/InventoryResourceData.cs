// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections.Generic;
using System.IO;
using System.Text;

public class InventoryResourceData
{
    ///<summary>TID</summary>
    public readonly int TID;

    ///<summary>소모 자원 타입</summary>
    public readonly InventoryResourceType InventoryResourceType;

    ///<summary>소모 자원 이름</summary>
    public readonly string Name;

    ///<summary>최대 개수</summary>
    public readonly int MaxCount;

    ///<summary>설명</summary>
    public readonly string Description;

    public InventoryResourceData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        InventoryResourceType = (InventoryResourceType)reader.ReadInt32();
        int name = reader.ReadInt32();
        Name = Encoding.UTF8.GetString(reader.ReadBytes(name));
        MaxCount = reader.ReadInt32();
        int description = reader.ReadInt32();
        Description = Encoding.UTF8.GetString(reader.ReadBytes(description));
    }
}
