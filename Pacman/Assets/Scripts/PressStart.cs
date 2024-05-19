using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressStart : MonoBehaviour
{
    [SerializeField] private GameObject _start;
    [SerializeField] private GameObject _title;

    private float _time = 0;

    private void Awake()
    {
        if (SceneManager.sceneCount > 1) { _title.SetActive(false); }
    }

    void Update()
    {
        if (_time + 1 < Time.realtimeSinceStartup)
        {

            if (_start.active) { _start.SetActive(false); }
            else { _start.SetActive(true); }

            _time = Time.realtimeSinceStartup;

        }

        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene(1);
        }

    }

}
