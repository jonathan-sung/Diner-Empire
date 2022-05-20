using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float boostSpeed;
    private Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float tempSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            tempSpeed = boostSpeed;
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        Vector2 dir = (new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))).normalized;
        body.MovePosition(new Vector2((transform.position.x + dir.x * tempSpeed * Time.deltaTime), (transform.position.y + dir.y * tempSpeed * Time.deltaTime)));
    }
}
