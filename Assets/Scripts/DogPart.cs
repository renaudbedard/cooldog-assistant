using System;
using System.Linq;
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

	SpriteRenderer SpriteRenderer;

	void Start()
	{
		SpriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void SetSprite(string name)
	{
		var option = Sprites.FirstOrDefault(x => x.Name == name);
		var sprite = option.Sprite;
		SpriteRenderer.sprite = sprite;
	}
	
	void Update() 
	{
	
	}
}
