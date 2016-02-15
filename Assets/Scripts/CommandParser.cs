using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;

public class CommandParser : MonoBehaviour {

	public InputField inputField;

	Dictionary<string[], Action> COMMANDS;
	Cooldog cooldog;
	TextTyper typer;
	float RemindAt = 0f;
	Facts Facts;

	void Update () {
		if (RemindAt != 0 && RemindAt < Time.realtimeSinceStartup) {
			RemindThing ();
			RemindAt = 0;
		}
	}

	void Start () {
		COMMANDS = new Dictionary<string[], Action> () {
			{ new []{ "hey", "hello", "hi", "sup", "yo", "cooldog" }, SayHello },
            { new []{ "who are you", "your name" }, Introduce },
			{ new []{ "batman" }, BecomeBatman },
			{ new []{ "email" }, OpenEmail },
			{ new []{ "note", "memo" }, TakeNotes },
			{ new []{ "remember", "remind" }, RememberThing },
			{ new []{ "trivia", "fact", "facts", "wiki" }, TellFact },
			{ new []{ "housekeeping", "poop", "pooping", "clean", "cleaning", "smell", "smelly" }, SuggestCleaning },
            { new []{ "bye", "cya", "quit", "exit"}, Quit }
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
		Facts = new Facts ();
	}

	public void Parse(string cmd)
	{
		bool found = false;
		foreach (KeyValuePair<string[],Action> command in COMMANDS) {
			foreach (string keyword in command.Key) {
				if( Regex.IsMatch(cmd, @"\b?"+keyword+@"\b", RegexOptions.IgnoreCase) ) {
					command.Value ();
					found = true;
					return;
				}
			}
		}

		if (!found)
		{
			string[] responses =
			{
				"ain't that the truth.",
				"oh yeah, absolutely.", 
				"i... i don't know about that.", 
				"uh-huh.",
				"i don't think i learned about that in dog school.",
				"this reminds me of a cool fact i heard about.",
				"batman might have something to say about that.",
				"... what was that? sorry i heard a weird noise.",
			};
			typer.Play(0, responses[UnityEngine.Random.Range(0, responses.Length)]);
		}
	}

	private void SayHello(){
		string[] responses = {
            "sup.",
            "hey man.",
            "yo.",
            "eyy"
        };
		typer.Play(0, responses[UnityEngine.Random.Range(0, responses.Length)]);
	}

    private void Introduce() {
        string[] responses = { 
			"im cooldog, your personal computer assistant",
            "the names cooldog",
            "just a cooldog doin cool things",
            "hey i'm cooldog, nice to meet you."
		};

        float delay = cooldog.LastCostume == "Normal" ? 0 : 1.5f;
        StartCoroutine(cooldog.ChangeCostume("Normal"));

        typer.Play(delay, responses[UnityEngine.Random.Range(0, responses.Length)]);
    }

	private void BecomeBatman(){
        float delay = cooldog.LastCostume == "Batman" ? 0 : 1.5f;
        StartCoroutine (cooldog.ChangeCostume ("Batman"));
		typer.Play (delay, "yes im batdog");
	}

	private void TakeNotes() {
        float delay = cooldog.LastCostume == "Picture" ? 0 : 1.5f;
        StartCoroutine (cooldog.ChangeCostume ("Picture"));

		typer.Play (delay, "lets just write that down. yeah.");

		System.Diagnostics.Process.Start("notepad.exe");
	}

	private void RememberThing() {
		string[] responses = { 
			"sure, i'll tell you about that later",
			"i dont really tell time, but i can try",
			"sure, i can do that",
			"i'l get back to you on that one"
		};
		RemindAt = Time.realtimeSinceStartup + 40 + (UnityEngine.Random.value * 200);

        float delay = cooldog.LastCostume == "Normal" ? 0 : 1.5f;
        StartCoroutine (cooldog.ChangeCostume ("Normal"));

		typer.Play (delay, responses[UnityEngine.Random.Range(0, responses.Length)]);
	}

	private void RemindThing() {
		string[] responses = { 
			"i think you wanted me to remind you about something...",
			"i was supposed to tell you something...",
			"i forgot whateer you told me before",
			"a cooldog always forgets"
		};
		string[] responses2 = { 
			"sorry",
			"your welcome"
		};

        float delay = cooldog.LastCostume == "Normal" ? 0 : 1.5f;
        StartCoroutine (cooldog.ChangeCostume ("Normal"));

		typer.Play(delay, responses[UnityEngine.Random.Range(0, responses.Length)], responses[UnityEngine.Random.Range(0, responses2.Length)]);
	}

    private void SuggestCleaning() {
        string[] responses = { 
			"a cooldogs gotta poop",
            "gotta poop somewhere",
            "when you have tyo go, you have to go",
            "i can't hold it in forever"
		};

        string[] responses2 = { 
			"i hope this is an okay spot.",
            "this folder is getting kinda full",
            "stoop and scoop? or... delete?",
            "this mess ain't cleaning up itself"
		};

        float delay = cooldog.LastCostume == "Normal" ? 0 : 1.5f;
        StartCoroutine(cooldog.ChangeCostume("Normal"));

        typer.Play(delay, responses[UnityEngine.Random.Range(0, responses.Length)], responses2[UnityEngine.Random.Range(0, responses2.Length)]);

        System.Diagnostics.Process.Start("explorer.exe", "/select," + new DirectoryInfo(Application.dataPath).FullName);
    }

	private void TellFact() {
		string input = Facts.RandomFact();
		var charCount = 0;
		var maxLineLength = 50;

		var lines = input.Split(' ')
			.GroupBy(w => (charCount += w.Length + 1) / maxLineLength)
			.Select(g => string.Join(" ", g.ToArray()));

        float delay = cooldog.LastCostume == "Trivia" ? 0 : 1.5f;
        StartCoroutine (cooldog.ChangeCostume ("Trivia"));
		typer.Play (delay, lines.ToArray());
	}

	private void OpenEmail() {
		StartCoroutine (cooldog.ChangeCostume ("Normal"));

		typer.Play (0, "an email? i can help with that");

		Application.OpenURL ("http://www.dogpile.com/search/web?q=cool+email+for+dogs");
	}


    private void Quit()
    {
        typer.Play(0, "k bye. i'll be around.");
        StartCoroutine(QuitIn2Sec());
    }
    IEnumerator QuitIn2Sec()
    {
        yield return new WaitForSeconds(2.5f);
        yield return cooldog.WalkOutOfFrame();
        Application.Quit();
    }
}
	