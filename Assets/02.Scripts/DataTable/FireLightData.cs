// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections.Generic;
using System.IO;
using System.Text;

public class FireLightData
{
    ///<summary>TID</summary>
    public readonly int TID;

    ///<summary>불 레벨</summary>
    public readonly int FireLevel;

    ///<summary>다음 레벨 밝기</summary>
    public readonly int NextFirePercent;

    ///<summary>필요 장작 수</summary>
    public readonly int WoodAmout;

    public FireLightData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        FireLevel = reader.ReadInt32();
        NextFirePercent = reader.ReadInt32();
        WoodAmout = reader.ReadInt32();
    }
}
