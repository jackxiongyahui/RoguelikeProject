using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    //外墙
    public GameObject[] OutWallArray;
    //地板
    public GameObject[] FloorArray;
    //障碍物
    public GameObject[] WallArray;
    //食物
    public GameObject[] FoodArray;
    //敌人
    public GameObject[] EnemyArray;
    //退出
    public GameObject ExitPrefab;

    private GameManager1 gameManager1;
    //地图的宽和高
    public int rows = 10;
    public int cols = 10;

    public int minCountWall = 2;
    public int maxCountWall = 8;

    //创建一个地图管理器
    private Transform mapHolder;

    //创建List存放位置信息
    private List<Vector2> positionList = new List<Vector2>(); 

    
	void Awake () {
       
        
        InitMap();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //初始化地图
    public void InitMap()
    {
        gameManager1 = this.GetComponent<GameManager1>();

        mapHolder = new GameObject("Map").transform;
        
        for(int i = 0; i < cols; i++)
        {
            for(int j = 0; j< rows; j++)
            {

                if (i == 0 || j == 0||i == cols-1||j ==rows-1) //外墙
                {
                    int index = Random.Range(0, OutWallArray.Length);
                    GameObject go = GameObject.Instantiate(OutWallArray[index],new Vector2(i,j),Quaternion.identity);
                    go.transform.SetParent(mapHolder);
                }
                else //地板
                {
                    int index = Random.Range(0, FloorArray.Length);
                    GameObject go = GameObject.Instantiate(FloorArray[index], new Vector2(i, j), Quaternion.identity);
                    go.transform.SetParent(mapHolder);
                }
            }
        }

        positionList.Clear();  //清空positionList
        for(int i = 2; i<cols-2;i++)
        {
            for(int j = 2; j < rows-2; j++)
            {
                positionList.Add(new Vector2(i, j));
            }
        }

        //创建障碍物 食物 敌人
        //创建障碍物
        int WallCount = Random.Range(minCountWall, maxCountWall+1);
        InstantiateItems(WallCount, WallArray);


        //创建食物 2~level*2
        int FoodCount = Random.Range(2, gameManager1.level * 2 + 1);
        InstantiateItems(FoodCount, FoodArray);

        //创建敌人 level/2+1
        int EnemyCount = gameManager1.level / 2+1;
        InstantiateItems(EnemyCount, EnemyArray);

        //创建出口
        GameObject go1 = Instantiate(ExitPrefab, new Vector2(cols - 2, rows - 2), Quaternion.identity) as GameObject;
        go1.transform.SetParent(mapHolder);

    }

    //随机取得位置
    private Vector2 RandomPosition()
    {
        int positionIndex = Random.Range(0, positionList.Count);
        Vector2 pos = positionList[positionIndex];
        positionList.RemoveAt(positionIndex); //移除队列中已经取得的障碍物
        return pos;
    }

    //随机取得障碍物
    private GameObject RandomPrefabs(GameObject[] prefabs)
    {
        int index = Random.Range(0, prefabs.Length);
        return prefabs[index];
    }

    //创建障碍物
    private void InstantiateItems(int count,GameObject[] prefabs)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = RandomPosition();
            GameObject Prefab = RandomPrefabs(prefabs);
            //生成障碍物
            GameObject go = GameObject.Instantiate(Prefab, pos, Quaternion.identity) as GameObject;
            go.transform.SetParent(mapHolder);
        }
        
    }
}
