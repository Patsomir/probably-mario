using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomMovement : MonoBehaviour
{

    [SerializeField]
    private float speed = 10;

    [SerializeField]
    private float maxSpeed = 50;

    private Rigidbody2D body;
    private Collider2D hitbox;
    private float direction;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Mario"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if(collision.gameObject == transform.parent)
        //{
            hitbox.isTrigger = false;
        //}
    }

    void Start()
    {
        direction = Mathf.Sign(Random.Range(-1, 1));
        body = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<Collider2D>();
        body.gravityScale = 0;
        hitbox.isTrigger = true;
    }

    void FixedUpdate()
    {
        if(canMove())
        {
            body.AddForce(new Vector2(direction * speed * Time.fixedDeltaTime, 0), ForceMode2D.Impulse);
        }
    }

    private bool canMove()
    {
        return body.gravityScale > 0 && !hitbox.isTrigger && speed < maxSpeed;
    }
}
