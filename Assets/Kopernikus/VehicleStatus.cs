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
            
        }

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
