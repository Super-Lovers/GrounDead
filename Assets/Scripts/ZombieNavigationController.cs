using UnityEngine;

public class ZombieNavigationController : MonoBehaviour {
    private Vector2 _direction;
    public LayerMask ObstaclesLayerMask;
    private Vector2 _size;
    
    private void Start()
    {
        if (transform.name == "LeftRayCollider")
        {
            _direction = Vector2.left;
            _size = new Vector2(0.64f, 0.64f);
        } else if (transform.name == "RightRayCollider")
        {
            _direction = Vector2.right;
            _size = new Vector2(0.64f, 0.64f);
        } else if (transform.name == "TopRayCollider")
        {
            _direction = Vector2.up;
            _size = new Vector2(0.32f, 0.64f);
        } else if (transform.name == "BottomRayCollider")
        {
            _direction = Vector2.down;
            _size = new Vector2(0.32f, 0.64f);
        }
        else
        {
            _direction = Vector2.left;
            _size = new Vector2(0.64f, 0.64f);
        }
    }

    private void Update()
    {
        Vector2 origin = gameObject.transform.position;
        float angle = 0f;
        float distance = 0f;

        Debug.DrawRay(origin, _direction);
        if (Physics2D.BoxCast(origin, _size, angle, _direction, distance, ObstaclesLayerMask))
        {
            if ((transform.name == "RightRayCollider" || transform.name == "LeftRayCollider"))
            {
                Debug.Log(transform.name);
                ZombieController.CloseToAWall = true;
                WalkToTheDirection("up");
            } else if ((transform.name == "TopRayCollider" || transform.name == "BottomRayCollider"))
            {
                Debug.Log(transform.name);
                ZombieController.CloseToAWall = true;
                WalkToTheDirection("left");
            }
            else
            {
                ZombieController.CloseToAWall = false;
            }
        }
    }
    /*
    private void OnCollisionStay2D(Collision2D other)
    {
        if ((transform.name == "RightTrigger" || transform.name == "LeftTrigger") && other.gameObject.layer == 9 && other.collider.isTrigger == false)
        {
            Debug.Log(transform.name);
            ZombiesController.CloseToAWall = true;
            WalkToTheDirection("up");
        } else if ((transform.name == "TopTrigger" || transform.name == "BottomTrigger") && other.gameObject.layer == 9 && other.collider.isTrigger == false)
        {
            Debug.Log(transform.name);
            ZombiesController.CloseToAWall = true;
            WalkToTheDirection("left");
        }
        else
        {
            ZombiesController.CloseToAWall = false;
        }
    }
    */
    
    // This function is used to move the zombie away or around
    // obstances in its path
    private void WalkToTheDirection(string dir)
    {
        gameObject.GetComponentInParent<Animator>().SetBool("isWalking", false);
        
        Vector2 zombiePos = gameObject.GetComponentInParent<Transform>().position;
        if (dir == "left")
        {
            zombiePos.x -= 0.01f;
            gameObject.GetComponentInParent<Rigidbody2D>().MovePosition(zombiePos);
        } else if (dir == "right")
        {
            zombiePos.x += 0.01f;
            gameObject.GetComponentInParent<Rigidbody2D>().MovePosition(zombiePos);
        } else if (dir == "up")
        {
            zombiePos.y += 0.01f;
            gameObject.GetComponentInParent<Rigidbody2D>().MovePosition(zombiePos);
        } else if (dir == "down")
        {
            zombiePos.y -= 0.01f;
            gameObject.GetComponentInParent<Rigidbody2D>().MovePosition(zombiePos);
        }

        gameObject.GetComponentInParent<Transform>().position = zombiePos;
    } 
}