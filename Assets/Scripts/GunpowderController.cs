using UnityEngine;
using UnityEngine.UI;

public class GunpowderController : MonoBehaviour {
	private void Start()
	{
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.tag == "Player")
		{
			var numberOfGunPowderToGive = Random.Range(1, 9);
			PlayerController.GunPowder += numberOfGunPowderToGive;
			PlayerPrefs.SetFloat("Gun Powder", numberOfGunPowderToGive);
			GameObject.FindGameObjectWithTag("PlayerGunPowder").GetComponent<Text>().text = PlayerPrefs.GetFloat("Gun Powder").ToString();
			
			// Play animation
		}
		
		Destroy(gameObject);
	}
}
