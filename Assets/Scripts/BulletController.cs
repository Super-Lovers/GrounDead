using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
	public AudioClip ZombieHitSound;
	public AudioClip StructureHitSound;
	
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
		// Before the bullet hits the object, we have to check if the
		// object is a platform or traps, because platforms and traps
		// work like bridges and the bullets shouldnt destroy them.
		/*string objName = "";
		bool isPlatform = true;
		
		for (int i = 0; i < other.name.Length; i++)
		{
			objName += other.name[i];
			if (objName == "platform" || objName == "BlockElectricFence" || objName == "BlockSpikes")
			{
				isPlatform = false;
				break;
			}
		} */
		
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
			
			// The error handling is necessary because not every game object contains
			// an AudioSource or a HitPointsController script, so it's causing errors.
			if (_obstacle.GetComponent<AudioSource>() != null)
			{
				_obstacle.GetComponent<AudioSource>().PlayOneShot(StructureHitSound);
			}

			if (_obstacle.GetComponent<HitPointsController>() != null)
			{
				_obstacle.GetComponent<HitPointsController>().HitPoints--;
				if (_obstacle.GetComponent<HitPointsController>().HitPoints <= 0)
				{
					// If the structure the bullet is hitting has no hitpoints left,
					// then destroy that obstacle and bullet, so that the bullet doesnt
					// keep going afterwards. We shouldn't forget to remove that object from the array of game objects
					// that stores all the game objects the player can interact with, because
					// that causes an error where the player is accessing an object that doesnt exist.
					Destroy(_obstacle);
					UiButtonController.PlacedBlocks.Remove(_obstacle);
				
					PlayerController.Score += 50;
					// Updating the score UI and player score after a obstacle is destroyed
					GameObject.FindGameObjectWithTag("PlayerScore").GetComponent<Text>().text = "Score: " + PlayerController.Score;
					Destroy(gameObject);
				}
				else
				{
					StartCoroutine("FlashObstacle");
				}
			}
		}
		
		if (other.transform.tag != "Player")
		{
			if (other.transform.tag == "Zombie")
			{
				DisableBullet();
				
				_obstacle = other.gameObject;
				_obstacle.GetComponent<AudioSource>().PlayOneShot(ZombieHitSound);
				_obstacle.GetComponent<ZombieController>().HitPoints--;
				if (_obstacle.GetComponent<ZombieController>().HitPoints <= 0)
				{
					Destroy(_obstacle);
					PlayerController.Score += 100;
					// Updating the score UI and player score after a zombie is killed
					GameObject.FindGameObjectWithTag("PlayerScore").GetComponent<Text>().text = "Score: " + PlayerController.Score;
					Destroy(gameObject);
				}
				else
				{
					// When the zombie is hit, its audioSource components runs the zombieHitSound
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
