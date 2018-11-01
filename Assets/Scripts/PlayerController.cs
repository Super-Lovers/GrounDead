using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Player stats
    public static int HitPoints = 100;
    public static ulong Score = 0;
    
    public static Animator Animator;
    private Rigidbody2D _rb;
    //private SpriteRenderer _spriteRenderer;
    
    private bool _right;
    private bool _left;
    private bool _top;
    private bool _bottom;

    private float speed = 4f;
    
    // Ui
    protected static GameObject[] ActionsUi;
    protected static GameObject[] PickUi;
    //private GameObject _notification;
    public static int Wood = 200;
    public static int Stone = 200;
    public static int Copper = 10;
    public static int GunPowder = 10;
    public static int Apples;
    public static int Bullets = 10;
    public Texture PlayerPortrait;
    public Texture HealthTexture;
    public Texture HealthBackgroundTexture;
    public static float PlayerHealth;
    
    // Player play modes
    public static string PlayMode = "Creative";
    
    // Sound Effects and Music for the player himself
    public GameObject CameraAudioSource;
    private AudioSource _cameraAudioSource;
    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;
    public GameObject ApplesUi;
    
    public AudioClip Walking;
    public AudioClip DayTheme;
    public AudioClip NightTheme;
    public GameObject Ammo;
    private Transform _gunHole;
    private Vector3 _gunHolePos;
    private bool _isWalking;
    private int _lastDir;
    private bool _strafing;
    private bool _canShoot = true;
    public static bool IsPaused;
    private GameObject _pauseMenu;
    public GameObject NotificationDamage;
    
    // Zombie parameter responsible for the rate of zombie spawns
    public GameObject Weapon;
    public Sprite Knife;
    public Sprite Axe;
    public static int NumberOfZombiesKilled;
    public static bool CanBuildOnTile = false;
	
    void Start ()
    {
        // Player resources
        PlayerPrefs.SetFloat("Wood", Wood);
        PlayerPrefs.SetFloat("Stone", Stone);
        PlayerPrefs.SetFloat("Copper", Copper);
        PlayerPrefs.SetFloat("Gun Powder", GunPowder);
        PlayerPrefs.SetFloat("Apples", Apples);
        PlayerPrefs.SetFloat("Bullets", Bullets);

        Weapon = GameObject.FindGameObjectWithTag("Melee Weapon");
        // Setting the starting weapon
        Weapon.GetComponent<SpriteRenderer>().sprite = Knife;
        
        Weapon.SetActive(false);
        Animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        ActionsUi = GameObject.FindGameObjectsWithTag("ActionUI");
        PickUi = GameObject.FindGameObjectsWithTag("PickUI");
        _audioSource = gameObject.GetComponent<AudioSource>();
        _cameraAudioSource = CameraAudioSource.GetComponent<AudioSource>();
        _pauseMenu = GameObject.Find("PauseMenu");
        //_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //_notification = GameObject.FindGameObjectWithTag("NotificationUi");
        
        // Hide the pause menu when the game begins, so we can
        // show it only when the player presses backspace.
        _pauseMenu.SetActive(false);


        PlayerHealth = (gameObject.GetComponent<HitPointsController>().HitPoints * 27) / 10f;
        
        // Hide the UI at the start of the game AFTER you select the components
        CloseButtonOnClick();
        ClosePickingBlocks();
        
        _cameraAudioSource.Play();
        
        _gunHole = GameObject.FindGameObjectWithTag("GunHole").transform;
        _gunHolePos = _gunHole.position;
        
        // *******
        // Day/Night Cycle (work in progress)
        // *******
        //InvokeRepeating("UpdateWorldTime", 20, 20);
    }
    
    private void OnGUI()
    {
        // Drawing the player's health bar when its updated
        PlayerHealth = (gameObject.GetComponent<HitPointsController>().HitPoints * 27) / 10f;
        
        // Player UI Health bar and portrait
        Rect playerPortraitRect = new Rect(10, Screen.height - 110, 120, 100);
        GUI.DrawTexture(playerPortraitRect, PlayerPortrait);
        
        Rect healthBackgroundRect = new Rect(150, Screen.height - 70, 270, 30);
        GUI.DrawTexture(healthBackgroundRect, HealthBackgroundTexture);
        
        Rect healthRect = new Rect(150, Screen.height - 70, PlayerHealth, 30);
        GUI.DrawTexture(healthRect, HealthTexture);
    }

    void Update () {
        // Setting the animation of the player to face the right direction
        // whenever he is pressing the buttons on the axis
        float horizontalMovement;
        float verticalMovement;
        
        if (!IsPaused)
        {
            horizontalMovement = Input.GetAxis("Horizontal");
            verticalMovement = Input.GetAxis("Vertical");
            
            var weaponTransform = Weapon.transform;
            var weaponPos = Weapon.transform.position;
            
            if (horizontalMovement > 0 && _strafing == false) // Right
            {
                ObjectivesController.WalkedRight = true;
                weaponPos.x = transform.position.x + 0.4f;
                weaponPos.y = transform.position.y + 0.007f;
                weaponTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            
                //Animator.enabled = true;
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
            
                Animator.SetBool("isWalking", true);
                Animator.SetInteger("direction", 3);
                _lastDir = 3;
            } else if (horizontalMovement < 0 && _strafing == false) // Left
            {
                ObjectivesController.WalkedLeft = true;
                weaponPos.x = transform.position.x + (0.4f * -1);
                weaponPos.y = transform.position.y + 0.007f;
                weaponTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            
                //Animator.enabled = true;
                CloseButtonOnClick();
                ClosePickingBlocks();

                if (!_isWalking)
                {
                    _audioSource.Play();
                    _isWalking = true;
                }

                _gunHolePos.x = transform.position.x - 0.32f;
                _gunHole.position = _gunHolePos;
            
                Animator.SetBool("isWalking", true);
                Animator.SetInteger("direction", 4);
                _lastDir = 4;
            } else  if (verticalMovement > 0 && _strafing == false) // Top
            {
                ObjectivesController.WalkedUp = true;
                weaponPos.x = transform.position.x + 0.01f;
                weaponPos.y = transform.position.y + 0.45f;
                weaponTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            
                //Animator.enabled = true;
                CloseButtonOnClick();
                ClosePickingBlocks();
            
                if (!_isWalking)
                {
                    _audioSource.Play();
                    _isWalking = true;
                }

                _gunHolePos.y = transform.position.y + 0.32f;
                _gunHole.position = _gunHolePos;
            
                Animator.SetBool("isWalking", true);
                Animator.SetInteger("direction", 2);
                _lastDir = 2;
            } else  if (verticalMovement < 0 && _strafing == false) // Bottom
            {
                ObjectivesController.WalkedDown = true;
                weaponPos.x = transform.position.x + 0.01f;
                weaponPos.y = transform.position.y + (0.45f * -1);
                weaponTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
            
                //Animator.enabled = true;
                CloseButtonOnClick();
                ClosePickingBlocks();
            
                if (!_isWalking)
                {
                    _audioSource.Play();
                    _isWalking = true;
                }

                _gunHolePos.y = transform.position.y - 0.32f;
                _gunHole.position = _gunHolePos;
            
                Animator.SetBool("isWalking", true);
                Animator.SetInteger("direction", 1);
                _lastDir = 1;
            }
            else // Idle
            {
                Animator.SetInteger("direction", 0);
                Animator.SetBool("isWalking", false);
                if (_strafing)
                {
                    if (horizontalMovement == 0 && verticalMovement == 0)
                    {
                        Animator.SetInteger("direction", 5);
                    }
                    else
                    {
                        Animator.SetInteger("lastDirection", _lastDir);
                    }
                }
                else
                {
                    Animator.SetInteger("lastDirection", _lastDir);
                }
            
                _gunHolePos.x = transform.position.x;
                _gunHolePos.y = transform.position.y - 0.16f;
                _gunHole.position = _gunHolePos;
            
                _audioSource.Stop();
                _isWalking = false;
            }

            Weapon.transform.position = weaponPos;
            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _strafing = true;
            }
            else
            {
                _strafing = false;
            }

            if (Input.GetMouseButtonDown(0) && PlayMode == "Survival" && _canShoot && !EventSystem.current.IsPointerOverGameObject() && Bullets > 0)
            {
                ObjectivesController.HasAttackedRange = true;
                
                if ((Animator.GetInteger("direction") == 1 || Animator.GetInteger("direction") == 2) ||
                    (Animator.GetInteger("lastDirection") == 1 || Animator.GetInteger("lastDirection") == 2))
                {
                    var bullet = Instantiate(Ammo,
                        new Vector3(transform.position.x, transform.position.y, transform.position.z)
                        , Quaternion.identity, transform);
                    bullet.GetComponent<Transform>().Rotate(0, 0, 90);
                
                    // The second parameter in the invoke is equivalent to the
                    // rate of fire the player has.
                    _canShoot = false;
                    Invoke("CanShoot", 0.7f);
                }
                else if ((Animator.GetInteger("direction") == 3 || Animator.GetInteger("direction") == 4) ||
                         (Animator.GetInteger("lastDirection") == 3 || Animator.GetInteger("lastDirection") == 4))
                {
                    Instantiate(Ammo,
                        new Vector3(transform.position.x, transform.position.y, transform.position.z)
                        , Quaternion.identity, transform);
                
                    _canShoot = false;
                    Invoke("CanShoot", 0.7f);
                } else if (Animator.GetInteger("direction") == 5 || Animator.GetInteger("direction") == 0)
                {
                    var bullet = Instantiate(Ammo,
                        new Vector3(transform.position.x, transform.position.y, transform.position.z)
                        , Quaternion.identity, transform);
                    bullet.GetComponent<Transform>().Rotate(0, 0, 90);
                
                    _canShoot = false;
                    Invoke("CanShoot", 0.7f);
                }

                Bullets--;
                GameObject.FindGameObjectWithTag("PlayerBullets").GetComponent<Text>().text = "Bullets: " + Bullets;
            }
            
            // Healing mechanic
            if (Input.GetKeyDown(KeyCode.Q) && Apples > 0 && PlayerHealth < 270)
            {
                ObjectivesController.HealedSelf = true;
                ApplesUi.GetComponent<Animator>().SetBool("shineApples", true);
                Invoke("StopApplesShineAnimation", 1);

                Apples--;
                PlayerPrefs.SetFloat("Apples", Apples);
                GameObject.FindGameObjectWithTag("PlayerApples").GetComponent<Text>().text = PlayerPrefs.GetFloat("Apples").ToString();

                gameObject.GetComponent<HitPointsController>().HitPoints += 27;
                if (gameObject.GetComponent<HitPointsController>().HitPoints > 270)
                {
                    gameObject.GetComponent<HitPointsController>().HitPoints = 270;
                }
            }
        
            // *********************
            // Melee attack keybinding
            // *********************
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ObjectivesController.HasAttackedMelee = true;
                
                Weapon.SetActive(true);
                Invoke("DisableKnife", 0.2f);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                ObjectivesController.HasSwitchedWeapons = true;
                
                if (Weapon.GetComponent<SpriteRenderer>().sprite == Knife)
                {
                    Weapon.GetComponent<SpriteRenderer>().sprite = Axe;
                    var weaponCollider = Weapon.GetComponent<BoxCollider2D>().size;
                    weaponCollider.x = 0.80f;
                    weaponCollider.y = 0.50f;
                    Weapon.GetComponent<BoxCollider2D>().size = weaponCollider;
                }
                else
                {
                    Weapon.GetComponent<SpriteRenderer>().sprite = Knife;
                    var weaponCollider = Weapon.GetComponent<BoxCollider2D>().size;
                    weaponCollider.x = 0.64f;
                    weaponCollider.y = 0.25f;
                    Weapon.GetComponent<BoxCollider2D>().size = weaponCollider;
                }
            }
        }
        
        // ***********
        // Pause Functionality
        // ***********
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsPaused = !IsPaused;
            
            if (IsPaused)
            {
                Time.timeScale = 0;
                _cameraAudioSource.enabled = false;
                _audioSource.enabled = false;
                _pauseMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                _cameraAudioSource.enabled = true;
                _audioSource.enabled = true;
                _pauseMenu.SetActive(false);
            }
        }
    }

    private void DisableKnife()
    {
        Weapon.SetActive(false);
    }

    private void StopApplesShineAnimation()
    {
        ApplesUi.GetComponent<Animator>().SetBool("shineApples", false);
    }
    
    void FixedUpdate()
    {
        Vector2 newPlayerVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _rb.velocity = newPlayerVelocity * speed;
    }

    public static void CloseButtonOnClick()
    {
        PlayMode = "Survival";
        foreach (var ui in ActionsUi)
        {
            var uiPos = ui.transform.position;
            uiPos.x += 1000;
            ui.transform.position = uiPos;
        }
    }

    public static void ClosePickingBlocks()
    {
        PlayMode = "Survival";
        foreach (var ui in PickUi)
        {
            var uiPos = ui.transform.position;
            uiPos.x += 1000;
            ui.transform.position = uiPos;
        }
    }

    private void CanShoot()
    {
        _canShoot = true;
    }
}