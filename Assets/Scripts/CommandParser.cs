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
			{ "batman", BecomeBatman }
		};

		inputField.onEndEdit.AddListener(val => {
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
				Parse(inputField.text.ToLower());
				inputField.text = "";
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
		parts.Add (new DialoguePart ("yes im batdog", 2f));

		Cooldog cooldog = GameObject.Find ("Cooldog").GetComponent<Cooldog>();
		cooldog.CurrentSet = cooldog.Costumes ["Batman"];

		TextTyper typer = GameObject.Find("Cooldog").GetComponent<TextTyper>();
		typer.Play (parts);


	}

}

