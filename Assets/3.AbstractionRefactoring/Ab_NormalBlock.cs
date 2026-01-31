using UnityEngine;

/// <summary>
/// 일반 블록 - 기본 기능만 사용
/// </summary>
public class Ab_NormalBlock : Ab_BaseBlock
{
    protected override void Initialize()
    {
        // 일반 블록은 추가 초기화 없음
        Debug.Log("NormalBlock 생성");
    }

    public override int GetScore()
    {
        return 1;  // 일반 블록은 1점
    }
}
