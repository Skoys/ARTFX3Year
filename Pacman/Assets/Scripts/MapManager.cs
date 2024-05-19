using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class MapManager : MonoBehaviour
{
    [Header("Folders")]
    [SerializeField] private GameObject _wallsFolder;
    [SerializeField] private GameObject _pillsFolder;

    [Header("Map properties")]
    [SerializeField] private GameObject _player;
    public GameObject _blinky;
    public GameObject _pinky;
    public GameObject _inky;
    public GameObject _clyde;
    [SerializeField] private ScoreManager _scoreManager;

    [Header("Game Property")]
    [SerializeField] private bool _gameEnded = false;
    public Vector2 playerPosition;
    [SerializeField] private TextMeshProUGUI _timer;

    public int[,] mapGrid = new int[,]
{
    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
    { 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1},
    { 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1},
    { 1, 1, 0, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 1},
    { 1, 10, 0, 1, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 1, 0, 10, 1},
    { 1, 0, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 0, 1},
    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
    { 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1},
    { 2, 2, 2, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 2, 2, 2},
    { 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1},
    { 5, 2, 2, 2, 0, 0, 0, 1, 7, 8, 6, 1, 0, 0, 0, 2, 2, 2, 4},
    { 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1},
    { 2, 2, 2, 1, 0, 1, 0, 0, 0, 9, 0, 0, 0, 1, 0, 1, 2, 2, 2},
    { 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1},
    { 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1},
    { 1, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 0, 1},
    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
    { 1, 10, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 10, 1},
    { 1, 0, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 0, 1},
    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
};

    [SerializeField] private GameObject[] _mapPrefabs;
    void Awake()
    {

        transform.position = new Vector3(0, 0);
        CreateMap();

        StartCoroutine(StartingCount(4));

    }

    private IEnumerator StartingCount(int time)
    {
        while (time > 0)
        {
            Debug.Log(time);

            if (time > 1)
            {
                _timer.text = (time - 1).ToString();
            }
            else if (time == 1)
            {
                _timer.text = "GO";
            }

            yield return new WaitForSeconds(1);
            time--;
        }

        StartTheGame();
        _timer.text = "";
        
    }

    private void StartTheGame()
    {

        _player.GetComponent<Player>().currentState = "play";

        GhostBehaviour ghost = _blinky.GetComponent<GhostBehaviour>();
        ghost.currentState = "spawn";
        ghost._timeToSpawn += Time.realtimeSinceStartup;

        ghost = _pinky.GetComponent<GhostBehaviour>();
        ghost.currentState = "spawn";
        ghost._timeToSpawn += Time.realtimeSinceStartup;

        ghost = _inky.GetComponent<GhostBehaviour>();
        ghost.currentState = "spawn";
        ghost._timeToSpawn += Time.realtimeSinceStartup;

        ghost = _clyde.GetComponent<GhostBehaviour>();
        ghost.currentState = "spawn";
        ghost._timeToSpawn += Time.realtimeSinceStartup;

    }

    private void CreateMap()
    {
        _scoreManager.InitGrid(mapGrid.GetLength(1), mapGrid.GetLength(0));

        for (int i = 0; i < mapGrid.GetLength(0); i++)
        {

            for (int j = 0; j < mapGrid.GetLength(1); j++)
            {

                switch (mapGrid[i, j])
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

                    case 6:
                        _clyde.transform.SetParent(transform);
                        _clyde.transform.localPosition = new Vector3(j, i);
                        GhostBehaviour _clydeScript = _clyde.GetComponent<GhostBehaviour>();

                        _clydeScript.currentTile = new Vector2(j, i);
                        break;

                    case 7:
                        _inky.transform.SetParent(transform);
                        _inky.transform.localPosition = new Vector3(j, i);
                        GhostBehaviour _inkyScript = _inky.GetComponent<GhostBehaviour>();

                        _inkyScript.currentTile = new Vector2(j, i);
                        break;

                    case 8:
                        _pinky.transform.SetParent(transform);
                        _pinky.transform.localPosition = new Vector3(j, i);
                        GhostBehaviour _pinkyScript = _pinky.GetComponent<GhostBehaviour>();

                        _pinkyScript.currentTile = new Vector2(j, i);
                        break;

                    case 9:
                        _blinky.transform.SetParent(transform);
                        _blinky.transform.localPosition = new Vector3(j, i);
                        GhostBehaviour _blinkyScript = _blinky.GetComponent<GhostBehaviour>();

                        _blinkyScript.currentTile = new Vector2(j, i);
                        break;

                    case 10:
                        GameObject megaPill = CreateGO(i, j, true);
                        megaPill.transform.SetParent(_wallsFolder.transform);
                        break;

                    default:
                        break;

                }

            }

        }

    }

    public Vector2 IsNextTileFree(Vector2 currentTile, string nextDirection, Transform entity,bool isPacman)
    {

        if (isPacman) { _gameEnded = _scoreManager.CheckSpaceForPill((int)currentTile.x, (int)currentTile.y); }

        if (mapGrid[(int)currentTile.y, (int)currentTile.x] == 10 && isPacman)
        {

            mapGrid[(int)currentTile.y, (int)currentTile.x] = 0;
            InvincibilityTime();

        }

        if (_gameEnded)
        {

            EndGame();

        }

        Vector2 position;
        switch (nextDirection)
        {

            case "north":

                if (mapGrid[(int)currentTile.y - 1, (int)currentTile.x] != 1)
                {

                    position = new Vector2(currentTile.x, currentTile.y - 1);
                    if (isPacman) { playerPosition = position; }
                    return position;

                }
                return Vector2.zero;

            case "south":

                if (mapGrid[(int)currentTile.y + 1, (int)currentTile.x] != 1)
                {

                    position = new Vector2(currentTile.x, currentTile.y + 1);

                    if (isPacman) { playerPosition = position; }
                    return position;

                }
                return Vector2.zero;

            case "east":

                if (mapGrid[(int)currentTile.y, (int)currentTile.x + 1] == 4)
                {

                    entity.position = new Vector3(0, currentTile.y);
                    return new Vector2(1, currentTile.y);

                }
                
                if (mapGrid[(int)currentTile.y, (int)currentTile.x + 1] != 1)
                {

                    position = new Vector2(currentTile.x + 1, currentTile.y);

                    if (isPacman) { playerPosition = position; }
                    return position;

                }
                return Vector2.zero;

            case "west":

                if (mapGrid[(int)currentTile.y, (int)currentTile.x - 1] == 5)
                {

                    entity.position = new Vector3(mapGrid.GetLength(1) - 1, currentTile.y);
                    return new Vector2(mapGrid.GetLength(1) - 2, currentTile.y);

                }

                if (mapGrid[(int)currentTile.y, (int)currentTile.x - 1] != 1)
                {

                    position = new Vector2(currentTile.x - 1, currentTile.y);

                    if (isPacman) { playerPosition = position; }
                    return position;

                }
                return Vector2.zero;

            default:
                return Vector2.zero;
        }
    }

    private GameObject CreateGO(int i, int j, bool isPill)
    {

        GameObject gameObject = Instantiate(
                            _mapPrefabs[mapGrid[i, j]],
                            new Vector3(j, i),
                            Quaternion.identity
                        );

        if( isPill ) { _scoreManager.AssignPillToTile(j, i, gameObject); }
        
        return gameObject;
    }

    public List<Transform> GetPhantomsPosition()
    {
        return new List<Transform>
        {
            _blinky.transform,
            _pinky.transform,
            _inky.transform,
            _clyde.transform
        };
    }

    private void InvincibilityTime()
    {

        Player playerScript = _player.GetComponent<Player>();
        playerScript.currentState = "invincible";
        playerScript.currentInvincibilityTime = playerScript.invincibilityTime; ;

        GhostBehaviour ghost = _blinky.GetComponent<GhostBehaviour>();
        if ( ghost.currentState != "spawn") { ghost.currentState = "invincible"; }

        ghost = _pinky.GetComponent<GhostBehaviour>();
        if (ghost.currentState != "spawn") { ghost.currentState = "invincible"; }

        ghost = _inky.GetComponent<GhostBehaviour>();
        if (ghost.currentState != "spawn") { ghost.currentState = "invincible"; }

        ghost = _clyde.GetComponent<GhostBehaviour>();
        if (ghost.currentState != "spawn") { ghost.currentState = "invincible"; }

    }

    public void StopInvincibility()
    {

        GhostBehaviour ghost = _blinky.GetComponent<GhostBehaviour>();
        ghost.FinishedInvincibility();

        ghost = _pinky.GetComponent<GhostBehaviour>();
        ghost.FinishedInvincibility();

        ghost = _inky.GetComponent<GhostBehaviour>();
        ghost.FinishedInvincibility();

        ghost = _clyde.GetComponent<GhostBehaviour>();
        ghost.FinishedInvincibility();

    }

    private void EndGame()
    {

        Player player = _player.GetComponent<Player>();
        player.currentState = "win";
        player.Invoke(nameof(player.PressStartScreen), 3f);

        _blinky.GetComponent<GhostBehaviour>().currentState = "end";
        _pinky.GetComponent<GhostBehaviour>().currentState = "end";
        _inky.GetComponent<GhostBehaviour>().currentState = "end";
        _clyde.GetComponent<GhostBehaviour>().currentState = "end";

    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(playerPosition.x, playerPosition.y), 1);

    }

}
