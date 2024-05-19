#include "Car.h"
#include <cmath>
#include <string>

Car::Car()
{
    
}

void Car::Start(int carSpeed, int carWidth, int carHeight, int carX, int carY, Color color)
{
    _sizeY = carHeight;
    _sizeX = carWidth;
    _carX = carX;
    _carY = carY;
    _spawnCarX = carX;
    _spawnCarY = carY;

    _carColor = color;
}

void Car::AssignKeys(KeyboardKey keyUp, KeyboardKey keyDown, KeyboardKey keyLeft, KeyboardKey keyRight) 
{
    _keyUp = keyUp;
    _keyDown = keyDown;
    _keyLeft = keyLeft;
    _keyRight = keyRight;
}

void Car::AssignName(std::string name)
{
    _carName = name;
}

bool Car::Update()
{
    if (!_finished)
    {
        KeyHold();
        KeyReleased();
        KeyEvents();
    }
    
    if (abs(_carSpeed) >= _carReversePwr) _carSpeed *= _carGroundDecayMult;
    else _carSpeed = 0;
    oldPosition.x = _carX; 
    oldPosition.y = _carY;
    _carX += sin(_carRotation) * -_carSpeed;
    _carY += cos(_carRotation) * -_carSpeed;

    if (_finished) return true;
    else return false;
}

void Car::Draw()
{
    Rectangle rec{ _carX, _carY, _sizeX, _sizeY };
    DrawRectanglePro(rec, { _sizeX * 0.5f, _sizeY * 0.5f }, _carRotation * -57.5, _carColor);

    if (_finished) {
        IsWinner();
    }
}

void Car::KeyHold()
{
    if (IsKeyDown(_keyUp)) keyHeld_Gas = true;
    if (IsKeyDown(_keyDown)) keyHeld_Reverse = true;
    if (IsKeyDown(_keyLeft)) keyHeld_TurnLeft = true;
    if (IsKeyDown(_keyRight)) keyHeld_TurnRight = true;
}

void Car::KeyReleased()
{
    if (IsKeyUp(_keyUp)) keyHeld_Gas = false;
    if (IsKeyUp(_keyDown)) keyHeld_Reverse = false;
    if (IsKeyUp(_keyLeft)) keyHeld_TurnLeft = false;
    if (IsKeyUp(_keyRight)) keyHeld_TurnRight = false;
}

void Car::KeyEvents()
{
    if (keyHeld_Gas) _carSpeed += _carDrivePwr;
    if (keyHeld_Reverse) _carSpeed -= _carReversePwr;
    if (keyHeld_TurnLeft && _carMinTurnSpeed <= abs(_carSpeed)) _carRotation += _carTurn_Rate * PI;
    if (keyHeld_TurnRight && _carMinTurnSpeed <= abs(_carSpeed)) _carRotation -= _carTurn_Rate * PI;
}

void Car::CheckCollision(int trackgrid[15][20], int trackWidth)
{
    _collisionTest = trackgrid[static_cast<int>((_carY - _sizeY * 0.5f) / trackWidth)][static_cast<int>((_carX - _sizeX * 0.5f) / trackWidth)];
    if ( _collisionTest != 0 && _collisionTest != 4) leftUpCollision = true;
    else leftUpCollision = false;
    _collisionTest = trackgrid[static_cast<int>((_carY - _sizeY * 0.5f) / trackWidth)][static_cast<int>((_carX + _sizeX * 0.5f) / trackWidth)];
    if (_collisionTest != 0 && _collisionTest != 4) rightUpCollision = true;
    else rightUpCollision = false;
    _collisionTest = trackgrid[static_cast<int>((_carY + _sizeY * 0.5f) / trackWidth)][static_cast<int>((_carX - _sizeX * 0.5f) / trackWidth)];
    if (_collisionTest != 0 && _collisionTest != 4) leftDownCollision = true;
    else leftDownCollision = false;
    _collisionTest = trackgrid[static_cast<int>((_carY + _sizeY * 0.5f) / trackWidth)][static_cast<int>((_carX + _sizeX * 0.5f) / trackWidth)];
    if (_collisionTest != 0 && _collisionTest != 4) rightDownCollision = true;
    else rightDownCollision = false;

    _collisionTest = trackgrid[static_cast<int>((_carY + _sizeY * 0.5f) / trackWidth)][static_cast<int>((_carX + _sizeX * 0.5f) / trackWidth)];
    if (_collisionTest == 4)
    {
        _finished = true;
    }

    newPosition();
}

void Car::newPosition()
{
    if (leftUpCollision || leftDownCollision || rightUpCollision || rightDownCollision) 
    {
        _carSpeed = 0;
        //_car
        _carX = oldPosition.x;
        _carY = oldPosition.y;
    }
}

void Car::IsWinner()
{
    DrawText(TextFormat("%s has won!", _carName.c_str()), GetRenderWidth() / 4, GetScreenHeight() * 4 / 8, 40, RED);
    DrawText(TextFormat("Press SPACE to Restart", _carName.c_str()), GetRenderWidth() / 4, GetScreenHeight() * 6 / 8, 40, RED);
}

void Car::Restart()
{
    _carX = _spawnCarX;
    _carY = _spawnCarY;
    _finished = false;
    _carRotation = 0;
}