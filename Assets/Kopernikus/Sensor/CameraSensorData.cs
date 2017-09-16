/*
 * (c) 2017 Kopernikus Automotive UG
 *
 */

namespace KopernikusWrapper
{
    public class CameraSensorData : SensorData
    {
        public enum CameraDataFormat
        {
            jpeg
        }

        CameraDataFormat format;
        int width;
        int height;
        byte[] image;
        public byte[] ImageData
        {
            get
            {
                return image;
            }
        }

        public CameraSensorData()
        {
        }

        public CameraSensorData(int w, int h, CameraDataFormat f, byte[] data)
        {
            width = w;
            height = h;
            format = f;
            image = data;
        }
    }
}