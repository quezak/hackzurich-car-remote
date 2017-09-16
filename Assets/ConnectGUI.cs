using UnityEngine;
using System.Collections;
using KopernikusWrapper;
using AssemblyCSharp;

public class ConnectGUI : MonoBehaviour {
    public enum ConnectionState
    {
        NotConnected,
        AttemptingConnect,
        Connected
    }

    public Renderer cameraDisplay;
    Texture2D imageTexture;

    Vehicle vehicle;
    bool isConnected;
	PedalInputController pedalInputController;
	SteeringInputController steeringInputController;
    public string serverIp = "172.30.5.184";

    // Use this for initialization
    void Start () {
        imageTexture = new Texture2D(255, 255);
		pedalInputController = new PedalInputController ();
		steeringInputController = new SteeringInputController();
		Debug.Log("CREATED");
    }

    // Update is called once per frame
    void Update () {
        if (vehicle != null)
        {
            if (!isConnected && vehicle.Connected)
            {
                Debug.Log("VehicleAvailable");
                isConnected = true;
            }

            if (vehicle.Connected)
			{
				pedalInputController.Update();
				steeringInputController.Update();
				vehicle.SetThrottle(pedalInputController.GetThrottleLevel());
				vehicle.SetBrake(pedalInputController.GetBrakeLevel());
				vehicle.SetSteeringAngle(steeringInputController.getSteeringAngle());
                vehicle.Update();

                CameraSensor s = (CameraSensor) vehicle.Sensor(SensorType.SENSOR_TYPE_CAMERA_FRONT);
                if (s != null)
                {
                    CameraSensorData d = (CameraSensorData) s.Data;
                    if (d != null)
                    {
                        imageTexture.LoadImage(d.ImageData);
                        cameraDisplay.material.mainTexture = imageTexture;
                    }
                }
            }
        }
    }

    Rect scaledCenteredRect(float ratioWidth, float ratioHeight, float ratioDx = 0f, float ratioDy = 0f) {
      float width = Screen.width * ratioWidth;
      float height = Screen.height * ratioHeight;
      float dx = Screen.width * ratioDx;
      float dy = Screen.height * ratioDy;
      return new Rect((Screen.width - width) * 0.5f + dx, (Screen.height - height) * 0.5f + dy, width, height);
    }

    GUIStyle scaledSizeStyle(GUIStyle style) {
      GUIStyle result = new GUIStyle(style);
      result.fontSize = (int) (0.05f * Screen.height);
      return result;
    }

    void drawServerIpTextField() {
      serverIp = GUI.TextField(scaledCenteredRect(0.8f, 0.1f, 0f, -0.3f), serverIp, 25, scaledSizeStyle(GUI.skin.textField));

    }

    void OnGUI() {
        if (!isConnected) {
            drawServerIpTextField();
            if (GUI.Button(scaledCenteredRect(0.8f, 0.2f), "Connect", scaledSizeStyle(GUI.skin.button))) {
                vehicle = Kopernikus.Instance.Vehicle(serverIp);
            }
        }
    }
}
