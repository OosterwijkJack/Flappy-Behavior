using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    public float moveSpeed = 5f;
 
    void Update()
    {
        this.transform.position += Vector3.right * Time.deltaTime * moveSpeed;
    }
}
