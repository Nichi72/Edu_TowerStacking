using UnityEngine;

/// <summary>
/// 추상 클래스: 모든 블록의 기본 기능을 정의
/// - 공통 기능은 여기서 구현
/// - 블록별 특수 기능은 추상 메서드로 정의하여 자식 클래스에서 구현
/// </summary>
public abstract class Ab_BaseBlock : MonoBehaviour
{
    #region 기본 변수
    // 이동 관련
    public float speed = 4f;
    public float range = 4f;
    public float startX;

    // 상태 관련
    public bool isDrop = false;
    public bool isFirst = true;
    public bool isDroped = false;
    public int index = 0;

    // 게임오버 판정용
    public static Ab_BaseBlock lastBlock;

    // 컴포넌트 캐싱
    protected Rigidbody rb;
    protected Collider col;
    protected Ab_ScoreManager scoreManager;
    #endregion

    #region Unity 생명주기
    protected virtual void Start()
    {
        // 컴포넌트 캐싱
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
       

        GameObject scoreManagerObj = GameObject.Find("ScoreManager");
        if (scoreManagerObj != null)
        {
            scoreManager = scoreManagerObj.GetComponent<Ab_ScoreManager>();
        }

        // 시작 위치 저장
        startX = transform.position.x;

        // 블록별 초기화 (추상 메서드)
        Initialize();

    }

    protected virtual void Update()
    {
        if (!isDrop)
        {
            Move();
        }
    }
    #endregion

    #region 공통 기능 (구현됨)
    /// <summary>
    /// 좌우 왕복 이동
    /// </summary>
    protected virtual void Move()
    {
        float x = Mathf.Sin(Time.time * speed) * range;
        transform.position = new Vector3(x + startX, transform.position.y, transform.position.z);
    }

    /// <summary>
    /// 블록 드랍
    /// </summary>
    public virtual void Drop()
    {
        if (rb != null) rb.isKinematic = false;
        if (col != null) col.enabled = true;
        isDrop = true;
    }
    #endregion

    #region 추상 메서드 (자식 클래스에서 구현 필수)
    /// <summary>
    /// 블록별 초기화 (외형, 속도 등)
    /// </summary>
    protected abstract void Initialize();

    /// <summary>
    /// 블록별 점수 반환
    /// </summary>
    public abstract int GetScore();
    #endregion

    #region 충돌 처리
    protected void OnCollisionEnter(Collision collision)
    {
        // 첫 블록 - Plane에 닿았을 때
        if (isFirst && collision.gameObject.CompareTag("Plane"))
        {
            lastBlock = this;
            if (rb != null) rb.constraints = RigidbodyConstraints.FreezeAll;
            OnSuccessfulStack();
            return;
        }

        // 게임오버 1: 첫 블록 아닌데 바닥에 떨어짐
        if (!isFirst && collision.gameObject.CompareTag("Plane"))
        {
            OnGameOver("바닥에 떨어짐");
            return;
        }

        if (isFirst || isDroped) return;

        // Block 충돌 처리
        if (collision.gameObject.CompareTag("Block"))
        {
            Ab_BaseBlock other = collision.transform.GetComponent<Ab_BaseBlock>();

            if (other == null)
            {
                Debug.LogError("충돌한 블록에 Ab_BaseBlock 컴포넌트가 없습니다!");
                return;
            }

            // 게임오버 2: 잘못된 블록에 닿음
            if (other != lastBlock)
            {
                OnGameOver("잘못된 블록에 닿음");
                return;
            }

            // 정상 쌓임
            if (rb != null) rb.constraints = RigidbodyConstraints.FreezeAll;
            Debug.Log($"블록 쌓임 Index: {index}");
            isDroped = true;
            lastBlock = this;
            OnSuccessfulStack();
        }
    }

    /// <summary>
    /// 성공적으로 쌓였을 때
    /// </summary>
    protected virtual void OnSuccessfulStack()
    {
        if (scoreManager != null)
        {
            int point = GetScore();  // 블록별 점수 (추상 메서드)
            scoreManager.AddScore(point);
            Debug.Log($"{GetType().Name} 쌓임! 점수: {point}");
        }
    }

    /// <summary>
    /// 게임오버 처리
    /// </summary>
    public virtual void OnGameOver(string reason)
    {
        Debug.Log($"게임오버 - {reason}");
        // 추가 수정
        Time.timeScale = 0f;
    }
    #endregion
}



