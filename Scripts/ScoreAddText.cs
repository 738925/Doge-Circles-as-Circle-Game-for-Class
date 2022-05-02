using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAddText : MonoBehaviour
{
    void Update()
    {
        //Debug.Log(GetComponent<Transform>().position);
        //Debug.Log(GetComponent<Text>().color);
        GetComponent<Text>().color = new Color(GetComponent<Text>().color.r, GetComponent<Text>().color.g, GetComponent<Text>().color.b, GetComponent<Text>().color .a - 0.004f);
        GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y + 0.004f, GetComponent<Transform>().position.z);
        if (GetComponent<Text>().color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
