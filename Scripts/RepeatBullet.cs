using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepeatBullet : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] int minAngle;
    [SerializeField] int maxAngle;
    [SerializeField] int b_speed;
    [SerializeField] float O_rate;
    float rate;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject PlayArea;
    [SerializeField] float offset;
    [SerializeField] Vector3 startPos;
    [SerializeField] Rigidbody2D bd_rigid;
    [SerializeField] RectTransform HealthBar;
    [SerializeField] Transform camTransform;
    [SerializeField] float shakeAmount = 0.7f;
    [SerializeField] float decreaseFactor = 1.0f;
    Vector3 originalPos;
    [SerializeField] FollowMouseLoosely followMouseLoosely;
    [SerializeField] Text scoreText;
    [SerializeField] GameObject EdgeCollisions;
    [SerializeField] GameObject DeathText;
    [SerializeField] GameObject RetryButton;
    [SerializeField] GameObject Cursor;
    [SerializeField] float DifficultyRate;
    [SerializeField] int healthFreq;
    [SerializeField] int scoreFreq;
    [SerializeField] Color32 healthColor;
    [SerializeField] Color scoreBallColor;
    [SerializeField] GameObject PlayButton;
    void Start()
    {
        if (name != "Source")
        { 
            if (Random.Range(0, 100) < healthFreq)
            {
                tag = "health";
                GetComponent<SpriteRenderer>().color = healthColor;
            } else if (Random.Range(0, 100) < scoreFreq)
            {
                tag = "score";
                GetComponent<SpriteRenderer>().color = scoreBallColor;
            }
            Invoke("killbulletclone", 10);
            //add force and rotation
            GetComponent<Rigidbody2D>().simulated = true;
            GetComponent<CircleCollider2D>().isTrigger = false;
            transform.localEulerAngles = new Vector3(0, 0, Random.Range(minAngle, maxAngle));
            Invoke("Push", 0.1f);
        } else
        {
            PlayArea.transform.localScale = new Vector3(PlayArea.transform.localScale.x * 5, PlayArea.transform.localScale.y * 5);
            Player.SetActive(false);
            RetryButton.SetActive(false);
            DeathText.SetActive(false);
            scoreText.gameObject.SetActive(false);
            HealthBar.gameObject.SetActive(false);
        }
    }
    void shrinkPlay()
    {
        if (PlayArea.transform.localScale.x <= 9.89244)
        {
            PlayArea.transform.localScale = new Vector3(9.89244f, PlayArea.transform.localScale.y, PlayArea.transform.localScale.z);
        }
        else
        {
            PlayArea.transform.localScale = new Vector3(PlayArea.transform.localScale.x / 1.01f, PlayArea.transform.localScale.y / 1.01f);
        }
        if (PlayArea.transform.localScale.y <= 6.000025)
        {
            PlayArea.transform.localScale = new Vector3(PlayArea.transform.localScale.x, 6.000025f, PlayArea.transform.localScale.z);
        }
        else
        {
            PlayArea.transform.localScale = new Vector3(PlayArea.transform.localScale.x / 1.01f, PlayArea.transform.localScale.y / 1.01f);
        }
        if (PlayArea.transform.localScale.x == 9.89244 && PlayArea.transform.localScale.y == 6.000025)
        {
            CancelInvoke("shrinkPlay");
        }
    }
    public void StartGame()
    {
        Player.SetActive(false);
        RetryButton.SetActive(false);
        DeathText.SetActive(false);
        scoreText.gameObject.SetActive(false);
        HealthBar.gameObject.SetActive(false);
        Atts.GameStarted = true;
        InvokeRepeating("shrinkPlay", 0f, 0.01f);
        PlayButton.SetActive(false);
        Player.SetActive(true);
        scoreText.gameObject.SetActive(true);
        HealthBar.gameObject.SetActive(true);
        storeDefaults();
            InvokeRepeating("increaseDifficulty", 0.0f, DifficultyRate);
            rate = O_rate;
            Cursor.SetActive(false);
            RetryButton.SetActive(false);
            sh = false;
            HealthBar.sizeDelta = new Vector2(0, 25);
            its = 0;
            InitialHealthIncrease = true;
            scoreTextOBJ.GetComponent<Animator>().SetBool("moveDown", false);
            scoreTextOBJ.GetComponent<Animator>().SetBool("moveUp", true);
            DeathText.SetActive(false);
            EdgeCollisions.SetActive(false);
            originalPos = camTransform.position;
            Atts.hits = 0;
            Atts.shake = 0;
            //disable
            GetComponent<Rigidbody2D>().simulated = false;
            GetComponent<CircleCollider2D>().isTrigger = true;
            InvokeRepeating("InstanBullet", offset, rate);
    }
    void killbulletclone()
    {
        Destroy(gameObject);
    }
    void Push()
    {
        bd_rigid.AddRelativeForce(new Vector2(b_speed, 0));
    }
    private void InstanBullet()
    {
        Instantiate(transform, startPos, transform.rotation);
    }
    [SerializeField] Color hurtColor;
    bool collided;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (name != "Source")
        {
            //bullet hit
            if (collision.gameObject.tag == "Player")
            {
                collided = true;
                if (tag == "health")
                {
                    followMouseLoosely.ScreenCircleEffect(healthColor);
                    if (Atts.hits > 0)
                    {
                        Atts.hits--;
                        Atts.HealthVariableYK = -5;
                    }
                }
                else if (tag == "score")
                {
                    followMouseLoosely.ScreenCircleEffect(scoreBallColor);
                    Atts.score += 20;
                    GameObject newGO = new GameObject("myTextGO");
                    newGO.transform.SetParent(pCanvas.transform);
                    Text CoolText = newGO.AddComponent<Text>();
                    CoolText.text = "+20";
                    newGO.transform.localScale = new Vector3(5, 5, 5);
                    newGO.GetComponent<Text>().font = pArialFont;
                    newGO.GetComponent<Text>().fontSize = 12;
                    newGO.GetComponent<Text>().color = new Color(0.87f, 0.34f, 0.34f, 1.00f);
                    newGO.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                    newGO.transform.position = this.transform.position;
                    newGO.AddComponent<ScoreAddText>();
                }
                else
                {
                    //followMouseLoosely.ScreenCircleEffect(GetComponent<SpriteRenderer>().color);
                    followMouseLoosely.ScreenCircleEffect(hurtColor);
                    Atts.shake = 0.2f;
                    Atts.hits++;
                    Atts.HealthVariableYK = 5;
                }
                Destroy(gameObject);
            }
        }
    }
    void increaseDifficulty()
    {
        rate /= increaseFactor;
        if(increaseFactor > 1)
        {
            CancelInvoke("InstanBullet");
            InvokeRepeating("InstanBullet", 0, rate);
            increaseFactor = (increaseFactor / 12) + 1;
            b_speed += 10;
        }
    }
    float its;
    bool sh;
    bool InitialHealthIncrease;
    [SerializeField] float increaseFactor;
    void Update()
    {
        if (name == "Source" && Atts.GameStarted)
        {
            //Debug.Log(its);
            scoreText.text = Atts.score.ToString();
            //Debug.Log(HealthBar.sizeDelta.x + ", " + (HealthBar.sizeDelta.x - Atts.hits * (400 / health)));
            if (InitialHealthIncrease)
            {
                //Debug.Log(HealthBar.sizeDelta.x);
                if (HealthBar.sizeDelta.x < 400)
                {
                    HealthBar.sizeDelta += new Vector2(its, 0);
                    its += 0.01f;
                }
                else if (HealthBar.sizeDelta.x >= 400)
                {
                    HealthBar.sizeDelta = new Vector2(400, 25);
                    its = 0;
                    InitialHealthIncrease = false;
                }
            }
            else if (its >= 5)
            {
                HealthBar.sizeDelta = new Vector2(400 - Atts.hits * (400 / health), 25);
                its = 0;
            }
            else if (HealthBar.sizeDelta.x > (400 - Atts.hits * (400 / health)))
            {
                HealthBar.sizeDelta -= new Vector2(its, 0);
                its += 0.05f;
            }
            else if (HealthBar.sizeDelta.x < (400 - Atts.hits * (400 / health)))
            {
                HealthBar.sizeDelta += new Vector2(its, 0);
                its += 0.05f;
            }
            else
            {
                its = 0;
            }
            if ((400 - Atts.hits * (400 / health)) == 0 && !sh)
            {
                //lose
                setDefaults();
                CancelInvoke("increaseDifficulty");
                sh = true;
                Invoke("ChangeMenu", 1f);
                EdgeCollisions.SetActive(true);
                HealthBar.sizeDelta = new Vector2(0, 0);
                followMouseLoosely.Shatter();
                CancelInvoke("InstanBullet");
            }
            if (Atts.shake > 0)
            {
                camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

                Atts.shake -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                Atts.shake = 0f;
                camTransform.localPosition = originalPos;
            }
            healthFreq += Atts.HealthVariableYK;
            Atts.HealthVariableYK = 0;
        }
        else if (name != "Source")
        {
            if (transform.position.y > 10 || transform.position.x < -10 || transform.position.x > 10 || transform.position.y < -6)
            {
                BulletExitsScreen();
            }
            if (Atts.hits >= health)
            {
                Destroy(gameObject);
            }
        }
    }
    [SerializeField] GameObject pCanvas;
    [SerializeField] Font pArialFont;
    void BulletExitsScreen()
    {
        //Debug.Log(Atts.hits);
        //Debug.Log(health);
        //Debug.Log(collided);
        if (!collided && Atts.hits < health)
        {
            //Debug.Log("succeeded invisible");
            Atts.score++;
            GameObject newGO = new GameObject("myTextGO");
            newGO.transform.SetParent(pCanvas.transform);
            Text CoolText = newGO.AddComponent<Text>();
            CoolText.text = "+1";
            newGO.transform.localScale = new Vector3(5, 5, 5);
            newGO.GetComponent<Text>().font = pArialFont;
            newGO.GetComponent<Text>().fontSize = 12;
            newGO.GetComponent<Text>().color = new Color(0.87f, 0.34f, 0.34f, 1.00f);
            newGO.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            newGO.transform.position = this.transform.position;
            //Debug.Log(newGO.GetComponent<RectTransform>().localPosition);
            if (newGO.GetComponent<RectTransform>().localPosition.x < -362)
            {
                newGO.GetComponent<RectTransform>().localPosition = new Vector3(-362, newGO.GetComponent<RectTransform>().localPosition.y, newGO.GetComponent<RectTransform>().localPosition.z);
            }
            else if (newGO.GetComponent<RectTransform>().localPosition.x > 362)
            {
                newGO.GetComponent<RectTransform>().localPosition = new Vector3(362, newGO.GetComponent<RectTransform>().localPosition.y, newGO.GetComponent<RectTransform>().localPosition.z);
            }
            else if (newGO.GetComponent<RectTransform>().localPosition.y < -198)
            {
                newGO.GetComponent<RectTransform>().localPosition = new Vector3(newGO.GetComponent<RectTransform>().localPosition.x, -198, newGO.GetComponent<RectTransform>().localPosition.z);
            }
            if (newGO.GetComponent<RectTransform>().localPosition.y > 198)
            {
                newGO.GetComponent<RectTransform>().localPosition = new Vector3(newGO.GetComponent<RectTransform>().localPosition.x, 198, newGO.GetComponent<RectTransform>().localPosition.z);
            }
            newGO.AddComponent<ScoreAddText>();
        }
        Destroy(gameObject);
    }
    void destroyBullet()
    {
        Destroy(gameObject);
    }
    [SerializeField] GameObject scoreTextOBJ;
    void ChangeMenu()
    {
        scoreTextOBJ.GetComponent<Animator>().SetBool("moveDown", true);
        scoreTextOBJ.GetComponent<Animator>().SetBool("moveUp", false);
        Invoke("setThingsActivenet", 1f);
    }
    void setThingsActivenet()
    {
        Cursor.SetActive(true);
        DeathText.SetActive(true);
        RetryButton.SetActive(true);
    }
    public void retryGame()
    {
        scoreTextOBJ.GetComponent<Animator>().SetBool("moveDown", false);
        scoreTextOBJ.GetComponent<Animator>().SetBool("moveUp", true);
        Invoke("StartGame", 1f);
        Atts.hits = 0;
        Atts.score = 0;
        RetryButton.SetActive(false);
        Cursor.SetActive(false);
        DeathText.SetActive(false);
    }
    int dBSpeed;
    int dHealthFreq;
    float dIncreaseFactor;
    void storeDefaults()
    {
        dBSpeed = b_speed;
        dHealthFreq = healthFreq;
        dIncreaseFactor = increaseFactor;
    }
    void setDefaults()
    {
        b_speed = dBSpeed;
        healthFreq = dHealthFreq;
        increaseFactor = dIncreaseFactor;
    }
}