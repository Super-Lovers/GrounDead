using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject Player;

	private AudioSource _audioSource;
	public AudioClip DayTheme;
	public AudioClip NightTheme;
	public static bool ChangedTheme;

	private void Start()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	void Update ()
	{
		Vector3 cameraPos = Player.transform.position;
		
		cameraPos.z = -10;
		gameObject.transform.position = cameraPos;

		
		if (ObjectivesController.IsItDay && ChangedTheme == false)
		{
			_audioSource.Stop();
			_audioSource.clip = DayTheme;
			_audioSource.Play();
			ChangedTheme = true;
		}
		if (ObjectivesController.IsItDay == false && ChangedTheme == false)
		{
			_audioSource.Stop();
			_audioSource.clip = NightTheme;
			_audioSource.Play();
			ChangedTheme = true;
		}
		
	}
}
