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
    public float MovementSpeed = 0.02f;
    public float RangeOfDetection = 100;
    
    // Used to confirm that the zombie will collide with an obstacle
    public static bool CloseToAWall = false;
    // Variable responsible for the delay between zombie attacks
    private bool _canAttack;
    private bool _enableDamageEffect;
    private GameObject _obstacle;
    public Material RedFlash;
    public Material YellowFlash;
    public Material WhiteFlash;
    public AudioClip StructureHitSound;
    private AudioSource _audioSource;
    public GameObject GunPowderPickUp;
    private bool _isHittingObject;
    private GameObject _player;
    private GameObject _playerDetector;
    private System.Collections.Generic.List<GameObject> _playerDetectorList;
    private string _zombieType = "Normal";
    private bool _isZombieGoingDown;
    public GameObject NotificationDamage;
    private bool _isFatZombie;
    private int _originalOrder;
    
    // Objects to ignore collision with
    public GameObject[] Stones;
    public GameObject[] Coppers;
    public GameObject[] Trees;
        
    //Vector2 prevPos;
    
    private void Start()
    {
        Invoke("EnableAttacks", 3f);

        _originalOrder = GetComponent<SpriteRenderer>().sortingOrder;
        
        if (gameObject.tag == "Zombie Boss")
        {
            _zombieType = "Armored";
            _isFatZombie = true;
        } else if (gameObject.tag == "Zombie")
        {
            _zombieType = "Normal";
        } else if (gameObject.tag == "Zombie Cop")
        {
            _zombieType = "Advanced";
        }
        
        foreach (GameObject block in UiButtonController.PlacedBlocks.ToArray())
        {
            string nameOfBlock = "";
            for (int i = 0; i < block.name.Length; i++)
            {
                nameOfBlock += block.name[i];
                if (_isFatZombie)
                {
                    if (nameOfBlock == "stone" || nameOfBlock == "copper")
                    {
                        Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), block.GetComponentInChildren<BoxCollider2D>());
                    }
                }
                else
                {
                    if (nameOfBlock == "stone" || nameOfBlock == "copper"|| nameOfBlock == "tree")
                    {
                        var circleCollidersInTree = block.GetComponentsInChildren<CircleCollider2D>();
                        foreach (var collider in circleCollidersInTree)
                        {
                            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), collider);
                        }
                        Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), block.GetComponentInChildren<BoxCollider2D>());
                        Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), block.GetComponentInChildren<CircleCollider2D>());
                    }
                }
            }
        }
        
        _player = GameObject.FindGameObjectWithTag("Player");
        _audioSource = GetComponent<AudioSource>();

        /*
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
        */
    }

    void Update ()
    {
        if (!PlayerController.IsPaused)
        {
            _audioSource.enabled = true;
            _playerDetector = GameObject.FindGameObjectWithTag("Player");
        
            Vector2 pos = transform.position;
            float distance = (float)0.64 * RangeOfDetection; // 0.64 is the size of one tile
            float radius = (float) 0.64 * RangeOfDetection;
            Vector2 dir = new Vector2(1f, 1f);
            RaycastHit2D castResult = Physics2D.CircleCast(pos, radius, dir, distance, PlayerDetectorLayerMask);

            if (castResult)
            {
                //Debug.DrawLine(transform.position, castResult.transform.position, Color.green, 1.0f);
                
                RaycastHit2D linecastResult = Physics2D.Linecast(transform.position, castResult.transform.position, PlayerLayerMask);
                if (linecastResult.transform.tag == "Player" || linecastResult.transform.gameObject.layer == 12 || linecastResult.transform.gameObject.layer == 11 ||
                    linecastResult.transform.gameObject.layer == 14 || linecastResult.transform.tag == "Player Visibility Detector" || linecastResult.transform.tag == "Player Range")
                {
                    gameObject.GetComponent<Animator>().SetBool("isWalking", true);
                    gameObject.GetComponent<Animator>().SetFloat("directionHorizontal", 1);
                    
                    if (linecastResult.transform.tag == "PlayerDetector" || linecastResult.transform.gameObject.layer == 12)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, linecastResult.transform.position, MovementSpeed);
                    }
                    else
                    {
                        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, MovementSpeed);
                    }
            
                    RaycastHit2D rayUp = Physics2D.Raycast(transform.position, Vector2.up, 5, PlayerDetectorLayerMask);
                    RaycastHit2D rayDown = Physics2D.Raycast(transform.position, Vector2.down, 5, PlayerDetectorLayerMask);
        
                    if (rayUp)
                    {
                        _isZombieGoingDown = false;
                    }
                    else if (rayDown)
                    {
                        _isZombieGoingDown = true;
                    }
                    else
                    {
                        _isZombieGoingDown = false;
                        if (linecastResult.transform.tag == "Player")
                        {
                            if (pos.x > linecastResult.transform.position.x)
                            {
                                if (_isFatZombie)
                                {
                                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                                }
                                else
                                {
                                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                                }
                            }
                            else if (pos.x < linecastResult.transform.position.x)
                            {
                                if (_isFatZombie)
                                {
                                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                                }
                                else
                                {
                                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                                }
                            }
                        } else if (pos.x > linecastResult.transform.position.x)
                        {
                            if (_isFatZombie)
                            {
                                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                            }
                            else
                            {
                                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                            }
                        }
                        else if (pos.x < linecastResult.transform.position.x)
                        {
                            if (_isFatZombie)
                            {
                                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                            }
                            else
                            {
                                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                            }
                        }
                        //gameObject.GetComponent<Animator>().SetFloat("directionVertical", 0);
                    }
                }
                else
                {
                    //Debug.DrawLine(transform.position, castResult.transform.position, Color.red, 1.0f); 
                    gameObject.GetComponent<Animator>().SetBool("isWalking", false);
                }
            }
            else
            {
                gameObject.GetComponent<Animator>().SetBool("isWalking", false);  
            }
        }
        else
        {
            _audioSource.enabled = false;
        }
    }

    /* if (linecastResult.transform.tag == "Player")
    {
        Debug.DrawLine(transform.position, _player.transform.position, Color.green, 1.0f);
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, MovementSpeed);
        gameObject.GetComponent<Animator>().SetBool("isWalking", true);
    }
    else
    {
        foreach (var block in UiButtonController.PlacedBlocks)
        {
            RaycastHit2D linecastBlockResult = Physics2D.Linecast(transform.position, block.transform.position, PlayerLayerMask);
            if (linecastBlockResult.transform.tag == "PlayerDetector")
            {
                transform.position = Vector2.MoveTowards(transform.position, block.transform.position, MovementSpeed);
                gameObject.GetComponent<Animator>().SetBool("isWalking", true);
            }
            else
            {
                gameObject.GetComponent<Animator>().SetBool("isWalking", false); 
            }
            Debug.DrawLine(transform.position, block.transform.position, Color.red, 1.0f);
        }
    }
    */

    //prevPos = transform.position;
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.transform.tag == "Player" || other.gameObject.layer == 10 || other.transform.gameObject.layer == 12)
        {
            //_obstacle.GetComponent<AudioSource>().PlayOneShot(StructureHitSound);
            if (_isZombieGoingDown)
            {
                gameObject.GetComponent<Animator>().SetBool("isHittingObjectDown", true); 
            }
            else
            {
                gameObject.GetComponent<Animator>().SetBool("isHittingObjectDown", false); 
                gameObject.GetComponent<Animator>().SetBool("isHittingObject", true);
            }
            gameObject.GetComponent<Rigidbody2D>().constraints =
                RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            gameObject.GetComponent<Animator>().SetBool("isWalking", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Melee Weapon")
        {
            StartCoroutine("FlashZombie");
            if (HitPoints <= 0)
            {
                if (Random.Range(0, 101) > 15)
                {
                    Instantiate(GunPowderPickUp, new Vector2(other.transform.position.x, other.transform.position.y), Quaternion.identity);
                }

                PlayerController.NumberOfZombiesKilled++;
                
                // Player Statistics
                MenuController.TotalZombiesDefeated++;
                if (gameObject.tag == "Zombie Boss")
                {
                    MenuController.TotalZombieBossesDefeated++;
                } else if (gameObject.tag == "Zombie")
                {
                    MenuController.TotalZombieCitizensDefeated++;
                } else if (gameObject.tag == "Zombie Cop")
                {
                    MenuController.TotalZombieCopsDefeated++;
                }

                MenuController.TotalHostilityScore += 100;
                
                MenuController.UpdateScore();
                
                Destroy(gameObject);
            }
            else
            {
                if (other.gameObject.layer == 15)
                {
                    HitPoints -= 30;
                
                    var notification = Instantiate(NotificationDamage, Camera.main.WorldToScreenPoint(gameObject.transform.position), Quaternion.identity, GameObject.Find("Canvas").transform);
                    notification.GetComponentInChildren<Text>().text = "-" + 30;
                }
                else if (other.gameObject.layer == 16)
                {
                    HitPoints -= 60;
                
                    var notification = Instantiate(NotificationDamage, Camera.main.WorldToScreenPoint(gameObject.transform.position), Quaternion.identity, GameObject.Find("Canvas").transform);
                    notification.GetComponentInChildren<Text>().text = "-" + 60;
                } 
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == 14 || other.gameObject.layer == 11)
        {
            MovementSpeed = 0.02f;
        }
        
        gameObject.GetComponent<Rigidbody2D>().constraints =
            RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        
        _obstacle = other.gameObject;

        if (_obstacle.transform.tag == "Player" || _obstacle.gameObject.layer == 12 ||
            _obstacle.transform.name == "BlockSpikes(Clone)" || _obstacle.transform.name == "BlockElectricFence(Clone)" || _obstacle.transform.gameObject.layer == 10)
        {
            if (_obstacle.GetComponent<HitPointsController>() != null)
            {
                if (_obstacle.GetComponent<HitPointsController>().HitPoints <= 0)
                {
                    // End game conditional
                    if (other.transform.tag == "Player")
                    {
                    }
                    else
                    {
                        // Because the sound effect of destroying a building is halted
                        // once its destroyed, we run another oneshot from the zombie
                        // closest that destroyed it.
                        
                        // Player Statistics
                        /*if (_obstacle.name == "BlockSpikes(Clone)")
                        {
                            MenuController.TotalSpikeTrapsBuilt++;
                        } else if (_obstacle.name == "BlockElectricFence(Clone)")
                        {
                            MenuController.TotalElectricFencesBuilt++;
                        } else if (_obstacle.name == "BlockWood(Clone)")
                        {
                            MenuController.TotalWoodenWallsBuilt++;
                        } else if (_obstacle.name == "BlockStoneWall(Clone)")
                        {
                            MenuController.TotalStoneWallsBuilt++;
                        } else if (_obstacle.name == "BlockPlatform(Clone)")
                        {
                            MenuController.TotalPlatformsBuilt++;
                        } else if (_obstacle.name == "BlockTorch(Clone)")
                        {
                            MenuController.TotalTorchesBuilt++;
                        }*/
                        
                        MenuController.TotalBuildingsDestroyed++;
                        MenuController.TotalBuildingScore += 100;
                        MenuController.TotalHostilityScore += 100;
                        
                        //_playerDetectorList.Remove(_obstacle);
                        UiButtonController.PlacedBlocks.Remove(_obstacle);
                        UiButtonController.PlacedStructures.Remove(_obstacle);
                        WorldGenerator.SumOfInteractableWorldObjects.Remove(_obstacle);

                        PlayerController.Score += 100;
                        GameObject.FindGameObjectWithTag("PlayerScore").GetComponent<Text>().text =
                            "Score: " + PlayerController.Score;
                        
                        MenuController.UpdateScore();

                        gameObject.GetComponent<Animator>().SetBool("isHittingObject", false);
                        gameObject.GetComponent<Animator>().SetBool("isHittingObjectDown", false);
                        Destroy(_obstacle);
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
                        if (_obstacle.transform.name == "BlockSpikes(Clone)" ||
                            _obstacle.transform.name == "BlockElectricFence(Clone)")
                        {
                            if (HitPoints <= 0)
                            {
                                PlayerController.Score += 100;
                                if (Random.Range(0, 101) > 15)
                                {
                                    Instantiate(GunPowderPickUp, new Vector2(other.transform.position.x, other.transform.position.y), Quaternion.identity);
                                }
                                GameObject.FindGameObjectWithTag("PlayerScore").GetComponent<Text>().text =
                                    "Score: " + PlayerController.Score;
                                
                                PlayerController.NumberOfZombiesKilled++;
					
                                MenuController.UpdateScore();
                                
                                Destroy(gameObject);
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
                                else
                                {
                                    HitPoints -= 30;
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
                            else
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
                            else
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
        if (_enableDamageEffect)
        {
            _obstacle.GetComponent<SpriteRenderer>().material = WhiteFlash;
        }

        _enableDamageEffect = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        /*
        if (other.transform.tag == "MainCamera")
        {
            Destroy(gameObject);
            // Clearing all the zombies that spawn on top of the player.
            Camera.main.GetComponent<BoxCollider2D>().enabled = false;
        }
        */
        
        // When the zombies are walking between trees, their movement speed
        // will be decreased temporarily.
        if (other.gameObject.layer == 14 || other.gameObject.layer == 11 && gameObject.transform.name != "Zombie Boss(Clone)")
        {
            // Make the zombie look smaller when traversing in forests
            GetComponent<Animator>().speed = 0.4f;
            
            MovementSpeed = 0.012f;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 14 || other.gameObject.layer == 11)
        {
            GetComponent<Animator>().speed = 1f;
            MovementSpeed = 0.02f;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag == "Player" || other.gameObject.layer == 10)
        {
            if (_isZombieGoingDown)
            {
                gameObject.GetComponent<Animator>().SetBool("isHittingObjectDown", false); 
                gameObject.GetComponent<Animator>().SetBool("isHittingObject", false);
            }
            else
            {
                gameObject.GetComponent<Animator>().SetBool("isHittingObjectDown", false);
                gameObject.GetComponent<Animator>().SetBool("isHittingObject", false);
            }
            gameObject.GetComponent<Rigidbody2D>().constraints =
                RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        }
        
        if (other.gameObject.layer == 14 || other.gameObject.layer == 11)
        {
            MovementSpeed = 0.02f;
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
        
        gameObject.GetComponent<SpriteRenderer>().material = WhiteFlash;
    }
}