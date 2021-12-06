using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{
    [SerializeField] BallGeneratorScript ballGenerator = default;
    bool isDragging;
    [SerializeField] List<BallScript> removeBalls = new List<BallScript>();
    BallScript currentDraggingBall;
    int score;
    [SerializeField] Text scoreText = default;
    public Text timerText;
    public float timeValue;
    public Text honors;
    public Text comboText;
    private float timeFixer;
    private float comboChecker;    
    private int comboIndex;
    private float timeMultiplier;
    //-------------------------------------------------------------------------------
    public GameObject good;
    public GameObject great;
    public GameObject awesome;
    public GameObject fever;
    //-------------------------------------------------------------------------------
    public GameObject endCanvas;
    public Text endScoretxt;
    public Text highScoretxt;
    public GameObject wall;
    //-------------------------------------------------------------------------------
    public GameObject rank5;
    public GameObject rank4;
    public GameObject rank3;
    public GameObject rank2;
    public GameObject rank1;


    void Start()
    {
        timeValue = 60f;
        score = 0;
        UpdateScore(0);
        StartCoroutine(ballGenerator.Spawns(70));
        comboChecker = 0f;
        timeMultiplier = 1f;
        
    }

    void Update()
    {
        if (timeValue > 0) timeValue -= Time.deltaTime;
        timerText.text = Mathf.FloorToInt(timeValue).ToString();

        if (Input.GetMouseButtonDown(0))
        {            
            OnDragBegin();
        }
        else if (Input.GetMouseButtonUp(0))
        {            
            OnDragEnd();
        }
        else if (isDragging)
        {
            OnDragging();
        }
        if (timeFixer - timeValue > 2f || timeFixer - timeValue < (-2f))
        {
            great.SetActive(false);
            good.SetActive(false);
            awesome.SetActive(false);
            fever.SetActive(false);
        }


        comboChecker += Time.deltaTime;
        
        if (comboIndex >= 2) comboText.text = "x" + comboIndex.ToString();
        
        else comboText.text = null;
        if (comboChecker > 1) {
            comboText.text = null;
            comboIndex = 1;
        }
        
        if (score > 10000) Time.timeScale = 1.3f;
        else if (score > 13000) timeMultiplier = 1.6f;
        else if (score > 17000) timeMultiplier = 1.9f;
        else if (score > 20000) timeMultiplier = 2f;
        else if (score > 25000) timeMultiplier = 2.2f;

        if (timeValue < 0) EndGame();

    }

    void OnDragBegin()
    {
        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (hit && hit.collider.GetComponent<BallScript>())
        {
            BallScript ball = hit.collider.GetComponent<BallScript>();
            AddRemoveBall(ball);
            isDragging = true;
            
            
        }
    }
    void OnDragging()
    {
        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (hit && hit.collider.GetComponent<BallScript>())
        {            
            BallScript ball = hit.collider.GetComponent<BallScript>();
            //hit.collider.GetComponent<SpriteRenderer>().color = new Color(226f, 102f, 91f, 255f);
            if (ball.id == currentDraggingBall.id)
            {                
                float distance = Vector2.Distance(ball.transform.position, currentDraggingBall.transform.position);
                if (distance < 1.5)
                {
                    AddRemoveBall(ball);
                }
                else
                {
                    ball.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 255f);
                }
            }
        }
    }
    void OnDragEnd()
    {
        
        int removeCount = removeBalls.Count;
        if (removeCount >= 3)
        {
            for (int i = 0; i < removeCount; i++)
            {
                Destroy(removeBalls[i].gameObject);
            }
            StartCoroutine(ballGenerator.Spawns(removeCount));            
            UpdateScore(removeCount * 100*comboIndex);
            timeValue += removeCount;
            if (removeCount >= 10)
            {
                fever.SetActive(true);
                comboIndex = 12;
            }
            else if (removeCount >= 6) awesome.SetActive(true);
            else if (removeCount >= 5) great.SetActive(true);
            else if (removeCount >= 4) good.SetActive(true);
            timeFixer = timeValue;
            /////////////////////////////////////////////////////                   COMBO SYSTEM
            
            if (comboChecker < 4f)
            {
                comboIndex++;
                comboChecker = 0f;
            }
            else
            {
                comboIndex = 1;
                comboChecker = 0f;
            }
        }
        removeBalls.Clear();

        isDragging = false;
    }

    void AddRemoveBall(BallScript ball)
    {
        currentDraggingBall = ball;
        //ball.GetComponent<SpriteRenderer>().color = new Color(226f, 102f, 91f, 255f);
        if (removeBalls.Contains(ball) == false)
        {
            removeBalls.Add(ball);
        }
    }

    void UpdateScore(int point)
    {
        score += point;
        scoreText.text = score.ToString();
    }

    public void OnPause()
    {
        Time.timeScale = 0;
    }
    public void OnContinue()
    {
        Time.timeScale = 1;
    }
    public void OnRestart()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }

    void EndGame()
    {
        int rank = 0;
        int savedscore = PlayerPrefs.GetInt("Highscore");
        if (score / savedscore >= 1f) rank = 5;
        else if (score / savedscore >= 0.8f) rank = 4;
        else if (score / savedscore >= 0.6f) rank = 3;
        else if (score / savedscore >= 0.4f) rank = 2;
        else rank = 1;

        if (score >= savedscore)
        {
            PlayerPrefs.SetInt("Highscore", score);
        }
        wall.SetActive(false);
        endCanvas.SetActive(true);
        
        endScoretxt.text=score.ToString();
        highScoretxt.text = PlayerPrefs.GetInt("Highscore").ToString();
        switch (rank)
        {
            case 5:
                rank5.SetActive(true);
                break;
            case 4:
                rank4.SetActive(true);
                break;
            case 3:
                rank3.SetActive(true);
                break;
            case 2:
                rank2.SetActive(true);
                break;
            case 1:
                rank1.SetActive(true);
                break;
            default:
                break;
        }


    }

}
