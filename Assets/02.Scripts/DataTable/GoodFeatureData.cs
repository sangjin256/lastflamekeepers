// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections.Generic;
using System.IO;
using System.Text;

public class GoodFeatureData
{
    ///<summary>TID</summary>
    public readonly int TID;

    ///<summary>특성 타입</summary>
    public readonly FeatureType FeatureType;

    ///<summary>특성 이름</summary>
    public readonly string FeatureName;

    ///<summary>설명</summary>
    public readonly string Description;

    ///<summary>반영값</summary>
    public readonly int MaxHealthValue;

    ///<summary>반영값</summary>
    public readonly int DamageValue;

    ///<summary>반영값</summary>
    public readonly int AttackSpeedValue;

    ///<summary>반영값</summary>
    public readonly int MoveSpeedValue;

    ///<summary>반영값</summary>
    public readonly int WoodSpeedValue;

    ///<summary>반영값</summary>
    public readonly int RockSpeedValue;

    public GoodFeatureData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        FeatureType = (FeatureType)reader.ReadInt32();
        int featurename = reader.ReadInt32();
        FeatureName = Encoding.UTF8.GetString(reader.ReadBytes(featurename));
        int description = reader.ReadInt32();
        Description = Encoding.UTF8.GetString(reader.ReadBytes(description));
        MaxHealthValue = reader.ReadInt32();
        DamageValue = reader.ReadInt32();
        AttackSpeedValue = reader.ReadInt32();
        MoveSpeedValue = reader.ReadInt32();
        WoodSpeedValue = reader.ReadInt32();
        RockSpeedValue = reader.ReadInt32();
    }
}
