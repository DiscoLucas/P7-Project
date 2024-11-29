
using UnityEngine;
//using libTeensySharp;
using TeensySharp;
using System.Linq;
using lunOptics.libUsbTree;
using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
public class TeensyLogger : MonoBehaviour
{
    Thread readThread;
    string latestData = "";
    SerialPort port;
    public bool readPort = false;
    CancellationTokenSource cancellationTokenSource;

    string portName = "COM3"; // Adjust to the correct port
    int baudRate = 9600;

    public int timeOut = 100;

    void Start()
    {
        string[] availablePorts = SerialPort.GetPortNames();
        Debug.Log("Available ports: " + string.Join(", ", availablePorts));

        if (Array.Exists(availablePorts, p => p == portName))
        {
            port = new SerialPort(portName, baudRate);
            port.ReadTimeout = timeOut;

            try
            {
                Debug.Log("Opening serial port " + portName + " at " + baudRate + " baud.");
                port.Open();

                cancellationTokenSource = new CancellationTokenSource();
                Task.Run(() => ReadSerialPort(cancellationTokenSource.Token));

                //readPort = true;
                //readThread = new Thread(ReadSerialPort);
                //readThread.Start();
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error opening serial port: " + e.Message + "on port " + port);
            }
        }
        else Debug.LogError("How about you connect the teensy to the right port, dumbass");
    }
    
    void Update()
    {
        // Display the latest data received from the serial port
        if (!string.IsNullOrEmpty(latestData))
        {
            Debug.Log("Received: " + latestData);
            latestData = ""; // Clear it after logging
        }

    }
    
    public void StartTransmision()
    {
        port.WriteLine("S");
    }

    public void StopTransmision()
    {
        port.WriteLine("E");
    }
    void OnDestroy()
    {
        ClosePort();
    }
    private void OnApplicationQuit()
    {
        ClosePort();
    }

    void ClosePort()
    {
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel();
        }

        if (port != null && port.IsOpen) 
        {
            port.Close();
            Debug.Log("Serial port closed.");
        }
    }

    async Task ReadSerialPort(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested && port != null && port.IsOpen)
            {
                try
                {
                    string data = await Task.Run(() => port.ReadLine());
                    lock (this)
                    {
                        latestData = data;
                    }
                }
                catch (TimeoutException) 
                { 
                    // Ignore timeouts to keep reading
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("Error reading serial port: " + e.Message);
        }
    }
}
