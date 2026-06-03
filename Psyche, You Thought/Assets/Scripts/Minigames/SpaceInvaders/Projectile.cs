using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject projectile;
    private SpaceInvadersController controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = FindFirstObjectByType<SpaceInvadersController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 5 * Time.deltaTime, 0));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            AudioManager.Instance?.PlaySfx(AudioManager.Instance?.hitAlienSFX);
            Destroy(collision.gameObject);
            Destroy(projectile);
            controller.EnemyDestroyed();
        }
        if (collision.gameObject.tag == "Finish")
        {
            Destroy(projectile);
        }
    }
}
