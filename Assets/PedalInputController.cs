using UnityEngine;
using System;

namespace AssemblyCSharp {
	public class PedalInputController {
		public PedalInputController () {
			Debug.Log(String.Format("screen size {0}x{1}", Screen.width, Screen.height));
			sliderHeight = Screen.height;
			sliderWidth = (float)Screen.width * SLIDER_WIDTH_RATIO;
		}

		const float SLIDER_WIDTH_RATIO = 0.16f;
		float throttleLevel = 0.0f;
		float brakeLevel = 0.0f;
		float sliderWidth, sliderHeight;

		public void Update() {
			if (Input.touchCount == 0) {
				throttleLevel = 0.0f;
				brakeLevel = 0.0f;
			}
			for (int i = 0; i < Input.touchCount; ++i) {
				Touch touch = Input.GetTouch(i);
				if (touch.position.x < sliderWidth) {
					brakeLevel = (float)touch.position.y / sliderHeight;
					Debug.Log(String.Format("left slider {0} level {1:F2}", touch.position.y, brakeLevel));
				} else if (touch.position.x > Screen.width - sliderWidth) {
					throttleLevel = (float)touch.position.y / sliderHeight;
					Debug.Log(String.Format("right slider {0} level {1:F2}", touch.position.y, throttleLevel));
				}
			}
		}

		public float GetThrottleLevel() {
			return throttleLevel;
		}

		public float GetBrakeLevel() {
			return brakeLevel;
		}
	}
}

