using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ZombieController : MonoBehaviour
{
    public LayerMask PlayerLayerMask;
    public LayerMask PlayerDetectorLayerMask;
    public int HitPoints = 80;
    public int Strength = 20;
    public float MovementSpeed = 0.03f;
    public float RangeOfDetection = 4;
    
    // Used to confirm that the zombie will collide with an obstacle
    public static bool CloseToAWall = false;
    // Variable responsible for the delay between zombie attacks
    private bool _canAttack = true;
    private GameObject _obstacle;
    public Material RedFlash;
    public Material YellowFlash;
    public Material WhiteFlash;
    public AudioClip StructureHitSound;
    private bool _isHittingObject;
    private GameObject _player;
    private GameObject _playerDetector;
    private string _zombieType = "Normal";

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerDetector = GameObject.FindGameObjectWithTag("PlayerDetector");

        var randomNum = Random.Range(0, 101);

        if (randomNum > 66.6f && randomNum < 87)
        {
            _zombieType = "Advanced";
            HitPoints = 120;
            Strength = 30;
        } else if (randomNum > 86.6f && randomNum < 101)
        {
            _zombieType = "Armored";
            HitPoints = 230;
        }
        else
        {
            _zombieType = "Normal";
            HitPoints = 80;
            Strength = 30;
        }
    }

    void Update ()
    {
        var playerDetectorPos = _playerDetector.transform.position;
        playerDetectorPos = _player.transform.position;
        _playerDetector.transform.position = playerDetectorPos;
        /*
        Ray2D rayLeft = new Ray2D(transform.position, Vector2.left);
        Ray2D rayRight = new Ray2D(transform.position, Vector2.right);
        Ray2D rayUp = new Ray2D(transform.position, Vector2.up);
        Ray2D rayDown = new Ray2D(transform.position, Vector2.down);
		*/
         
        Vector2 pos = transform.position;
        // Distance length of the rays to be cast
        float distance = (float)0.64 * RangeOfDetection; // 0.64 is the size of one tile
        float radius = (float) 0.64 * RangeOfDetection;
        Vector2 dir = new Vector2(1f, 1f);
        
        RaycastHit2D castResult = Physics2D.CircleCast(pos, radius, dir, distance, PlayerDetectorLayerMask);
        RaycastHit2D linecastResult = Physics2D.Linecast(transform.position, _player.transform.position, PlayerLayerMask);
        if (castResult)
        {
            if (linecastResult.transform.tag == "Player")
            {
                Debug.DrawLine(transform.position, _player.transform.position, Color.green, 1.0f);
                transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, MovementSpeed);
                gameObject.GetComponent<Animator>().SetBool("isWalking", true);
            }
            else
            {
                Debug.DrawLine(transform.position, _player.transform.position, Color.red, 1.0f);
                gameObject.GetComponent<Animator>().SetBool("isWalking", false);
            }   
        }
        else
        {
            Debug.DrawLine(transform.position, _playerDetector.transform.position, Color.blue, 1.0f);
        }
        // _audioSource.Play();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _isHittingObject = true;
        if (other.transform.tag == "Player" || other.gameObject.layer == 10)
        {
            gameObject.GetComponent<Rigidbody2D>().constraints =
                RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            gameObject.GetComponent<Animator>().SetBool("isWalking", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Knife")
        {
            HitPoints -= 30;
            StartCoroutine("FlashZombie");
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        _isHittingObject = true;
        gameObject.GetComponent<Rigidbody2D>().constraints =
            RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        
        _obstacle = other.gameObject;

        if (_obstacle.transform.tag == "Player" || _obstacle.gameObject.layer == 10 ||
            _obstacle.transform.name == "BlockSpikes(Clone)" || _obstacle.transform.name == "BlockElectricFence(Clone)")
        {
            if (_obstacle.GetComponent<HitPointsController>() != null)
            {
                if (_obstacle.GetComponent<HitPointsController>().HitPoints <= 0)
                {
                    // End game conditional
                    if (other.transform.tag == "Player")
                    {
                        SceneManager.LoadScene("GameOverScene");
                    }
                    else
                    {
                        _isHittingObject = false;

                        // Because the sound effect of destroying a building is halted
                        // once its destroyed, we run another oneshot from the zombie
                        // closest that destroyed it.
                        Destroy(_obstacle);
                        UiButtonController.PlacedBlocks.Remove(_obstacle);

                        PlayerController.Score += 100;
                        GameObject.FindGameObjectWithTag("PlayerScore").GetComponent<Text>().text =
                            "Score: " + PlayerController.Score;
                    }
                }
                else
                {
                    if (_canAttack)
                    {
                        if (_obstacle.GetComponent<AudioSource>() != null)
                        {
                            _obstacle.GetComponent<AudioSource>().PlayOneShot(StructureHitSound);
                        }

                        // This is used to check whether the zombie is colliding with a trap.
                        if (_obstacle.transform.tag == "Player" || _obstacle.transform.name == "BlockSpikes(Clone)" ||
                            _obstacle.transform.name == "BlockElectricFence(Clone)" || _obstacle.transform.tag == "Knife")
                        {
                            if (HitPoints <= 0)
                            {
                                Destroy(gameObject);
                                PlayerController.Score += 100;
                                GameObject.FindGameObjectWithTag("PlayerScore").GetComponent<Text>().text =
                                    "Score: " + PlayerController.Score;
                            }
                            else
                            {
                                StartCoroutine("FlashZombie");
                                StartCoroutine("FlashObstacle");
                                if (_obstacle.transform.name == "BlockSpikes(Clone)")
                                {
                                    HitPoints -= 15;
                                }
                                else if (_obstacle.transform.name == "BlockElectricFence(Clone)")
                                {
                                    HitPoints -= 35;
                                }
                            }
                        }

                        if (_zombieType == "Normal" || _zombieType == "Armored")
                        {
                            if (_obstacle.transform.name == "BlockSpikes(Clone)" ||
                                _obstacle.transform.name == "BlockElectricFence(Clone)")
                            {
                                _obstacle.GetComponent<HitPointsController>().HitPoints -= 5;
                            } else if (_obstacle.transform.tag == "Player")
                            {
                                _obstacle.GetComponent<HitPointsController>().HitPoints -= 20; 
                            }
                        }
                        else if (_zombieType == "Advanced")
                        {
                            if (_obstacle.transform.name == "BlockSpikes(Clone)" ||
                                _obstacle.transform.name == "BlockElectricFence(Clone)")
                            {
                                _obstacle.GetComponent<HitPointsController>().HitPoints -= 10;
                            } else if (_obstacle.transform.tag == "Player")
                            {
                                _obstacle.GetComponent<HitPointsController>().HitPoints -= 30; 
                            }
                        }

                        _canAttack = false;

                        Invoke("EnableAttacks", 1.5f);
                    }
                }
            }
        }
    }

    private void EnableAttacks()
    {
        _canAttack = true;
        _obstacle.GetComponent<SpriteRenderer>().material = WhiteFlash;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        _isHittingObject = false;
        if (other.transform.tag == "Player" || other.gameObject.layer == 10)
        {
            gameObject.GetComponent<Rigidbody2D>().constraints =
                RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        }
    }
    

    private IEnumerator FlashZombie()
    {
        for (int i = 0; i < 2; i++)
        {
            gameObject.GetComponent<SpriteRenderer>().material = RedFlash;
            yield return new WaitForSeconds(.1f);
            gameObject.GetComponent<SpriteRenderer>().material = WhiteFlash;
            yield return new WaitForSeconds(.1f);	
        }
		
        gameObject.GetComponent<SpriteRenderer>().material = WhiteFlash;
    }

    private IEnumerator FlashObstacle()
    {
        for (int i = 0; i < 2; i++)
        {
            _obstacle.GetComponent<SpriteRenderer>().material = YellowFlash;
            yield return new WaitForSeconds(.1f);
            _obstacle.GetComponent<SpriteRenderer>().material = WhiteFlash;
            yield return new WaitForSeconds(.1f);	
        }
    }
}