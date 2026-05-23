using UnityEngine;

public class AddLives : MonoBehaviour
{
    [SerializeField]
    private Transform lifeRow;

    [SerializeField]
    private GameObject lifeSprite;

    void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject card_to_add = Instantiate(lifeSprite);
            card_to_add.name = "" + i;
            card_to_add.transform.SetParent(lifeRow, false);
        }
    }

}
