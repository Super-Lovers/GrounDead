using UnityEngine;
using UnityEngine.UI;

public class GunpowderController : MonoBehaviour {
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.tag == "Player")
		{
			var numberOfGunPowderToGive = Random.Range(1, 9);
			PlayerController.GunPowder += numberOfGunPowderToGive;
			PlayerPrefs.SetFloat("Gun Powder", PlayerController.GunPowder);
			GameObject.FindGameObjectWithTag("PlayerGunPowder").GetComponent<Text>().text = PlayerPrefs.GetFloat("Gun Powder").ToString();
		
			// Player Statistics
			MenuController.TotalGunPowderCollected++;
			MenuController.TotalGatheringScore += 100;
			
			Destroy(gameObject);
		}
	}
}
