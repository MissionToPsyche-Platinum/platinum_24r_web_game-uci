using UnityEngine;

public class Enemy : MonoBehaviour
{
    float timer = 0;
    float timeToMove = 0.5f;
    int numMovements = 0;
    int maxTicks = 18;
    float speed = 0.25f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeToMove)
        {
            transform.Translate(new Vector3(speed, 0, 0));
            timer = 0;
            numMovements += 1;
        }
        // move enemies closer to player
        if (numMovements == maxTicks)
        {
            transform.Translate(new Vector3(0, -1, 0));
            numMovements = 0;
            speed = -speed;
        }
    }
}
