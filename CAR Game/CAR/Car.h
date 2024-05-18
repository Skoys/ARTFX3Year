#pragma once
#include "raylib.h"
#include <string>

class Car {
private:
	
	bool keyHeld_Gas = false;
	bool keyHeld_Reverse = false;
	bool keyHeld_TurnLeft= false;
	bool keyHeld_TurnRight = false;

	bool leftUpCollision = false;
	bool leftDownCollision = false;
	bool rightUpCollision = false;
	bool rightDownCollision = false;

	bool _finished = false;

	float _carDrivePwr = 0.5f;
	float _carReversePwr = 0.5f;
	float _carTurn_Rate = 0.03f;
	float _carMinTurnSpeed = 1;
	float _carGroundDecayMult = 0.94f;

	int _spawnCarX = 10;
	int _spawnCarY = 10;

	int _collisionTest;
	std::string _carName;

	Vector2 oldPosition{ 0,0 };

	Color _carColor = BLUE;

public:
	float _carSpeed = 0;
	float _carRotation = 0;
	float _carX = 10;
	float _carY = 10;
	int _sizeX = 5;
	int _sizeY = 5;

	int _spawnCarTile = 10;

	KeyboardKey _keyUp;
	KeyboardKey _keyDown;
	KeyboardKey _keyLeft;
	KeyboardKey _keyRight;

	Car();
	void Start(int carSpeed, int carWidth, int carHeight, int carX, int carY, Color color);
	void AssignKeys(KeyboardKey _keyUp, KeyboardKey _keyDown, KeyboardKey _keyLeft, KeyboardKey _keyRight);
	void AssignName(std::string name);

	bool Update();
	void Draw();

	void KeyEvents();
	void KeyHold();
	void KeyReleased();
	void CheckCollision(int trackgrid[15][20], int trackwidth);
	void newPosition();
	void IsWinner();

	void Restart();
};

