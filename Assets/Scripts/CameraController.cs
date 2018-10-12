using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject Player;

	void Update ()
	{
		Vector3 cameraPos = Player.transform.position;
		
		cameraPos.z = -10;
		gameObject.transform.position = cameraPos;
	}
}
