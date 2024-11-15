using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsLessonParticleDeathExplosion : MonoBehaviour
{
	//1
	/// <summary>
	/// The particle event that will be spawned when the explode function is called.
	/// </summary>
	public ParticleSystem explosionParticle;

	//2
	/// <summary>
	/// Spawns particle system at the location of the object that was killed
	/// </summary>
	/// <param name="object">Object killed.</param>
	private void Explode(GameObject @object)
	{
		//7
		Instantiate(explosionParticle, @object.transform.position, @object.transform.rotation);
	}

	//3
	/// <summary>
	/// On enable, sub Explode to OnJohnKilled
	/// </summary>
	private void OnEnable()
	{
		//5
		EventsLessonScript.OnJohnKilled += Explode;
	}

	//4
	/// <summary>
	/// On disable, unsub Explode to prevent issues
	/// </summary>
	private void OnDisable()
	{
		//6
		EventsLessonScript.OnJohnKilled -= Explode;
	}
}
