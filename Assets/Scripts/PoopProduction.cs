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

	Cooldog cooldog;

	void Start()
	{
		exeDirectory = new DirectoryInfo(Application.dataPath).Parent.FullName;
		poopTimer = PoopDelay * UnityEngine.Random.Range(0.8f, 1.2f);

		var texture = Resources.Load("Poop") as Texture2D;
		crunchedPoop = texture.EncodeToPNG();

		cooldog = GetComponent<Cooldog>();
	}
	
	void Update()
	{
		poopTimer -= Time.deltaTime;
		if (poopTimer <= 0)
		{
			Poop();
			poopTimer = PoopDelay * UnityEngine.Random.Range(0.8f, 1.2f);
		}
	}

	void Poop()
	{
		//StartCoroutine()

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
	}
}
