using UnityEngine;

public class GoldenBlock : MonoBehaviour
{
  // 기본 이동 관련 변수
    public float speed = 4f;        // 이동 속도
    public float range = 4f;        // 이동 범위
    public float startX;            // 시작 X 위치

    // 상태 관련 변수
    public bool isDrop = false;     // 드랍 여부
    public bool isFirst = true;     // 첫 블록 여부
    public bool isDroped = false;   // 이미 드랍되었는지 여부
    public int index = 0;           // 블록 인덱스

    // 기준 블록 (게임오버 판정용)
    public static GoldenBlock lastBlock;  // 마지막으로 성공적으로 쌓인 블록 // @이 부분만 GoldenBlock으로 변경

    protected Rigidbody rb;
    protected Collider col;

    void Start()
    {
        // 컴포넌트 캐싱
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        // 시작 위치 저장
        startX = transform.position.x;

        // 추가 초기화 (상속 클래스에서 오버라이드 가능)
        Initialize();
    }

    void Update()
    {
        // 드랍 전에만 이동
        if (!isDrop)
        {
            Move();
        }
    }

    /// <summary>
    /// 좌우로 왕복하는 이동 로직
    /// </summary>
    protected virtual void Move()
    {
        float x = Mathf.Sin(Time.time * speed) * range;
        transform.position = new Vector3(x + startX, transform.position.y, transform.position.z);
    }

    /// <summary>
    /// 블록을 드랍시키는 메서드
    /// </summary>
    public virtual void Drop()
    {
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        if (col != null)
        {
            col.enabled = true;
        }

        isDrop = true;
    }

    /// <summary>
    /// 초기화 메서드 (상속 클래스에서 오버라이드)
    /// </summary>
    protected virtual void Initialize()
    {
        // 상속 클래스에서 추가 초기화 구현
    }

    /// <summary>
    /// 충돌 처리 메서드
    /// </summary>
    protected void OnCollisionEnter(Collision collision)
    {
        #region 예외 처리
        Debug.Log($"isFirst : {isFirst}");
        // ⭐ 첫 블록이 Plane에 닿았을 때
        if (isFirst && collision.gameObject.CompareTag("Plane"))
        {
            // 기준 블록 설정
            // lastBlock = this;
            lastBlock = this;

            // 물리 고정
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }

            // 점수 처리는 상속 클래스에서
            OnSuccessfulStack();
            return;
        }

        // ⭐ 게임오버 조건 1: 첫 블록이 아닌데 Plane에 닿으면
        if (!isFirst && collision.gameObject.CompareTag("Plane"))
        {
            OnGameOver("바닥에 떨어짐");
            return;
        }

        // 첫 블록이면 이후 로직 스킵
        if (isFirst)
        {
            return;
        }

        // 이미 드랍된 블록이면 충돌 체크 안함
        if (isDroped)
        {
            return;
        }
        #endregion

        // Plane 충돌 처리 (여기 도달하면 문제 있는 상황)
        if (collision.gameObject.CompareTag("Plane"))
        {
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
            Debug.Log("Plane에 닿음");
        }

        // ⭐ Block 충돌 처리
        if (collision.gameObject.CompareTag("Block"))
        {
            // 닿은 상대 블록의 BaseBlock 스크립트
            BaseBlock other = collision.transform.GetComponent<BaseBlock>();
            

            if (other == null)
            {
                Debug.LogError("충돌한 블록에 BaseBlock 컴포넌트가 없습니다!");
                return;
            }

            // ⭐ 게임오버 조건 2: 마지막 기준 블록이 아니면
            if (other != lastBlock)
            {
                OnGameOver("잘못된 블록에 닿음");
                return;
            }

            // ⭐ 정상적으로 쌓였을 때
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }

            Debug.Log($"블록에 닿음 Index : {index}");
            isDroped = true;

            // 기준 블록 업데이트
            lastBlock = this;

            // 점수 처리는 상속 클래스에서
            OnSuccessfulStack();
        }
    }

    /// <summary>
    /// 게임오버 처리
    /// </summary>
    protected virtual void OnGameOver(string reason)
    {
        Debug.Log($"게임오버 - {reason}");
        Time.timeScale = 0f;
    }

    /// <summary>
    /// 성공적으로 블록이 쌓였을 때 호출 (점수 처리 등)
    /// </summary>
    protected virtual void OnSuccessfulStack()
    {
        // 상속 클래스에서 점수 처리 등 구현
    }
}
