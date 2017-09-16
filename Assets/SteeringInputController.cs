using System;
using UnityEngine;


namespace AssemblyCSharp {
	public class SteeringInputController {
		public SteeringInputController() {
			Input.gyro.enabled = true;
			Input.gyro.updateInterval = 0.0167f;
			Debug.Log("Gyroscope initialized");
		}

		float desiredAngle = 0.0f;
		float angle = 0.0f;
		float STEERING_DEADZONE_DEG = 5.0f;
		float MAX_ANGLE_CHANGE_SEC = 60.0f;

		public void Update() {
			Quaternion gyroState = new Quaternion(0.5f, 0.5f, -0.5f, 0.5f)
				* Input.gyro.attitude
				* new Quaternion(0, 0, 1, 0);
			Quaternion referenceRotation = Quaternion.identity;
			Quaternion eliminationOfXY = Quaternion.Inverse(
				Quaternion.FromToRotation(referenceRotation * Vector3.forward, 
					gyroState * Vector3.forward)
			);
			Quaternion rotationZ = eliminationOfXY * gyroState;
			float linearAngle = rotationZ.eulerAngles.z;

			//Quaternion gyroState = Input.gyro.attitude;
			// Phone horizontal = 270deg reading, right is less
			// Car input with phone horizontal = 0.0, left is less
			//float linearAngle = -(gyroState.eulerAngles.z - 270.0f);
			if (Math.Abs(linearAngle) < STEERING_DEADZONE_DEG) {
				linearAngle = 0.0f;
			} else {
				linearAngle -= Math.Sign(linearAngle) * STEERING_DEADZONE_DEG;
			}
			// Apply a non-linear scale, so light turns won't jerk the car
			desiredAngle = linearAngle * linearAngle / 120.0f * Math.Sign(linearAngle);
			// For big changes, smoothen the steering, don't jump to desired position immediately
			float maxChange = MAX_ANGLE_CHANGE_SEC * Time.deltaTime;
			Utils.LogEveryN(10, String.Format("change: {0:F2}, maxChange: {1:F2}", desiredAngle - angle, maxChange));
			if (Math.Abs(angle - desiredAngle) < maxChange) {
				angle = desiredAngle;
			} else {
				angle += Math.Sign(desiredAngle - angle) * maxChange;
			}
			Utils.LogEveryN(10, String.Format("angle: {0:F2} desired: {1:F2} raw: {2:F2} linear: {3:F2}",
				angle, desiredAngle, gyroState.eulerAngles.z, linearAngle));

		}

		public float getSteeringAngle() {
			return angle;
		}
	}
}

