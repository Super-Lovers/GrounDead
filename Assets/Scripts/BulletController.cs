using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
	private Rigidbody2D _rb;
	private float _bulletSpeedVel;
	public float SpeedX;
	public float SpeedY;
	private GameObject _hostile;

	// Visual on-hit effects
	public Material RedFlash;
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
			Destroy(gameObject);
		}
		
		if (other.transform.tag != "Player")
		{
			if (other.transform.tag == "Zombie")
			{
				_hostile = other.gameObject;

				StartCoroutine("FlashColors");
			}
		}
		// And if the bullet doesnt hit anything, itll dissapear
		// after a second, so that it doesnt go forever.
		Invoke("DestroyObject", 1f);
	}

	private IEnumerator FlashColors()
	{
		// Make the game object invisible and undetectable after it hits the
		// zombie so that it can still run the flash animation but not look
		// like its still going
		gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
		gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
		
		for (int i = 0; i < 2; i++)
		{
		
			_hostile.GetComponent<SpriteRenderer>().material = RedFlash;
			yield return new WaitForSeconds(.1f);
			_hostile.GetComponent<SpriteRenderer>().material = WhiteFlash;
			yield return new WaitForSeconds(.1f);	
		}
				
		Destroy(gameObject);
	}

	private void DestroyObject()
	{
		Destroy(gameObject);
	}
}
