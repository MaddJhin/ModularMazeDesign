using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	public Transform Player;
	public float smoothing = 5f;
	private Vector3 InitialPosition;

	// Use this for initialization
	void Start () {
		InitialPosition = transform.position - Player.position;
	}
	
	// Update is called once per frame
	void Update () {
		// Create a postion the camera is aiming for based on the offset from the target.
		Vector3 targetCamPos = Player.position + InitialPosition;
		
		// Smoothly interpolate between the camera's current position and it's target position.
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}
