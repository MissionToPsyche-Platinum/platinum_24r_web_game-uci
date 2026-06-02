using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private SpaceInvadersController controller;
    public GameObject enemyProjectile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = FindFirstObjectByType<SpaceInvadersController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, -7 * Time.deltaTime, 0));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
            Destroy(enemyProjectile);
            controller.PlayerHit();
        }
        if (collision.gameObject.tag == "Finish")
        {
            Destroy(enemyProjectile);
        }
    }
}
