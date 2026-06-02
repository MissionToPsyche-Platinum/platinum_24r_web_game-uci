using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    public float speed = 5f;
    public string lane;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if(transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }
}