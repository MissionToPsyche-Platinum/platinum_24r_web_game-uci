using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;

    public Transform laneA;
    public Transform laneS;
    public Transform laneD;
    public Transform laneF;

    public float spawnInterval = 1f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= spawnInterval)
        {
            SpawnNote();
            timer = 0;
        }
    }

    void SpawnNote()
    {
        int lane = Random.Range(0,4);

        Transform spawnPoint = lane switch
        {
            0 => laneA,
            1 => laneS,
            2 => laneD,
            _ => laneF
        };

        GameObject note = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);

        NoteMovement nm = note.GetComponent<NoteMovement>();
        SpriteRenderer sr = note.GetComponent<SpriteRenderer>();

        switch(lane)
        {
            case 0:
                sr.color = Color.red;
                nm.lane = "A";
                break;

            case 1:
                sr.color = Color.green;
                nm.lane = "S";
                break;

            case 2:
                sr.color = Color.blue;
                nm.lane = "D";
                break;

            case 3:
                sr.color = Color.yellow;
                nm.lane = "F";
                break;
        }
    }
}