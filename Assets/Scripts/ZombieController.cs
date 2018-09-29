using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ZombieController : MonoBehaviour
{
    public LayerMask PlayerLayerMask;
    public int HitPoints = 5;
    public int Strength = 1;
    public float MovementSpeed = 0.03f;
    public float RangeOfDetection = 4;
    
    // Used to confirm that the zombie will collide with an obstacle
    public static bool CloseToAWall = false;
    // Variable responsible for the delay between zombie attacks
    private bool _canAttack = true;
    private GameObject _obstacle;
    public Material YellowFlash;
    public Material WhiteFlash;
    public AudioClip StructureHitSound;

    void Update () {
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
		
        RaycastHit2D isPlayerClose = Physics2D.CircleCast(pos, radius, dir, distance, PlayerLayerMask);
		
        if (isPlayerClose)
        {
            // Make the zombie walk when the player is in radius
            gameObject.GetComponent<Animator>().SetBool("isWalking", true);
           // _audioSource.Play();
            transform.position = Vector2.MoveTowards(transform.position, isPlayerClose.transform.position, MovementSpeed);
        }
        else
        {
            //_audioSource.Stop();
            gameObject.GetComponent<Animator>().SetBool("isWalking", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player" || other.gameObject.layer == 10)
        {
            gameObject.GetComponent<Rigidbody2D>().constraints =
                RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        gameObject.GetComponent<Rigidbody2D>().constraints =
            RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        
        _obstacle = other.gameObject;
        if (other.transform.tag == "Player" || other.gameObject.layer == 10 && _canAttack)
        {
            if (_obstacle.GetComponent<AudioSource>() != null)
            {
                _obstacle.GetComponent<AudioSource>().PlayOneShot(StructureHitSound);
            }

            if (_obstacle.GetComponent<HitPointsController>() != null)
            {
                _obstacle.GetComponent<HitPointsController>().HitPoints--;
                if (_obstacle.GetComponent<HitPointsController>().HitPoints <= 0)
                {
                    Destroy(_obstacle);
                    UiButtonController.PlacedBlocks.Remove(_obstacle);
				
                    GameObject.FindGameObjectWithTag("PlayerScore").GetComponent<Text>().text = "Score: " + PlayerController.Score;
                }
                else
                {
                    StartCoroutine("FlashObstacle");
                }
            }
            
            Invoke("EnableAttacks", 1);
            
            _canAttack = false;
        }
    }

    private void EnableAttacks()
    {
        _canAttack = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag == "Player" || other.gameObject.layer == 10)
        {
            gameObject.GetComponent<Rigidbody2D>().constraints =
                RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        }
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