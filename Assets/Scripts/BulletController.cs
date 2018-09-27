using UnityEngine;

public class BulletController : MonoBehaviour
{
	private Rigidbody2D _rb;
	private float _bulletSpeedVel;
	public float SpeedX;
	public float SpeedY;
	
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
		if (other.transform.tag != "Player")
		{
			if (other.transform.tag == "Zombie")
			{
				Destroy(gameObject);
			}
		}
	}
}
