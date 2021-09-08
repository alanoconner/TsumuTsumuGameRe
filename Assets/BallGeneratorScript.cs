using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGeneratorScript : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab = default;
    [SerializeField] Sprite[] balls = default;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Spawns(40    ));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Spawns(int count )
    {
        for(int i = 0; i < count; i++)
        {
            Vector2 position = new Vector2(Random.Range(-0.2f,0.3f),8f);
            GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);

            int ballcolor = Random.Range(0, balls.Length);
            ball.GetComponent<SpriteRenderer>().sprite = balls[ballcolor];
            ball.GetComponent<BallScript>().id = ballcolor;

            yield return new WaitForSeconds(0.03f);
        }
        
    }
}
