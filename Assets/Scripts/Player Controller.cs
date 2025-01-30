using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    Animator animator;
    Flashlight flashlight;

    Vector2 velocity;

    public float moveSpeed;
    public bool FacingRight = true;

    private bool canMove = false;

    private CircleCollider2D circleCollider;

    public GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        flashlight = GetComponent<Flashlight>();
        circleCollider = GetComponent<CircleCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if(!canMove)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            velocity = new Vector2(1 * moveSpeed, 0);
            spriteRenderer.flipX = false;
            animator.SetBool("walking", true);
            FacingRight = true;
            flashlight.DirectionChange();
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            velocity = new Vector2(-1 * moveSpeed, 0);
            spriteRenderer.flipX = true;
            animator.SetBool("walking", true);
            FacingRight = false;
            flashlight.DirectionChange();
        }
       

        rigidBody.linearVelocity = velocity;
    }

    public void StopMovement()
    {
        circleCollider.enabled = false;
        canMove = false;
        velocity = Vector2.zero;
        rigidBody.linearVelocity = velocity;
    }

    public void EnableMovement()
    {
        circleCollider.enabled = true;
        animator.SetBool("walking", false);
        canMove = true;
        animator.SetBool("shaking", false);
    }

    public void PlayShakeAnim()
    {
        animator.SetBool("shaking", true);

       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            gameManager.Eaten();
        }
    }
}
