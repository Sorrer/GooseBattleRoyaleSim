using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlModes { Player, BaseBuilding}

public class ControlPlayer : MonoBehaviour {
	[HideInInspector]
	public CharacterController playerController;
	public float speed = 11f;

	public Transform CameraPos;
	public Camera cam;
	public CameraMovement cameraMovement;
	public AttachTransform cameraAttachment;

	public KeyCode SwitchMode = KeyCode.B;

	public bool isWalking = false;

	public bool isDashing = false;
	public Timer dashTimer;

	public float dashCoolDown = 2;
	public float dashTime = 0;
	public float dashSpeed = 1; // Multiplier

	void Start () {
		playerController = this.GetComponent<CharacterController>();

		dashTimer = new Timer(dashTime);

	}

	public ControlModes controlMode = ControlModes.Player;

	void BaseBuildingMovement() {
		
		//Initiates base building camera settings and movement
		if (initSwitchMode) {
			cameraMovement.ZoomEnabled = false;
			cam.transform.localPosition = new Vector3(0, 25, -4.408f);
			cam.transform.localRotation = Quaternion.Euler(80, 0, 0);
			cameraMovement.XZMovementEnabled = true;
			cameraAttachment.enabled = false;
			initSwitchMode = false;
		}

		/*
		//Switch to playermode when pressed
		if (Input.GetKeyDown(SwitchMode)) {
			print("Switching to Player");
			controlMode = ControlModes.Player;
			GlobalGame.ControlMode = ControlModes.Player;
			initSwitchMode = true;
		}*/

	}

	bool initSwitchMode = false;

	void Update() {


		//Prevent updates if game is paused
		if (GlobalGame.Paused) return;

		switch (controlMode) {
			case ControlModes.BaseBuilding:
				BaseBuildingMovement();
				return;
		}


		//When control mode is player, init to reset values back to normal.
		if (initSwitchMode) {
			cameraMovement.XZMovementEnabled = false;
			cameraMovement.ZoomEnabled = true;
			cameraAttachment.enabled = true;
			cameraMovement.ZoomUpdate();
			initSwitchMode = false;
			return;
		}



		

		/*
		//Switch to basebuilding when pressed
		if (Input.GetKeyDown(SwitchMode)) {
			print("Switching to BaseBuilding");
			controlMode = ControlModes.BaseBuilding;
			GlobalGame.ControlMode = ControlModes.BaseBuilding;
			initSwitchMode = true;
		}*/
	}

	private float sideDash, frontDash;
	private bool wasDashing, isDashCooldown;
	private void FixedUpdate() {
		switch (controlMode) {
			case ControlModes.BaseBuilding:
				BaseBuildingMovement();
				return;
		}

		float multiplier = 1;
		isWalking = false;

		//Dasher
		if (Input.GetKey(KeyCode.LeftShift) && !isDashing && !wasDashing && !isDashCooldown) {
			
			isDashing = true;
			wasDashing = true;
			if (Input.GetKey(KeyCode.A) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyUp(KeyCode.A)) {
				sideDash += -dashSpeed;
			}
			if (Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyUp(KeyCode.D)) {
				sideDash += dashSpeed;
			}
			if (Input.GetKey(KeyCode.W) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyUp(KeyCode.W)) {
				frontDash += dashSpeed;
			}
			if (Input.GetKey(KeyCode.S) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyUp(KeyCode.S)) {
				frontDash += -dashSpeed;
			}

			if(sideDash == 0 && frontDash == 0) {
				isDashing = false;
				wasDashing = false;
			} else {
				dashTimer.SetTimer(dashTime);
				dashTimer.Start();
			}

		}

		if (isDashCooldown) {
			if (dashTimer.IsDone()) {
				isDashCooldown = false;
			}
		}
		if(!Input.GetKey(KeyCode.LeftShift) && wasDashing && !isDashCooldown && !isDashCooldown) {
			wasDashing = false;
		}

		

		if (isDashing) {
			SideMove(sideDash);
			ForwardMove(frontDash);

			if (dashTimer.IsDone()) {
				isDashing = false;
				sideDash = 0;
				frontDash = 0;
				isDashCooldown = true;
				dashTimer.SetTimer(dashCoolDown);
				dashTimer.Start();
			}

		} else {
			curMovement = Vector3.zero;
			sideMoveActive = false;
			if (Input.GetKey(KeyCode.A)) {
				SideMove(-multiplier);
				isWalking = true;
			}

			if (Input.GetKey(KeyCode.D)) {
				SideMove(multiplier);
				isWalking = true;
			}

			if (Input.GetKey(KeyCode.W)) {
				ForwardMove(multiplier);
				isWalking = true;
			}

			if (Input.GetKey(KeyCode.S)) {
				ForwardMove(-multiplier);
				isWalking = true;
			}
		}


		//CONTROLLER INPUT 

		//TODO Make controller inputs change control mode (From pc to console and vis versa)

		//if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.D) && lastMousePouse == Input.mousePosition) {

		//	SideMove(Input.GetAxis("HorizontalController"));
		//	ForwardMove(Input.GetAxis("VerticalController"));

		//	if (Vector3.Distance(new Vector3(0, 0, 0), curMovement) >= 0.02) {
		//		transform.rotation = Quaternion.LookRotation(curMovement);
		//	}

		//} else {
		//}

		if (RotationEnabled) UpdateAngle();
		
		transform.rotation = Quaternion.AngleAxis((-CurrentAngle) - 180, Vector3.up);


		if (!playerController.isGrounded)
			curMovement.y = -9.87f * Time.fixedDeltaTime;	
		playerController.Move(curMovement);
		curMovement = new Vector3(0,0,0);
	}




