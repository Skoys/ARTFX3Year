using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public GameObject[,] _mapGrid;
    [SerializeField] private int _pillsLeft;

    public void InitGrid(int rows, int columns)
    {
        _mapGrid = new GameObject[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                _mapGrid[i, j] = null; 
            }
        }
    }

    public void AssignPillToTile(int row, int column, GameObject pill)
    {
        _mapGrid[row, column] = pill;
        _pillsLeft++;
    }

    public bool CheckSpaceForPill(int row, int column)
    {
        if (_mapGrid[row, column] != null)
        {
            Debug.Log("There's pill there");
            Destroy(_mapGrid[row, column]);
            _mapGrid[row, column] = null;
            _pillsLeft--;
        }

        if (_pillsLeft == 0)
        {
            return true;
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
