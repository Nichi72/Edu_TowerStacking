using UnityEngine;

/// <summary>
/// 큰 블록 - 크기 1.5배
/// </summary>
public class Ab_BigBlock : Ab_BaseBlock
{
    [SerializeField] private float sizeMultiplier = 1.5f;

    protected override void Initialize()
    {
        // 크기 증가
        transform.localScale *= sizeMultiplier;

        Debug.Log("BigBlock 생성 - 1.5배 크기!");
    }

    public override int GetScore()
    {
        return 1;  // 큰 블록도 1점 (크기만 다름)
    }
}
