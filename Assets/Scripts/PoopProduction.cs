using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Collections;

public class PoopProduction : MonoBehaviour
{
	public float PoopDelay;

	float poopTimer;
	int poops;
	string exeDirectory;
	byte[] crunchedPoop;
	bool waitingForCleanup;

	Cooldog cooldog;
	Scratch scratch;
	AudioSource audioSource;
	TextTyper textTyper;

	public AudioClip[] FartSounds;

	public bool Poopin { get; private set; }

	void Start()
	{
		exeDirectory = new DirectoryInfo(Application.dataPath).Parent.FullName;
		poopTimer = PoopDelay * UnityEngine.Random.Range(0.8f, 1.2f);

		var texture = Resources.Load("Poop") as Texture2D;
		crunchedPoop = texture.EncodeToPNG();

		audioSource = GetComponent<AudioSource>();
		cooldog = GetComponent<Cooldog>();
		textTyper = GetComponent<TextTyper>();
		scratch = GetComponentInChildren<Scratch>();
	}

	private void CheckPoopCount()
	{
		var directory = new DirectoryInfo(exeDirectory);
		var poopCount = directory.GetFiles("Poop*.png").Length;
		if (poopCount > 2)
		{
			textTyper.Play(new List<DialoguePart>
			{
				new DialoguePart("gettin' smelly in here", 1.5f),
				new DialoguePart(string.Format("there's like {0} poops around", poopCount), 1.5f),
				new DialoguePart("would appreciate some housekeeping", 1.5f),
			});
			waitingForCleanup = true;
		}
	}

	void CheckForClean()
	{
		var directory = new DirectoryInfo(exeDirectory);
		var poopCount = directory.GetFiles("Poop*.png").Length;
		if (poopCount == 0)
		{
			textTyper.Play(new List<DialoguePart>
			{
				new DialoguePart("aw thanks man, all clean and nice here now", 2f),
			});
			waitingForCleanup = false;
		}
	}
	
	void Update()
	{
        if (textTyper.SinceIdle > 5)
        {
            poopTimer -= Time.deltaTime;
            if (poopTimer <= 0 && !Poopin && !cooldog.Blinking && !scratch.Scratching)
                StartCoroutine(Poop());
        }

		if (waitingForCleanup)
			CheckForClean();
	}

	IEnumerator Poop()
	{
		Poopin = true;

		yield return StartCoroutine(cooldog.PoopPhase1());

		audioSource.PlayOneShot(FartSounds[Random.Range(0, (int) FartSounds.Length)]);

#if !UNITY_EDITOR
		// find the numbering
		int i = 1;
		var poopPath = Path.Combine(exeDirectory, "Poop.png");
		while (File.Exists(poopPath))
		{
		    i++;
			poopPath = Path.Combine(exeDirectory, string.Format("Poop ({0}).png", i));
		}

		//Debug.Log("Pooped to " + poopPath);

		File.WriteAllBytes(poopPath, crunchedPoop);
#endif

		yield return StartCoroutine(cooldog.PoopPhase2());

		poopTimer = PoopDelay * UnityEngine.Random.Range(0.8f, 1.2f);
		CheckPoopCount();

		Poopin = false;
	}
}
