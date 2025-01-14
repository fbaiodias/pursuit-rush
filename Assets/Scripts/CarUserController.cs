﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

public class CarUserController : MonoBehaviour {
    public float BustedIncRate = 3;
    public float BustedDecRate = 1;
    public GameObject Explosion;
    public GameObject Model;
    public int CopCount; // number of cops nearby
    public SphereCollider CopCountArea;

    [HideInInspector] public float BustedLevel = 0;

    CarController Car;
    float Steering;
    float Accel;

    void Awake() {
        Car = GetComponent<CarController>();
    }

    void Update() {
        Steering = CrossPlatformInputManager.GetAxis("Steering");
        float positive = CrossPlatformInputManager.GetAxis("Accelarate");
        float negative = CrossPlatformInputManager.GetAxis("Reverse");
        Accel = positive - negative;

        //if (CrossPlatformInputManager.GetButtonDown("Bomb")) Bomb();

        // temporary way to drown player
        if (transform.position.y < -10) BustedLevel = int.MaxValue;
    }

    void FixedUpdate() {
        if (BustedLevel > 0) BustedLevel -= BustedDecRate;

        Car.Move(Steering, Accel);
    }

    void OnCollisionStay(Collision collision) {
        switch (collision.gameObject.tag) {
            case "Cop":
                BustedLevel += BustedIncRate;
                break;
        }

    }

    void Bomb() {
        if (GameManager.KilledCops <= 0) return;

        // decrease bomb counter
        GameManager.KilledCops--;

        // create the explosion object
        Instantiate(Explosion, transform.position, Quaternion.identity);
    }

    void OnTriggerEnter(Collider col) {
        CarAIController cop = col.GetComponent<CarAIController>();
        if (cop != null) CopCount++;
    }

    void OnTriggerExit(Collider col) {
        CarAIController cop = col.GetComponent<CarAIController>();
        if (cop != null) CopCount--;
    }

    public void ReplaceModel(GameObject model) {
        Destroy(Model);
        Model = Instantiate<GameObject>(model);
        Model.transform.parent = transform;
        Model.transform.localPosition = model.transform.position;
        Model.transform.localRotation = model.transform.rotation;
    }
}
