using System;
using UnityEngine;
using System.Collections.Generic;

public class CommandParser {

	static Dictionary<string, Action> COMMANDS = new Dictionary<string, Action>() {
		{"hey", SayHello},
		{"hello", SayHello},
		{"hi", SayHello},
		{"sup", SayHello},
	};

	public CommandParser () {
		Parse("hey");
	}

	public static void Parse(string cmd) {
		foreach(KeyValuePair<string,Action> command in COMMANDS) {
			if (cmd.Contains (command.Key)) {
				COMMANDS[cmd] ();
			}
		}
	}


	private static void SayHello(){
		List<DialoguePart> parts = new List<DialoguePart> ();
		parts.Add (new DialoguePart ("Eeeyyyyyyy", 2f));

		TextTyper typer = GameObject.Find("Cooldog").GetComponent<TextTyper>();
		typer.Play (parts);
	}

}

