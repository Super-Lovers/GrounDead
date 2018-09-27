using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public LayerMask PlayerLayerMask;
    public int HitPoints = 5;
    
    // Used to confirm that the zombie will collide with an obstacle
    public static bool CloseToAWall = false;

    void Update () {
        /*
        Ray2D rayLeft = new Ray2D(transform.position, Vector2.left);
        Ray2D rayRight = new Ray2D(transform.position, Vector2.right);
        Ray2D rayUp = new Ray2D(transform.position, Vector2.up);
        Ray2D rayDown = new Ray2D(transform.position, Vector2.down);
		*/
         
        Vector2 pos = transform.position;
        // Distance length of the rays to be cast
        float distance = (float)0.64 * 4; // 0.64 is the size of one tile
        float radius = (float) 0.64 * 5;
        Vector2 dir = new Vector2(1f, 1f);
		
        RaycastHit2D hit = Physics2D.CircleCast(pos, radius, dir, distance, PlayerLayerMask);
		
        if (hit && CloseToAWall == false)
        {
            // Make the zombie walk when the player is in radius
            gameObject.GetComponent<Animator>().SetBool("isWalking", true);
            transform.position = Vector2.MoveTowards(transform.position, hit.transform.position, 0.03f);
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("isWalking", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            gameObject.GetComponent<Rigidbody2D>().constraints =
                RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        }
    }
}