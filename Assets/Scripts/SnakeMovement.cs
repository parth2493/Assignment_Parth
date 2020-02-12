using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeMovement : MonoBehaviour
{
    public List<Transform> BodyPartsList = new List<Transform>();
    public float minDistance = 0.25f;
    public float speed = 1.0f;
    public float rotationSpeed = 50.0f;
    public int beginSize;
    public GameObject bodyPrefab;
    public float TimeFromLastRetry;
    
    public Text CurrentScore;
    public Text ScoreText;
    public GameObject DeadScreen;
    public bool isAlive;

    private float distance;
    private Transform currBodyPart;
    private Transform PrevBodyPart;
    
    void Start()
    {
        StartLevel();
    }

    public void StartLevel()
    {
        TimeFromLastRetry = Time.time;
        DeadScreen.SetActive(false);

        for(int i = BodyPartsList.Count - 1 ; i > 1; i--)
        {
            Destroy(BodyPartsList[i].gameObject);
            BodyPartsList.Remove(BodyPartsList[i]);
        }

        BodyPartsList[0].position = new Vector3(0, 0.5f, 0);
        BodyPartsList[0].rotation = Quaternion.identity;

        CurrentScore.gameObject.SetActive(true);
        CurrentScore.text = "Score : 0";

        isAlive = true;

        for (int i = 0; i < beginSize - 1; i++)
        {
            AddBodyPart();
        }

        BodyPartsList[0].position = new Vector3(2, 0.5f, 0);
    }


    void Update()
    {
        if(isAlive == true)
        {
            SnakeMove();
        }
        
    }

    void SnakeMove()
    {
        float currspeed = speed;
        if (Input.GetKey(KeyCode.W))
        {
            currspeed *= 2;
        }

        BodyPartsList[0].Translate(BodyPartsList[0].forward * currspeed * Time.smoothDeltaTime, Space.World);

        if(Input.GetAxis("Horizontal") != 0)
        {
            BodyPartsList[0].Rotate(Vector3.up * rotationSpeed * Time.deltaTime * Input.GetAxis("Horizontal"));
        }

        for (int i = 1; i < BodyPartsList.Count; i++)
        {
            currBodyPart = BodyPartsList[i];
            PrevBodyPart = BodyPartsList[i - 1];

            distance = Vector3.Distance(PrevBodyPart.position, currBodyPart.position);

            Vector3 newPos = PrevBodyPart.position;

            newPos.y = BodyPartsList[0].position.y;

            float time = Time.deltaTime * distance / minDistance * currspeed;

            if(time > 0.5f)
            {
                time = 0.5f;
            }
            currBodyPart.position = Vector3.Slerp(currBodyPart.position, newPos, time);
            currBodyPart.rotation = Quaternion.Slerp(currBodyPart.rotation, PrevBodyPart.rotation, time);
        }
    }

    public void AddBodyPart()
    {
        Transform newPart = (Instantiate(bodyPrefab, BodyPartsList[BodyPartsList.Count - 1].position, BodyPartsList[BodyPartsList.Count - 1].rotation) as GameObject).transform;

        newPart.SetParent(transform);
        BodyPartsList.Add(newPart);

        CurrentScore.text = "Score : " + (BodyPartsList.Count - beginSize).ToString();
    }

    public void Die()
    {
        isAlive = false;
        ScoreText.text = "Your Score :" + (BodyPartsList.Count - beginSize).ToString();

        CurrentScore.gameObject.SetActive(false);
        DeadScreen.SetActive(true);
    }
}
