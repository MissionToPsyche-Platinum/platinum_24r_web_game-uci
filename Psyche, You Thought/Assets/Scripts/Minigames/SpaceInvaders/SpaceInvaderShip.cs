// using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceInvaderShip : MonoBehaviour
{
    public GameObject player;
    public GameObject projectile; // prefab projectile
    public GameObject projectileClone; // copy that is added to the game
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        movement();
        fireProjectile();
    }

    void movement()
    {
        // move right
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            transform.Translate(new Vector3(5 * Time.deltaTime, 0, 0));
        }
        // move left
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            transform.Translate(new Vector3(-5 * Time.deltaTime, 0, 0));
        }
        // move up
        if (Keyboard.current.upArrowKey.isPressed)
        {
            transform.Translate(new Vector3(0, 5 * Time.deltaTime, 0));
        }
        // move down
        if (Keyboard.current.downArrowKey.isPressed)
        {
            transform.Translate(new Vector3(0, -5 * Time.deltaTime, 0));
        }
    }

    void fireProjectile()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            AudioManager.Instance?.PlaySfx(AudioManager.Instance?.shootSFX);
            projectileClone = Instantiate(projectile, new Vector3(player.transform.position.x, player.transform.position.y + 0.8f, 0), player.transform.rotation) as GameObject;
        }
    }
}
