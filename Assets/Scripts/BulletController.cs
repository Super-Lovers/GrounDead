using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
	private Rigidbody2D _rb;
	private float _bulletSpeedVel;
	public float SpeedX;
	public float SpeedY;
	private GameObject _obstacle;

	// Visual on-hit effects
	public Material RedFlash;
	public Material YellowFlash;
	public Material WhiteFlash;
	
	void Start ()
	{
		_rb = GetComponent<Rigidbody2D>();

		if (PlayerController.Animator.GetInteger("direction") == 1)
		{
			SpeedY *= -1;
			SpeedX = 0;
		} else if (PlayerController.Animator.GetInteger("direction") == 2)
		{
			SpeedY *= 1;
			SpeedX = 0;
		} else if (PlayerController.Animator.GetInteger("direction") == 3)
		{
			SpeedX *= 1;
			SpeedY = 0;
		} else if (PlayerController.Animator.GetInteger("direction") == 4)
		{
			SpeedX *= -1;
			SpeedY = 0;
		}
	}
	
	void FixedUpdate()
	{
		_rb.velocity = new Vector3(SpeedX, SpeedY, 0);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == 10)
		{
			// Make the game object invisible and undetectable after it hits the
			// zombie so that it can still run the flash animation but not look
			// like its still going
			DisableBullet();
			// The obstacle variable will be used for storing the current
			// object hit by the bullet, and depending on that object, different
			// code blocks will be executed.
			_obstacle = other.gameObject;
			
			_obstacle.GetComponent<HitPointsController>().HitPoints--;
			if (_obstacle.GetComponent<HitPointsController>().HitPoints <= 0)
			{
				// If the structure the bullet is hitting has no hitpoints left,
				// then destroy that obstacle and bullet, so that the bullet doesnt
				// keep going afterwards.
				Destroy(_obstacle);
				Destroy(gameObject);
			}
			else
			{
				StartCoroutine("FlashObstacle");
			}
		}
		
		if (other.transform.tag != "Player")
		{
			if (other.transform.tag == "Zombie")
			{
				DisableBullet();
				
				_obstacle = other.gameObject;
				_obstacle.GetComponent<ZombieController>().HitPoints--;
				if (_obstacle.GetComponent<ZombieController>().HitPoints <= 0)
				{
					Destroy(_obstacle);
					Destroy(gameObject);
				}
				else
				{
					StartCoroutine("FlashZombie");	
				}
			}
		}
		// And if the bullet doesnt hit anything, itll dissapear
		// after a second, so that it doesnt go forever.
		Invoke("DestroyObject", 1f);
	}

	private IEnumerator FlashZombie()
	{
		for (int i = 0; i < 2; i++)
		{
			_obstacle.GetComponent<SpriteRenderer>().material = RedFlash;
			yield return new WaitForSeconds(.1f);
			_obstacle.GetComponent<SpriteRenderer>().material = WhiteFlash;
			yield return new WaitForSeconds(.1f);	
		}
				
		Destroy(gameObject);
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
				
		Destroy(gameObject);
	}

	private void DestroyObject()
	{
		Destroy(gameObject);
	}

	private void DisableBullet()
	{
		gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
		var newPos = gameObject.transform.position;
		newPos.x = 1000;
		gameObject.transform.position = newPos;
	}
}
