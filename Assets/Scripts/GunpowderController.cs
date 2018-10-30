using UnityEngine;
using UnityEngine.UI;

public class GunpowderController : MonoBehaviour
{
	private GameObject _gunPowderUi;

	private void Start()
	{
		_gunPowderUi = GameObject.FindGameObjectWithTag("PlayerGunPowderUi");
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.tag == "Player")
		{
			_gunPowderUi.GetComponent<Animator>().SetBool("shineGunPowder", true);
			Invoke("StopGunPowderShineAnimation", 1);

			gameObject.GetComponent<CircleCollider2D>().enabled = false;
			gameObject.GetComponent<SpriteRenderer>().sprite = null;
			
			var numberOfGunPowderToGive = Random.Range(1, 9);
			PlayerController.GunPowder += numberOfGunPowderToGive;
			PlayerPrefs.SetFloat("Gun Powder", PlayerController.GunPowder);
			GameObject.FindGameObjectWithTag("PlayerGunPowder").GetComponent<Text>().text = PlayerPrefs.GetFloat("Gun Powder").ToString();
		
			// Player Statistics
			MenuController.TotalGunPowderCollected++;
			MenuController.TotalGatheringScore += 100;
			
			MenuController.UpdateScore();
			
		}
	}

	private void StopGunPowderShineAnimation()
	{
		_gunPowderUi.GetComponent<Animator>().SetBool("shineGunPowder", false);
		Destroy(gameObject);
	}
}
