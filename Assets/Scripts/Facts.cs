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
		var NewFact = "did you know that";
        bool first = true;
		foreach (string FactWord in FactChain.Chain()) {
            var fw = FactWord;
            if (first)
                fw = char.ToLower(FactWord[0]) + FactWord.Substring(1);
            NewFact += " " + fw;
            first = false;
        }
		Debug.Log(NewFact);
        return NewFact.Substring(0, NewFact.Length - 2) + "?";
	}
}
