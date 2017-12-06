using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager1 : MonoBehaviour {

    private static GameManager1 instance;

    public static GameManager1 Instance
    {
        get
        {
            return instance;
        }
    }

    public int level = 0;  //当前关卡
    public int food = 100;  //主角初始体能

    public AudioClip dieClip;   //死亡的音乐

    [HideInInspector]
    public List<Enemy1> Enmey1List = new List<Enemy1>();

    private bool SleepStep = true;  //敌人休息判断
    [HideInInspector]
    public bool isEnd = false; //是否到达终点

    private Text footText;
    private Text failText;
    private Image dayImage;
    private Text dayText;
    private Player1 player1;
    private MapManager mapManager;

    

	void Awake () {
        instance = this;
        DontDestroyOnLoad(gameObject);  //不要销毁游戏物体
        InitGame();
	}

    public void InitGame()
    {
        mapManager = GetComponent<MapManager>();
        mapManager.InitMap();   //初始化地图

        //初始化UI
        footText = GameObject.Find("FoodText").GetComponent<Text>();      
        UpdateFootText(0);
        failText = GameObject.Find("FailText").GetComponent<Text>();
        failText.enabled = false;

        dayImage = GameObject.Find("DayImage").GetComponent<Image>();
        dayText = GameObject.Find("DayText").GetComponent<Text>();
        dayText.text = "Day" + level;
        Invoke("HideBlack", 1);  //1s后调用HideBlack()函数

        player1 = GameObject.FindGameObjectWithTag("Player").GetComponent<Player1>();

        //初始化参数
        isEnd = false;
        Enmey1List.Clear();
    }

    void UpdateFootText(int foodChange)
    {
        if(foodChange==0)
        {
            footText.text = "  Food:" + food;
        }
        else
        {
            string str = "";
            if(foodChange<=0)
            {
                str = foodChange.ToString();
            }
            else
            {
                str = "+" + foodChange.ToString();
            }
            footText.text = str + "  Food:" + food;
        }
        
    }
	
	public void AddFood(int count)
    {
        food += count;
        UpdateFootText(count);
    }

    public void ReduceFood(int count)
    {
        food -= count;
        UpdateFootText(-count);
        //如果敌人的能量小于等于0，则游戏结束
        if(food<=0)
        {
            failText.enabled = true;
            AudioManager.Instance.StopBgMusic();
            AudioManager.Instance.RandomPlay(dieClip);
        }
    }

    public void OnPlayerMove()
    {
        if (SleepStep == true)  //如果敌人在休息，则不移动，将休息状态取消
        {
            SleepStep = false;
        }
        else  //如果不在休息状态，则将每个敌人移动
        {
            foreach (var enemy in Enmey1List)
            {
                enemy.Move();
            }
            SleepStep = true;
        }
        //检测主角是否到达终点
        if(player1.targetpos.x==mapManager.cols-2&&player1.targetpos.y==mapManager.rows-2)
        {
            isEnd = true; //游戏结束

            //加载下一个关卡
            //Application.LoadLevel(Application.loadedLevel);  //重新加载本关卡,此方法已过时
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); //重新加载本关卡
            
        }
    }

    /// <summary>
    /// OnLevelWasLoaded()函数已经被SceneManager.sceneLoaded取代，具体实现如下
    /// </summary>
    //private void OnLevelWasLoaded(int level1)
    //{
    //    level++;
    //    InitGame();  //重新初始化游戏
    //}

    private void OnEnable()
    {  
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        level++;
        InitGame();  //初始化游戏界面
    }

    //隐藏dayImage的UI
    void HideBlack() 
    {
        dayImage.gameObject.SetActive(false);
    }

}
