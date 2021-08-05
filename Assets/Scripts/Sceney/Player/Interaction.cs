using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour, Target
{
  [SerializeField] float throwingPower;
  [SerializeField] float interactionRate;
  [SerializeField] Camera eyes;
  [SerializeField] GameObject particles_prefab;
  [SerializeField] GameObject throwCube_prefab;
  [SerializeField] GameObject handcube;
  [SerializeField] Material handcube_m;
  [SerializeField] Transform throwSpawnpoint;
  [SerializeField] Text healthText;
  [SerializeField] int health;
  public enum interactionType {shooting, throwing};
  private interactionType interType = interactionType.throwing;
  private float nextInteractTime = 0f;
  private float prevInteractTime = 0f;

  private void Start() {
    displayHealth();
  }

  private void Update() {
    // throwCube();
    colourHandCube();
    if (Input.GetButton("Fire1") && Time.time >= nextInteractTime && Movement.doInput) doInteraction();
    if (Input.GetKeyDown(KeyCode.Y)) toggleInteractionType();
  }

  private void colourHandCube() {
    float col = 1 - Mathf.Clamp((Time.time - prevInteractTime) / (nextInteractTime - prevInteractTime), 0, 1);
    switch (interType) {
      case interactionType.shooting : handcube_m.color = new Color(0, 0, col); break;
      case interactionType.throwing : handcube_m.color = new Color(col, 0, 0); break;
    }
  }

  private void doInteraction() {
    prevInteractTime = Time.time;
    nextInteractTime = Time.time + 1f / interactionRate;
    switch (interType) {
      case interactionType.shooting : shootToDrop(); break;
      case interactionType.throwing : throwCube(); break;
    }
  }

  private void toggleInteractionType() {
    switch (interType) {
      case interactionType.shooting: interType = interactionType.throwing; break;
      case interactionType.throwing: interType = interactionType.shooting; break;
    }
  }

  private void shootToDrop() {
    RaycastHit hit;
    if (Physics.Raycast(eyes.transform.position + 0.5f*eyes.transform.forward, eyes.transform.forward, out hit)) {
      if (hit.rigidbody != null) {
        hit.rigidbody.useGravity = !hit.rigidbody.useGravity;
        if (!hit.rigidbody.useGravity) {
          hit.rigidbody.velocity = Vector3.zero;
          hit.rigidbody.angularVelocity = Vector3.zero;
        }
        GameObject parts = Instantiate(particles_prefab, hit.transform);
        Destroy(parts, 3f);
      }
    }
  }

  private void throwCube() {
    Vector3 instantiatePosition = throwSpawnpoint.position+0.1f*eyes.transform.forward;
    GameObject cube = Instantiate(throwCube_prefab, instantiatePosition, throwSpawnpoint.rotation);
    RaycastHit hit;
    if (Physics.Raycast(eyes.transform.position + 0.5f*eyes.transform.forward, eyes.transform.forward, out hit)) {
      cube.GetComponent<Rigidbody>().velocity = throwingPower * (hit.transform.position-handcube.transform.position).normalized;
    } else {
      cube.GetComponent<Rigidbody>().velocity = throwingPower * eyes.transform.forward;
    }
  }

  public void hit(Collision other, Transform culprit) {
    health--;
    displayHealth();
  }

  private void displayHealth() {
    if (health > 0) healthText.text = String.Format("Health <3 :: {0}", health);
    else {
      healthText.text = "The Errors have fixed YOU!";
      Movement.doInput = false;
    }
  }
}
