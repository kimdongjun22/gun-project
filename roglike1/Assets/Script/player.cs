using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

//State
    public static float maxHP; //최대체력
    public static float nowHP; //현재체력
    public static float Speed; //속도
    public static float AD; // 물리공격력
    public static float AP; // 마법공격력
    public static float Avoid; // 회피력
    public static float Criticaldmg; //치명타데미지
    public static float Critical; //치명타 확률
    
     //Defalt
    public static float DefaltHP;
    public static float Defaltspeed;
    public static float DefaltAD;
    public static float DefaltAP;
    public static float DefaltAvoid;
    public static float DefaltCriticaldmg;
    public static float DefaltCritical;
    
     //Rigidbody, Gameobject, Transform
    public static Rigidbody2D rigid;
    
     void Start()
    {
        maxHP = 200;
        nowHP = 200;
        Speed = 6;
        AD = 10;
        AP = 10;
        Avoid = 0;
        Criticaldmg = 1;
        Critical = 0;
        rigid = GetComponent<Rigidbody2D>();
        
        //Defalt Value;
        Defaltspeed = Speed;
        DefaltAD = AD;
        DefaltAP = AP;
        DefaltAvoid = Avoid;
        DefaltCritical = Critical;
        DefaltCriticaldmg = Criticaldmg;
        DefaltHP = maxHP;

    }
    
    private void Update()
    {
    	Move();
    }
    
     void Move() //기본적인 좌우 이동
    {

        float h = Input.GetAxis("Horizontal");
        rigid.velocity = new Vector2(h * Speed, rigid.velocity.y);
    }
 }
