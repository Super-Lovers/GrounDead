using UnityEngine;

public class HitPointsController : MonoBehaviour
{
	private string _name;
	public int HitPoints = 3;

	void Start ()
	{
		_name = gameObject.name;
		string currentObjectName = "";
		
		for (int i = 0; i < _name.Length; i++)
		{
			currentObjectName += _name[i];
			// TODO: check for specific objects
		}
	}
}
