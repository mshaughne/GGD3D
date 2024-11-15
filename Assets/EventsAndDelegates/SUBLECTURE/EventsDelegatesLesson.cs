using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsDelegatesLesson : MonoBehaviour
{
	// this defines a delegate of void functions called KillEvent that required GameObject killedEntity.
	public delegate void KillEvent(GameObject killedEntity);

	/// <summary>
	/// This event calls the functions that execute when Jimmy is killed.
	/// </summary>
	public static event KillEvent OnJimmyKilled;

    private void Start()
    {
		OnJimmyKilled += HideMesh;
		OnJimmyKilled += SayNameOfKilled;
    }

    public void KillJimmy()
	{
		if(OnJimmyKilled != null)
		{
            OnJimmyKilled.Invoke(this.gameObject);
        }
	}

	/// <summary>
	/// The function looks for a meshrenderer component in the object listed.
	/// </summary>
	/// <param name="inObject">The object that should hold a meshrenderer.</param>
	public void HideMesh(GameObject inObject)
	{
		if(inObject.TryGetComponent<MeshRenderer>(out MeshRenderer rend))
		{
			rend.enabled = false;
		}
	}

	public void SayNameOfKilled(GameObject obj)
	{
		Debug.Log(obj.name);
	}
}
