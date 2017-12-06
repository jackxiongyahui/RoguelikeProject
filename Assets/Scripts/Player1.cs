using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour {

    public float smoothing = 3f;   //移动速度
    public float restTime = 0.5f;  //休息时间
    private float restTimer = 0f;    //休息计时
    public AudioClip chops1Audio;
    public AudioClip chops2Audio;
    public AudioClip step1Audio;
    public AudioClip step2Audio;
    public AudioClip soda1Auido;
    public AudioClip soda2Audio;
    public AudioClip fruit1Audio;
    public AudioClip fruit2Audio;

    [HideInInspector]
    public Vector2 targetpos = new Vector2(1, 1);
    private Rigidbody2D rigidbody2D1;
    private BoxCollider2D boxcollider2D1;
    private Animator animator;

	// Use this for initialization
	void Start () {
        rigidbody2D1 = GetComponent<Rigidbody2D>();
        boxcollider2D1 = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();       
	}
	
	// Update is called once per frame
	void Update () {

        if (GameManager1.Instance.food <= 0 || GameManager1.Instance.isEnd == true)  //停止update函数
            return;

        rigidbody2D1.MovePosition(Vector2.Lerp(transform.position, targetpos, smoothing * Time.deltaTime)); //主角的移动
        restTimer += Time.deltaTime;

        if (restTimer < restTime)
            return;

        float h = Input.GetAxisRaw("Horizontal");  //水平
        float v = Input.GetAxisRaw("Vertical");   //垂直
        //如果水平移动，则垂直移动就变为0
        if(h>0)
        {
            v = 0;
        }
       
        if(h!=0||v!=0)
        {
            //丢失食物
            GameManager1.Instance.ReduceFood(1);
            //检测碰撞
            boxcollider2D1.enabled = false;  //先排除自己
            //发射射线，检测碰撞的物体
            RaycastHit2D hit = Physics2D.Linecast(targetpos,targetpos+new Vector2(h,v));
            boxcollider2D1.enabled = true;  //再恢复自己
            if(hit.transform ==null)  //如果射线检测为空，则移动
            {
                targetpos += new Vector2(h, v);
                AudioManager.Instance.RandomPlay(step1Audio, step2Audio);

            }
            else   //判断和谁碰撞了
            {
                switch(hit.collider.tag)
                {
                    case "OutWall":  //外墙
                        break;
                    case "Wall":   //障碍物
                        animator.SetTrigger("Attack");
                        AudioManager.Instance.RandomPlay(chops1Audio, chops2Audio);
                        hit.collider.SendMessage("TakeDamage");
                        break;
                    case "Food":  //水果
                        GameManager1.Instance.AddFood(10);  //增加体能
                        targetpos += new Vector2(h, v);  //向前移动
                        AudioManager.Instance.RandomPlay(step1Audio, step2Audio);
                        Destroy(hit.transform.gameObject);  //销毁水果
                        AudioManager.Instance.RandomPlay(fruit1Audio, fruit2Audio);
                        break;
                    case "Soda":  //苏打
                        GameManager1.Instance.AddFood(20);
                        targetpos += new Vector2(h, v);
                        AudioManager.Instance.RandomPlay(step1Audio, step2Audio);
                        Destroy(hit.transform.gameObject);
                        AudioManager.Instance.RandomPlay(fruit1Audio, fruit2Audio);
                        break;
                    case "Enemy":  //敌人
                        break;
                }
            }
            GameManager1.Instance.OnPlayerMove();  //敌人的移动
            restTimer = 0;  //不管是攻击还是移动，都需要休息，这里将休息计时器调为0
        }
       
	}

    public void TakeDamage(int lossFood)
    {
        GameManager1.Instance.ReduceFood(lossFood);
    }
}
