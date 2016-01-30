using UnityEngine;
using System.Collections;

public class ClickDetector : MonoBehaviour
{
	public Sprite Click;
	public Sprite Idle;

	void Start() 
	{
	
	}
	
	void Update() 
	{
	}

	void OnMouseDown()
	{
		GetComponentInChildren<SpriteRenderer>().sprite = Click;
	}
	void OnMouseUp()
	{
		GetComponentInChildren<SpriteRenderer>().sprite = Idle;
	}
}
