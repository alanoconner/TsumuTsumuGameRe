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


    void Start()
    {
        timeValue = 60f;
        score = 0;
        UpdateScore(0);
        StartCoroutine(ballGenerator.Spawns(70));
        comboChecker = 0f;
        
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
        if (timeFixer - timeValue > 3f || timeFixer - timeValue < (-3f)) honors.text = "";


        comboChecker += Time.deltaTime;
        
        if (comboIndex >= 2) comboText.text = "x" + comboIndex.ToString();
        
        else comboText.text = null;
        if (comboChecker > 4) {
            comboText.text = null;
            comboIndex = 1;
        }

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
            if (ball.id == currentDraggingBall.id)
            {                
                float distance = Vector2.Distance(ball.transform.position, currentDraggingBall.transform.position);
                if (distance < 1.5)
                {
                    AddRemoveBall(ball);
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
            if (removeCount >= 29)
            {
                honors.text = "Fever!";
                comboIndex = 10;
            }
            else if (removeCount >= 9) honors.text = "Awesome!";
            else if (removeCount >= 6) honors.text = "Great!";
            else honors.text = "Good!";
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

}
