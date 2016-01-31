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

	Cooldog cooldog;

	float speed;
	AudioSource	talkingSpeaker;

	public bool typing = false;
	string targetText = "";
	public bool busy = false;

	// Use this for initialization
	void Start () {
		dialogueBox.enabled = false;
		hideAlso.SetActive(false);
		busy = false;
		talkingSpeaker = GetComponent<AudioSource> ();
		cooldog = GetComponent<Cooldog>();

		talkingSpeaker.clip = coolBark[Random.Range(0, coolBark.Length)];
		talkingSpeaker.Play();
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
			hideAlso.SetActive(true);

			foreach (char letter in targetText.ToCharArray()) {
				if (targetText == "") {
					break;
				}
				dialogueBox.text += letter;

				if (letter == ' ' || letter == '0') {
					talkingSpeaker.clip = coolBark[Random.Range(0, coolBark.Length)];
					talkingSpeaker.Play();

					cooldog.CloseMouth();
				}
				if (".aeiou?!1".Contains(letter.ToString())) {
					cooldog.OpenMouth();
				}

				yield return new WaitForSeconds(speed / (float)targetText.Length);
			}
			cooldog.CloseMouth();
			yield return new WaitForSeconds(0.5f);
		}
		busy = false;
		Hide();
	}

	public void Hide() {
		dialogueBox.enabled = false;
		hideAlso.SetActive(false);
		dialogueBox.text = "";
		targetText = "";
		typing = false;
		StopCoroutine("PlayInternal");
	}
}
