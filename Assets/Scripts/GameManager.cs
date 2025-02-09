﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour {
    public Canvas UI;
    public Slider BustedSlider;
    public Text GameOverText;
    public Text ScoreText;
    public Text CopCountText;
    public Text KilledCopsText;

    public GameObject PlayerPrefab;
    public GameObject CarModel;
    public static CarUserController Player;
    public Transform StartPosition;

    public Camera CameraInstance;
    public static Camera GameCamera;

    public static int Score;
    public static int CopCount;
    public static int KilledCops;

    bool duplicate = false;
    bool roundEnded = false;
    float InitialTimeScale = 1;

    // Mission-related stuff
    public static GameObject Houses;
    public static GameObject PizzaPlaces;
    public static GameObject TrafficCars;

    public GameObject HousesInstance;
    public GameObject PizzaPlacesInstance;
    public GameObject TrafficCarsInstance;

    void Awake() {
        GameObject playerObject = Instantiate<GameObject>(PlayerPrefab);
        playerObject.transform.position = StartPosition.position;
        playerObject.transform.rotation = StartPosition.transform.rotation;
        Player = playerObject.GetComponent<CarUserController>();
        Player.ReplaceModel(CarModel);
        StartPosition.gameObject.SetActive(false);
        GameCamera = CameraInstance;
        CameraInstance.GetComponent<CameraControllerTransparency>().target = Player.transform;
        Houses = HousesInstance;
        PizzaPlaces = PizzaPlacesInstance;
        TrafficCars = TrafficCarsInstance;
        //CameraInstance.GetComponent<CameraController>().target = Player.transform;
    }

    private void Start() {
        Time.timeScale = InitialTimeScale;
    }

    void OnLevelWasLoaded() {
        if (duplicate)
            return;

        StartRound();
    }

    void StartRound () {
        Time.timeScale = 1;
        GameOverText.gameObject.SetActive(false);
        roundEnded = false;
        Score = 0;
        KilledCops = 0;
        CopCount = 0;
        UpdateText();
    }

    private void Update () {
        if (CrossPlatformInputManager.GetButton("Start")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        if (roundEnded)
            return;


        if (Player.BustedLevel > 0) {
            BustedSlider.gameObject.SetActive(true);
            BustedSlider.value = Player.BustedLevel;

            if (Player.BustedLevel >= BustedSlider.maxValue) {
                EndRound();
            }
        } else {
            BustedSlider.gameObject.SetActive(false);
        }
        UpdateText();
    }

    void UpdateText() {
        ScoreText.text = "Score: " + Score;
        CopCountText.text = "Active Cops: " + CopCount;
        KilledCopsText.text = "Destroyed Cops: " + KilledCops;
    }

    void EndRound () {
        Time.timeScale = 0;
        GameOverText.gameObject.SetActive(true);
        roundEnded = true;
        GetComponent<AudioSource>().Play();
    }
}