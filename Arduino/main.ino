#include <Adafruit_NeoPixel.h>

#define PIN 10
#define GET_COLOR(x) ((x & 0b00001110) >> 1)
#define GET_MODE(x)  ((x & 0b00000001))
#define GET_ID(x)    ((x & 0b11110000) >> 4)

Adafruit_NeoPixel strip[] = {
  Adafruit_NeoPixel(60, PIN, NEO_GRB + NEO_KHZ800),
  Adafruit_NeoPixel(60, PIN+1, NEO_GRB + NEO_KHZ800),
  Adafruit_NeoPixel(60, PIN+2, NEO_GRB + NEO_KHZ800),
  Adafruit_NeoPixel(60, PIN+3, NEO_GRB + NEO_KHZ800),
};

const int buttonPin = 2;     // the number of the pushbutton pin
unsigned char color = 0;
typedef struct t_color {
  uint32_t color;
  uint16_t state_machine;
} t_color;

t_color color_available[4] = {
  { strip[0].Color(255, 0, 0),   0b0000101010101010 },
  { strip[0].Color(0, 255, 0),   0b0000000000000111 },
  { strip[0].Color(0, 0, 255),   0b0000000011100000 },
  { strip[0].Color(255,64,0), 0b0000111000000000 }
};

void setup() {

  pinMode(buttonPin, INPUT);
  pinMode(buttonPin+2, INPUT);
  pinMode(buttonPin+4, INPUT);
  pinMode(buttonPin+6, INPUT);

  Serial.begin(9600);     // opens serial port, sets data rate to 9600 bps
  // put your setup code here, to run once:
  int i = 0;
  while (i != 4) {
    strip[i].begin();
    strip[i].show(); // Initialize all pixels to 'off'  
    ++i;
  }
}

void print_color_rgb(uint8_t id, uint32_t color) {
  unsigned int i = 0;

  for (unsigned int i = 0; i != 12; ++i) {
    strip[id].setPixelColor(i, color);
  }
}

void print_color_colorblind(uint8_t id, uint16_t state_machine) {
  uint16_t j = 1;
  for (unsigned int i = 0; i != 16; ++i) {
    if (state_machine & j)
        strip[id].setPixelColor(i, 255,0,0);
     j <<= 1;
  }
}

void clean(uint8_t id) {
  for (unsigned int i = 0; i != 12; i++) {
    strip[id].setPixelColor(i, 0,0,0);    
  }
}

void print_color(uint8_t id, unsigned char color, unsigned char mode) {
  if (mode == 0) print_color_rgb(id, color_available[color].color);
  else print_color_colorblind(id, color_available[color].state_machine);
}
char a = 0;
int oldButtonState[4] = {HIGH, HIGH, HIGH, HIGH}; 
void loop() {

  if (Serial.available() > 0)
  {
    uint8_t incomingByte = Serial.read();
    
    clean(GET_ID(incomingByte));
    print_color(GET_ID(incomingByte), GET_COLOR(incomingByte), GET_MODE(incomingByte));
    strip[GET_ID(incomingByte)].show();
  }

  uint32_t count = 0;
  while (count != 4)
  {
    int buttonState = digitalRead(count * 2 + buttonPin);
    
    if (oldButtonState[count] != buttonState)
    {
        oldButtonState[count] = buttonState;
        uint8_t octet = 0;
        octet = (count) << 1;
        if (buttonState == HIGH)
        {
          Serial.write(octet|0b00000000);
        }
        else
        {
          Serial.write(octet|0b00000001);      
        }
    }
    ++count;
  }
  
}
