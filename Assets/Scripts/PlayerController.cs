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
    
    // Ui
    private GameObject[] _actionsUi;
    private GameObject[] _pickUi;
    public static int wood = 0;
    public static int stone = 0;
    public static int gold = 0;
	
    void Start ()
    {
        // Player resources
        PlayerPrefs.SetFloat("Wood", wood);
        PlayerPrefs.SetFloat("Stone", stone);
        PlayerPrefs.SetFloat("Gold", gold);
        
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _actionsUi = GameObject.FindGameObjectsWithTag("ActionUI");
        _pickUi = GameObject.FindGameObjectsWithTag("PickUI");
    }
	
    void Update () {
        // Setting the animation of the player to face the right direction
        // whenever he is pressing the buttons on the axis
        var horizontalMovement = Input.GetAxis("Horizontal");
        var verticalMovement = Input.GetAxis("Vertical");
        
        if (horizontalMovement > 0) // Right
        {
            // If the player moves when he has the menu for actions
            // up then it will be automatically closed
            CloseButtonOnClick();
            ClosePickingBlocks();
            _animator.SetInteger("direction", 3);
        } else if (horizontalMovement < 0) // Left
        {
            CloseButtonOnClick();
            ClosePickingBlocks();
            _animator.SetInteger("direction", 4);
        } else  if (verticalMovement > 0) // Top
        {
            CloseButtonOnClick();
            ClosePickingBlocks();
            _animator.SetInteger("direction", 2);
        } else  if (verticalMovement < 0) // Bottom
        {
            CloseButtonOnClick();
            ClosePickingBlocks();
            _animator.SetInteger("direction", 1);
        }
        else // Idle
        {
            _animator.SetInteger("direction", 5);
        }
    }

    void FixedUpdate()
    {
        Vector2 newPlayerVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _rb.velocity = newPlayerVelocity * speed;
    }

    public void CloseButtonOnClick()
    {
        foreach (var ui in _actionsUi)
        {
            ui.SetActive(false);
        }
    }

    public void ClosePickingBlocks()
    {
        foreach (var ui in _pickUi)
        {
            ui.SetActive(false);
        }
    }
}