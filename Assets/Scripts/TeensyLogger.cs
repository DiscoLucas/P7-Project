using System.IO.Ports;
using UnityEngine;
public class TeensyLogger : MonoBehaviour
{
    SerialPort serialPort;
    string portName = "COM3"; // Adjust to the correct port
    int baudRate = 9600;

    void Start()
    {
        try
        {
            serialPort = new SerialPort(portName, baudRate);
            serialPort.Open();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error opening serial port: " + e.Message);
        }
        
    }

    void Update()
    {
        if (serialPort.IsOpen && serialPort.BytesToRead > 0)
        {
            string data = serialPort.ReadLine();
            Debug.Log(data);
        }
    }

    void OnDestroy()
    {
        if (serialPort.IsOpen)
            serialPort.Close();
    }

}
