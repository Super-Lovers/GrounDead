using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject Player;

	void Update ()
	{
		Vector3 cameraPos = gameObject.transform.position;
		cameraPos = Player.transform.position;
		cameraPos.z = -10;
		gameObject.transform.position = cameraPos;
	}
}
