using UnityEngine;
using System.Collections;

public class FloorMaker : MonoBehaviour {

	public float yieldTest;

	[Range (0,100)]
	public int RotateChance, BacktrackChance;
	[Range(0,100)]
	public int TurnDirection;
	[Range (1,100)]
	public int SpawnRoomChance;
	[Range (0,99)]
	public int SpawnFloorMakerChance;
	[Range (0,100)]
	public int SelfDestroyChance;
	public int MaxRooms;

	public IntVector2 CurrentPossition;


	public GameObject HallwayTile;
	public GameObject[] RoomTiles;

	public LevelManager Level;

	private bool SelfDestructed = false;

	public IEnumerator GenerateLevel() {
		while (Level.CurrentRooms < MaxRooms)
		{
			DropFloor();
			Move();
			SpawnFloormaker();
			yield return new WaitForSeconds (yieldTest);
		}
		GameObject room  = Instantiate (RoomTiles[Random.Range(0, RoomTiles.Length)], transform.position, Quaternion.identity) as GameObject;
		room.transform.parent = Level.transform;
		Level.DropFoor();
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
		int roomRoll = Random.Range(1, 101);
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
		int turnRoll = Random.Range(1, 201);
		if (turnRoll <= RotateChance)
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
		else if (RotateChance > turnRoll && turnRoll <= (RotateChance + BacktrackChance))
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
		int spawnRoll = Random.Range (1, 101);
		if (spawnRoll <= SpawnFloorMakerChance && Level.CurrentFloorMakers < Level.MaxFloorMakers)
		{
			Vector3 coordinates = transform.position;
			Level.SpawnFloormakerCopy(coordinates);
		}
	}

	void SelfDestruct (){
		int destroyRoll = Random.Range (1, 101);
		if (destroyRoll <= SelfDestroyChance)
		{
			GameObject room  = Instantiate (RoomTiles[Random.Range(0, RoomTiles.Length)], transform.position, Quaternion.identity) as GameObject;
			room.transform.parent = Level.transform;
			SelfDestructed = true;
			Destroy(gameObject);
			Level.CurrentFloorMakers--;
		}
	}
	
}
