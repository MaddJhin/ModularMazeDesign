using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public LevelManager LevelPrefab;

	private LevelManager LevelInstance;
	// Use this for initialization
	void Start () {
		StartGame();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			RestartGame();
		}
	}

	void StartGame(){
		LevelInstance = Instantiate (LevelPrefab) as LevelManager;
	}

	void RestartGame() {
		StopAllCoroutines();
		Destroy(LevelInstance.gameObject);
		StartGame();
	}
}
