using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouseLoosely : MonoBehaviour
{
    public void StartGame()
    {
        if (name != "Player")
        {
            GetComponent<Rigidbody2D>().freezeRotation = false;
            transform.localScale = new Vector2(0.6f, 0.6f);
            transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            Invoke("Push", 0.01f);
        }
    }
    private void Start()
    {
        GetComponent<Rigidbody2D>().freezeRotation = true;
        Atts.PRScale = 5;
        Cursor.visible = false;
    }
    void Push()
    {
        GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(25, 10));
    }
    float temp12;
    void Update()
    {
        Vector2 pos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x / 2, Camera.main.ScreenToWorldPoint(Input.mousePosition).y / 2);
        if (name == "Player" && Atts.GameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            if (Atts.PRScale >= 1)
            {
                temp12 = 0.6f;
            } else
            {
                temp12 = 0;
            }
            transform.localScale = new Vector2((Atts.PRScale - 1) * 0.1f + temp12, (Atts.PRScale - 1) * 0.1f + temp12);
            transform.position = pos;
        } else if (name != "Player")
        {
            if (Atts.hits == 0)
            {
                //Debug.Log(Vector2.Distance(transform.position, pos));
                GetComponent<Rigidbody2D>().simulated = false;
                transform.position = Vector3.MoveTowards(transform.position, pos, 10 * Time.deltaTime);
                if (Vector2.Distance(transform.position, pos) <= 0.2)
                {
                    Atts.PRScale++;
                    //Debug.Log(Atts.PRScale);
                    Destroy(gameObject);
                }
            }
        }
    }
    public void Shatter()
    {
        Atts.PRScale = 0;
        Atts.shake = 1;
        Instantiate(transform, transform.position, transform.rotation);
        Instantiate(transform, transform.position, transform.rotation);
        Instantiate(transform, transform.position, transform.rotation);
        Instantiate(transform, transform.position, transform.rotation);
        Instantiate(transform, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
    public void retryGame()
    {
        gameObject.SetActive(true);
    }
    private Color ee_color;
    [SerializeField] GameObject ScreenCircle;
    public void ScreenCircleEffect(Color color)
    {
        if (ScreenCircle.transform.localScale == new Vector3(0, 0, 0))
        {
            ee_color = color;
            ScreenCircle.GetComponent<SpriteRenderer>().color = ee_color;
            ScreenCircle.transform.localScale = new Vector3(0, 0, 0);
            InvokeRepeating("ScreenCircleMake", 0, 0.01f);
        } else
        {
            Atts.screenCircleColor = color;
            Instantiate(ScreenCircle);
        }
    }
    void ScreenCircleMake()
    {
        ScreenCircle.transform.localScale += new Vector3(0.3f, 0.3f, 0);
        ScreenCircle.GetComponent<SpriteRenderer>().color = new Color(ScreenCircle.GetComponent<SpriteRenderer>().color.r, ScreenCircle.GetComponent<SpriteRenderer>().color.g, ScreenCircle.GetComponent<SpriteRenderer>().color.b, ScreenCircle.GetComponent<SpriteRenderer>().color.a - 0.013f);
        if (ScreenCircle.GetComponent<SpriteRenderer>().color.a <= 0)
        {
            ScreenCircle.transform.localScale = new Vector3(0, 0, 0);
            ScreenCircle.GetComponent<SpriteRenderer>().color = new Color(ScreenCircle.GetComponent<SpriteRenderer>().color.r, ScreenCircle.GetComponent<SpriteRenderer>().color.g, ScreenCircle.GetComponent<SpriteRenderer>().color.b, 1);
            CancelInvoke("ScreenCircleMake");
        }
    }
}