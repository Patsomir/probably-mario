using UnityEngine;

public class BoxBehaviour : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private float deploymentForce = 10;

    [SerializeField]
    private GameObject item;

    private Rigidbody2D itemBody;

    private bool isHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Mario") && !isHit)
        {
            isHit = true;
            DeployItem();
            animator.SetTrigger("getsHit");
        }
    }
    void Start()
    {
        isHit = false;
        animator = GetComponent<Animator>();
        if(item != null)
        {
            itemBody = item.GetComponent<Rigidbody2D>();
        }
    }

    private void DeployItem()
    {
        if(item != null)
        {
            itemBody.gravityScale = 4;
            itemBody.AddForce(new Vector2(0, deploymentForce), ForceMode2D.Impulse);
        } 
    }
}
