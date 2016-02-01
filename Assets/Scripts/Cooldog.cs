using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

[Serializable]
public struct AnimatedSprite
{
	public string Frame;
	public float Time;

    public override string ToString()
    {
        return Frame;
    }
}

public class Cooldog : MonoBehaviour
{
	bool flipped;
	public bool Flipped
	{
		set
		{
			foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
				sr.flipX = value;
			flipped = value;

			var armPos = Arms.transform.localPosition;
			Arms.transform.localPosition = new Vector3(Mathf.Abs(armPos.x) * (flipped ? 1 : -1), armPos.y, armPos.z);
		}
		get { return flipped; }
	}

	DogPart Body;
	DogPart Face;
	DogPart Headdress;
	DogPart Arms;
	DogPart Eyes;
	DogPart NeckDecoration;
	DogPart Overlay;
	DogPart OtherOverlay;

	[Serializable]
	public struct Costume
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

	public readonly Dictionary<string, Costume> Costumes = new Dictionary<string, Costume>
	{
		// Fallback
		{ 
			"Normal", new Costume
			{
				Body = new [] { new AnimatedSprite { Frame = "Normal" } },
				Face = new[] { new AnimatedSprite { Frame = "Normal" } },
				Headdress = new[] { new AnimatedSprite { Frame = "Ears" } },
			} 
		},

		// Keywords
		{ 
			"Batman", new Costume
			{
				Body = new [] { new AnimatedSprite { Frame = "Batman" } },
				Headdress = new[] { new AnimatedSprite { Frame = "Batman" } },
			} 
		},
		{ 
			"Horoscope", new Costume
			{
				Headdress = new[] { new AnimatedSprite { Frame = "GipsyHat" } },
				Arms = new [] { new AnimatedSprite { Frame = "CrystalBall" }  },
			} 
		},
		{ 
			"SortingHat", new Costume
			{
				Headdress = new[] { new AnimatedSprite { Frame = "WizardHat" } },
				NeckDecoration = new[] { new AnimatedSprite { Frame = "Scarf" } },
			} 
		},
		{ 
			"Quiz", new Costume
			{
				Arms = new[] { new AnimatedSprite { Frame = "Microphone" } },
			} 
		},
		{ 
			"Trivia", new Costume
			{
				Arms = new[] { new AnimatedSprite { Frame = "Book" } },
				Eyes = new[] { new AnimatedSprite { Frame = "Glasses" } },
			} 
		},
		{ 
			"ELIZA", new Costume
			{
				Arms = new[] { new AnimatedSprite { Frame = "Pipe" } },
				Eyes = new[] { new AnimatedSprite { Frame = "Glasses" } },
			} 
		},
		{ 
			"Music", new Costume
			{
				Headdress = new[] { new AnimatedSprite { Frame = "Parappa" } },
			} 
		},
		{ 
			"Picture", new Costume
			{
				Arms = new[] { new AnimatedSprite { Frame = "Pen" } },
			} 
		},

		// Weather
		{ 
			"Hot", new Costume
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
			"Cold", new Costume
			{
				Headdress = new[] { new AnimatedSprite { Frame = "Toque" } },
				NeckDecoration = new[] { new AnimatedSprite { Frame = "Scarf" } },
			} 
		},
		{ 
			"Rain", new Costume
			{
				Headdress = new[] { new AnimatedSprite { Frame = "Hoodie" } },
				//TODO : Rain overlay
			} 
		},

		// Time
		{ 
			"Morning", new Costume
			{
				Arms = new [] { new AnimatedSprite { Frame = "Coffee" }  },
			} 
		},
		{ 
			"Night", new Costume
			{
				Eyes = new[] { new AnimatedSprite { Frame = "Closed" } },
				Headdress = new[] { new AnimatedSprite { Frame = "Nightcap" } },
			} 
		},

		// State
		{ 
			"Dirty", new Costume
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
			"Walk", new Costume
			{
				NeckDecoration = new[] { new AnimatedSprite { Frame = "Collar" } },
			} 
		},
		{ 
			"Eat", new Costume
			{
				Face = new[]
				{
					new AnimatedSprite { Frame = "Chewing1", Time = 0.4f },
					new AnimatedSprite { Frame = "Chewing2", Time = 0.275f }
				},
			} 
		},
		{ 
			"Barf", new Costume
			{
				Eyes = new[] { new AnimatedSprite { Frame = "Buggy" } },
				Face = new[] { new AnimatedSprite { Frame = "TongueOut" } },
			} 
		},
	};

	PoopProduction PoopSack;
	public bool Blinking { get; private set; }
    float toNextBlink;

    void Start()
	{
		Body = GameObject.Find("Body").GetComponent<DogPart>();
		Face = GameObject.Find("Face").GetComponent<DogPart>();
		Headdress = GameObject.Find("Headdress").GetComponent<DogPart>();
		Arms = GameObject.Find("Arms").GetComponent<DogPart>();
		Eyes = GameObject.Find("Eyes").GetComponent<DogPart>();
		NeckDecoration = GameObject.Find("NeckDecoration").GetComponent<DogPart>();
		Overlay = GameObject.Find("Overlay").GetComponent<DogPart>();
		OtherOverlay = GameObject.Find("OtherOverlay").GetComponent<DogPart>();

		PoopSack = GetComponent<PoopProduction>();

        toNextBlink = UnityEngine.Random.Range(1, 10);

        StartCoroutine(ChangeCostume("Normal"));
	}

