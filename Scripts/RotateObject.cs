using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public Vector3 dir;
    public float speed = 220;

    void Update()
    {
        transform.Rotate(dir, Time.deltaTime * speed);
    }
}
