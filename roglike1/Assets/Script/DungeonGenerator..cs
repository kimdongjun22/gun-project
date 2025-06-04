using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    public int roomCount = 5;

    void Start()
    {
        Vector2 currentPos = Vector2.zero;

        for (int i = 0; i < roomCount; i++)
        {
            Instantiate(roomPrefab, currentPos, Quaternion.identity);
            currentPos += new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)) * 10f;
        }
    }
}