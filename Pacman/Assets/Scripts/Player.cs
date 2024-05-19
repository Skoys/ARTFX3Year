using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using Cinemachine;

public class Player : MonoBehaviour
{
    [SerializeField]private MapManager _mapManager;
    [SerializeField] private Material _playerMaterial;
    [SerializeField] private Color _startingColor;

    [SerializeField] private float _speed;
    [SerializeField] private float _time = 0;
    public float invincibilityTime = 10;
    public float currentInvincibilityTime;

    public Vector2 currentTile = Vector2.zero;
    [SerializeField] private Vector2 _nextTile = Vector2.zero;

    [SerializeField] private string _turnDirection = "west";
    [SerializeField] private string _facingDirection  = "west";

    public string currentState = "wait";

    void Start()
    {

        _playerMaterial.SetColor("_EmissionColor", _startingColor);
        _nextTile = currentTile;
        NextDirection(_facingDirection);

    }

    // Update is called once per frame
    void Update()
    {
        PlayerInputs();

        switch (currentState)
        {
            case "play":

                CheckCollisions();
                PlayerMovement();
                break;

            case "invincible":

                CheckCollisions();
                PlayerMovement();
                Shine();
                break;

            case "dead":

                Dead();
                break;

            case "win":

                Shine();
                break;

            default:
                break;
        }
        
    }

    void PlayerInputs()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            NextDirection("south");
        }

        else if (Input.GetKey(KeyCode.DownArrow))
        {
            NextDirection("north");
        }

        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            NextDirection("west");
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            NextDirection("east");
        }
    }

    private void PlayerMovement()
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

        _nextTile = _mapManager.IsNextTileFree(currentTile, _turnDirection, transform, true);

        if (_nextTile != Vector2.zero)
        {

            _facingDirection = _turnDirection;
            return;

        }

        _nextTile = _mapManager.IsNextTileFree(currentTile, _facingDirection, transform, true);

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

            Vector2 isfreeTile = _mapManager.IsNextTileFree(_nextTile, direction, transform,true);

            if (isfreeTile != Vector2.zero)
            {

                _turnDirection = direction;

            }
        }

    }

    private void CheckCollisions()
    {
        List<Transform> phantoms = _mapManager.GetPhantomsPosition();

        foreach (Transform phantom in phantoms)
        {
            if(phantom.position.x + 0.25f > transform.position.x && transform.position.x > phantom.position.x - 0.5f)
            {

                if (phantom.position.y + 0.25f > transform.position.y && transform.position.y > phantom.position.y - 0.5f)
                {

                    if(currentState == "invincible")
                    {

                        phantom.GetComponent<GhostBehaviour>().currentState = "killed";

                    }
                    else
                    {

                        currentState = "dead";
                        Invoke(nameof(PressStartScreen), 1);

                    }
                }
            }
        }
    }

    public void Dead()
    {

        if (transform.localScale.x >=  0.01f)
        {
            transform.localScale = transform.localScale * 0.95f;
        }

    }

    public void Shine()
    {

        currentInvincibilityTime -= Time.deltaTime;
        if (currentInvincibilityTime <= 0 && currentState == "invincible")
        {

            DropInvincibility();

        }

        switch (_time + 0.25f < Time.realtimeSinceStartup)
        {

            case true:

                if (_playerMaterial.GetColor("_EmissionColor") == Color.yellow)
                {

                    _playerMaterial.SetColor("_EmissionColor", Color.white);

                }
                else
                { 

                    _playerMaterial.SetColor("_EmissionColor", Color.yellow);

                }
                _time = Time.realtimeSinceStartup;
                break;

            default:
                break;
            
        }
    }

    public void DropInvincibility()
    {

        _mapManager.StopInvincibility();
        _playerMaterial.SetColor("_EmissionColor", Color.yellow);
        currentState = "play";

    }

    public void PressStartScreen()
    {

        if(SceneManager.sceneCount < 2)
        {

            SceneManager.LoadScene(0, LoadSceneMode.Additive);

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
