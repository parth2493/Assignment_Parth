using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    public SnakeMovement snakeMovement;
    public SpwanFood spwanFood;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Food")
        {
            snakeMovement.AddBodyPart();

            Destroy(collision.gameObject);
            spwanFood.SpwanFoodObject();
        }
        else if(collision.transform != snakeMovement.BodyPartsList[1] && snakeMovement.isAlive)
        {
            if(Time.time - snakeMovement.TimeFromLastRetry > 5)
            {
                snakeMovement.Die();
            }
        }
    }
}
