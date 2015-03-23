using UnityEngine;
using System.Collections;

public class FloorMaker : MonoBehaviour {

	public float yieldTest;

	public int RotateChance, BacktrackChance;

	[Range(0,100)]
	public int TurnDirection;
	public int SpawnRoomChance;
	public int SpawnFloorMakerChance;
	public int SelfDestroyChance;
	public int MaxRooms;

	public IntVector2 CurrentPossition;
	public IntVector2 LevelSize;

	public GameObject HallwayTile;
	public GameObject[] RoomTiles;

	public LevelManager Level;
//	public FloorMaker FloorMakerPrefab;
	
//	public int CurrentRooms;

	private bool SelfDestructed = false;

	public IntVector2 RandomCoordinates 
	{
		get 
		{
			return new IntVector2 (Random.Range(0, LevelSize.x),Random.Range(0, LevelSize.z));
		}
	}
	
	public bool ContainsCoordinates (IntVector2 coordinates)
	{
		return coordinates.x >= 0 && coordinates.x <= LevelSize.x && coordinates.z >= 0 && coordinates.z <= LevelSize.z;
	}

	public IEnumerator GenerateLevel() {
		while (Level.CurrentRooms < MaxRooms)
		{
			DropFloor();
			Move();
			SpawnFloormaker();
			yield return new WaitForSeconds (yieldTest);
		}
	}

	public IEnumerator GenerateSideRooms() {
		while (Level.CurrentRooms < MaxRooms && SelfDestructed == false )
		{
			DropFloor();
			Move();
			SpawnFloormaker();
			SelfDestruct();
			yield return new WaitForSeconds (yieldTest);
		}
	}

	void DropFloor(){
		int roomRoll = Random.Range(0, 100);
		if (roomRoll <= SpawnRoomChance)
		{
			GameObject room  = Instantiate (RoomTiles[Random.Range(0, RoomTiles.Length)], transform.position, Quaternion.identity) as GameObject;
			room.transform.parent = Level.transform;
			Level.CurrentRooms++;
		}
		else
		{
			GameObject hallway = Instantiate(HallwayTile, transform.position, Quaternion.identity) as GameObject;
			hallway.transform.parent = Level.transform;
		}
	}

	void Move(){
		int turnRoll = Random.Range(0, 200);
		if (turnRoll < RotateChance)
		{
			int turnDirectionRoll = Random.Range (0, 100);
			if ( turnDirectionRoll < TurnDirection)
			{
				transform.Rotate (0,90,0);
				transform.Translate (Vector3.forward);
			}
			else
			{
				transform.Rotate (0,-90,0);
				transform.Translate (Vector3.forward);
			}
		}
		else if (RotateChance < turnRoll && turnRoll <= (RotateChance + BacktrackChance))
		{
			transform.Rotate (0,180,0);
			transform.Translate (Vector3.forward);
		}
		else
		{
			transform.Translate (Vector3.forward);
		}
	}

	void SpawnFloormaker(){
		int spawnRoll = Random.Range (0, 100);
		if (spawnRoll < SpawnFloorMakerChance)
		{
			Vector3 coordinates = transform.position;
			Level.SpawnFloormakerCopy(coordinates);
		}
	}

	void SelfDestruct (){
		int destroyRoll = Random.Range (0, 100);
		if (destroyRoll < SelfDestroyChance)
		{
			GameObject room  = Instantiate (RoomTiles[Random.Range(0, RoomTiles.Length)], transform.position, Quaternion.identity) as GameObject;
			room.transform.parent = Level.transform;
			SelfDestructed = true;
			Destroy(gameObject);
		}
	}
	
}
