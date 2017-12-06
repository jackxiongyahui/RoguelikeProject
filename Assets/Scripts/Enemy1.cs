using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour {

    private Transform player;  //玩家
    private Vector2 targetPosition;

    public float smoothing = 4f;  //移动速度
    public int lossFood = 10;  //丢失体能量

    private Rigidbody2D rigidbody2D2;  //刚体

    private BoxCollider2D boxcollider;  //碰撞器
    private Animator animator; //动画状态机

    public AudioClip attackAudio;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;  //得到玩家
        rigidbody2D2 = GetComponent<Rigidbody2D>();
        boxcollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        targetPosition = transform.position;
        GameManager1.Instance.Enmey1List.Add(this);

    }

    private void Update()
    {
        rigidbody2D2.MovePosition(Vector2.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime)); //敌人的移动
       
    }

    public void Move()
    {
        Vector2 offest = player.position - transform.position; 
        if(offest.magnitude<2.1f&&offest.magnitude>1.1f)  //如果主角与敌人之间的间距小于2，敌人不移动，不然会重合
        {
            return;
        }
        else if (offest.magnitude <= 1.1f)
        {
            //攻击
            animator.SetTrigger("Attack");
            //播放音乐
            AudioManager.Instance.RandomPlay(attackAudio);  
            player.SendMessage("TakeDamage", lossFood);
        }
        else
        {
            float x = 0, y = 0;
            //追赶
            if (Mathf.Abs(offest.y) > Mathf.Abs(offest.x))
            {
                //按照y轴移动
                if (offest.y < 0)
                {
                    y = -1;
                }
                else
                {
                    y = 1;
                }
            }
            else
            {
                //按照x轴移动
                if (offest.x > 0)
                {
                    x = 1;
                }
                else
                {
                    x = -1;
                }
            }
            //设置目标位置之前先做检测
            boxcollider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(targetPosition, targetPosition + new Vector2(x, y));
            //Debug.Log(hit.transform);
            boxcollider.enabled = true;
            if (hit.transform == null)
            {
                targetPosition += new Vector2(x, y);
            }
            else
            {
                if (hit.collider.tag == "Food" || hit.collider.tag == "Soda")
                {
                    targetPosition += new Vector2(x, y);
                }
            }
        }


    }
}
