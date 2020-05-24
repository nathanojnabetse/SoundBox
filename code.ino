//ASIGNACIÓN DE PINES
//Pin assignation
const int tecla1 = 2;
const int led = 3;
const int tecla2 = 4;
const int tecla3 = 5;
const int tecla4 = 6;
const int tecla5 = 7;
const int tecla6 = 8;
const int tecla7 = 9;
const int tecla8 = 10;
const int tecla9 = 11;
const int tecla10 = 12;
const int tecla11 = 13;

const int pot = 0;


// VARIABLES DE ESTADO DE BOTONES
// Button State variables
int volumen;
int volumen1=0;
int presionado=0; //is pressed

void setup() {
  // CONFIGURAR PINES COMO ENTRADAS
  // input configuration
  Serial.begin (9600);
  pinMode(tecla1, INPUT);
  pinMode(tecla2, INPUT);
  pinMode(tecla3, INPUT);
  pinMode(tecla4, INPUT);
  pinMode(tecla5, INPUT);
  pinMode(tecla6, INPUT);
  pinMode(tecla7, INPUT);
  pinMode(tecla8, INPUT);
  pinMode(tecla9, INPUT);
  pinMode(tecla10, INPUT);
  pinMode(tecla11, INPUT);

  // CONFIGURAR PIN DE LED COMO SALIDA
  // Led for volume as output
  pinMode(led, OUTPUT);
}

void loop() {
  volumen = analogRead(pot)/4;
  analogWrite(led,volumen);
  Serial.print(volumen);
  Serial.print(' ');
 
  //tecla1
  // button1 is pressed
  if(digitalRead(tecla1) == LOW){
    presionado = 1;
    while(digitalRead(tecla1) == LOW){
      presionado == 1;      
    }
    if(digitalRead(tecla1) == HIGH && presionado == 1){
      Serial.println();
      Serial.print("@1");
      delay(50);
      presionado=0;
  }
  }
  //tecla2
  // button2 is pressed
  if(digitalRead(tecla2) == LOW){
    presionado = 1;
    while(digitalRead(tecla2) == LOW){
      presionado == 1;      
    }
    if(digitalRead(tecla2) == HIGH && presionado == 1){
      Serial.println();
      Serial.print("@2");
      delay(50);
      presionado=0;
  }
  }
  //tecla3
  // button3 is pressed
  if(digitalRead(tecla3) == LOW){
    presionado = 1;
    while(digitalRead(tecla3) == LOW){
      presionado == 1;      
    }
    if(digitalRead(tecla3) == HIGH && presionado == 1){
      Serial.println();
      Serial.print("@3");
      delay(50);
      presionado=0;
  }
  }
  //tecla4
  // button4 is pressed
  if(digitalRead(tecla4) == LOW){
    presionado = 1;
    while(digitalRead(tecla4) == LOW){
      presionado == 1;      
    }
    if(digitalRead(tecla4) == HIGH && presionado == 1){
      Serial.println();
      Serial.print("@4");
      delay(50);
      presionado=0;
  }
  }
  //tecla5
  // button5 is pressed
  if(digitalRead(tecla5) == LOW){
    presionado = 1;
    while(digitalRead(tecla5) == LOW){
      presionado == 1;      
    }
    if(digitalRead(tecla5) == HIGH && presionado == 1){
      Serial.println();
      Serial.print("@5");
      delay(50);
      presionado=0;
  }
  }
  //tecla6
  // button6 is pressed
  if(digitalRead(tecla6) == LOW){
    presionado = 1;
    while(digitalRead(tecla6) == LOW){
      presionado == 1;      
    }
    if(digitalRead(tecla6) == HIGH && presionado == 1){
      Serial.println();
      Serial.print("@6");
      delay(50);
      presionado=0;
  }
  }
  //tecla7
  // button7 is pressed
  if(digitalRead(tecla7) == LOW){
    presionado = 1;
    while(digitalRead(tecla7) == LOW){
      presionado == 1;      
    }
    if(digitalRead(tecla7) == HIGH && presionado == 1){
      Serial.println();
      Serial.print("@7");
      delay(50);
      presionado=0;
  }
  }
  //tecla8
  // button8 is pressed
  if(digitalRead(tecla8) == LOW){
    presionado = 1;
    while(digitalRead(tecla8) == LOW){
      presionado == 1;      
    }
    if(digitalRead(tecla8) == HIGH && presionado == 1){
      Serial.println();
      Serial.print("@8");
      delay(50);
      presionado=0;
  }
  }
  //tecla9
  // button9 is pressed
  if(digitalRead(tecla9) == LOW){
    presionado = 1;
    while(digitalRead(tecla9) == LOW){
      presionado == 1;      
    }
    if(digitalRead(tecla9) == HIGH && presionado == 1){
      Serial.println();
      Serial.print("@9");
      delay(50);
      presionado=0;
  }
  }
  //tecla10
  // button10 is pressed
  if(digitalRead(tecla10) == LOW){
    presionado = 1;
    while(digitalRead(tecla10) == LOW){
      presionado == 1;      
    }
    if(digitalRead(tecla10) == HIGH && presionado == 1){
      Serial.println();
      Serial.print("@10");
      delay(50);
      presionado=0;
  }
  }
  //tecla11
  // button11 is pressed
  if(digitalRead(tecla11) == LOW){
    presionado = 1;
    while(digitalRead(tecla11) == LOW){
      presionado == 1;      
    }
    if(digitalRead(tecla11) == HIGH && presionado == 1){
      Serial.println();
      Serial.print("@11");
      delay(50);
      presionado=0;
  }
  }

  Serial.print('\n');
}
  
