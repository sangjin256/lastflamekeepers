// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections.Generic;
using System.IO;
using System.Text;

public class StatData
{
    ///<summary>TID</summary>
    public readonly int TID;

    ///<summary>스탯 이름</summary>
    public readonly string StatName;

    ///<summary>최소값</summary>
    public readonly int MinValue;

    ///<summary>최대값</summary>
    public readonly int MaxValue;

    public StatData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        int statname = reader.ReadInt32();
        StatName = Encoding.UTF8.GetString(reader.ReadBytes(statname));
        MinValue = reader.ReadInt32();
        MaxValue = reader.ReadInt32();
    }
}
