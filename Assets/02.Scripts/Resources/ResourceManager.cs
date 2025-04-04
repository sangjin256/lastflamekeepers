using System.Collections.Generic;

// 게임 내 자원의 현재 개수와 최대 보유량을 관리하는 클래스
// 필드 자원 : 나무(Wood), 돌(Rock)
// 전투 자원 : 잿가루(Ash)

public class ResourceManager : BehaviourSingleton<ResourceManager>
{

    private List<int> CurrentResourceCountList;
}
