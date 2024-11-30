/* Code steps:
1. Initialize the MAX30102 sensor
2. Send ready signal to host
3. Wait for command from host
4. If the last command was 'S', start sensor logging.
    Data logged is:
    - Time since start of logging
    - Red LED value
    - IR LED value
    - Heart rate
    - SpO2
    - Heart rate validity
    - SpO2 validity
    - Heart rate variance
    
5. Fill the buffer with 100 samples and transmit to host
6. check if the last command was 'E', if not, repeat step 5
7. If the last command was 'E', stop sensor logging
8. If host connection is lost, stop sensor logging


*/


#include <Arduino.h>
#include "algorithm.h"
#include "max30102.h"
#include <Wire.h>

int maxBrightness = 255;

// Sensor pins
const int intPin = 20;
const int sclPin = 19;
const int sdaPin = 18;
const int GSRPin = A2;

// Sensor values
int gsrSensorValue = 0;
int gsrAverage = 0;

uint32_t irBuffer[100];
uint32_t redBuffer[100];
int32_t irBufferLength;
int32_t spo2;
int8_t spo2Valid;
int32_t heartRate;
int8_t heartRateValid;
uint8_t dummy;

float hrv_sdnn = 0;
float hrv_rmssd = 0;

bool isTransmitting = false;
void StopDataTransmission()
{
  digitalWriteFast(LED_BUILTIN, LOW);
  isTransmitting = false;
  Serial.println("Ending transmission");
}

void setup()
{
  while (!Serial && millis() < 5000);

  pinMode(LED_BUILTIN, OUTPUT);
  pinMode(intPin, INPUT_PULLUP); // pin connects to the interrupt output pin of the MAX30102
  
  Serial.println("Initializing MAX30102...");
  maxim_max30102_reset(); //resets the MAX30102
  if (!maxim_max30102_init())
  {
    Serial.println(F("MAX30102 was not found. Please check wiring/power."));
    while (1);
  }  
  Serial.println("MAX30102 initialized successfully");
  maxim_max30102_read_reg(REG_INTR_STATUS_1, &dummy); // reads and clears the interrupt status register
  Serial.println("Ready to recive commands");
  
}

