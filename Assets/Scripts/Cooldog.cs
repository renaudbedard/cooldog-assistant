using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Cooldog : MonoBehaviour
{
	DogPart Body;
	DogPart Face;
	DogPart Headdress;
	DogPart Arms;
	DogPart Eyes;
	DogPart NeckDecoration;
	DogPart Overlay;
	DogPart OtherOverlay;

	[Serializable]
	public struct SpriteMapping
	{
		public string Body;
		public string Face;
		public string Headdress;
		public string Arms;
		public string Eyes;
		public string NeckDecoration;
		public string Overlay;
		public string OtherOverlay;
	}

	public SpriteMapping DefaultSet;
	public SpriteMapping CurrentSet;

	void Start()
	{
		CurrentSet = DefaultSet;

		var sprites = transform.FindChild("Sprites");

		Body = sprites.FindChild("Body").GetComponent<DogPart>();
		Face = sprites.FindChild("Face").GetComponent<DogPart>();
		Headdress = sprites.FindChild("Headdress").GetComponent<DogPart>();
		Arms = sprites.FindChild("Arms").GetComponent<DogPart>();
		Eyes = sprites.FindChild("Eyes").GetComponent<DogPart>();
		NeckDecoration = sprites.FindChild("NeckDecoration").GetComponent<DogPart>();
		Overlay = sprites.FindChild("Overlay").GetComponent<DogPart>();
		OtherOverlay = sprites.FindChild("OtherOverlay").GetComponent<DogPart>();

		ApplySpriteSet();
	}

	void ApplySpriteSet()
	{
		Body.SetSprite(CurrentSet.Body);
		Face.SetSprite(CurrentSet.Face);
		Headdress.SetSprite(CurrentSet.Headdress);
		Arms.SetSprite(CurrentSet.Arms);
		Eyes.SetSprite(CurrentSet.Eyes);
		NeckDecoration.SetSprite(CurrentSet.NeckDecoration);
		Overlay.SetSprite(CurrentSet.Overlay);
		OtherOverlay.SetSprite(CurrentSet.OtherOverlay);
	}
	
	void Update()
	{
	
	}
}
