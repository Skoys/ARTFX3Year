#include "raylib.h"
#include "Wall.h"
#include "Car.h"
#include <iostream>

using namespace std;

void Start();
void Restart();
void Update();
void Draw();
void End();

bool gameEnded = false;

int track_Cols = 20;
int track_Rows = 15;
int track_width = 40;

int trackGrid[15][20] = {
    {3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3 },
    {3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 },
    {1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
    {1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1 },
    {1, 0, 0, 0, 2, 1, 1, 1, 3, 3, 3, 1, 1, 1, 1, 2, 1, 0, 0, 1 },
    {1, 0, 0, 1, 1, 0, 0, 1, 1, 3, 1, 1, 0, 0, 0, 1, 1, 0, 0, 1 },
    {1, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1 },
    {1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1 },
    {1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2, 0, 0, 1, 0, 0, 1 },
    {1, 0, 0, 1, 0, 0, 2, 0, 0, 0, 2, 0, 0, 1, 0, 0, 1, 0, 0, 1 },
    {1, 10, 11, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 2, 0, 0, 1 },
    {1, 1, 1, 2, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 },
    {0, 4, 0, 0, 0, 0, 1, 3, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1 },
    {0, 4, 0, 0, 0, 0, 1, 3, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1 },
    {1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 3 }
};

Wall wallGrid[15][20];

Car player1;
Car player2;

void Start()
{
    InitWindow(800, 600, "Car Game");
    SetTargetFPS(60);

    for (int i = 0; i < track_Rows; i++)
    {
        for (int j = 0; j < track_Cols; j++)
        {
            if (trackGrid[i][j] == 1) {
                wallGrid[i][j].Start(j * track_width, i * track_width, track_width, ORANGE);
            }
            if (trackGrid[i][j] == 2) {
                wallGrid[i][j].Start(j * track_width, i * track_width, track_width, GRAY);
            }
            if (trackGrid[i][j] == 3) {
                wallGrid[i][j].Start(j * track_width, i * track_width, track_width, GREEN);
            }
            if (trackGrid[i][j] == 4) {
                wallGrid[i][j].Start(j * track_width, i * track_width, track_width, RAYWHITE);
            }
            if (trackGrid[i][j] == 10) {
                player1.Start(5, 20, 30, j * track_width + track_width * 0.5f, i * track_width + track_width * 0.5f, YELLOW);
            }
            if (trackGrid[i][j] == 11) {
                player2.Start(5, 20, 30, j * track_width + track_width * 0.5f, i * track_width + track_width * 0.5f, BLUE);
            }
        }
    }

    player1.AssignKeys(KEY_W, KEY_S, KEY_A, KEY_D);
    player2.AssignKeys(KEY_UP, KEY_DOWN, KEY_LEFT, KEY_RIGHT);
    player1.AssignName("Player 1");
    player2.AssignName("Player 2");

    trackGrid[10][2] = 0;
    trackGrid[10][1] = 0;

}

void Restart()
{
    player1.Restart();
    player2.Restart();

    gameEnded = false;
}

void Update()
{
    if (!gameEnded)
    {
        gameEnded = player1.Update();
        gameEnded = player2.Update();
    }
    
    player1.CheckCollision(trackGrid, track_width);    
    player2.CheckCollision(trackGrid, track_width);

    if(gameEnded && IsKeyDown(KEY_SPACE))
    {
        Restart();
    }
}

void Draw()
{
    BeginDrawing();

    ClearBackground(BLACK);

    for (int i = 0; i < track_Rows; i++)
    {
        for (int j = 0; j < track_Cols; j++)
        {
            wallGrid[i][j].Draw();
        }
    }

    player1.Draw();
    player2.Draw();

    EndDrawing();
}

void End()
{
    CloseWindow();
}

int main() 
{
    Start();

    while (!WindowShouldClose())
    {
        Update();
        Draw();
    }

    End();
    return 0;
}