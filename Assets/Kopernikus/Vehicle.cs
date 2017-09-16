/*
 * (c) 2017 Kopernikus Automotive UG
 *
 */

using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace KopernikusWrapper
{
    public class Vehicle
    {
        public enum ConnectionState
        {
            NotConnected,
            AttemptingConnect,
            Unavailable,
            Connected
        }

        enum RequestCommand
        {
            Undefined,
            VehicleType = 1,
            VehicleStatus
        }

        ConnectionState connectState = ConnectionState.NotConnected;
        int port = 10001;
        string address;
        Socket clientSocket;
        byte[] writeBuffer = new byte[1024];

        int receiveDataOffset = 0;
        int receiveDataLength;
        byte[] readHeader = new byte[4];
        byte[] readBuffer = new byte[1024];

        RequestCommand request = RequestCommand.Undefined;

        Sensor frontCameraSensor;

        VehicleType vehicleType;
        public VehicleType VehicleType
        {
            get 
            {
                return vehicleType;
            }
        }

        VehicleStatus vehicleStatus;
        public VehicleStatus Status
        {
            get
            {
                return vehicleStatus;
            }
        }

        public Vehicle(string _address)
        {
            address = _address;
            vehicleStatus = new VehicleStatus();
            StartConnect();
        }
	

        public bool Available
        {
            get 
            {
                return connectState != ConnectionState.Unavailable;
            }
        }
           
        public bool Connected
        {
            get 
            {
                return connectState == ConnectionState.Connected;
            }
        }

        bool gearDirectionRequested = false;
        GearDirection gearDirectionRequest;
        public void SetGear(GearDirection g)
        {
            gearDirectionRequest = g;
            gearDirectionRequested = true;
        }

        bool steeringAngleRequested = false;
        float steeringAngleRequest;
        public void SetSteeringAngle(float s)
        {
            steeringAngleRequest = s;
            steeringAngleRequested = true;
        }

        bool throttleRequested = false;
        float throttleRequest;
        public void SetThrottle(float t)
        {
            throttleRequest = t;
            throttleRequested = true;
        }

        bool brakeRequested = false;
        float brakeRequest;
        public void SetBrake(float b)
        {
            brakeRequest = b;
            brakeRequested = true;
        }

        bool turnSinalRequested = false;
        TurnSignal turnSignalRequest;
        public void SetTurnSignal(TurnSignal t)
        {
            turnSignalRequest = t;
            turnSinalRequested = true;
        }

        public Sensor Sensor(SensorType type)
        {
            if (type == SensorType.SENSOR_TYPE_CAMERA_FRONT)
            {
                return frontCameraSensor;
            }
            return null;
        }
       
        public void Update()
        {
            if (request != RequestCommand.Undefined)
            {
                SendRequest(request);
                request = RequestCommand.Undefined;
            }

            if (frontCameraSensor != null)
            {
                ((CameraSensor)frontCameraSensor).Update();
            }
        }

        void StartConnect()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectState = ConnectionState.AttemptingConnect;

            try
            {
                System.IAsyncResult result = clientSocket.BeginConnect(address, port, EndConnect, null);
                bool connectSuccess = result.AsyncWaitHandle.WaitOne(System.TimeSpan.FromSeconds(10));
                if (!connectSuccess)
                {
                    clientSocket.Close();
                    connectState = ConnectionState.Unavailable;
                    UnityEngine.Debug.LogError(string.Format("Client unable to connect. Failed"));
                }
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError(string.Format("Client exception on beginconnect: {0}", ex.Message));
                connectState = ConnectionState.Unavailable;
            }

        }
        void EndConnect(System.IAsyncResult iar)
        {
            clientSocket.EndConnect(iar);
            clientSocket.NoDelay = true;

            SendRequest(RequestCommand.VehicleType);
        }

        void SendRequest(RequestCommand command)
        {
            writeBuffer[0] = (byte)command;
            int requestLength = 1;
            string requestCommands = "";
            if (gearDirectionRequested)
            {
                requestCommands += "g:"+(int)gearDirectionRequest+";";
                gearDirectionRequested = false;
            }
            if (steeringAngleRequested)
            {
                requestCommands += "s:"+steeringAngleRequest+";";
                steeringAngleRequested = false;
            }
            if (throttleRequested)
            {
                requestCommands += "t:"+throttleRequest+";";
                throttleRequested = false;
            }
            if (brakeRequested)
            {
                requestCommands += "b:"+brakeRequest+";";
                brakeRequested = false;
            }
            if (turnSinalRequested)
            {
                requestCommands += "ts:"+(int)turnSignalRequest+";";
                turnSinalRequested = false;
            }
            if (requestCommands.Length > 0)
            {
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                byte[] b = enc.GetBytes(requestCommands);

                System.Buffer.BlockCopy(b, 0, writeBuffer, 1, b.Length);

                requestLength += b.Length;
            }
                
            clientSocket.BeginSend(writeBuffer, 0, requestLength, SocketFlags.None, EndSend, writeBuffer);
            BeginReceiveData();
        }
        void EndSend(System.IAsyncResult iar)
        {
            clientSocket.EndSend(iar);
            byte[] msg = (iar.AsyncState as byte[]);
            System.Array.Clear(msg, 0, msg.Length);
            msg = null;
        }


        void BeginReceiveData()
        {
            receiveDataOffset = 0;
            clientSocket.BeginReceive(readHeader, 0, readHeader.Length, SocketFlags.None, EndReceiveData, null);
        }
        void EndReceiveData(System.IAsyncResult iar)
        {
            int numBytesReceived = receiveDataOffset + clientSocket.EndReceive(iar);
            if (numBytesReceived < readHeader.Length)
            {
                receiveDataOffset = numBytesReceived;
                clientSocket.BeginReceive(readHeader, receiveDataOffset, readHeader.Length - receiveDataOffset, SocketFlags.None, EndReceiveData, null);
            }
            else
            {
                receiveDataLength = 
                    readHeader[0] +
                    (readHeader[1] << 8) +
                    (readHeader[2] << 16) +
                    (readHeader[3] << 24);
                if (receiveDataLength > readBuffer.Length)
                {
                    readBuffer = new byte[receiveDataLength];
                }
                BeginReceiveDataBody();
            }
        }

        void BeginReceiveDataBody()
        {
            receiveDataOffset = 0;
            clientSocket.BeginReceive(readBuffer, 0, readBuffer.Length, SocketFlags.None, EndReceiveDataBody, null);
        }
        void EndReceiveDataBody(System.IAsyncResult iar)
        {
            int numBytesReceived = receiveDataOffset + clientSocket.EndReceive(iar);
            if (numBytesReceived < receiveDataLength)
            {
                receiveDataOffset = numBytesReceived;
                clientSocket.BeginReceive(readBuffer, receiveDataOffset, readBuffer.Length - receiveDataOffset, SocketFlags.None, EndReceiveDataBody, null);
            }
            else
            {
                ProcessData(numBytesReceived);
            }
        }

        void ProcessData(int numBytesRecv)
        {
            if (connectState == ConnectionState.AttemptingConnect)
            {
                connectState = ConnectionState.Connected;
                UnityEngine.Debug.Log("Client connected");

                frontCameraSensor = new CameraSensor(address);
            }

            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            string s = enc.GetString(readBuffer,0,numBytesRecv);

            //UnityEngine.Debug.Log(s);

            request = RequestCommand.VehicleStatus;
        }
    }
}
