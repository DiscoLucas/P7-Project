#include <Arduino.h>
#include "algorithm.h"
#include "max30102.h"

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

void setup()
{

  Serial.begin(9600);
  pinMode(intPin, INPUT_PULLUP); // pin connects to the interrupt output pin of the MAX30102
  Serial.println("Initializing MAX30102...");
  delay(1000);
  maxim_max30102_reset(); //resets the MAX30102
  if (!maxim_max30102_init())
  {
    Serial.println(F("MAX30102 was not found. Please check wiring/power."));
    while (1);
  }  
  Serial.println("MAX30102 initialized successfully");
  maxim_max30102_read_reg(REG_INTR_STATUS_1, &dummy); // reads and clears the interrupt status register
  while (!Serial)  // wait for the serial port to connect. Needed for native USB port only
  {
      // wait for serial port to connect. Needed for native USB port only
  }
}


void loop() 
{
  uint32_t red, ir;
  if (maxim_max30102_read_fifo(&red, &ir)) {
    Serial.print("Red LED: ");
    Serial.println(red);
    Serial.print("IR LED: ");
    Serial.println(ir);
  } else {
    Serial.println("Failed to read from MAX30102");
  }

  uint32_t un_min, un_max, un_prev_data, un_brightness;  //variables to calculate the on-board LED brightness that reflects the heartbeats
  int32_t i;
  float f_temp;
  
  un_brightness=0;
  un_min=0x3FFFF;
  un_max=0;
  
  irBufferLength = 100;  //buffer length of 100 stores 4 seconds of samples running at 25sps

  //read the first 100 samples, and determine the signal range
  for(i=0;i< irBufferLength;i++)
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
  }
  un_prev_data=irBuffer[i];
  //calculate heart rate and SpO2 after first 100 samples (first 4 seconds of samples)
  maxim_heart_rate_and_oxygen_saturation(irBuffer, irBufferLength, redBuffer, &spo2, &spo2Valid, &heartRate, &heartRateValid); 

  //Continuously taking samples from MAX30102.  Heart rate and SpO2 are calculated every 1 second
  while(1)
  {
    i=0;
    un_min=0x3FFFF;
    un_max=0;

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

      Serial.print(F("red="));
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
      Serial.println(spo2Valid, DEC);
    }
    maxim_heart_rate_and_oxygen_saturation(irBuffer, irBufferLength, redBuffer, &spo2, &spo2Valid, &heartRate, &heartRateValid);
  }
}
