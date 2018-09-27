using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Animator Animator;
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
    public GameObject Weapon;
    public GameObject Ammo;
    private Transform _gunHole;
    private Vector3 _gunHolePos;
    private bool _isWalking;
    private int _lastDir;
	
    void Start ()
    {
        // Player resources
        PlayerPrefs.SetFloat("Wood", Wood);
        PlayerPrefs.SetFloat("Stone", Stone);
        PlayerPrefs.SetFloat("Copper", Copper);
        PlayerPrefs.SetFloat("Gun Powder", GunPowder);
        PlayerPrefs.SetFloat("Apples", Apples);
        
        Animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        ActionsUi = GameObject.FindGameObjectsWithTag("ActionUI");
        PickUi = GameObject.FindGameObjectsWithTag("PickUI");
        _audioSource = gameObject.GetComponent<AudioSource>();
        _cameraAudioSource = CameraAudioSource.GetComponent<AudioSource>();
        
        // Hide the UI at the start of the game AFTER you select the components
        CloseButtonOnClick();
        ClosePickingBlocks();
        
        _cameraAudioSource.Play();
        
        _gunHole = GameObject.FindGameObjectWithTag("GunHole").transform;
        _gunHolePos = _gunHole.position;
    }
	
    void Update () {
        // Setting the animation of the player to face the right direction
        // whenever he is pressing the buttons on the axis
        var horizontalMovement = Input.GetAxis("Horizontal");
        var verticalMovement = Input.GetAxis("Vertical");
        
        if (horizontalMovement > 0) // Right
        {
            Animator.enabled = true;
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

            _gunHolePos.x = transform.position.x + 0.32f;
            _gunHole.position = _gunHolePos;
            
            Animator.SetInteger("direction", 3);
            _lastDir = 3;
        } else if (horizontalMovement < 0) // Left
        {
            Animator.enabled = true;
            CloseButtonOnClick();
            ClosePickingBlocks();

            if (!_isWalking)
            {
                _audioSource.Play();
                _isWalking = true;
            }

            _gunHolePos.x = transform.position.x - 0.32f;
            _gunHole.position = _gunHolePos;
            
            Animator.SetInteger("direction", 4);
            _lastDir = 4;
        } else  if (verticalMovement > 0) // Top
        {
            Animator.enabled = true;
            CloseButtonOnClick();
            ClosePickingBlocks();
            
            if (!_isWalking)
            {
                _audioSource.Play();
                _isWalking = true;
            }

            _gunHolePos.y = transform.position.y + 0.32f;
            _gunHole.position = _gunHolePos;
            
            Animator.SetInteger("direction", 2);
            _lastDir = 2;
        } else  if (verticalMovement < 0) // Bottom
        {
            Animator.enabled = true;
            CloseButtonOnClick();
            ClosePickingBlocks();
            
            if (!_isWalking)
            {
                _audioSource.Play();
                _isWalking = true;
            }

            _gunHolePos.y = transform.position.y - 0.32f;
            _gunHole.position = _gunHolePos;
            
            Animator.SetInteger("direction", 1);
            _lastDir = 1;
        }
        else // Idle
        {
            Animator.SetInteger("direction", _lastDir);
            Animator.enabled = false;
            
            _gunHolePos.x = transform.position.x;
            _gunHolePos.y = transform.position.y - 0.16f;
            _gunHole.position = _gunHolePos;
            
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

        if (Input.GetMouseButtonDown(0))
        {
            if (Animator.GetInteger("direction") == 1 || Animator.GetInteger("direction") == 2)
            {
                var bullet = Instantiate(Ammo,
                    new Vector3(transform.position.x, transform.position.y, transform.position.z)
                    , Quaternion.identity, transform);
                bullet.GetComponent<Transform>().Rotate(0, 0, 90);
            }
            else if (Animator.GetInteger("direction") == 3 || Animator.GetInteger("direction") == 4)
            {
                Instantiate(Ammo,
                    new Vector3(transform.position.x, transform.position.y, transform.position.z)
                    , Quaternion.identity, transform);
            }
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