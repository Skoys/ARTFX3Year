#pragma once
#include "raylib.h"
#include "Car.h"
class Wall
{
private:
	
	int _wallX = 800;
	int _wallY = 600;
	int _wallWidth = 40;
	int _wallHeight = 40;

	Color _wallColor = BLACK;

public:

	Wall();
	void Start(int row, int column, int width, Color color);
	//void Update(Car car);
	void Draw();
};

