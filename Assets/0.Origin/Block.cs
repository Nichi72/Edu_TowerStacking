using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Block : MonoBehaviour
{
    public enum BlockType  //enum이뭔데?   //BlockType: 👉 “블럭의 종류를 표현하는 자료형(종류 목록)”, 타입이름
    {
        Normal,
        Golden,     // 점수 2배
        Big,        // 큰 블럭
        Fast        // 빠른 블럭
    }
    
   
    public BlockType blockType = BlockType.Normal;  //위의 enum은 “종류 목록”, blockType은 “이 블럭의 현재 종류"이며 변수이름, 기본값은 Normal

    public int index = 0;

    public float speed = 1f;
    public float range = 1f;

    public float startX;

    public bool isDrop = false;

    public bool isFirst = true;
    public bool isDroped = false;
    public ScoreManager scoreManager;

    public string nameTest;

    //public Transform startTr;   // 기준점

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void InitBlock(int index)
    {
        index = this.index;
        this.index = index;
    }
    void Start()
    {
        startX = transform.position.x;

        GameObject scoreManagerObj =  GameObject.Find("ScoreManager"); // 캐싱
        scoreManager =  scoreManagerObj.GetComponent<ScoreManager>();

        //Debug.Log($"맨처음 block 시작점 : {transform.position}");

        ApplyBlockType();
        //Debug.Log($"BlockType {BlockType.Normal}");
        //Debug.Log($"BlockType ToString : {BlockType.Normal.ToString()}");
        //Debug.Log($"BlockType int : {(int)BlockType.Normal}");

        //Debug.Log($"BlockType {BlockType.Big}");
        //Debug.Log($"BlockType ToString : {BlockType.Big.ToString()}");
        //Debug.Log($"BlockType int : {(int)BlockType.Big}");
    }

    // Update is called once per frame
    void Update()
    {

        if (isDrop == true)
        {

        }
        else
        {
            Move();
        }
    }

    void Move()
    {
        float x = Mathf.Sin(Time.time * speed) * range;
        //Debug.Log($"Move전 : {transform.position}");
        transform.position = new Vector3(x + startX, transform.position.y, transform.position.z);
        //Debug.Log($"Move후 : {transform.position}");
    }

    public void Drop()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Collider>().enabled = true;

        isDrop = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isFirst && collision.gameObject.CompareTag("Plane"))
        {
            //260114
            blockSpawner.lastBlock = this;   // ⭐ 기준 블록 설정

            int point = 1;

            if(blockType == BlockType.Golden)
            {
                point = 2;
            }

            scoreManager.AddScore(point);
            return;
        }

        //260114
        // ⭐ 첫 블록이 아닌데 Plane에 닿으면 게임오버
        if (!isFirst && collision.gameObject.CompareTag("Plane"))
        {
            Debug.Log("게임오버 - 바닥에 떨어짐");
            Time.timeScale = 0f;
            return;
        }


        if (isFirst == true)
        {
            return;
        }


        // 드랍이 되었던 블럭이면 충돌체크를 안한다. ( 아래로 로직이 넘어가지 않도록 막아준다.)
        if (isDroped  == true)
        {
            return;
        }


        if (collision.gameObject.CompareTag("Plane"))
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            Debug.Log("Plain에 닿음");
        }

        if (collision.transform.CompareTag("Block"))
        {
            //260114
            // ⭐ 닿은 상대 블록의 Block 스크립트
            Block other = collision.transform.GetComponent<Block>();

            // ⭐ 마지막 기준 블록이 아니면 게임오버
            if (other != blockSpawner.lastBlock)
            {
                Debug.Log("게임오버 - 잘못된 블록에 닿음");
                Time.timeScale = 0f;
                return;
            }

            // ⭐ 정상적으로 쌓였을 때만 실행
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            Debug.Log($"블럭에 닿음 Index : {index}");
            isDroped = true;


            //260114
            blockSpawner.lastBlock = this;

            int point = 1;

            if(blockType == BlockType.Golden)
            {
                point = 2;
            }

            scoreManager.AddScore(point);
        }


        // 현재 문제 : 스코어가 2씩 올라가는 문제
        // 
        // 1. collision될때 아래 블록이 collision이 안되게 하던가,
        // 2. score합산을 아래 블록이 안되게 하던가.
        // 
        // 아래 블럭인지 어떻게 알아내지?
        // 이미 드랍된 블럭은 bool값으로 체크를 해놓자.
        // 이미 드랍된 블럭은 어디에서 설정하지?
        //      드랍 관련된 로직에 추가하면 되겠네 
        // 

        // 
        // UI 추가 , 스코어 추가
        // 블록 타입 나눠서 처리하기

    }


    void ApplyBlockType()
    {
        switch (blockType)
        {
            case BlockType.Golden:
                // 외형으로 표시하고 싶으면 색 변경도 가능
                GetComponent<Renderer>().material.color = Color.yellow;
                break;

            case BlockType.Big:
                transform.localScale *= 1.5f;  //크기를 2배로   //speed *= 1.5f;이거는  speed = speed * 1.5 f;이거랑 같은말임  ->즉, 지금 값에 곱해서 다시 저장해라
                break;

            case BlockType.Fast:
                speed *= 3f;
                break;
        }
    }

}



