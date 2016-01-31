using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CommandParser : MonoBehaviour {

	public InputField inputField;

	Dictionary<string, Action> COMMANDS;

	void Start () {
		COMMANDS = new Dictionary<string, Action> () {
			{ "hey", SayHello },
			{ "hello", SayHello },
			{ "hi", SayHello },
			{ "sup", SayHello },
			{ "batman", BecomeBatman },
			{ "email", OpenEmail },
			{ "note", TakeNotes },
			{ "memo", TakeNotes },
			{ "remember", TakeNotes }
		};

		inputField.onEndEdit.AddListener(val => {
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
				Parse(inputField.text.ToLower());
				inputField.text = "";
				inputField.DeactivateInputField();
				inputField.interactable = false;
			}
		});
	}

	public void Parse(string cmd) {
		foreach(KeyValuePair<string,Action> command in COMMANDS) {
			if (cmd.Contains (command.Key)) {
				command.Value();
				break;
			}
		}
	}

	private void SayHello(){
		List<DialoguePart> parts = new List<DialoguePart> ();
		parts.Add (new DialoguePart ("Eeeyyyyyyy", 2f));

		TextTyper typer = GameObject.Find("Cooldog").GetComponent<TextTyper>();
		typer.Play (parts);
	}

	private void BecomeBatman(){
		List<DialoguePart> parts = new List<DialoguePart> ();
		parts.Add (new DialoguePart ("yes im batdog", 1f));

		Cooldog cooldog = GameObject.Find ("Cooldog").GetComponent<Cooldog>();
		StartCoroutine (cooldog.ChangeCostume ("Batman"));

		TextTyper typer = GameObject.Find("Cooldog").GetComponent<TextTyper>();
		typer.Play (parts);
	}

	private void TakeNotes() {
		List<DialoguePart> parts = new List<DialoguePart> ();
		parts.Add (new DialoguePart ("lets just write that down. yeah.", 2f));

		Cooldog cooldog = GameObject.Find ("Cooldog").GetComponent<Cooldog> ();
		StartCoroutine (cooldog.ChangeCostume ("Picture"));

		TextTyper typer = GameObject.Find ("Cooldog").GetComponent<TextTyper> ();
		typer.Play (parts);

		System.Diagnostics.Process.Start("notepad.exe");
	}

	private void OpenEmail() {
		List<DialoguePart> parts = new List<DialoguePart> ();
		parts.Add (new DialoguePart ("an email? i can help with that", 2f));

		Cooldog cooldog = GameObject.Find ("Cooldog").GetComponent<Cooldog> ();
		StartCoroutine (cooldog.ChangeCostume ("Normal"));

		TextTyper typer = GameObject.Find ("Cooldog").GetComponent<TextTyper> ();
		typer.Play (parts);

		Application.OpenURL ("http://www.dogpile.com/search/web?q=cool+email+for+dogs");
	}

}

