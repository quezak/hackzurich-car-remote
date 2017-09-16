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

    // Use this for initialization
    void Start () 
    {
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
        
    void OnGUI()
    {
        if (!isConnected)
        {
            if (GUI.Button(new Rect(Screen.width * 0.5f - 200, Screen.height * 0.5f - 40, 400, 80), "Connect"))
            {
                vehicle = Kopernikus.Instance.Vehicle("172.30.5.184");
            }
        }
    }
}
