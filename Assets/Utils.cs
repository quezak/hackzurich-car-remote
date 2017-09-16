using System;
using UnityEngine;


namespace AssemblyCSharp {
	public static class Utils {
		

		public static void LogEveryN(int frames, String message) {
			if (Time.frameCount % frames == 0) {
				Debug.Log(message);
			}
		}
	}
}

