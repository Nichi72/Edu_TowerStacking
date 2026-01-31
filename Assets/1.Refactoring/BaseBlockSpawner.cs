using UnityEngine;

/// <summary>
/// 블록 생성 및 관리의 기본 기능을 담당하는 베이스 클래스
/// - 블록 생성
/// - 드랍 처리
/// - 위치 및 카메라 관리
/// - 인덱스 관리
/// </summary>
public class BaseBlockSpawner : MonoBehaviour
{
    [Header("프리팹 설정")]
    [SerializeField] protected GameObject blockPrefab;

    [Header("위치 참조")]
    public GameObject startTr;      // 블록 생성 위치
    public GameObject cameraUp;     // 카메라 객체

    [Header("스포너 설정")]
    public float startUp = 1f;      // 블록 생성 시 상승 높이

    // 현재 상태
    protected GameObject currentBlock = null;   // 현재 활성화된 블록
    protected int currentIndex = 0;             // 현재 블록 인덱스
    protected bool isFirstBlock = true;         // 첫 블록 여부

    void Start()
    {
        Initialize();
        CreateBlock();
    }

    void Update()
    {
        HandleInput();
    }

    /// <summary>
    /// 초기화 (상속 클래스에서 오버라이드 가능)
    /// </summary>
    protected virtual void Initialize()
    {
        // 상속 클래스에서 추가 초기화
    }

    /// <summary>
    /// 입력 처리
    /// </summary>
    protected virtual void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DropCurrentBlock();
            CreateBlock();
        }
    }

    /// <summary>
    /// 현재 블록을 드랍시킴
    /// </summary>
    protected void DropCurrentBlock()
    {
        if (currentBlock == null) return;

        BaseBlock block = currentBlock.GetComponent<BaseBlock>();
        if (block != null)
        {
            block.Drop();

            // 첫 블록 여부 설정
            block.isFirst = isFirstBlock;
            isFirstBlock = false;  // 첫 블록 이후는 모두 false
        }
    }

    /// <summary>
    /// 새로운 블록 생성
    /// </summary>
    protected void CreateBlock()
    {
        if (blockPrefab == null)
        {
            Debug.LogError("BlockPrefab이 설정되지 않았습니다!");
            return;
        }

        // 블록 인스턴스 생성
        currentBlock = Instantiate(blockPrefab);

        // 블록 컴포넌트 가져오기
        BaseBlock block = currentBlock.GetComponent<BaseBlock>();
        if (block == null)
        {
            Debug.LogError("BlockPrefab에 BaseBlock 컴포넌트가 없습니다!");
            return;
        }

        // 블록 초기 설정
        SetupBlock(block);

        // 인덱스 증가
        currentIndex++;

        // 위치 업데이트
        UpdatePositions();
    }

    /// <summary>
    /// 블록 초기 설정
    /// </summary>
    protected void SetupBlock(BaseBlock block)
    {
        // 위치 설정
        if (startTr != null)
        {
            currentBlock.transform.position = startTr.transform.position;
        }

        // 물리 설정
        Rigidbody rb = currentBlock.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        Collider col = currentBlock.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        // 인덱스 설정
        block.index = currentIndex;
    }

    /// <summary>
    /// 스폰 위치 및 카메라 위치 업데이트
    /// </summary>
    protected void UpdatePositions()
    {
        if (startTr != null)
        {
            startTr.transform.position += new Vector3(0, startUp, 0);
        }

        if (cameraUp != null)
        {
            cameraUp.transform.position += new Vector3(0, startUp, 0);
        }
    }
}
