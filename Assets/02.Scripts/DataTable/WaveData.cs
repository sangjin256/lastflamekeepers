// 툴에서 자동으로 생성하는 소스 파일입니다. 수정하지 마세요!
using System.Collections.Generic;
using System.IO;
using System.Text;

public class WaveData
{
    ///<summary>TID</summary>
    public readonly int TID;

    ///<summary>웨이브 번호</summary>
    public readonly int WaveNum;

    ///<summary>적 체력</summary>
    public readonly int EnemyHealth;

    ///<summary>적 공격력</summary>
    public readonly int EnemyDamage;

    ///<summary>적 수</summary>
    public readonly int EnemyCount;

    ///<summary>잿가루 수</summary>
    public readonly int EnemyAsh;

    public WaveData(BinaryReader reader)
    {
        TID = reader.ReadInt32();
        WaveNum = reader.ReadInt32();
        EnemyHealth = reader.ReadInt32();
        EnemyDamage = reader.ReadInt32();
        EnemyCount = reader.ReadInt32();
        EnemyAsh = reader.ReadInt32();
    }
}
