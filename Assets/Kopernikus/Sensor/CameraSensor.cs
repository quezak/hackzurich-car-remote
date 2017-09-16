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
    public class CameraSensor : Sensor
    {
        public enum ConnectionState
        {
            NotConnected,
            AttemptingConnect,
            Unavailable,
            Connected
        }
            
        ConnectionState connectState = ConnectionState.NotConnected;
        int port = 10011;
        string address;
        Socket clientSocket;
        byte[] writeBuffer = new byte[1024];

        int receiveDataOffset = 0;
        int receiveDataLength;
        byte[] readHeader = new byte[4];
        byte[] readBuffer = new byte[1024];

        bool requestImage;



        public CameraSensor(string _address)
        {
            address = _address;
            data = new CameraSensorData();
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

        public void Update()
        {
            if (connectState == ConnectionState.Connected)
            {
                if (requestImage)
                {
                    requestImage = false;
                    SendRequest(1);
                }
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
            connectState = ConnectionState.Connected;
            requestImage = true;
        }

        void SendRequest(int command)
        {
            writeBuffer[0] = (byte)command;
            int requestLength = 1;

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
            /*
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            string s = enc.GetString(readBuffer,0,numBytesRecv);
            */
            //UnityEngine.Debug.Log(s);

            UnityEngine.Debug.Log("Received: "+numBytesRecv);

            data = new CameraSensorData(640, 360, CameraSensorData.CameraDataFormat.jpeg, readBuffer);
            requestImage = true;

        }
    }
}