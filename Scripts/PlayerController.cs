using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int p_speed;
    [SerializeField] private Transform p_cursor;
    private Transform p_transform;
    private Rigidbody2D p_rigid;
    public void Start()
    {
        p_transform = GetComponent<Transform>();
        p_rigid = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        p_transform.right = p_cursor.position - p_transform.position;
        if (Input.GetKey("w"))
        {
            p_rigid.AddRelativeForce(new Vector2(p_speed, 0));
        }
        if (Input.GetKey("s"))
        {
            p_rigid.AddRelativeForce(new Vector2(-p_speed, 0));
        }
    }
}
