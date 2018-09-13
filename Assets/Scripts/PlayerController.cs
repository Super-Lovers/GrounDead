using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Animator animator;
	private Rigidbody2D rb;
	private SpriteRenderer spriteRenderer;

	private bool right;
	private bool left;
	private bool top;
	private bool bottom;

	private float speed = 4f;
	
	void Start ()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	void Update () {
		// Setting the animation of the player to face the right direction
		// whenever he is pressing the buttons on the axis
		
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			animator.enabled = true;
			animator.SetInteger("direction", 3); // Right
		} else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			animator.enabled = true;
			animator.SetInteger("direction", 4); // Left
		} else  if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			animator.enabled = true;
			animator.SetInteger("direction", 2); // Top
		} else  if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			animator.enabled = true;
			animator.SetInteger("direction", 1); // Bottom
		}
		else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
		{
			animator.enabled = false;
		}
	}

	void FixedUpdate()
	{
		Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		rb.velocity = targetVelocity * speed;
	}
}
