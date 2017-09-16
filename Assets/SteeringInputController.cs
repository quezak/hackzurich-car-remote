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
			Quaternion gyroState = Input.gyro.attitude;
			// Phone horizontal = 90deg reading, right is less
			// Car input with phone horizontal = 0.0, left is less
			float linearAngle = -(gyroState.eulerAngles.z - 90.0f);
			if (Math.Abs(linearAngle) < STEERING_DEADZONE_DEG) {
				linearAngle = 0.0f;
			} else {
				linearAngle -= Math.Sign(desiredAngle) * STEERING_DEADZONE_DEG;
			}
			// Apply a non-linear scale, so light turns won't jerk the car
			desiredAngle = linearAngle * linearAngle / 90.0f * Math.Sign(linearAngle);
			// For big changes, smoothen the steering, don't jump to desired position immediately
			float maxChange = MAX_ANGLE_CHANGE_SEC * Time.deltaTime;
			Utils.LogEveryN(1, String.Format("change: {0:F2}, maxChange: {1:F2}", desiredAngle - angle, maxChange));
			if (Math.Abs(angle - desiredAngle) < maxChange) {
				angle = desiredAngle;
			} else {
				angle += Math.Sign(desiredAngle - angle) * maxChange;
			}
			Utils.LogEveryN(1, String.Format("angle: {0:F2} desired: {1:F2}", angle, desiredAngle));

		}

		public float getSteeringAngle() {
			return angle;
		}
	}
}

