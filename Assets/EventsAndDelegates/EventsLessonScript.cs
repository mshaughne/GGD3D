using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventsLessonScript : MonoBehaviour
{
	//1
	// this defines a type of delegate
	// a delegate event is a variable that stores functions, plural
	//		v	a delegate
	//				v	the type of the functions it can store
	//						v the name of the delegate
	//								the parameters that a function stored in this event must meet
	public delegate void KillEvent(GameObject killedEntity);

	//2 don't include static
	//11 switch to static
	//12 add event v		event makes it so we can only sub or unsub outside of this script, instead of setting equals.
	/// <summary>
	/// Calls the functions that execute when John is killed.
	/// </summary>
	public static event KillEvent OnJohnKilled;

	private void Start()
	{
		//7
		// At the start, subscribe the "SayNameOfKilled" and "HideMesh" functions to OnJohnKilled.
		OnJohnKilled += SayNameOfKilled;
		OnJohnKilled += HideMesh;
	}

	//8
	/// <summary>
	/// Kills John. Invokes the OnJohnKilled event.
	/// </summary>
	public void KillJohn()
	{
		//10
		// make sure that the invoked event isn't empty
		if (OnJohnKilled != null)
		{
            //9
            // we invoke the OnJohnKilled event with this object as its target.
            OnJohnKilled.Invoke(this.gameObject);
        }
	}

	//3
	/// <summary>
	/// the function looks for a meshrenderer component in the object sent to it
	/// if it finds it, it hides it
	/// </summary>
	/// <param name="inObject">The Object where the mesh renderer is located</param>
	public void HideMesh(GameObject inObject)
	{
		//4
		// if the object has a mesh renderer
		if (inObject.TryGetComponent<MeshRenderer>(out MeshRenderer rend))
		{
			// disable the mesh renderer
			rend.enabled = false;
		}
	}

	//5
	/// <summary>
	/// this function displays the name of the game object being "killed".
	/// </summary>
	/// <param name="obj">The object we're killing.</param>
	public void SayNameOfKilled(GameObject obj)
	{
		//6
		// display the object name in the console
		Debug.Log(obj.name);
	}
}