using TMPro;
using UnityEngine;

/// <summary>
/// 점수 관리 클래스
/// - 싱글톤이 아닌 일반 클래스로 구현 (GameObject.Find로 찾아서 사용)
/// - Ab_BaseBlock에서 AddScore() 호출
/// </summary>
public class Ab_ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;

    /// <summary>
    /// 점수 추가 - Ab_BaseBlock.OnSuccessfulStack()에서 호출됨
    /// </summary>
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
        
        Debug.Log($"점수 추가: +{points} (총점: {score})");
    }

    /// <summary>
    /// UI 업데이트
    /// </summary>
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    /// <summary>
    /// 현재 점수 반환
    /// </summary>
    public int GetScore()
    {
        return score;
    }

    /// <summary>
    /// 점수 초기화
    /// </summary>
    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }
}
