using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{
    [SerializeField] BallGeneratorScript ballGenerator = default;
    bool isDragging;
    [SerializeField] List<BallScript> removeBalls = new List<BallScript>();
    BallScript currentDraggingBall;
    int score;
    [SerializeField] Text scoreText = default;

    void Start()
    {
        score = 0;
        UpdateScore(0);
        StartCoroutine(ballGenerator.Spawns(40));
        
    }

    void Update()
    {
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
            UpdateScore(removeCount * 100);
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

}
