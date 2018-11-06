using UnityEngine;

public class StructuresColliderController : MonoBehaviour {
    private void Start()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "BlockWood(Clone)" || other.gameObject.name == "BlockStoneWall(Clone)")
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "BlockWood(Clone)" || other.gameObject.name == "BlockStoneWall(Clone)")
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}