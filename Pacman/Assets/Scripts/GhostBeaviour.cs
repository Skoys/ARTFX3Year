using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class GhostBehaviour : MonoBehaviour
{
    [SerializeField]private MapManager _mapManager;
    [SerializeField] private Material _gohstMaterial;

    [SerializeField] private float _speed;
    [SerializeField] private float _time = 0;

    public Vector2 currentTile = Vector2.zero;
    [SerializeField] private Vector2 _nextTile = Vector2.zero;

    [SerializeField] private string _turnDirection = "west";
    [SerializeField] private string _facingDirection  = "west";

    public bool gameEnded = false;

    void Start()
    {

        _nextTile = currentTile;
        NextDirection(_facingDirection);

    }

    // Update is called once per frame
    void Update()
    {

        switch (gameEnded)
        {
            case true:

                GameEnded(); 
                break;

            default:

                GhostMovement(); 
                break;
        }
        
    }

    private void GhostMovement()
    {
        if (currentTile != _nextTile)
        {

            transform.position += new Vector3(
                (_speed * Time.deltaTime) * SpeedCalculation(_nextTile.x - transform.position.x),
                (_speed * Time.deltaTime) * SpeedCalculation(_nextTile.y - transform.position.y)
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

        _nextTile = _mapManager.IsNextTileFree(currentTile, _turnDirection);

        if (_nextTile != Vector2.zero)
        {

            _facingDirection = _turnDirection;
            return;

        }

        _nextTile = _mapManager.IsNextTileFree(currentTile, _facingDirection);

        if (_nextTile != Vector2.zero)
        {

            return;

        }

        _nextTile = currentTile;

    }

    private void NextDirection(string direction)
    {

        //Debug.Log("Turn Direction");
        if (direction != _turnDirection)
        {

            Vector2 isfreeTile = _mapManager.IsNextTileFree(_nextTile, direction);

            if (isfreeTile != Vector2.zero)
            {

                _turnDirection = direction;

            }
        }

    }

    private int SpeedCalculation(float distance)
    {

        if (distance > 0) { return 1; }
        if (distance < 0) { return -1; }
        return 0;

    }

    public void GameEnded()
    {

        switch (_time + 0.5f < Time.realtimeSinceStartup)
        {

            case true:

                if (_gohstMaterial.color == Color.white)
                {

                    _gohstMaterial.color = Color.red;

                }
                else
                {

                    _gohstMaterial.color = Color.white;

                }
                //_time = Time.realtimeSinceStartup;
                break;

            default:
                break;

        }

    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector3(currentTile.x, currentTile.y), 1);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(_nextTile.x, _nextTile.y), 1);

    }
    
}
