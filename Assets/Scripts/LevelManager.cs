using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public int CurrentRooms, MaxFloorMakers;

	public FloorMaker FloorMakerPrefab;

	public Vector3 LevelSize;
	public GameObject WallPrefab;

	private FloorMaker FloorMakerInstance;

	[HideInInspector]
	public int CurrentFloorMakers = 0;

	private GameObject[] ExistingFloors;
	private GameObject[,] FinishedLevel;
	private Vector3 StartingCoordinates;

	public Vector3 RandomCoordinates 
	{
		get 
		{
			return new Vector3 (Random.Range(0, LevelSize.x), 0, Random.Range(0, LevelSize.z));
		}
	}
	
	public bool ContainsCoordinates (IntVector2 coordinates)
	{
		return coordinates.x >= 0 && coordinates.x <= LevelSize.x && coordinates.z >= 0 && coordinates.z <= LevelSize.z;
	}

	void Awake(){
		SpawnFloorMaker();
	}

	void StartGame(){
		SpawnFloorMaker();
	}

	public void SpawnFloorMaker(){
		Vector3 coordinates = new Vector3 (1,0,1);
		FloorMakerInstance = Instantiate (FloorMakerPrefab, coordinates, Quaternion.identity) as FloorMaker;
		FloorMakerInstance.Level = this;
		FloorMakerInstance.transform.parent = this.transform;
		StartCoroutine( FloorMakerInstance.GenerateLevel());
		CurrentFloorMakers++;
	}

	public void SpawnFloormakerCopy (Vector3 coordinates){
		FloorMakerInstance = Instantiate (FloorMakerPrefab, coordinates, Quaternion.identity) as FloorMaker;
		FloorMakerInstance.Level = this;
		FloorMakerInstance.transform.parent = this.transform;
		StartCoroutine(FloorMakerInstance.GenerateSideRooms());
		CurrentFloorMakers++;
	}
	 
	void RestartGame(){
		Destroy (FloorMakerInstance);
		SpawnFloorMaker();
	}

	public void DropFoor()
	{
		ExistingFloors = GameObject.FindGameObjectsWithTag("Floor");
		Debug.Log ("Amount of floor tiles: " + ExistingFloors.Length);
		float maxX = 0;
		float minX = 0;
		float maxZ = 0;
		float minZ = 0;
		foreach (GameObject floor in ExistingFloors)
		{
			if (floor.transform.position.x > maxX)
			{
				maxX = floor.transform.position.x;
			}
			if (floor.transform.position.x < minX)
			{
				minX = floor.transform.position.x;
			}
			if (floor.transform.position.z > maxZ)
			{
				maxZ = floor.transform.position.z;
			}
			if (floor.transform.position.z < minZ)
			{
				minZ = floor.transform.position.z;
			}
		}
		Debug.Log ("Max X: " + maxX + ", Min X " + minX);
		Debug.Log ("Max Z: " + maxZ + ", Min Z " + minZ);
		int sizeX = Mathf.RoundToInt (Mathf.Abs (maxX - minX)) + 1;
		int sizeZ = Mathf.RoundToInt (Mathf.Abs (maxZ - minZ)) + 1;
		Debug.Log ("2D array should be " + (Mathf.RoundToInt (Mathf.Abs (maxX - minX)))+ " x " + (Mathf.RoundToInt (Mathf.Abs (maxZ - minZ))));
		FinishedLevel = new GameObject[sizeX,sizeZ];
		Debug.Log ("Array X " + sizeX + " Array Z " + sizeZ);
		Debug.Log (FinishedLevel.GetLength (0) + FinishedLevel.GetLength(1));
		Debug.Log ("Array Size is " + sizeX + " x " + sizeZ);
		Debug.Log ("Minimum X is " + minX);
		Debug.Log ("Minimum Z is " + minZ);

		foreach (GameObject floor in ExistingFloors)
		{
			Debug.Log( "Floor X is: " + floor.transform.position.x);
			Debug.Log ( "Floor Z is: " + floor.transform.position.z);
			int index1 = Mathf.RoundToInt(floor.transform.position.x - minX);
			int index2 = Mathf.RoundToInt(floor.transform.position.z - minZ);
			Debug.Log (index1);
			Debug.Log (index2);
			FinishedLevel[index1, index2] = floor;
		}

		for (int x = 0; x < FinishedLevel.GetLength(0); x++)
		{
			for (int z = 0; z < FinishedLevel.GetLength(1); z++)
			{
				if (FinishedLevel[x,z] == null)
				{
					Vector3 coordinates = new Vector3 (x, 0, z);
					Instantiate (WallPrefab, coordinates, Quaternion.identity);
				}
			}
		}
	}
}
