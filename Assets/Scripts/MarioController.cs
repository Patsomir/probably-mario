using UnityEngine;

public class MarioController : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D smallCollider;
    private BoxCollider2D bigCollider;

    private float horizontalVelocity;

    private bool isJumping = true;
    private bool isFalling = true;
    private bool isCroaching = false;
    private bool isLookingUp = false;
    private bool isBig = false;

    [SerializeField]
    private float speed = 50;

    [SerializeField]
    private float maxSpeed = 50;

    [SerializeField]
    private float jumpForce = 100;

    [SerializeField]
    private string jumpKey = "z";

    [SerializeField]
    private string croachKey = "down";

    [SerializeField]
    private string lookingUpKey = "up";

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Mushroom") && !isBig)
        {
            isJumping = false;
            isBig = true;
            bigCollider.enabled = true;
            smallCollider.enabled = false;
            animator.SetTrigger("eatsMushroom");
            print("eat mushroom");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isJumping = false;
            //animator.SetBool("isJumping", false);
            print("hit ground");
        }
    }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        smallCollider = GetComponents<BoxCollider2D>()[0];
        bigCollider = GetComponents<BoxCollider2D>()[1];
    }

    void Update()
    {
        // running and flipping
        horizontalVelocity = Input.GetAxisRaw("Horizontal");
        if ((isLookingUp || isCroaching) && !isJumping)
        {
            horizontalVelocity = 0;
        }
        if(horizontalVelocity != 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * horizontalVelocity,
                                               transform.localScale.y,
                                               transform.localScale.z);
        } else
        {
            NormalizeSpeed();
        }

        // croaching
        if(!isJumping && !isLookingUp && Input.GetKey(croachKey))
        {
            isCroaching = true;
            //animator.SetBool("isCroaching", true);
        }
        if (Input.GetKeyUp(croachKey))
        {
            isCroaching = false;
            //animator.SetBool("isCroaching", false);
        }

        // looking up
        if (!isJumping && !isCroaching && horizontalVelocity == 0 && Input.GetKey(lookingUpKey))
        {
            isLookingUp = true;
            //animator.SetBool("isLookingUp", true);
        }
        if (Input.GetKeyUp(lookingUpKey))
        {
            isLookingUp = false;
            //animator.SetBool("isLookingUp", false);
        }

        // jumping
        if (!isJumping && !isLookingUp && !isFalling && Input.GetKeyDown(jumpKey))
        {
            body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
            //animator.SetBool("isJumping", true);
        }

        // falling
        if(body.velocity.y < 0)
        {
            isFalling = true;
            //animator.SetBool("isFalling", true);
        }
        else
        {
            isFalling = false;
            //animator.SetBool("isFalling", false);
        }

        //animator.SetFloat("Speed", Mathf.Abs(body.velocity.x));
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        if(Mathf.Abs(body.velocity.x) < maxSpeed || body.velocity.x * horizontalVelocity < 0)
        {
            body.AddForce(new Vector2(horizontalVelocity * speed * Time.fixedDeltaTime, 0), ForceMode2D.Impulse);
        }
    }

    private void UpdateAnimations()
    {
        animator.SetFloat("Speed", Mathf.Abs(body.velocity.x));
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isCroaching", isCroaching);
        animator.SetBool("isLookingUp", isLookingUp);
        animator.SetBool("isBig", isBig);
    }

    private void NormalizeSpeed()
    {
        body.velocity = new Vector2(body.velocity.x/1.1f, body.velocity.y);
    }
}
