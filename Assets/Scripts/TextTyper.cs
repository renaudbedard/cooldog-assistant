using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class DialoguePart {
	public string Line;
	public float speed = 1.5f;

	public DialoguePart(string line, float speed) {
		this.Line = line;
		this.speed = speed;
	}
}

public class TextTyper : MonoBehaviour {
	[SerializeField]
	Text dialogueBox;

	[SerializeField]
	GameObject hideAlso;

	[SerializeField]
	AudioClip[] coolBark;

	float speed;
	AudioSource	talkingSpeaker;

	public bool typing = false;
	string targetText = "";
	public bool busy = false;

	// Use this for initialization
	void Start () {
		dialogueBox.enabled = false;
		hideAlso.active = false;
		busy = false;
		talkingSpeaker = GetComponent<AudioSource> ();
	}

	public void Play(List<DialoguePart> parts) {
		busy = true;
		StartCoroutine(PlayInternal(parts));
	}

	IEnumerator PlayInternal(List<DialoguePart> parts) {
		foreach (DialoguePart part in parts) {
			targetText = part.Line;
			speed = part.speed;

			dialogueBox.text = "";
			dialogueBox.enabled = true;
			hideAlso.active = true;

			foreach (char letter in targetText.ToCharArray()) {
				if (targetText == "") {
					break;
				}
				dialogueBox.text += letter;

				if (letter == ' ' || letter == '0') {
					//talkingSpeaker.clip = coolBark[Random.Range(0, coolBark.Length)];
					//talkingSpeaker.Play();

					// Open mouth (Animate)
				}
				if (".aeiou?!1".Contains(letter.ToString())) {
					// Shut mouth (Animate)
				}

				yield return new WaitForSeconds(speed / (float)targetText.Length);
			}

			yield return new WaitForSeconds(0.5f);
		}
		busy = false;
		Hide();
	}

	public void Hide() {
		dialogueBox.enabled = false;
		hideAlso.active = false;
		dialogueBox.text = "";
		targetText = "";
		typing = false;
		StopCoroutine("PlayInternal");
	}
}
