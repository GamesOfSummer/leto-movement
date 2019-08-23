using UnityEngine;

public class Leto2DTrigger : MonoBehaviour
{

	private PlayerScript player;


	public void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Leto").GetComponent<PlayerScript>();
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.isTrigger == true)
		{
			//Debug.Log("---" + other.name + " --- ground flag :: " + touching + " --- is trigger :: " + other.isTrigger);
			if (other.tag != "ground")
			{
				player.TriggerEnter(other);
			}

		}

	}



	public void OnTriggerStay2D(Collider2D other)
	{
		if (other.isTrigger == true)
		{
			//Debug.Log("---" + other.name + " --- ground flag :: " + touching + " --- is trigger :: " + other.isTrigger);
			if (other.tag != "ground")
			{
				player.TriggerEnter(other);
			}

		}
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if (other.isTrigger == true)
		{
			//Debug.Log("Exiting ---" + other.name );
			if (other.tag != "ground")
			{
				player.TriggerExit(other);
			}

		}
	}

}
