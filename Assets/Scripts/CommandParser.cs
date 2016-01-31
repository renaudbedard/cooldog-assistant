using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class CommandParser : MonoBehaviour {

	public InputField inputField;

	Dictionary<string[], Action> COMMANDS;
	Cooldog cooldog;
	TextTyper typer;

	void Start () {
		COMMANDS = new Dictionary<string[], Action> () {
			{ new []{ "hey", "hello", "hi", "sup" }, SayHello },
			{ new []{ "batman" }, BecomeBatman },
			{ new []{ "email" }, OpenEmail },
			{ new []{ "note", "memo" }, TakeNotes },
			{ new []{ "remember", "remind" }, RememberThing }
		};

		inputField.onEndEdit.AddListener (val => {
			if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.KeypadEnter)) {
				Parse (inputField.text.ToLower ());
				inputField.text = "";
				inputField.DeactivateInputField ();
				inputField.interactable = false;
			}
		});

		cooldog = GameObject.Find ("Cooldog").GetComponent<Cooldog>();
		typer = cooldog.GetComponent<TextTyper>();
	}

	public void Parse(string cmd) {
		foreach (KeyValuePair<string[],Action> command in COMMANDS) {
			foreach (string keyword in command.Key) {
				if( Regex.IsMatch(cmd, @"\b?"+keyword+@"\b", RegexOptions.IgnoreCase) ) {
					command.Value ();
					return;
				}
			}
		}
	}

	private void SayHello(){
		List<DialoguePart> parts = new List<DialoguePart> ();
		string[] responses = { "sup.", "hey man.", "yo.", "eyy" };
		parts.Add (new DialoguePart (responses[UnityEngine.Random.Range(0, responses.Length)], 0.8f));

		typer.Play (parts);
	}

	private void BecomeBatman(){
		List<DialoguePart> parts = new List<DialoguePart> ();
		parts.Add (new DialoguePart ("yes im batdog", 1f));

		StartCoroutine (cooldog.ChangeCostume ("Batman"));

		typer.Play (parts);
	}

	private void TakeNotes() {
		List<DialoguePart> parts = new List<DialoguePart> ();
		parts.Add (new DialoguePart ("lets just write that down. yeah.", 2f));

		StartCoroutine (cooldog.ChangeCostume ("Picture"));

		typer.Play (parts);

		System.Diagnostics.Process.Start("notepad.exe");
	}

	private void RememberThing() {
		List<DialoguePart> parts = new List<DialoguePart> ();
		parts.Add (new DialoguePart ("lets just write that down. yeah.", 2f));

		StartCoroutine (cooldog.ChangeCostume ("Picture"));

		typer.Play (parts);

		System.Diagnostics.Process.Start("notepad.exe");
	}

	private void OpenEmail() {
		List<DialoguePart> parts = new List<DialoguePart> ();
		parts.Add (new DialoguePart ("an email? i can help with that", 2f));

		StartCoroutine (cooldog.ChangeCostume ("Normal"));

		typer.Play (parts);

		Application.OpenURL ("http://www.dogpile.com/search/web?q=cool+email+for+dogs");
	}

}