    string LastCostume;
	public IEnumerator ChangeCostume(string costume) 
	{
        if (LastCostume != costume)
        {
            if (LastCostume != null)
                yield return WalkOutOfFrame();
            ApplyCostume(Costumes[costume]);
            yield return WalkIntoFrame();
        }
        LastCostume = costume;
    }

	const float WalkOffset = 17;
	const float WalkSpeed = 20;
	const float BobHeight = 0.75f;
	const float BobSpeed = 3;

	public IEnumerator WalkOutOfFrame()
	{
		float sign = Flipped ? -1 : 1;

		float step = 0;
		while (step < 1)
		{
			var easedStep = Easing.EaseIn(Mathf.Clamp01(step), EasingType.Sine);
			transform.localPosition = new Vector3(easedStep * sign * WalkOffset, Math.Abs(Mathf.Cos(easedStep * BobSpeed * Mathf.PI)) * BobHeight - BobHeight, 0);
			step += Time.deltaTime * WalkSpeed / WalkOffset;
			yield return new WaitForEndOfFrame();
		}
		transform.localPosition = new Vector3(WalkOffset * sign, 0, 0);
	}
	public IEnumerator WalkIntoFrame()
	{
		float sign = Flipped ? -1 : 1;

		float step = 0;
		while (step < 1)
		{
			var easedStep = Easing.EaseOut(Mathf.Clamp01(step), EasingType.Sine);
			transform.localPosition = new Vector3((1 - easedStep) * sign * WalkOffset, Math.Abs(Mathf.Cos(easedStep * BobSpeed * Mathf.PI)) * BobHeight - BobHeight, 0);
			step += Time.deltaTime * WalkSpeed / WalkOffset;
			yield return new WaitForEndOfFrame();
		}
		transform.localPosition = new Vector3(0, 0, 0);
	}

	readonly AnimatedSprite[] BlinkEyes = new[] { new AnimatedSprite { Frame = "Closed" } };
	public IEnumerator Blink()
	{
		if (!hasMouthOpen)
		{
			if (Eyes.CurrentAnimation == null || Eyes.CurrentAnimation.Length == 0 || Eyes.CurrentAnimation[0].Frame == "Buggy")
			{
				Blinking = true;
				Eyes.PushAnimation(BlinkEyes);
				yield return new WaitForSeconds(UnityEngine.Random.Range(0.075f, 0.175f));
                Eyes.PopAnimation(BlinkEyes);
				Blinking = false;
			}
		}
	}

	readonly AnimatedSprite[] PoopEyes = new[] { new AnimatedSprite { Frame = "Buggy" } };
	readonly AnimatedSprite[] ReliefFace = new[] { new AnimatedSprite { Frame = "TongueOut" } };
	public IEnumerator PoopPhase1()
	{
		Eyes.PushAnimation(PoopEyes);
		Body.SetShakeFactor(4);
		yield return new WaitForSeconds(0.75f);
		Body.SetShakeFactor(5);
		yield return new WaitForSeconds(0.7f);
		Body.SetShakeFactor(6);
		yield return new WaitForSeconds(0.65f);
		Eyes.PopAnimation(PoopEyes);
	}
	public IEnumerator PoopPhase2()
	{
		Body.SetShakeFactor(1);
		Face.PushAnimation(ReliefFace);
		Eyes.PushAnimation(BlinkEyes);
		yield return new WaitForSeconds(UnityEngine.Random.Range(1.5f, 2.0f));
		Face.PopAnimation(ReliefFace);
		Eyes.PopAnimation(BlinkEyes);
	}

	bool hasMouthOpen;
	readonly AnimatedSprite[] TalkFace = new [] { new AnimatedSprite { Frame = "Talk" } };
	public void OpenMouth()
	{
		if (Face.CurrentAnimation == null || Face.CurrentAnimation[0].Frame == "Normal")
		{
			Face.PushAnimation(TalkFace);
			hasMouthOpen = true;
		}
	}
	public void CloseMouth()
	{
		if (hasMouthOpen)
		{
			Face.PopAnimation(TalkFace);
			hasMouthOpen = false;
		}
	}

	readonly AnimatedSprite[] HeartEyes = new[] { new AnimatedSprite { Frame = "Heart" } };
	public void SetScratching(bool value, bool heartEyes)
	{
		if (value)
			Face.PushAnimation(ReliefFace);
		else
			Face.PopAnimation(ReliefFace);

		if (heartEyes)
			Eyes.PushAnimation(HeartEyes);
		else
			Eyes.PopAnimation(HeartEyes);
	}

	void ApplyCostume(Costume costume)
	{
		var normal = Costumes["Normal"];

		Body.SetAnimation(costume.Body ?? normal.Body);
		Face.SetAnimation(costume.Face ?? normal.Face);
		Headdress.SetAnimation(costume.Headdress ?? normal.Headdress);
		Arms.SetAnimation(costume.Arms ?? normal.Arms);
		Eyes.SetAnimation(costume.Eyes ?? normal.Eyes);
		NeckDecoration.SetAnimation(costume.NeckDecoration ?? normal.NeckDecoration);
		Overlay.SetAnimation(costume.Overlay ?? normal.Overlay);
		OtherOverlay.SetAnimation(costume.OtherOverlay ?? normal.OtherOverlay);
	}

	void Update()
	{
		if (!PoopSack.Poopin)
		{
			toNextBlink -= Time.deltaTime;
			if (toNextBlink <= 0)
			{
				StartCoroutine(Blink());
				toNextBlink = UnityEngine.Random.Range(1, 10);

				// double blink
				if (UnityEngine.Random.value < 0.25)
					toNextBlink = UnityEngine.Random.Range(0.3f, 0.6f);
			}
		}
	}
}
