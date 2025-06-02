using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public static void Activate(Vector3 center)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(center, 3f);
        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Monster"))
            {
                enemy.GetComponent<Monster>().TakeDamage(50);
            }
        }
    }
}
