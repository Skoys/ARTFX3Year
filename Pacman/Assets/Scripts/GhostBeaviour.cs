using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class GhostBehaviour : MonoBehaviour
{
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private Material _ghostMaterial;
    public Color _startingColor;

    [SerializeField] private int _chanceToFollow;
    [SerializeField] private float _speed;
    [SerializeField] private float _time = 0;
    [SerializeField] private float _timeout = 0;
    [SerializeField] private int _numberOnMap;

    public float _timeToSpawn;
    [SerializeField] private bool _hasSpawned;

    public Vector2 currentTile = Vector2.zero;
    [SerializeField] private Vector2 _nextTile = Vector2.zero;

    [SerializeField] private string _facingDirection  = "west";

    public string currentState = "wait";

    void Start()
    {

        _time -= Time.realtimeSinceStartup;
        _ghostMaterial.SetColor("_EmissionColor", _startingColor);
        _nextTile = currentTile;

    }

    // Update is called once per frame
    void Update()
    {

        switch (currentState)
        {

            case "spawn":

                Spawn();
                break;

            case "move":

                GhostMovement();
                break;

            case "invincible":

                GhostMovement();
                PlayerInvincibility();
                break;

            case "killed":

                Iskilled();
                break;

            case "End":

                GameEnded();
                break;

            default:
                break;
        }
    }

    private void Spawn()
    {

        if (Time.realtimeSinceStartup >= _timeToSpawn)
        {

            currentState = "move";
            Vector2 tempPosition = PacmanFunctions.GetPositionOnMap(_mapManager.mapGrid, 9);
            transform.position = new Vector3(tempPosition.x, tempPosition.y);
            currentTile = tempPosition;
            _nextTile = tempPosition;

        }
    }

    private void GhostMovement()
    {

        if (currentTile != _nextTile)
        {

            transform.position += new Vector3(
                (_speed * Time.deltaTime) * PacmanFunctions.SpeedCalculation(_nextTile.x - transform.position.x),
                (_speed * Time.deltaTime) * PacmanFunctions.SpeedCalculation(_nextTile.y - transform.position.y)
                );

            if (Vector3.Distance(transform.localPosition, new Vector3(currentTile.x, currentTile.y)) >= 1)
            {

                currentTile = _nextTile;
                transform.position = new Vector3(currentTile.x, currentTile.y);

            }
        }
        else
        {

            NextMove();

        }
    }

    private void NextMove()
    {
        Vector2 playerPosition = _mapManager.playerPosition;

        Vector2 distances;
        if (currentState == "invincible") 
        {

            PlayerInvincibility();
            distances = new Vector2(currentTile.x - playerPosition.x, currentTile.y - playerPosition.y);

        }
        else 
        {

            distances = new Vector2(playerPosition.x - currentTile.x, playerPosition.y - currentTile.y);

        }

        Movement(distances, _chanceToFollow);

    }

    private void Movement(Vector2 distances, int luck)
    {

        List<string> operations;
        operations = PacmanFunctions.GetOperationsinOrder(distances);

        string newDirection;

        System.Random random = new System.Random();
        if (random.Next(0, 100) > luck)
        {
            operations.Add(operations[0]);
            operations.RemoveAt(0);

        }

        operations = PacmanFunctions.PutOriginLast(operations, _facingDirection);

        while (operations.Count > 0)
        {

            newDirection = operations[0];
            operations.RemoveAt(0);

            _nextTile = _mapManager.IsNextTileFree(currentTile, newDirection, transform, false);

            if (_nextTile != Vector2.zero)
            {

                _facingDirection = newDirection;
                operations.Clear();
                return;

            }
        }
    }

    private void PlayerInvincibility()
    {

        switch (_time + 0.5f < Time.realtimeSinceStartup)
        {

            case true:

                if (_ghostMaterial.GetColor("_EmissionColor") == Color.blue)
                {

                    _ghostMaterial.SetColor("_EmissionColor", Color.white);

                }
                else
                {

                    _ghostMaterial.SetColor("_EmissionColor", Color.blue);

                }
                _time = Time.realtimeSinceStartup;
                break;

            default:
                break;

        }
    }

    public void FinishedInvincibility()
    {

        _ghostMaterial.SetColor("_EmissionColor", _startingColor) ;
        currentState = "move";

    }

    public void Iskilled()
    {

        if (currentTile != _nextTile)
        {

            transform.position += new Vector3(
                (_speed * Time.deltaTime * 2) * PacmanFunctions.SpeedCalculation(_nextTile.x - transform.position.x),
                (_speed * Time.deltaTime * 2) * PacmanFunctions.SpeedCalculation(_nextTile.y - transform.position.y)
                );

            if (Vector3.Distance(transform.localPosition, new Vector3(currentTile.x, currentTile.y)) >= 1)
            {

                currentTile = _nextTile;
                transform.position = new Vector3(currentTile.x, currentTile.y);

            }
        }
        else
        {

            Vector2 spawn = PacmanFunctions.GetPositionOnMap(_mapManager.mapGrid, 9);
            Vector2 distances = new Vector2(spawn.x - currentTile.x, spawn.y - currentTile.y);
            if(Vector2.Distance(spawn, currentTile) == 0)
            {

                _ghostMaterial.SetColor("_EmissionColor", Color.black);
                currentState = "wait";
                Invoke(nameof(FinishedInvincibility), _timeout);

            }
            else
            {

                Movement(distances, 100);

            }
        }
    }

    public void GameEnded()
    {

        _ghostMaterial.SetColor("_EmissionColor", _startingColor);

    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector3(currentTile.x, currentTile.y), 1);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(_nextTile.x, _nextTile.y), 1);

    }
}
