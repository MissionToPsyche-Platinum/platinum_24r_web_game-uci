using UnityEngine;

public class AddCards : MonoBehaviour
{
    [SerializeField]
    private Transform cardField;

    [SerializeField]
    private GameObject matchcard;

    void Awake()
    {
        for (int i = 0; i < 12; i++) {
            GameObject card_to_add = Instantiate(matchcard);
            card_to_add.name = "" + i;
            card_to_add.transform.SetParent(cardField, false);
        }
    }

}