void SensorLogger()
{
  isTransmitting = true;
  unsigned long startTime = millis();

  uint32_t un_min, un_max, un_prev_data, un_brightness;  //variables to calculate the on-board LED brightness that reflects the heartbeats
  int32_t i;
  float f_temp;
  
  un_brightness=0;
  un_min=0x3FFFF;
  un_max=0;
  
  irBufferLength = 100;  //buffer length of 100 stores 4 seconds of samples running at 25sps
  long gsrSum = 0;
  int gsrSampleCount = 0;

  //read the first 100 samples, and determine the signal range
  for(i = 0; i < irBufferLength; i++)
  {
    while(digitalRead(intPin)==1);  //wait until the interrupt pin asserts
    maxim_max30102_read_fifo((irBuffer+i), (redBuffer+i));  //read from MAX30102 FIFO
    
    if(un_min>irBuffer[i])
      un_min=irBuffer[i];  //update signal min
    if(un_max<irBuffer[i])
      un_max=irBuffer[i];  //update signal max
    Serial.print(F("red="));
    Serial.print(irBuffer[i], DEC);
    Serial.print(F(", ir="));
    Serial.println(redBuffer[i], DEC);

    // Read GSR sensor data during initialization
    gsrSensorValue = analogRead(GSRPin);
    gsrSum += gsrSensorValue;
    gsrSampleCount++;
  }

  un_prev_data=irBuffer[i];
  //calculate heart rate and SpO2 after first 100 samples (first 4 seconds of samples)
  maxim_heart_rate_and_oxygen_saturation(irBuffer, irBufferLength, redBuffer, &spo2, &spo2Valid, &heartRate, &heartRateValid, &hrv_sdnn, &hrv_rmssd); 
  gsrAverage = gsrSum / gsrSampleCount;
  
  

  //Continuously taking samples from MAX30102.  Heart rate and SpO2 are calculated every 1 second
  while(isTransmitting)
  {
    // reset the HR, SpO2 and GSR values
    i=0;
    un_min=0x3FFFF;
    un_max=0;

    gsrSum = 0;
    gsrSampleCount = 0;

    //dumping the first 25 sets of samples in the memory and shift the last 75 sets of samples to the top
    for(i=25;i<100;i++)
    {
      redBuffer[i-25]=redBuffer[i];
      irBuffer[i-25]=irBuffer[i];

      //update the signal min and max
      if(un_min>redBuffer[i])
        un_min=redBuffer[i];
      if(un_max<redBuffer[i])
        un_max=redBuffer[i];
    }

    //take 25 sets of samples before calculating the heart rate.
    for(i=75; i < 100; i++)
    {
      un_prev_data = redBuffer[i-1];
      while(digitalRead(intPin)==1);
      digitalWrite(9, !digitalRead(9));
      maxim_max30102_read_fifo((redBuffer+i), (irBuffer+i));

      // Read GSR data
      gsrSensorValue = analogRead(GSRPin);
      gsrSum += gsrSensorValue;
      gsrSampleCount++;

      unsigned long elapsedTime = millis() - startTime;

      //calculate the brightness of the LED
      if (redBuffer[i] > un_prev_data)
      {
        f_temp = redBuffer[i] - un_prev_data;
        f_temp /= (un_max - un_min);
        f_temp *= maxBrightness;
        f_temp = un_brightness - f_temp;
        
        if (f_temp < 0)
          un_brightness = 0;
        else
          un_brightness = (int)f_temp;
      }

      else
      {
        f_temp = un_prev_data - redBuffer[i];
        f_temp /= (un_max - un_min);
        f_temp *= maxBrightness;
        un_brightness += (int)f_temp;
        if (un_brightness > maxBrightness)
          un_brightness = maxBrightness;
      }

      Serial.print(F("Time: "));
      Serial.print(elapsedTime);

      Serial.print(F("; red="));
      Serial.print(redBuffer[i], DEC);
      Serial.print(F("; ir="));
      Serial.print(irBuffer[i], DEC);

      Serial.print(F("; HR="));
      Serial.print(heartRate, DEC);

      Serial.print(F("; HR Valid="));
      Serial.print(heartRateValid, DEC);

      Serial.print(F("; SPO2="));
      Serial.print(spo2, DEC);

      Serial.print(F("; SPO2 Valid="));
      Serial.print(spo2Valid, DEC);

      Serial.print(F("; HRV SDNN="));
      Serial.print(hrv_sdnn, 2);
      Serial.print(F("; HRV RMSSD="));
      Serial.print(hrv_rmssd, 2);
      
      gsrAverage = gsrSum / gsrSampleCount;

      // Output GSR average
      Serial.print(F("; GSR Average="));
      Serial.println(gsrAverage);
    }
    
    maxim_heart_rate_and_oxygen_saturation(irBuffer, irBufferLength, redBuffer, &spo2, &spo2Valid, &heartRate, &heartRateValid, &hrv_sdnn, &hrv_rmssd);

    // check for 'E' command to end transmission
    if (Serial.available() > 0 && Serial.read() == 'E')
    {
      StopDataTransmission();
    }
  }
}
// Counter function to simulate data transmission
void counter(){
  while (isTransmitting)
  {
    static unsigned cnt = 0;
    Serial.println(cnt++);
    if (Serial.read() == 'E')
    {
      StopDataTransmission();
    }
    delay(50);
  }
}

void AttemptReconnect()
{

}

void loop() 
{
  
  if (Serial.available() > 0)
  {
    char command = Serial.read();
    switch (command){
      case 'S': // start transmision
        digitalWriteFast(LED_BUILTIN, HIGH);
        isTransmitting = true;
        Serial.println("Starting transmision");
        SensorLogger();
        break;

      case 'E': // end transmision
        StopDataTransmission();
        break;
        
      default:
        Serial.println(String(command) + " is an invalid command");
        digitalWriteFast(LED_BUILTIN, LOW);
        break;
    }
  }
  else // TODO: if serial connection is lost, stop transmission and try to reconnect
  {
    
    digitalWrite(LED_BUILTIN, HIGH);
    delay(100);
    digitalWrite(LED_BUILTIN, LOW);
    delay(100);
    if (isTransmitting)
    {
      StopDataTransmission();
    }
  }

}
