using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rb;

    private bool _right;
    private bool _left;
    private bool _top;
    private bool _bottom;

    private float speed = 4f;
	
    void Start ()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }
	
    void Update () {
        // Setting the animation of the player to face the right direction
        // whenever he is pressing the buttons on the axis
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _animator.enabled = true;
            _animator.SetInteger("direction", 3); // Right
        } else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _animator.enabled = true;
            _animator.SetInteger("direction", 4); // Left
        } else  if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _animator.enabled = true;
            _animator.SetInteger("direction", 2); // Top
        } else  if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _animator.enabled = true;
            _animator.SetInteger("direction", 1); // Bottom
        }
        else if (Input.GetAxis("Horizontal") == 0f && Input.GetAxis("Vertical") == 0f)
        {
            _animator.enabled = false;
        }
    }

    void FixedUpdate()
    {
        Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _rb.velocity = targetVelocity * speed;
    }
}