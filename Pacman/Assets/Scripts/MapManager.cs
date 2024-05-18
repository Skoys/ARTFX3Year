using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("Folders")]
    [SerializeField] private GameObject _wallsFolder;
    [SerializeField] private GameObject _pillsFolder;

    [Header("Map properties")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _blinky;
    [SerializeField] private ScoreManager _scoreManager;

    [Header("Gam Property")]
    [SerializeField] private bool _gameEnded = false;

    private int[,] _mapGrid = new int[,]
{
    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
    { 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1},
    { 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1},
    { 1, 1, 0, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 1},
    { 1, 0, 0, 1, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 1, 0, 0, 1},
    { 1, 0, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 0, 1},
    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
    { 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1},
    { 2, 2, 2, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 2, 2, 2},
    { 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1},
    { 5, 2, 2, 2, 0, 0, 0, 1, 2, 2, 2, 1, 0, 0, 0, 2, 2, 2, 4},
    { 1, 1, 1, 1, 0, 1, 0, 1, 1, 2, 1, 1, 0, 1, 0, 1, 1, 1, 1},
    { 2, 2, 2, 1, 0, 1, 0, 0, 0, 9, 0, 0, 0, 1, 0, 1, 2, 2, 2},
    { 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1},
    { 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1},
    { 1, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 0, 1},
    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
    { 1, 0, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 0, 1},
    { 1, 0, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 0, 1},
    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
};

    [SerializeField] private GameObject[] _mapPrefabs;
    void Awake()
    {
        transform.position = new Vector3(0, 0);

        _scoreManager.InitGrid(_mapGrid.GetLength(1), _mapGrid.GetLength(0));

        for (int i = 0; i < _mapGrid.GetLength(0) ; i++)
        {

            for(int j = 0; j  < _mapGrid.GetLength(1); j++)
            {

                switch(_mapGrid[i, j])
                {

                    case 0:
                        GameObject wall = CreateGO(i, j, true);
                        wall.transform.SetParent(_pillsFolder.transform);
                        break;

                    case 1:
                        GameObject pill = CreateGO(i, j, false);
                        pill.transform.SetParent(_wallsFolder.transform);
                        break;

                    case 3:
                        _player.transform.SetParent(transform);
                        _player.transform.localPosition = new Vector3(j, i);
                        Player playerScript = _player.GetComponent<Player>();

                        playerScript.currentTile = new Vector2(j, i);
                        break;

                    case 9:
                        _blinky.transform.SetParent(transform);
                        _blinky.transform.localPosition = new Vector3(j, i);
                        GhostBehaviour _blinkyScript = _blinky.GetComponent<GhostBehaviour>();

                        _blinkyScript.currentTile = new Vector2(j, i);
                        break;

                    default:
                        break;

                }
            }
        }

    }

    public Vector2 IsNextTileFree(Vector2 currentTile, string nextDirection)
    {

        _gameEnded = _scoreManager.CheckSpaceForPill((int)currentTile.x, (int)currentTile.y);

        if (_gameEnded)
        {

            EndGame();

        }

        switch(nextDirection)
        { 

            case "north":

                if(_mapGrid[(int)currentTile.y - 1, (int)currentTile.x] != 1)

                    return  new Vector2(currentTile.x, currentTile.y - 1);
                return Vector2.zero;

            case "south":

                if (_mapGrid[(int)currentTile.y + 1, (int)currentTile.x] != 1)

                    return new Vector2(currentTile.x, currentTile.y +1);
                return Vector2.zero;

            case "east":

                if (_mapGrid[(int)currentTile.y, (int)currentTile.x + 1] == 4)
                {
                    Debug.Log("Teleport");
                    _player.transform.position = new Vector3(0, currentTile.y);
                    return new Vector2(1, currentTile.y);
                }
                
                    if (_mapGrid[(int)currentTile.y, (int)currentTile.x + 1] != 1)

                    return new Vector2(currentTile.x + 1, currentTile.y);
                return Vector2.zero;

            case "west":

                if (_mapGrid[(int)currentTile.y, (int)currentTile.x - 1] == 5)
                {
                    Debug.Log("Teleport");
                    _player.transform.position = new Vector3(_mapGrid.GetLength(1) - 1, currentTile.y);
                    return new Vector2(_mapGrid.GetLength(1) - 2, currentTile.y);
                }

                if (_mapGrid[(int)currentTile.y, (int)currentTile.x - 1] != 1)

                    return new Vector2(currentTile.x - 1, currentTile.y);
                return Vector2.zero;

            default:
                return Vector2.zero;
        }
    }

    private GameObject CreateGO(int i, int j, bool isPill)
    {

        GameObject gameObject = Instantiate(
                            _mapPrefabs[_mapGrid[i, j]],
                            new Vector3(j, i),
                            Quaternion.identity
                        );

        if( isPill ) { _scoreManager.AssignPillToTile(j, i, gameObject); }
        
        return gameObject;
    }

    private void EndGame()
    {
        _player.GetComponent<Player>().gameEnded = true;
    }
}
