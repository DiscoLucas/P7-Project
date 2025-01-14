
using UnityEngine;
using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;
using System.Collections;
using TMPro;
public class TeensyLogger : SingletonPersistent<TeensyLogger>
{
    Thread readThread;
    string latestData = "";
    SerialPort port;
    public bool readPort = false;
    CancellationTokenSource cancellationTokenSource;

    string portName = "COM3"; // Adjust to the correct port
    int baudRate = 9600;

    public int timeOut = 100;
    public float baselineRemainingTime = 0;
    Coroutine baselineCoroutine;

    public TMP_Text baselineTimerText;

    string BaselineFilePath() 
    {
        string basePath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        string folderPath = Path.Combine(basePath, "Logs");
        string filePath = Path.Combine(folderPath, SessionLogTracker.Instance.sessionLog.name + "_baseline.csv");
        return filePath; 
    }
    string GameFilePath()
    {
        string basePath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        string folderPath = Path.Combine(basePath, "Logs");
        string filePath = Path.Combine(folderPath, SessionLogTracker.Instance.sessionLog.name + "_gameData.csv");
        return filePath;
    }

    ConcurrentQueue<string> dataQueue = new ConcurrentQueue<string>();
    StreamWriter fileWriter;
    bool isLogging = false;
    void StartCommand() => port.WriteLine("S");
    void EndCommand() { port.WriteLine("E"); }

    void Start()
    {
        OnStart();
    }
    /*
    public void OnStart()
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
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error opening serial port: " + e.Message + "on port " + port);
            }
        }
        else
        {
            Debug.LogError("How about you connect the teensy to the right port, dumbass");
            gameObject.SetActive(false);
        }
        
    }
    */
    public void OnStart()
    {
        string[] availablePorts = SerialPort.GetPortNames();
        Debug.Log("Available ports: " + string.Join(", ", availablePorts));

        if (availablePorts.Length == 0)
        {
            Debug.LogError("No serial devices detected. Deactivating script.");
            Destroy(this);
            return;
        }

        foreach (string portNameCandidate in availablePorts)
        {
            string currentPort = portNameCandidate; // Assign to a new variable
            Debug.Log($"Checking port '{currentPort}' for Teensy...");

            try
            {
                using (var testPort = new SerialPort(currentPort, baudRate))
                {
                    testPort.ReadTimeout = 500; // Short timeout for device response
                    testPort.Open();

                    // Send a query to the device
                    testPort.WriteLine("IDENTIFY");
                    string response = testPort.ReadLine();

                    // Check for a Teensy-specific response
                    if (response.Contains("TEENSY_IDENTIFIER")) // Replace with your Teensy's actual response
                    {
                        Debug.Log($"Teensy detected on port '{currentPort}'. Connecting...");
                        portName = currentPort;

                        // Initialize the actual port
                        port = new SerialPort(portName, baudRate)
                        {
                            ReadTimeout = timeOut
                        };
                        port.Open();

                        cancellationTokenSource = new CancellationTokenSource();
                        Task.Run(() => ReadSerialPort(cancellationTokenSource.Token));
                        return;
                    }
                    else
                    {
                        Debug.Log($"Port '{currentPort}' is not a Teensy (response: {response}).");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error testing port '{currentPort}': {e.Message}");
            }
        }

        // No Teensy was found
        Debug.LogError("No Teensy detected. Deactivating script.");
        Destroy(this);
    }




    void Update()
    {
        // process and save data from queue
        while (dataQueue.TryDequeue(out string data))
        {
            if (fileWriter != null)
            {
                fileWriter.WriteLine($"{DateTime.Now};{data}");
            }
        }

        if (baselineTimerText == null) return;

        if (baselineRemainingTime > 0)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(baselineRemainingTime);
            baselineTimerText.text = $"Time remaing:\n{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }

        /*// Display the latest data received from the serial port
        if (!string.IsNullOrEmpty(latestData))
        {
            Debug.Log("Received: " + latestData);
            latestData = ""; // Clear it after logging
        }
        */
    }
    public void StartBaseLineLogging()
    {
        if (baselineCoroutine != null)
        {
            StopCoroutine(baselineCoroutine);
        }
        baselineCoroutine = StartCoroutine(BaselineLoggingCoroutine());
    }

    IEnumerator BaselineLoggingCoroutine()
    {
        StartLogging(BaselineFilePath());
        baselineRemainingTime = 150f; // funni magic number

        while (baselineRemainingTime > 0)
        {
            baselineRemainingTime -= Time.deltaTime;
            yield return null;
        }

        StopLogging();
        Debug.Log("Baseline logging compled");
    }

    public void StartGameLogging()
    {
       StartLogging(GameFilePath());
    }

    void StartLogging(string filePath)
    {
        StopLogging(); // just to be sure that no logging is already happening

        StartCommand();
        fileWriter = new StreamWriter(filePath, true);
        fileWriter.WriteLine("Reference timestamp; MCU timestamp; Red LED; IR LED; HR; HR validity; SpO2; SpO2 validity; HRV SDNN; HRV RMSSD; GSR average"); // Headers for the CSV
        isLogging = true;
        Debug.Log("Started logging to " + filePath);
    }

    public void StopLogging()
    {
        EndCommand();
        if (fileWriter != null)
        {
            fileWriter.Flush();
            fileWriter.Close();
            fileWriter = null;
        }
        isLogging = false;
        Debug.Log("Stopped logging");
    }
    /*
    void OnDestroy()
    {
        StopLogging();
        ClosePort();
    }*/

    protected override void OnApplicationQuit()
    {
        StopLogging();
        ClosePort();
    }

    public void ClosePort()
    {
        EndCommand();
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
                    if (isLogging)
                    {
                        dataQueue.Enqueue(data);
                    }
                    
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