	// -------------- Rotation Calculations ---------------
	[Header("Rotation")]

	public bool RotationEnabled = true;

	public float TargetAngle = 0;
	public float RotationSpeed = 0.1f;
	public float MinAngleRotation = 0.1f;
	public float CurrentAngle = 0;

	void UpdateAngle() {

		TargetAngle = GetMouseAngle();

		//Normalize within 10 Revolutions
		TargetAngle = (TargetAngle + 3600) % 360;
		CurrentAngle = (CurrentAngle + 3600) % 360;

		//Calculate the distance from current to target
		float UpperDistance = Mathf.Abs(CurrentAngle - (TargetAngle + 360));
		float LowerDistance = Mathf.Abs(CurrentAngle - (TargetAngle - 360));
		float NormalDistance = Mathf.Abs(CurrentAngle - TargetAngle );
		
		//Find the smallest distance, than move towards it
		if(UpperDistance < NormalDistance || LowerDistance < NormalDistance) {
			
			if (UpperDistance < LowerDistance) {
				CurrentAngle += RotationSpeed * Time.fixedDeltaTime;
			} else {
				CurrentAngle -= RotationSpeed * Time.fixedDeltaTime;
			}

		} else {

			if(CurrentAngle < TargetAngle) {
				CurrentAngle += RotationSpeed * Time.fixedDeltaTime;
			} else {
				CurrentAngle -= RotationSpeed * Time.fixedDeltaTime;
			}
		}

		//Normalize angle back to 0-360
		CurrentAngle = (CurrentAngle + 3600) % 360;

		//If angle is in range, snap to it
		if (CurrentAngle > TargetAngle - MinAngleRotation && CurrentAngle < TargetAngle + MinAngleRotation) {
			CurrentAngle = TargetAngle;
		}
		
		//Also if angle is in range, snap to it (But make sures it passes the devide line [Cross between 360 to 0])
		if(UpperDistance > -MinAngleRotation+ 1 && UpperDistance < MinAngleRotation + 1) {
			CurrentAngle = TargetAngle;
		}

		if(LowerDistance > -MinAngleRotation + 1 && LowerDistance < MinAngleRotation + 1) {
			CurrentAngle = TargetAngle;
		}

		if (NormalDistance > -MinAngleRotation + 1 && NormalDistance < MinAngleRotation + 1) {
			CurrentAngle = TargetAngle;
		}




	}
	
	float GetMouseAngle() {
		return ((Mathf.Atan2(Input.mousePosition.y - GetCenterScreen().y, Input.mousePosition.x - GetCenterScreen().x) * 180 / Mathf.PI) + 180) - CameraPos.eulerAngles.y - 90;
	}

	Vector3 GetCenterScreen() {
		return new Vector3(Screen.width / 2, Screen.height / 2);
	}


	//float velocity = 9.87f;


	//----------- XZ Movement Calculations --------------
	
	Vector3 curMovement = Vector3.zero;

	float VelocityDamping = 1;

	bool sideMoveActive = false;

	void SideMove(float amount) {
		//playerController.Move(this.CameraPos.right * speed * amount * Time.deltaTime);
		curMovement += this.CameraPos.right * speed * amount * Time.fixedDeltaTime;
		sideMoveActive = true;
	}

	void ForwardMove(float amount) {
		//playerController.Move(this.CameraPos.forward * speed * amount * Time.deltaTime);

		if (sideMoveActive) {
			curMovement *= 0.75f;
			curMovement += this.CameraPos.forward * speed * amount * Time.fixedDeltaTime * 0.75f;
		} else {
			curMovement += this.CameraPos.forward * speed * amount * Time.fixedDeltaTime;
		}
	}
}
