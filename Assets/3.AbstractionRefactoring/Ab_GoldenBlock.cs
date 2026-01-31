using UnityEngine;

/// <summary>
/// 황금 블록 - 2배 점수
/// </summary>
public class Ab_GoldenBlock : Ab_BaseBlock
{
    protected override void Initialize()
    {
        // 황금색으로 변경
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.yellow;
        }

        Debug.Log("GoldenBlock 생성 - 2배 점수!");
    }

    public override int GetScore()
    {
        return 2;  // 황금 블록은 2점
    }
}
