// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections.Generic;
using System.IO;
using System.Text;

public class TestData
{
    ///<summary>TID</summary>
    public readonly int TID;

    ///<summary>아이템 이름</summary>
    public readonly string ItemName;

    ///<summary>아이템 설명</summary>
    public readonly string Content;

    ///<summary>섭취가능</summary>
    public readonly bool IsEatable;

    ///<summary>치유여부</summary>
    public readonly bool Healable;

    ///<summary>포만감</summary>
    public readonly int Saturation;

    ///<summary>아이템 가치</summary>
    public readonly int ItemValue;

    ///<summary>도구 종류</summary>
    public readonly ToolType ToolType;

    public TestData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        int itemname = reader.ReadInt32();
        ItemName = Encoding.UTF8.GetString(reader.ReadBytes(itemname));
        int content = reader.ReadInt32();
        Content = Encoding.UTF8.GetString(reader.ReadBytes(content));
        IsEatable = reader.ReadBoolean();
        Healable = reader.ReadBoolean();
        Saturation = reader.ReadInt32();
        ItemValue = reader.ReadInt32();
        ToolType = (ToolType)reader.ReadInt32();
    }
}
