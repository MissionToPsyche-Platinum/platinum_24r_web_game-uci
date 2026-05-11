using UnityEngine;

public class EdgeOfScene : MonoBehaviour
{
    public GameObject edge;
    public GameObject enemyProjectile;
    public GameObject enemyProjectileClone;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        fireEnemyProjectile();
    }

    void fireEnemyProjectile()
    {
        if (Random.Range(0f, 500f) < 1)
        {
            float xCoordinate = Random.Range(-8f, 8f);
            enemyProjectileClone = Instantiate(enemyProjectile, new Vector3(xCoordinate, edge.transform.position.y - 0.8f, 0), edge.transform.rotation) as GameObject;
        }
    }
}
