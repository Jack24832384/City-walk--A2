using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacteraController : MonoBehaviour
{
    public float speed = 8;
    public float rotateSpeed = 0.2f;
    private Animator ani;
    private DateTime markedDate;

    void Start()
    {
        ani = GetComponent<Animator>();
    }

    private float h, v = 0;
    private Vector3 dir;


    void Update()
    {
        if (GameManager.Instance.IsDead 
            || GameManager.Instance.WinFlag 
            || GameManager.Instance.isPaused)
        {
            return;
        }

        h = Input.GetAxis("Horizontal");
        //print("h=" + Mathf.Abs(h));
        //if (Mathf.Abs(h) > 0.8f)
        //{
        //    timer += Time.deltaTime;
        //    print("timer=" + timer);
        //    if (timer > 0.5f)
        //    {
        //        h = 0;
        //    }
        //}
        //else
        //{
        //    timer = 0;
        //}

        v = Input.GetAxis("Vertical");
        if (v != 0)
        {
            ani.SetFloat("Vert", 1);
        }
        else
        {
            ani.SetFloat("Vert", 0);
        }

        dir = new Vector3(0, 0, v);
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - 90, transform.eulerAngles.z);
        //}
        //else if (Input.GetKeyDown(KeyCode.D))
        //{
        //    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 90, transform.eulerAngles.z);
        //}

        transform.Rotate(h * rotateSpeed * Vector3.up);
        transform.Translate(speed * Time.deltaTime * dir);
    }

    private void OnTriggerEnter(Collider _other)
    {
        print("" + _other.name);
        if (_other.CompareTag("Coin") || _other.CompareTag("Corn") || _other.CompareTag("Meat"))
        {
            GameManager.Instance.ScoreIncrease();
            _other.gameObject.SetActive(false);
        }
        else if (_other.CompareTag("Car") || _other.CompareTag("House"))
        {
            if (markedDate == null)
            {
                markedDate = DateTime.Now;
            }
            else
            {
                if (DateTime.Now.Subtract(markedDate).TotalMilliseconds < 500f )
                {
                    return;
                }
                markedDate = DateTime.Now;
            }
            GameManager.Instance.LifeDecrease();
        }
    }
}
