#include <Arduino.h>
#include <U8g2lib.h>

#ifdef U8X8_HAVE_HW_SPI
#include <SPI.h>
#endif
#ifdef U8X8_HAVE_HW_I2C
#include <Wire.h>
#endif

//U8G2_ST7920_128X64_1_HW_SPI u8g2(U8G2_R0, /* CS=*/ 10, /* reset=*/ 8);
U8G2_ST7920_128X64_1_HW_SPI u8g2(U8G2_R0, /* CS=*/ 15, /* reset=*/ 16);

void setup(void) {
  Serial.begin(9600);
  u8g2.begin();
  u8g2.enableUTF8Print();	
}

void loop(void) {
  u8g2.setFont(u8g2_font_unifont_t_korean1);
  u8g2.setFontDirection(0);
  u8g2.clearBuffer();
  u8g2.setCursor(0, 15);
  u8g2.print("LAB 8");
  while(Serial.available()){
    int data = Serial.parseInt();
    if(data == 131){
      u8g2.setCursor(0, 40);
      u8g2.print("최우석");
      u8g2.sendBuffer();
    }
    else if(data == 67) {
      u8g2.setCursor(0, 40);
      u8g2.print("조현지");
      u8g2.sendBuffer();
    }
  }
  
  delay(1000);
}
