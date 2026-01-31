using System;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class blockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;

    private GameObject blockTemp = null;

    public GameObject startTr;

    public GameObject cameraUp;

    //260114
    public static Block lastBlock;//👉 이건 “기준 블록을 저장할 자리” 만 만든 상태야. 아직 아무 값도 안 들어 있음(null 상태)

    public int currentIndex = 0;

    
    public float speed = 1f;
    public float range = 1f;
    public float startUp = 1f;

    private bool isFirstBlock = true; 



    void Start()
    {

        createBlock();

        //blockTemp.GetComponent<Collider>().enabled = true;
        float a  = 10.5f;
        int b = (int)a; // -> 10
    }

  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            blockTemp.GetComponent<Block>().Drop();

            blockTemp.GetComponent<Block>().isFirst = isFirstBlock;
            isFirstBlock = false;



            createBlock();

           
        }

    }

    void createBlock()
    {
        blockTemp = Instantiate(blockPrefab);


        //Block 컴포넌트를 가져온다  ;;물어보기;;  block.blockType = Block.BlockType.Golden;로 연결되는거
        Block block = blockTemp.GetComponent<Block>();
        blockTemp.transform.position = startTr.transform.position;
        blockTemp.GetComponent<Rigidbody>().isKinematic = true;
        blockTemp.GetComponent<Collider>().enabled = false;
        blockTemp.GetComponent<Block>().index = currentIndex;

        // 여기서 난이도 계산
        int difficultyLevel = currentIndex / 5;  // 블럭 5개마다 증가 // @잘 모르겠음  5개  5/5  = 1  , 10/5 = 2 12/5 = 2.25 
        float newSpeed = 1f + difficultyLevel * 1f;



        // Block에 속도 전달
        blockTemp.GetComponent<Block>().speed = newSpeed;



        //특수 블럭 랜덤 결정
        int rand = UnityEngine.Random.Range(0, 10);  //0~9 // @잘 모르겠음 // 0~ 100 10f 
        // 0~9        0 ,1, 2, 3, 
        if (0<= rand && rand <= 2)   //if((0<= rand && rand <= 4)이런식도가능
        {
            block.blockType = Block.BlockType.Golden;
        }
        else if (rand == 3)
        {
            block.blockType = Block.BlockType.Big;
        }
        else if (rand == 4)
        {
            block.blockType = Block.BlockType.Fast;
        }
        else
        {
            block.blockType = Block.BlockType.Normal;
        }


        currentIndex += 1;

        startTr.transform.position = new Vector3(startTr.transform.position.x, startTr.transform.position.y + startUp, startTr.transform.position.z);
        cameraUp.transform.position = new Vector3(cameraUp.transform.position.x, cameraUp.transform.position.y + startUp, cameraUp.transform.position.z);


        

    }


    
}

