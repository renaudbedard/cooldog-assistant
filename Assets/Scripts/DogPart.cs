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

	float animationTimer;
	AnimatedSprite[] currentAnimation;
	int currentFrame;

	float rotationStep;
	float rotationSpeed;
	float shakeFactor;

	SpriteRenderer SpriteRenderer;

	void Start()
	{
		SpriteRenderer = GetComponent<SpriteRenderer>();

		rotationStep = UnityEngine.Random.Range(0, Mathf.PI * 2);
		rotationSpeed = UnityEngine.Random.Range(1, 5);
		shakeFactor = 1;
	}

	public void SetAnimation(AnimatedSprite[] anim)
	{
		if (anim == null || anim.Length == 0)
			SpriteRenderer.sprite = null;
		else
		{
			var option = Sprites.FirstOrDefault(x => x.Name == anim[0].Frame);
			SpriteRenderer.sprite = option.Sprite;
		}

		currentAnimation = anim;
		currentFrame = 0;
		animationTimer = 0;
	}
	
	void Update()
	{
		transform.localRotation = Quaternion.AngleAxis(Mathf.Sin(rotationStep * shakeFactor), Vector3.forward);
		rotationStep += Time.deltaTime * rotationSpeed * shakeFactor;

		if (currentAnimation != null && currentAnimation.Length > 1)
		{
			animationTimer += Time.deltaTime;

			if (animationTimer > currentAnimation[currentFrame].Time)
			{
				animationTimer -= currentAnimation[currentFrame].Time;

				currentFrame = (currentFrame + 1) % currentAnimation.Length;
				var option = Sprites.FirstOrDefault(x => x.Name == currentAnimation[currentFrame].Frame);
				SpriteRenderer.sprite = option.Sprite;
			}
		}
	}

	public void SetShakeFactor(float factor)
	{
		shakeFactor = factor;
	}
}
