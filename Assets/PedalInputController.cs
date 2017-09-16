using UnityEngine;
using System;

namespace AssemblyCSharp {
	public class PedalInputController {
		public PedalInputController () {
			Debug.Log(String.Format("screen size {0}x{1}", Screen.width, Screen.height));
			sliderHeight = Screen.height;
			sliderWidth = (float)Screen.width * SLIDER_WIDTH_RATIO;
			midZoneBottom = sliderHeight * (1.0f - SLIDER_MID_ZONE_RATIO) / 2.0f;
			midZoneTop = sliderHeight * (1.0f + SLIDER_MID_ZONE_RATIO) / 2.0f;
		}

		const float SLIDER_WIDTH_RATIO = 0.16f;
		// The slider is divided into 3 zones: middle = brake, up = forward, bottom = backward
		// Level adjusted proportionally to touch position
		const float SLIDER_MID_ZONE_RATIO = 0.2f;
		float throttleLevel = 0.0f;
		float brakeLevel = 0.0f;
		float sliderWidth, sliderHeight, midZoneBottom, midZoneTop;

		public void Update() {
			if (Input.touchCount == 0 || SpriteToggle.INSTANCE == null) {
				throttleLevel = 0.0f;
				brakeLevel = 0.0f;
				return;
			}
			for (int i = 0; i < Input.touchCount; ++i) {
				Touch touch = Input.GetTouch(i);
				if (touch.position.x > Screen.width - sliderWidth) {
					float pos = (float)touch.position.y;
					if (pos < midZoneBottom) {
						// backward, midZoneBot = zero throttle, 0 = full throttle
						SpriteToggle.INSTANCE.setBackward();
						throttleLevel = (midZoneBottom - pos) / midZoneBottom;
						brakeLevel = 0.0f;
					} else if (pos > midZoneTop) {
						// forward, midZoneTop = zero throttle, sliderHeight = full throttle
						SpriteToggle.INSTANCE.setForward();
						throttleLevel = (pos - midZoneTop) / midZoneBottom;
						brakeLevel = 0.0f;
					} else {
						// brake, middle = full force, edge = zero force
						throttleLevel = 0.0f;
						brakeLevel = 1.0f - Math.Abs(sliderHeight - pos - pos) / (midZoneTop - midZoneBottom);
					}
					Debug.Log(String.Format("throttle {0:F2} brake {1:F2}", throttleLevel, brakeLevel));
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

