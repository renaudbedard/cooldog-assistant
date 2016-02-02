using System.Collections.Generic;
using UnityEngine;

public class Scratch : MonoBehaviour
{
	Cooldog Cooldog;
	TextTyper textTyper;

	bool draggingWindow;
	Vector2 startPosition;
	Vector2 lastPosition;
	float totalDistance;
	bool lovingIt;

	public bool Scratching { get; private set; }

	public void Start()
	{
		Cooldog = transform.parent.GetComponent<Cooldog>();
		textTyper = transform.parent.GetComponent<TextTyper>();
	}

	void OnMouseDown()
	{
		lastPosition = startPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		draggingWindow = false;
	}
	void OnMouseUp()
	{
		totalDistance = 0;
        if (Scratching)
            Cooldog.SetScratching(false, false);
		Scratching = false;
		lovingIt = false;
	}

	void OnMouseDrag()
	{
		var thisPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		if (!Scratching && (startPosition - thisPosition).magnitude > Draggable.DragLimit)
		{
			draggingWindow = true;
		}
		if (draggingWindow) return;

		totalDistance += (lastPosition - thisPosition).magnitude;
		lastPosition = thisPosition;

		if (totalDistance > 500)
		{
			if (!Scratching)
			{
				Cooldog.SetScratching(true, false);
				textTyper.Play(0, "aaah yeah");
			}

			if (!lovingIt && totalDistance > 1500)
			{
				lovingIt = true;
				Cooldog.SetScratching(true, true);
				textTyper.Play(0, "that's the spot");
			}
			Scratching = true;
		}
	}
}
