/*
 * (c) 2017 Kopernikus Automotive UG
 *
 */


namespace KopernikusWrapper
{
    public enum VehicleType
    {
        VEHICLE_TYPE_UNKNOWN = 0,
        VEHICLE_TYPE_SIMULATOR,
        VEHICLE_TYPE_JETSON_CAR
    }

    public enum GearDirection
    {
        GEAR_DIRECTION_UNKNOWN = 0,
        GEAR_DIRECTION_FORWARD,
        GEAR_DIRECTION_BACKWARD,
        GEAR_DIRECTION_NEUTRAL,
    }

    public enum TurnSignal
    {
        TURN_SIGNAL_OFF = 0,
        TURN_SIGNAL_LEFT,
        TURN_SIGNAL_RIGHT
    }

    public enum SensorType
    {
        SENSOR_TYPE_UNKNOWN = 0,
        SENSOR_TYPE_CAMERA_FRONT,
        SENSOR_TYPE_GPS,
        SENSOR_TYPE_IMU
    }

    public enum RecorderType
    {
        RECORDER_TYPE_UNKNOWN = 0,
        RECORDER_TYPE_STATUS,
        RECORDER_TYPE_SENSOR
    }

    public enum DownloaderStatus
    {
        DOWNLOADER_STATUS_SCHEDULED = 0,
        DOWNLOADER_STATUS_RUNNING,
        DOWNLOADER_STATUS_FINISHED,
        DOWNLOADER_STATUS_ERROR
    }
        
    public class Kopernikus
    {
        private static Kopernikus instance;
        public static Kopernikus Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new Kopernikus();
                }
                return instance;
            }
        }

        public Vehicle Vehicle(string address = "localhost")
        {
            return new Vehicle(address);
        }

        public Recorder RegisterRecorder(RecorderType type)
        {
            return new Recorder();
        }

        public Recorder LoadRecorder(string filename)
        {
            return new Recorder();
        }

        public Downloader Downloader(string url)
        {
            return new Downloader();
        }
    }
}