using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Transform p_transform;
    [SerializeField] private Rigidbody2D b_rigid;
    [SerializeField] private int b_speed;
    private bool active;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !active)
        {
            Instantiate(transform, p_transform.position, p_transform.rotation);
            active = true;
            b_rigid.AddRelativeForce(new Vector2(b_speed, 0));
        } else if (!active)
        {
            transform.position = new Vector3(p_transform.position.x, p_transform.position.y, 0);
            transform.rotation = p_transform.rotation;
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
