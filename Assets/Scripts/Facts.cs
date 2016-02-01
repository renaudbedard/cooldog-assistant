using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Facts
{
	Markov.MarkovChain<string> FactChain;

	public Facts() {
		FactChain = new Markov.MarkovChain<string>(1);

		var FactsFile = Resources.Load("facts") as TextAsset;
		var Facts = FactsFile.text.Split("\n"[0]);

		foreach (string line in Facts) {
			FactChain.Add (line.Split (' '));
		}
	}

	public string RandomFact() {
		var NewFact = "Did you know that";
		foreach (string FactWord in FactChain.Chain()) {
			NewFact += " " + FactWord;
		}
		Debug.Log(NewFact);
		return NewFact;
	}
}
