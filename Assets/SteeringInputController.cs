using System;
using UnityEngine;


namespace AssemblyCSharp {
	public class SteeringInputController {
		public SteeringInputController() {
			Input.gyro.enabled = true;
			Input.gyro.updateInterval = 0.0167f;
			Debug.Log("Gyroscope initialized");
		}

		float angle = 0.0f;
		float STEERING_DEADZONE_DEG = 5.0f;

		public void Update() {
			Quaternion gyroState = Input.gyro.attitude;
			// Phone horizontal = 90deg reading, right is less
			// Car input with phone horizontal = 0.0, left is less
			angle = -(gyroState.eulerAngles.z - 90.0f);
			if (Math.Abs(angle) < STEERING_DEADZONE_DEG) {
				angle = 0.0f;
			} else {
				angle -= Math.Sign(angle) * STEERING_DEADZONE_DEG;
			}
			Utils.LogEveryN(20, String.Format("angle: {0}", angle));

		}

		public float getSteeringAngle() {
			return angle;
		}
	}
}

