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
    public static int Wood = 0;
    public static int Stone = 0;
    public static int Copper = 0;
    
    // Sound Effects for the player himself
    public AudioClip Walking;
    private AudioSource _audioSource;
    private bool _isWalking = false;
	
    void Start ()
    {
        // Player resources
        PlayerPrefs.SetFloat("Wood", Wood);
        PlayerPrefs.SetFloat("Stone", Stone);
        PlayerPrefs.SetFloat("Copper", Copper);
        
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _actionsUi = GameObject.FindGameObjectsWithTag("ActionUI");
        _pickUi = GameObject.FindGameObjectsWithTag("PickUI");
        _audioSource = gameObject.GetComponent<AudioSource>();
        
        // Hide the UI at the start of the game AFTER you select the components
        CloseButtonOnClick();
        ClosePickingBlocks();
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
            
            // Play the walking sound effect
            if (!_isWalking)
            {
                _audioSource.Play();
                _isWalking = true;
            }
            
            _animator.SetInteger("direction", 3);
        } else if (horizontalMovement < 0) // Left
        {
            CloseButtonOnClick();
            ClosePickingBlocks();
            
            if (!_isWalking)
            {
                _audioSource.Play();
                _isWalking = true;
            }
            
            _animator.SetInteger("direction", 4);
        } else  if (verticalMovement > 0) // Top
        {
            CloseButtonOnClick();
            ClosePickingBlocks();
            
            if (!_isWalking)
            {
                _audioSource.Play();
                _isWalking = true;
            }
            
            _animator.SetInteger("direction", 2);
        } else  if (verticalMovement < 0) // Bottom
        {
            CloseButtonOnClick();
            ClosePickingBlocks();
            
            if (!_isWalking)
            {
                _audioSource.Play();
                _isWalking = true;
            }
            
            _animator.SetInteger("direction", 1);
        }
        else // Idle
        {
            _animator.SetInteger("direction", 5);
            
            _audioSource.Stop();
            _isWalking = false;
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
            var uiPos = ui.transform.position;
            uiPos.x += 1000;
            ui.transform.position = uiPos;
        }
    }

    public void ClosePickingBlocks()
    {
        foreach (var ui in _pickUi)
        {
            var uiPos = ui.transform.position;
            uiPos.x += 1000;
            ui.transform.position = uiPos;
        }
    }
}