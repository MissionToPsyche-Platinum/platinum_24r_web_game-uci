using UnityEngine;

public class Enemy : MonoBehaviour
{
    float timer = 0;
    float timeToMove = 0.5f;
    int numMovements = 0;
    int maxTicks = 12;
    float speed = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // move enemies closer to player
        if (numMovements == maxTicks)
        {
            transform.Translate(new Vector3(0, -1, 0));
            numMovements = -1;
            speed = -speed;
            timer = 0;
        }
        // move enemies side to side
        timer += Time.deltaTime;
        if (timer > timeToMove && numMovements != maxTicks)
        {
            transform.Translate(new Vector3(speed, 0, 0));
            timer = 0;
            numMovements += 1;
        }
    }
}
