using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{

    public Transform target; //당연히 타겟은 플레이어 일 것이다. Transform으로 Target의 위치를 받아온다.
    void Update()
    {
        float dis = Vector3.Distance(transform.position, target.position); //내위치와 target의 위치 사이의 거리를 구함
        if (dis <= 10) // 거리가 10칸 안으로 좁혀졌으면 쫒기 시작
        {
            Move();
        }
        else return;
    }

    void Move()
    {
        float dir = target.position.x - transform.position.x; //2d이기에 좌우만 빼면됨 (내x위치 - targetx위치)
        dir = (dir < 0) ? -1 : 1; //방향조절 dir의 x거리가 -라면 -1,아니면 1
        transform.Translate(new Vector2(dir, 0) * Enemy.speed * Time.deltaTime);
        
    }
}

