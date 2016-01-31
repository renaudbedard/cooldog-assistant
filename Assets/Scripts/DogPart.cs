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

	float timer;
	AnimatedSprite[] currentAnimation;
	int currentFrame;

	SpriteRenderer SpriteRenderer;

	void Start()
	{
		SpriteRenderer = GetComponent<SpriteRenderer>();
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
		timer = 0;
	}
	
	void Update()
	{
		if (currentAnimation == null || currentAnimation.Length <= 1)
			return;

		timer += Time.deltaTime;

		if (timer > currentAnimation[currentFrame].Time)
		{
			timer -= currentAnimation[currentFrame].Time;

			currentFrame = (currentFrame + 1) % currentAnimation.Length;
			var option = Sprites.FirstOrDefault(x => x.Name == currentAnimation[currentFrame].Frame);
			SpriteRenderer.sprite = option.Sprite;
		}
	}
}
