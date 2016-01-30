using System;
using UnityEngine;
using System.Collections;

public class DogPart : MonoBehaviour 
{
	[Serializable]
	public struct SpriteOption
	{
		public string Name;
		public Sprite Sprite;
	}

	public SpriteOption[] Sprites;

	void Start() 
	{
		
	}
	
	void Update() 
	{
	
	}
}
