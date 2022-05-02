using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform p_transform;
    void Update()
    {
        transform.position = new Vector3(p_transform.position.x, p_transform.position.y, -10);
    }
}
