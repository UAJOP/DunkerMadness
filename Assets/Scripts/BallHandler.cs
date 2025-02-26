using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class BallHandler : MonoBehaviour
{
	
	[SerializeField] private GameObject ballPrefab;
	[SerializeField] private Rigidbody2D pivot;
	[SerializeField] private float detachDelay;
	[SerializeField] private float respawnDelay;
	
	 private Rigidbody2D currentBallRigiBody;
	 private SpringJoint2D currentBallSpringJoint;
		private Camera mainCamera;
	private bool isDragging;
	
	
	
	// Start is called before the first frame update
	void Start()
	{
		mainCamera = Camera.main;
		SpawnNewBall();
	}

	// Update is called once per frame
	void Update()
	{
		
		if(currentBallRigiBody == null)
		{
			return;
		}
		if(!Touchscreen.current.primaryTouch.press.isPressed)
		{
			if(isDragging)
			{
				LanuchBall();
			}
			
			isDragging = false;
			
			return;
			
		}
		
		
		
		isDragging = true;
		
		currentBallRigiBody.isKinematic = true;
		
		Vector2 touchPosition =	Touchscreen.current.primaryTouch.position.ReadValue();
	
		Vector3 worldPosition =	mainCamera.ScreenToWorldPoint(touchPosition);
	
		currentBallRigiBody.position = worldPosition;
		
		
	}
	
	private void SpawnNewBall()
	{
		GameObject ballInstance = 	Instantiate(ballPrefab, pivot.position, Quaternion.identity);
		currentBallRigiBody = ballInstance.GetComponent<Rigidbody2D>();
		currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();
		
		currentBallSpringJoint.connectedBody = pivot;
		
	}
	
	
	private void LanuchBall()
	{
		currentBallRigiBody.isKinematic = false;
		currentBallRigiBody = null;
		
		Invoke(nameof(DetachBall), detachDelay);
		
	}	
	private void DetachBall()
	{
	
		currentBallSpringJoint.enabled = false;
		currentBallSpringJoint = null;
		
		Invoke(nameof(SpawnNewBall), respawnDelay);
		
	}
	
}
