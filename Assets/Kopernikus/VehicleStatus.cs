/*
 * (c) 2017 Kopernikus Automotive UG
 *
 */

namespace KopernikusWrapper
{
    public class VehicleStatus 
    {
        public VehicleStatus()
        {
        }

        public VehicleStatus(float t, GearDirection g, float v, float sa, float th, float b, TurnSignal ts)
        {
            timestamp = t;
            gearDirection = g;
            velocity = v;
            steeringAngle = sa;
            throttle = th;
            braking = b;
            turnSignal = ts;
        }

        public VehicleStatus(string vs)
        {
            char[] sep1 = {';'};
            char[] sep2 = {':'};
            string[] commands = vs.Split(sep1);
            foreach(string command in commands)
            {
                string[] cmdparts = command.Split(sep2);
                if (cmdparts.Length > 1)
                {
                    switch (cmdparts[0])
                    {
                        case "time":
                            timestamp = float.Parse(cmdparts[1]);
                        break;

                        case "g":
                            gearDirection = (GearDirection) int.Parse(cmdparts[1]);
                        break;

                        case "v":
                            velocity = float.Parse(cmdparts[1]);
                        break;

                        case "s":
                            steeringAngle = float.Parse(cmdparts[1]);
                        break;

                        case "t":
                            throttle = float.Parse(cmdparts[1]);
                        break;

                        case "b":
                            braking = float.Parse(cmdparts[1]);
                        break;

                    }
                }
            }
        }
        /*
        public void DebugOut()
        {
            UnityEngine.Debug.Log("Timestamp: "+timestamp);
            UnityEngine.Debug.Log("Gear: "+gearDirection);
            UnityEngine.Debug.Log("Velocity: "+velocity);
            UnityEngine.Debug.Log("Steering: "+steeringAngle);
            UnityEngine.Debug.Log("Throttle: "+throttle);
            UnityEngine.Debug.Log("Braking: "+braking);
        }
        */

        float timestamp = -1;
        public float Timestamp
        {
            get 
            {
                return timestamp;
            }
        }



        GearDirection gearDirection = GearDirection.GEAR_DIRECTION_UNKNOWN;
        public GearDirection GearDirection
        {
            get
            {
                return gearDirection;
            }
        }

        float velocity = 0;
        public float Velocity
        {
            get
            {
                return velocity;
            }
        }

        float steeringAngle = 0;
        public float SteeringAngle
        {
            get
            {
                return steeringAngle;
            }
        }

        float throttle = 0;
        public float Throttle
        {
            get
            {
                return throttle;
            }
        }

        float braking = 0;
        public float Brake
        {
            get
            {
                return braking;
            }
        }

        TurnSignal turnSignal = TurnSignal.TURN_SIGNAL_OFF;
        public TurnSignal TurnSignal
        {
            get
            {
                return turnSignal;
            }
        }
		
	}
}
