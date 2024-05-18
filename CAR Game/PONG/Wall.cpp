#include "Wall.h"
Wall::Wall() : _wallColor(BLACK)
{

}

void Wall::Start(int X, int Y, int width, Color _color)
{
    _wallX = X;
    _wallY = Y;
    _wallWidth = width;
    _wallHeight = width;

    _wallColor = _color;
}

void Wall::Draw()
{
    DrawRectangle(_wallX, _wallY, _wallWidth, _wallHeight, _wallColor);
    
}
