using UnityEngine;

/// <summary>
/// 블록 스포너 - 추상화된 블록들을 생성하고 관리
/// - 모든 Ab_BaseBlock 계열 블록 지원
/// - 난이도 시스템
/// - 랜덤 블록 생성
/// </summary>
public class Ab_BaseBlockSpawner : MonoBehaviour
{
    #region Inspector 설정
    [Header("블록 프리팹들")]
    [SerializeField] private GameObject normalBlockPrefab;
    [SerializeField] private GameObject goldenBlockPrefab;
    [SerializeField] private GameObject bigBlockPrefab;
    [SerializeField] private GameObject fastBlockPrefab;

    [Header("위치 참조")]
    public GameObject startTr;      // 블록 생성 위치
    public GameObject cameraUp;     // 카메라 객체

    [Header("스포너 설정")]
    public float startUp = 1f;      // 블록 생성 시 상승 높이

    [Header("출현 확률 설정 (0~10)")]
    [Tooltip("골든 블록 확률 (0~2 = 30%)")]
    [SerializeField] private int goldenProbability = 2;
    [Tooltip("큰 블록 확률 (3 = 10%)")]
    [SerializeField] private int bigProbability = 3;
    [Tooltip("빠른 블록 확률 (4 = 10%)")]
    [SerializeField] private int fastProbability = 4;

    [Header("난이도 설정")]
    [Tooltip("몇 개마다 난이도 증가?")]
    [SerializeField] private int difficultyInterval = 5;
    [Tooltip("난이도당 속도 증가량")]
    [SerializeField] private float speedIncreasePerLevel = 1f;
    #endregion

    #region 상태 변수
    private GameObject currentBlock = null;
    private int currentIndex = 0;
    private bool isFirstBlock = true;
    #endregion

    #region Unity 생명주기
    private void Start()
    {
        Debug.Log("Ab_BaseBlockSpawner 초기화");
        CreateBlock();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DropCurrentBlock();
            CreateBlock();
        }
    }
    #endregion

    #region 블록 생성 및 관리

    /// <summary>
    /// 블록 생성 - 랜덤 타입 선택 및 난이도 적용
    /// </summary>
    private void CreateBlock()
    {
        // 1. 랜덤으로 블록 타입 결정
        GameObject selectedPrefab = SelectRandomBlockPrefab();

        if (selectedPrefab == null)
        {
            Debug.LogError("선택된 블록 프리팹이 없습니다!");
            return;
        }

        // 2. 블록 생성
        currentBlock = Instantiate(selectedPrefab); // 골든 큐브
        Ab_BaseBlock block = currentBlock.GetComponent<Ab_BaseBlock>();

        if (block == null)
        {
            Debug.LogError("블록에 Ab_BaseBlock 컴포넌트가 없습니다!");
            Destroy(currentBlock);
            return;
        }

        // 3. 기본 설정 (위치, 물리, 인덱스)
        SetupBlock(currentBlock, block);

        // 4. 난이도에 따른 속도 설정
        ApplyDifficulty(block);

        // 5. 인덱스 증가
        currentIndex++;

        // 6. 위치 업데이트
        UpdatePositions();
    }

    /// <summary>
    /// 현재 블록 드랍
    /// </summary>
    private void DropCurrentBlock()
    {
        if (currentBlock == null) return;

        Ab_BaseBlock block = currentBlock.GetComponent<Ab_BaseBlock>();
        if (block != null)
        {
            block.Drop();
            block.isFirst = isFirstBlock;
            isFirstBlock = false;
        }
    }

    /// <summary>
    /// 블록 기본 설정
    /// </summary>
    private void SetupBlock(GameObject blockObj, Ab_BaseBlock block)
    {
        // 위치 설정
        if (startTr != null)
        {
            blockObj.transform.position = startTr.transform.position;
        }

        // 물리 설정
        Rigidbody rb = blockObj.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider col = blockObj.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // 인덱스 설정
        block.index = currentIndex;
    }

    /// <summary>
    /// 위치 업데이트
    /// </summary>
    private void UpdatePositions()
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
    #endregion

    #region 블록 선택 및 난이도
    /// <summary>
    /// 확률 기반 랜덤 블록 선택
    /// </summary>
    private GameObject SelectRandomBlockPrefab()
    {
        int rand = Random.Range(0, 10);

        // 골든 블록 (0~2 = 30%)
        if (rand <= goldenProbability && goldenBlockPrefab != null)
        {
            return goldenBlockPrefab;
        }
        // 큰 블록 (3 = 10%)
        else if (rand == bigProbability && bigBlockPrefab != null)
        {
            return bigBlockPrefab;
        }
        // 빠른 블록 (4 = 10%)
        else if (rand == fastProbability && fastBlockPrefab != null)
        {
            return fastBlockPrefab;
        }
        // 일반 블록 (5~9 = 50%)
        else
        {
            return normalBlockPrefab;
        }
    }

    /// <summary>
    /// 난이도에 따른 속도 증가
    /// </summary>
    private void ApplyDifficulty(Ab_BaseBlock block)
    {
        int difficultyLevel = currentIndex / difficultyInterval;
        float speedBonus = difficultyLevel * speedIncreasePerLevel;
        block.speed += speedBonus;

        if (difficultyLevel > 0)
        {
            Debug.Log($"난이도 레벨: {difficultyLevel}, 속도 보너스: +{speedBonus}");
        }
    }
    #endregion

    #region 디버그
    [ContextMenu("블록 타입별 확률 표시")]
    private void ShowProbabilities()
    {
        float goldenPercent = ((goldenProbability + 1) / 10f) * 100f;
        float bigPercent = (1 / 10f) * 100f;
        float fastPercent = (1 / 10f) * 100f;
        float normalPercent = 100f - goldenPercent - bigPercent - fastPercent;

        Debug.Log("=== 블록 출현 확률 ===");
        Debug.Log($"골든 블록: {goldenPercent}%");
        Debug.Log($"큰 블록: {bigPercent}%");
        Debug.Log($"빠른 블록: {fastPercent}%");
        Debug.Log($"일반 블록: {normalPercent}%");
    }
    #endregion
}
