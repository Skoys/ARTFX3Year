using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PacmanFunctions
{

    public static int SpeedCalculation(float distance)
    {

        if (distance > 0) { return 1; }
        if (distance < 0) { return -1; }
        return 0;

    }

    public static Vector2 GetPositionOnMap(int[,] map, int getPos)
    {

        for (int i = 0; i<map.GetLength(0) ; i++)
        {

            for(int j = 0; j<map.GetLength(1); j++)
            {

                if(map[i, j] == getPos)
                {

                        return new Vector2(j, i);

                }

            }

        }
        return Vector2.zero;
    }

    public static List<string> PutOriginLast(List<string> operations, string lastDirection)
    {
        string temporary = "";

        if(lastDirection == "north") { temporary = "south"; }
        else if (lastDirection == "south") { temporary = "north"; }
        else if (lastDirection == "east") { temporary = "west"; }
        else if (lastDirection == "west") { temporary = "east"; }

        for (int i = 0; i < operations.Count - 1; i++)
        {
            if (operations[i] == temporary)
            {
                operations.RemoveAt(i);
            }
        }

        operations.Add(temporary);

        return operations;
    }

    //Je ne sais pas comment optimiser ca plus
    public static List<string> GetOperationsinOrder(Vector2 distances)
    {

        bool isXFirst = Mathf.Abs(distances.x) > Mathf.Abs(distances.y);
        bool isXPositive = (distances.x >= 0);
        bool isYPositive = (distances.y >= 0);

        if (isXFirst)
        {
            if (isXPositive)
            {

                if (isYPositive){   return new List<string>(){ "east", "north", "south", "west" }; }
                else { return new List<string>() { "east", "south", "north", "west" }; }

            }
            else
            {

                if (isYPositive) { return new List<string>() { "west", "north", "south", "east" }; }
                else { return new List<string>() { "west", "south", "north", "east" }; }

            }
        }
        else
        {
            if (isYPositive)
            {

                if (isXPositive) { return new List<string>() { "south", "east", "west", "north" }; }
                else { return new List<string>() { "south", "west", "east", "north" }; }

            }
            else
            {

                if (isXPositive) { return new List<string>() { "north", "east", "west", "south" }; }
                else { return new List<string>() { "north", "west", "east", "south" }; }

            }

        }

    }

}
