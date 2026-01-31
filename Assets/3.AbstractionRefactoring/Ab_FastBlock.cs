using UnityEngine;

/// <summary>
/// 빠른 블록 - 이동 속도 3배
/// </summary>
public class Ab_FastBlock : Ab_BaseBlock
{
    [SerializeField] private float speedMultiplier = 3f;

    public string nameTest;

    protected override void Initialize()
    {
        // 속도 증가
        speed *= speedMultiplier;

        Debug.Log("FastBlock 생성 - 3배 속도!");
    }

    public override int GetScore()
    {
        return 1;  // 빠른 블록도 1점 (속도만 다름)
    }
}
