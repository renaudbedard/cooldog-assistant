using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[Serializable]
public struct AnimatedSprite
{
	public string Frame;
	public float Time;
}

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
		public AnimatedSprite[] Body;
		public AnimatedSprite[] Face;
		public AnimatedSprite[] Headdress;
		public AnimatedSprite[] Arms;
		public AnimatedSprite[] Eyes;
		public AnimatedSprite[] NeckDecoration;
		public AnimatedSprite[] Overlay;
		public AnimatedSprite[] OtherOverlay;
	}

	// TODO: Custom overrides for talk/blink/hearteyes etc

	public readonly Dictionary<string, SpriteMapping> Costumes = new Dictionary<string, SpriteMapping>
	{
		// Fallback
		{ 
			"Normal", new SpriteMapping
			{
				Body = new [] { new AnimatedSprite { Frame = "Normal" } },
				Face = new[] { new AnimatedSprite { Frame = "Normal" } },
				Headdress = new[] { new AnimatedSprite { Frame = "Ears" } },
			} 
		},

		// Keywords
		{ 
			"Batman", new SpriteMapping
			{
				Body = new [] { new AnimatedSprite { Frame = "Batman" } },
				Headdress = new[] { new AnimatedSprite { Frame = "Batman" } },
			} 
		},
		{ 
			"Horoscope", new SpriteMapping
			{
				Headdress = new[] { new AnimatedSprite { Frame = "GipsyHat" } },
				Arms = new [] { new AnimatedSprite { Frame = "CrystalBall" }  },
			} 
		},
		{ 
			"SortingHat", new SpriteMapping
			{
				Headdress = new[] { new AnimatedSprite { Frame = "WizardHat" } },
				NeckDecoration = new[] { new AnimatedSprite { Frame = "Scarf" } },
			} 
		},
		{ 
			"Quiz", new SpriteMapping
			{
				Arms = new[] { new AnimatedSprite { Frame = "Microphone" } },
			} 
		},
		{ 
			"Trivia", new SpriteMapping
			{
				Arms = new[] { new AnimatedSprite { Frame = "Book" } },
			} 
		},
		{ 
			"ELIZA", new SpriteMapping
			{
				Arms = new[] { new AnimatedSprite { Frame = "Pipe" } },
				Eyes = new[] { new AnimatedSprite { Frame = "Glasses" } },
			} 
		},
		{ 
			"Music", new SpriteMapping
			{
				Headdress = new[] { new AnimatedSprite { Frame = "Parappa" } },
			} 
		},
		{ 
			"Picture", new SpriteMapping
			{
				Arms = new[] { new AnimatedSprite { Frame = "Pencil" } },
			} 
		},

		// Weather
		{ 
			"Hot", new SpriteMapping
			{
				Face = new[] { new AnimatedSprite { Frame = "TongueOut" } },
				Eyes = new[] { new AnimatedSprite { Frame = "Sunglasses" } },
				Overlay = new[]
				{
					new AnimatedSprite { Frame = "Sweat1", Time = 0.25f },
					new AnimatedSprite { Frame = "Sweat2", Time = 0.25f }
				},
			} 
		},
		{ 
			"Cold", new SpriteMapping
			{
				Headdress = new[] { new AnimatedSprite { Frame = "Toque" } },
				NeckDecoration = new[] { new AnimatedSprite { Frame = "Scarf" } },
			} 
		},
		{ 
			"Rain", new SpriteMapping
			{
				Headdress = new[] { new AnimatedSprite { Frame = "Hoodie" } },
				//TODO : Rain overlay
			} 
		},

		// Time
		{ 
			"Morning", new SpriteMapping
			{
				Arms = new [] { new AnimatedSprite { Frame = "Coffee" }  },
			} 
		},
		{ 
			"Night", new SpriteMapping
			{
				Eyes = new[] { new AnimatedSprite { Frame = "Closed" } },
				Headdress = new[] { new AnimatedSprite { Frame = "Nightcap" } },
			} 
		},

		// State
		{ 
			"Dirty", new SpriteMapping
			{
				Overlay = new[] { new AnimatedSprite { Frame = "Dirt" } },
				OtherOverlay = new[]
				{
					new AnimatedSprite { Frame = "Flies1", Time = 0.25f },
					new AnimatedSprite { Frame = "Flies2", Time = 0.25f }
				},
			} 
		},
		{ 
			"Walk", new SpriteMapping
			{
				NeckDecoration = new[] { new AnimatedSprite { Frame = "Collar" } },
			} 
		},
		{ 
			"Eat", new SpriteMapping
			{
				Face = new[]
				{
					new AnimatedSprite { Frame = "Chewing1", Time = 0.4f },
					new AnimatedSprite { Frame = "Chewing2", Time = 0.275f }
				},
			} 
		},
		{ 
			"Barf", new SpriteMapping
			{
				Eyes = new[] { new AnimatedSprite { Frame = "Buggy" } },
				Face = new[] { new AnimatedSprite { Frame = "TongueOut" } },
			} 
		},
	};

	public SpriteMapping CurrentSet {
		get { return CurrentSet; }
		set {
			CurrentSet = value;
			ApplyCostume();
		}
	}

	void Start()
	{
		CurrentSet = Costumes["Normal"];

		var sprites = transform.FindChild("Sprites");

		Body = sprites.FindChild("Body").GetComponent<DogPart>();
		Face = sprites.FindChild("Face").GetComponent<DogPart>();
		Headdress = sprites.FindChild("Headdress").GetComponent<DogPart>();
		Arms = sprites.FindChild("Arms").GetComponent<DogPart>();
		Eyes = sprites.FindChild("Eyes").GetComponent<DogPart>();
		NeckDecoration = sprites.FindChild("NeckDecoration").GetComponent<DogPart>();
		Overlay = sprites.FindChild("Overlay").GetComponent<DogPart>();
		OtherOverlay = sprites.FindChild("OtherOverlay").GetComponent<DogPart>();

		ApplyCostume();
	}

	void ApplyCostume()
	{
		var normal = Costumes["Normal"];

		Body.SetAnimation(CurrentSet.Body ?? normal.Body);
		Face.SetAnimation(CurrentSet.Face ?? normal.Face);
		Headdress.SetAnimation(CurrentSet.Headdress ?? normal.Headdress);
		Arms.SetAnimation(CurrentSet.Arms ?? normal.Arms);
		Eyes.SetAnimation(CurrentSet.Eyes ?? normal.Eyes);
		NeckDecoration.SetAnimation(CurrentSet.NeckDecoration ?? normal.NeckDecoration);
		Overlay.SetAnimation(CurrentSet.Overlay ?? normal.Overlay);
		OtherOverlay.SetAnimation(CurrentSet.OtherOverlay ?? normal.OtherOverlay);
	}
	
	void Update()
	{
		
	}
}
