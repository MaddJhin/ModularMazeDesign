using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public int CurrentRooms;

	public FloorMaker FloorMakerPrefab;

	private FloorMaker FloorMakerInstance;

	void Awake(){
		SpawnFloorMaker();
	}

	void StartGame(){
		SpawnFloorMaker();
	}

	public void SpawnFloorMaker(){
		FloorMakerInstance = Instantiate (FloorMakerPrefab) as FloorMaker;
		FloorMakerInstance.Level = this;
		FloorMakerInstance.transform.parent = this.transform;
		StartCoroutine(FloorMakerInstance.GenerateLevel());
	}

	public void SpawnFloormakerCopy (Vector3 coordinates){
		FloorMakerInstance = Instantiate (FloorMakerPrefab, coordinates, Quaternion.identity) as FloorMaker;
		FloorMakerInstance.Level = this;
		FloorMakerInstance.transform.parent = this.transform;
		StartCoroutine (FloorMakerInstance.GenerateSideRooms());
	}
	 
	void RestartGame(){
		Destroy (FloorMakerInstance);
		SpawnFloorMaker();
	}
}
