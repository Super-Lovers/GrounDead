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
    protected static GameObject[] ActionsUi;
    protected static GameObject[] PickUi;
    public static int Wood = 0;
    public static int Stone = 0;
    public static int Copper = 0;
    public static int GunPowder = 0;
    public static int Apples = 0;
    
    // Sound Effects and Music for the player himself
    public GameObject CameraAudioSource;
    private AudioSource _cameraAudioSource;
    private AudioSource _audioSource;
    
    public AudioClip Walking;
    public AudioClip DayTheme;
    public AudioClip NightTheme;
    private bool _isWalking;
	
    void Start ()
    {
        // Player resources
        PlayerPrefs.SetFloat("Wood", Wood);
        PlayerPrefs.SetFloat("Stone", Stone);
        PlayerPrefs.SetFloat("Copper", Copper);
        PlayerPrefs.SetFloat("Gun Powder", GunPowder);
        PlayerPrefs.SetFloat("Apples", Apples);
        
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        ActionsUi = GameObject.FindGameObjectsWithTag("ActionUI");
        PickUi = GameObject.FindGameObjectsWithTag("PickUI");
        _audioSource = gameObject.GetComponent<AudioSource>();
        _cameraAudioSource = CameraAudioSource.GetComponent<AudioSource>();
        
        // Hide the UI at the start of the game AFTER you select the components
        CloseButtonOnClick();
        ClosePickingBlocks();

        _cameraAudioSource.clip = DayTheme;
        _cameraAudioSource.Play();
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
        
        // *******
        // Day/Night Cycle (work in progress)
        // *******
        if (gameObject.GetComponentInChildren<Light>().transform.rotation.x < -45)
        {
            //_cameraAudioSource.clip = NightTheme;
        } else if (gameObject.GetComponentInChildren<Light>().transform.rotation.x < 0 &&
                   gameObject.GetComponentInChildren<Light>().transform.rotation.x > -46)
        {
            //_cameraAudioSource.clip = DayTheme;
        }

        // Changing the light (dark/bright) for debug purposes
        if (Input.GetKey(KeyCode.J))
        {
            gameObject.GetComponentInChildren<Light>().transform.Rotate(-0.5f, 0, 0);
        } else if (Input.GetKey(KeyCode.K))
        {
            gameObject.GetComponentInChildren<Light>().transform.Rotate(0.5f, 0, 0);
        }
    }

    void FixedUpdate()
    {
        Vector2 newPlayerVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _rb.velocity = newPlayerVelocity * speed;
    }

    public static void CloseButtonOnClick()
    {
        foreach (var ui in ActionsUi)
        {
            var uiPos = ui.transform.position;
            uiPos.x += 1000;
            ui.transform.position = uiPos;
        }
    }

    public static void ClosePickingBlocks()
    {
        foreach (var ui in PickUi)
        {
            var uiPos = ui.transform.position;
            uiPos.x += 1000;
            ui.transform.position = uiPos;
        }
    }
}