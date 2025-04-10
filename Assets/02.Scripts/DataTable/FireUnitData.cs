// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections.Generic;
using System.IO;
using System.Text;

public class FireUnitData
{
    ///<summary>TID</summary>
    public readonly int TID;

    ///<summary>레벨</summary>
    public readonly int Level;

    ///<summary>필요 잿가루 수</summary>
    public readonly int  AshAmout;

    public FireUnitData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        Level = reader.ReadInt32();
         AshAmout = reader.ReadInt32();
    }
}
