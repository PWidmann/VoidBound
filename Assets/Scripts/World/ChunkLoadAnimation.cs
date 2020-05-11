using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoadAnimation : MonoBehaviour
{
    float speed = 1.5f;
    Vector3 targetPos;

    float waitTimer;
    float timer;
    private void Start()
    {
        targetPos = transform.position;
        transform.position = new Vector3(transform.position.x, -VoxelData.ChunkHeight, transform.position.z);
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
        if ((targetPos.y - transform.position.y) < 0.05f)
        {
            transform.position = targetPos;
            Destroy(this);
        }
    }
}
