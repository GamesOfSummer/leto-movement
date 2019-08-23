using UnityEngine;
using System.Collections;



public class LetoAnimationEvents : MonoBehaviour {

	public GameObject RunningDustParticleEffect;


	private PlayerScript playerScript;





	private void Awake()
	{
		GameObject leto = GameObject.FindGameObjectWithTag("Leto");
		playerScript = leto.GetComponent<PlayerScript>();


		if (playerScript == null)
		{
			Debug.Log("Leto Animation Events - Leto's Script is missing?");
		}
	}



	void SpawnRunningDust()
	{

		   if (playerScript.groundedCheckWideTrue)
		   {
			   GameObject particle = Instantiate(RunningDustParticleEffect) as GameObject;
			   particle.transform.position = gameObject.transform.position + new Vector3(0, -0.2F, 0);
			   Destroy(particle, 2.0F);


			   if (!playerScript.FacingRight())
			   {
				   particle.transform.Rotate(0, 90, 0);
			   }
		   }

  

	}
}
