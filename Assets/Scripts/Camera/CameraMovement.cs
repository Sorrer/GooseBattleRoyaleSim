using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	[Space(10)]
	[Header("Controls Config")]
	public bool XZMovementEnabled = true;

	[Space(10)]
	[Header("Speed Config")]
	public float speed = 25;
	public float speedMultiplier = 1.75f; //When key is held boost camera at this factor.

	[Space(10)]
	[Header("Edge Scroll Config")]
	public float edgeScrollSpeed = 100;
	public float detectRangeFromEdge = 100; //Pixels from edge to consider in
	public float startEdgeSpeed = 5; //Speed to start when entering range;

	private float aSpeed = 1f;

	[Space(10)]
	[Header("Bounds")]
	public bool BoundsEnabled = false;
	public Rect bounds = new Rect();


	[Space(10)]
	[Header("Zoom Config (y = ab^(zx+c)+d)")]
	public bool ZoomEnabled = true;
	public float z_Z = -1.1f;
	public float z_A = 10f;
	public float z_B = 0.9f;
	public float z_C = 8.7f;
	public float z_D = -1.4f;

	public float z_Scale = 0.5f;

	[Space(5)]
	public float z_MinX = 0;
	public float z_MaxX = 5;
	public float z_XOffset = 0;
	public float z_YOffset = 0;
	[Space(5)]
	[Range(0f, 5f)] public float ZoomIncreaseSpeeed = .8f;
	[Range(0, 1)] public float ZoomPercentage = .8f;
	[Range(0, 1)] public float ZoomPercentageCurrent = .8f;
	public float ZoomSmoothSpeed = 0.1f; //Amount of increase to add every second to reach goal;

	[Space(10)]

	public float ZoomCurrentX = 0;
	public float ZoomCurrentY = 0;
	public float ZoomCurrentAngleX = 0;

	//public AnimationCurve ZoomCurve;


	//Vector3 DefaultPosition = Vector3.zero;

	
	//Implement mouse move on edge
	//Implement middle mouse move temp move
	
    // Use this for initialization
    void Start () {
		//float zoomDefault = Camera.main.transform.position.y;
		//DefaultPosition = this.transform.position;
		ZoomPercentageCurrent = ZoomPercentage;
	}
	//Add tilt

	void FixedUpdate() {

		//Prevent camera movement when paused
		if (GlobalGame.Paused) return;

		edgeScroll();

		float dSpeed = speed;

		if (Input.GetKey(KeyCode.LeftShift)) {
			dSpeed *= speedMultiplier;
		}

		if (XZMovementEnabled) {
			//Main Moving
			if (Input.GetKey(KeyCode.A)) { this.moveCameraLeft(dSpeed); }
			if (Input.GetKey(KeyCode.D)) { this.moveCameraRight(dSpeed); }
			if (Input.GetKey(KeyCode.S)) { this.moveCameraDown(dSpeed); }
			if (Input.GetKey(KeyCode.W)) { this.moveCameraUp(dSpeed); }
		}


		if (Input.GetKey(KeyCode.Q)) {
			transform.parent.transform.Rotate(0f, -200f * Time.fixedDeltaTime, 0f, Space.World);
			//Camera.main.transform.position = Camera.main.transform.position + transform.parent.transform.right * speed * aSpeed * Time.fixedDeltaTime;
		}
		if (Input.GetKey(KeyCode.E)) {
			transform.parent.Rotate(0f, 200f * Time.fixedDeltaTime, 0f, Space.World);
			//Camera.main.transform.position = Camera.main.transform.position + -transform.parent.transform.right * speed * aSpeed * Time.fixedDeltaTime;
		}

		/*
		if (Input.GetAxis("HorizontalController2") >= deadZoneHorizontal2 || Input.GetAxis("HorizontalController2") <= -deadZoneHorizontal2) {

			if (Input.GetAxis("VerticalController2") <= deadZoneVertical2 * deadZoneActiveMultiplier || Input.GetAxis("VerticalController2") >= -deadZoneVertical2 * deadZoneActiveMultiplier) {
				if (Input.GetAxis("VerticalController") > 0) {
					transform.parent.Rotate(0f, -100f * Time.fixedDeltaTime * Input.GetAxis("HorizontalController2"), 0f, Space.World);
				} else {
					transform.parent.Rotate(0f, -100f * Time.fixedDeltaTime * Input.GetAxis("HorizontalController2"), 0f, Space.World);
				}

			}


		}
		*/

		//Zoom update and input
		if (ZoomEnabled) {
			//	if (Input.GetAxis("Mouse ScrollWheel") != 0) {
			//		Zoom(1 * Input.GetAxis("Mouse ScrollWheel"));
			//	}
			/*
			if (Input.GetAxis("VerticalController2") >= deadZoneVertical2 || Input.GetAxis("VerticalController2") <= -deadZoneVertical2) {
				if (Input.GetAxis("HorizontalController2") <= deadZoneHorizontal2 * deadZoneActiveMultiplier || Input.GetAxis("HorizontalController2") >= -deadZoneHorizontal2 * deadZoneActiveMultiplier) {
					Zoom(0.1f * Input.GetAxis("VerticalController2"));
				}
			}*/

			ZoomUpdate();
		}


	}

	private void Update() {
		if (ZoomEnabled) {
			if (Input.GetAxis("Mouse ScrollWheel") != 0) {
				Zoom(-1 * Input.GetAxis("Mouse ScrollWheel"));
			}
		}
	}

	// ----------- Zooming -------------------

	float deadZoneVertical2 = 0.15f;
	float deadZoneHorizontal2 = 0.15f;

	float deadZoneActiveMultiplier = 1f;

	public void ZoomUpdate() {

		if (this.ZoomPercentageCurrent < this.ZoomPercentage) {
			this.ZoomPercentageCurrent += this.ZoomSmoothSpeed * Time.fixedDeltaTime;

			if (this.ZoomPercentageCurrent > this.ZoomPercentage) {
				this.ZoomPercentageCurrent = this.ZoomPercentage;
			}

		}

		if (this.ZoomPercentageCurrent > this.ZoomPercentage) {
			this.ZoomPercentageCurrent -= this.ZoomSmoothSpeed * Time.fixedDeltaTime;

			if (this.ZoomPercentageCurrent < this.ZoomPercentage) {
				this.ZoomPercentageCurrent = this.ZoomPercentage;
			}
		}


		CalculateZoom(this.ZoomPercentageCurrent);
		setZoom();
	}

	void Zoom(float i) {
		this.ZoomPercentage += i * this.ZoomIncreaseSpeeed;
		this.ZoomPercentage = Mathf.Clamp(ZoomPercentage, 0, 1);
	}


	//ab^(zx+c)+d

	void CalculateZoom() {
		CalculateZoom(this.ZoomPercentage);
	}

	void CalculateZoom(float Percentage) {
		float vX = 0;
		float vY = 0;

		//vX Percentage Calculate

		vX = (this.z_MaxX - this.z_MinX) * Percentage;
		vX += this.z_MinX;

		//Formula expontential

		//Step #1 v1 = (b^(zx+c))

		float v1 = Mathf.Pow(z_B, ((z_Z * vX) + z_C));

		//Step #2 v2 = (a * v1)

		float v2 = z_A * v1;

		//Step #3 vY = (v2 + d)

		vY = (v2 + z_D);

		this.ZoomCurrentX = vX * z_Scale;
		this.ZoomCurrentY = vY * z_Scale;

		CalculateZoomAngle();
	}

	void CalculateZoomAngle() {
		this.ZoomCurrentAngleX = Mathf.Atan2(this.ZoomCurrentY + this.z_YOffset, this.ZoomCurrentX + this.z_XOffset) * 180 / Mathf.PI;
	}

	void setZoom() {

		Vector3 zPosition = new Vector3(0, this.ZoomCurrentY + this.z_YOffset, -this.ZoomCurrentX - this.z_XOffset);
		Quaternion zAngle = Quaternion.Euler(this.ZoomCurrentAngleX, 0, 0);

		Camera.main.transform.localPosition = zPosition;
		Camera.main.transform.localRotation = zAngle;


	}

	//------------------- XZ Movement
	
	void moveCameraUp(float eSpeed) {
		Camera.main.transform.parent.position = Camera.main.transform.parent.position + transform.parent.forward * eSpeed * aSpeed * Time.fixedDeltaTime;

		Rect camRect = new Rect(new Vector2(Camera.main.transform.parent.transform.position.x, Camera.main.transform.parent.transform.position.z), new Vector2(1, 1));

		if (!camRect.Overlaps(bounds)) {
			moveCameraDown(eSpeed);
		}

	}
	void moveCameraDown (float eSpeed) {
		Camera.main.transform.parent.position = Camera.main.transform.parent.position + -transform.parent.forward * eSpeed * aSpeed * Time.fixedDeltaTime;

		Rect camRect = new Rect(new Vector2(Camera.main.transform.parent.transform.position.x, Camera.main.transform.parent.transform.position.z), new Vector2(1, 1));

		if (!camRect.Overlaps(bounds)) {
			moveCameraUp(eSpeed);
		}
	}
	void moveCameraLeft (float eSpeed) {
		Camera.main.transform.parent.position = Camera.main.transform.parent.position + -transform.parent.right * eSpeed * aSpeed * Time.fixedDeltaTime;

		Rect camRect = new Rect(new Vector2(Camera.main.transform.parent.transform.position.x, Camera.main.transform.parent.transform.position.z), new Vector2(1, 1));

		if (!camRect.Overlaps(bounds)) {
			moveCameraRight(eSpeed);
		}
	}
	void moveCameraRight (float eSpeed) {
		Camera.main.transform.parent.position = Camera.main.transform.parent.position + transform.parent.right * eSpeed * aSpeed * Time.fixedDeltaTime;

		if (isOutOfBounds()) {
			moveCameraLeft(eSpeed);
		}
	}

	public bool isOutOfBounds() {
		Rect camRect = new Rect(new Vector2(Camera.main.transform.parent.transform.position.x, Camera.main.transform.parent.transform.position.z), new Vector2(1, 1));

		return !camRect.Overlaps(bounds);
	}


	// --------- Movement Via Screen Edge ---------------

	void edgeScroll() {
		if (!GlobalGame.EnableEdgeScroll) return;

		Vector2 mousePos = Input.mousePosition;

		if (mousePos.x < 0 + 10 + detectRangeFromEdge) {


			if (mousePos.x < 0 + 10) {
				this.moveCameraLeft(edgeScrollSpeed);
			} else {

				float speedPercent = edgeScrollSpeed * ((detectRangeFromEdge - mousePos.x) / detectRangeFromEdge);


				if (speedPercent < this.startEdgeSpeed) {
					this.moveCameraLeft(this.startEdgeSpeed);
				} else {
					if (speedPercent > edgeScrollSpeed) {
						this.moveCameraLeft(this.edgeScrollSpeed);
					} else {
						this.moveCameraLeft(speedPercent);
					}
				}

			}

		} else if (mousePos.x > Screen.width - detectRangeFromEdge) {

			if (mousePos.x > Screen.width) {
				this.moveCameraRight(this.edgeScrollSpeed);
			} else {

				float speedPercent = edgeScrollSpeed * ((detectRangeFromEdge - (Screen.width - mousePos.x)) / detectRangeFromEdge);

				if (speedPercent < this.startEdgeSpeed) {
					this.moveCameraRight(this.startEdgeSpeed);
				} else {

					if(speedPercent > edgeScrollSpeed) {
						this.moveCameraRight(this.edgeScrollSpeed);
					} else {
						this.moveCameraRight(speedPercent);
					}

				}
			}
		}

		if (mousePos.y < 0 + detectRangeFromEdge) {

			if (mousePos.y < 0) {
				this.moveCameraDown(this.edgeScrollSpeed);
			} else {
				float speedPercent = edgeScrollSpeed * ((detectRangeFromEdge - mousePos.y) / detectRangeFromEdge);

				if (speedPercent < this.startEdgeSpeed) {
					this.moveCameraDown(this.startEdgeSpeed);
				} else {
					if (speedPercent > edgeScrollSpeed) {
						this.moveCameraDown(this.edgeScrollSpeed);
					} else {
						this.moveCameraDown(speedPercent);
					}
				}
			}

		} else if (mousePos.y > (Screen.height - 10) - detectRangeFromEdge) {

			if (mousePos.y > (Screen.height - 10)) {
				this.moveCameraUp(this.edgeScrollSpeed);
			} else {
				float speedPercent = edgeScrollSpeed * ((detectRangeFromEdge - ((Screen.height - 10) - mousePos.y)) / detectRangeFromEdge);

				if (speedPercent < this.startEdgeSpeed) {
					this.moveCameraUp(this.startEdgeSpeed);
				} else {
					if (speedPercent > edgeScrollSpeed) {
						this.moveCameraUp(this.edgeScrollSpeed);
					} else {
						this.moveCameraUp(speedPercent);
					}
				}

			}
		}
	}

	
	
}
