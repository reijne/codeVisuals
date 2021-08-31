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
  [SerializeField] UserInterface_s userInterface;
  [SerializeField] int health;
  public enum InteractionType {none, shooting, throwing};
  public InteractionType interType = InteractionType.throwing;
  private LayerMask interactMask;
  private float nextInteractTime = 0f;
  private float prevInteractTime = 0f;
  private int startingHealth;

  /// <summary> Save the starting health for reset and create the default layermast. </summary>
  private void Start() {
    startingHealth = health;
    interactMask = LayerMask.GetMask("Default");
  }

  /// <summary> Reset the health to the starting health and update the interface. </summary>
  public void resetHealth() {
    health = startingHealth;
    userInterface.setCount(health);
  }

  /// <summary> Handle inputs from the user to trigger interaction. </summary>
  private void Update() {
    colourHandCube();
    if (Input.GetButton("Fire1") && Time.time >= nextInteractTime && Movement.doInput) doInteraction();
    if (Input.GetKeyDown(KeyCode.Y)) toggleInteractionType();
    resetParent();
  }

  /// <summary> Set the cube correctly in 3rd person. </summary>
  private void resetParent() {
    if (Movement.thirdPerson && handcube.transform.parent != transform) {
      Vector3 offset = handcube.transform.localPosition;
      handcube.transform.SetParent(transform);
      handcube.transform.localPosition = offset;
    }
  }

  /// <summary> Colour the handheld cube according to the interaction time. </summary>
  private void colourHandCube() {
    float col = 1 - Mathf.Clamp((Time.time - prevInteractTime) / (nextInteractTime - prevInteractTime), 0, 1);
    switch (interType) {
      case InteractionType.shooting : handcube_m.color = new Color(0, 0, col); break;
      case InteractionType.throwing : handcube_m.color = new Color(col, 0, 0); break;
    }
  }

  /// <summary> Do an interaction, i.e. shoot or throw </summary>
  private void doInteraction() {
    prevInteractTime = Time.time;
    nextInteractTime = Time.time + 1f / interactionRate;
    switch (interType) {
      case InteractionType.shooting : shootToDrop(); break;
      case InteractionType.throwing : throwCube(); break;
    }
  }

  /// <summary> Toggle which interaction type will be used. </summary>
  private void toggleInteractionType() {
    switch (interType) {
      case InteractionType.shooting: interType = InteractionType.throwing; break;
      case InteractionType.throwing: interType = InteractionType.shooting; break;
    }
  }

  /// <summary> Set the interaction type. </summary>
  public void setInteractionType(string interaction) {
    switch (interaction) {
      case "shoot" : setInteractionType(InteractionType.shooting); break;
      case "throw" : setInteractionType(InteractionType.throwing); break;
      default: setInteractionType(InteractionType.none); break;
    }
  }

  /// <summary> Set the interaction type. </summary>
  public void setInteractionType(InteractionType it) {
    interType = it;
  }

  /// <summary> Shoot a raycast and toggle the gravity of the hit gameobject. </summary>
  private void shootToDrop() {
    RaycastHit hit;
    if (Physics.Raycast(eyes.transform.position + 0.5f*eyes.transform.forward, eyes.transform.forward, out hit, 9999f, interactMask)) {
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

  /// <summary> Throw a cube at the target the user is pointing at or straight out if no target is found. </summary>
  private void throwCube() {
    Vector3 instantiatePosition = throwSpawnpoint.position+0.1f*eyes.transform.forward;
    GameObject cube = Instantiate(throwCube_prefab, instantiatePosition, throwSpawnpoint.rotation);
    RaycastHit hit;
    if (Physics.Raycast(eyes.transform.position + 0.5f*eyes.transform.forward, eyes.transform.forward, out hit, 9999f, interactMask)) {
      cube.GetComponent<Rigidbody>().velocity = throwingPower * (hit.transform.position-handcube.transform.position).normalized;
    } else {
      cube.GetComponent<Rigidbody>().velocity = throwingPower * eyes.transform.forward;
    }
  }

  /// <summary> Target interface function, triggers when hit by a thrown cube (from enemy). </summary>
  public void hit(Collision other, Transform culprit) {
    health--;
    userInterface.setCount(health);
  }
}
