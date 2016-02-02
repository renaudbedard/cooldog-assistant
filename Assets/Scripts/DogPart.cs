using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

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
	public AnimatedSprite[] CurrentAnimation
    {
        get { return AnimationStack.Count == 0 ? null : AnimationStack.Peek(); }
    }
    readonly Stack<AnimatedSprite[]> AnimationStack = new Stack<AnimatedSprite[]>();

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
        AnimationStack.Clear();
        PushAnimation(anim);
	}

    public void PushAnimation(AnimatedSprite[] anim)
    {
        if (AnimationStack.Count == 0 || AnimationStack.Peek() != anim)
        {
            AnimationStack.Push(anim);
            UpdateAnimation();
        }
    }
    public void PopAnimation(AnimatedSprite[] anim)
    {
        if (AnimationStack.Count > 0 && AnimationStack.Peek() == anim)
        {
            AnimationStack.Pop();
            UpdateAnimation();
        }
    }

    void UpdateAnimation()
    {
        if (CurrentAnimation == null || CurrentAnimation.Length == 0)
            SpriteRenderer.sprite = null;
        else
        {
            var option = Sprites.FirstOrDefault(x => x.Name == CurrentAnimation[0].Frame);
            SpriteRenderer.sprite = option.Sprite;
        }

        currentFrame = 0;
        animationTimer = 0;
    }
	
	void Update()
	{
		transform.localRotation = Quaternion.AngleAxis(Mathf.Sin(rotationStep * shakeFactor), Vector3.forward);
		rotationStep += Time.deltaTime * rotationSpeed * shakeFactor;

		if (CurrentAnimation != null && CurrentAnimation.Length > 1)
		{
			animationTimer += Time.deltaTime;

			if (animationTimer > CurrentAnimation[currentFrame].Time)
			{
				animationTimer -= CurrentAnimation[currentFrame].Time;

				currentFrame = (currentFrame + 1) % CurrentAnimation.Length;
				var option = Sprites.FirstOrDefault(x => x.Name == CurrentAnimation[currentFrame].Frame);
				SpriteRenderer.sprite = option.Sprite;
			}
		}
	}

	public void SetShakeFactor(float factor)
	{
		shakeFactor = factor;
	}
}
