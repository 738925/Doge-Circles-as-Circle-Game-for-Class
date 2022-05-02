using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public void StartGame()
    {
        Cursor.visible = false;
    }
    void Update()
    {
        //transform.position = Input.mousePosition;
        transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 10);
    }
}
