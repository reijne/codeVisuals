using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
  [SerializeField] float throwingPower;
  [SerializeField] float interactionRate;
  [SerializeField] Camera eyes;
  [SerializeField] ParticleSystem particles;
  [SerializeField] GameObject handCubePrefab;
  [SerializeField] Transform throwSpawnpoint;
  public enum interactionType {shooting, throwing};
  private interactionType interType = interactionType.shooting;
  private float nextInteractTime = 0f;

  private void Update() {
    if (Input.GetButton("Fire1") && Time.time >= nextInteractTime) doInteraction();
  }

  private void doInteraction() {
    nextInteractTime = Time.time + 1f / interactionRate;
    switch (interType) {
      case interactionType.shooting : shootToDrop(); break;
      case interactionType.throwing : throwCube(); break;
    }
  }

  private void shootToDrop() {
    RaycastHit hit;
    if (Physics.Raycast(eyes.transform.position + 0.5f*eyes.transform.forward, eyes.transform.forward, out hit)) {
      Debug.Log(hit.transform.name);
      if (hit.rigidbody != null) {
        hit.rigidbody.useGravity = true;
        particles.Play();
      }
    }
  }

  private void throwCube() {
    Vector3 instantiatePosition = throwSpawnpoint.position+0.1f*eyes.transform.forward;
    GameObject cube = Instantiate(handCubePrefab, instantiatePosition, throwSpawnpoint.rotation);
    // cube.transform.SetParent(throwSpawnpoint);
    cube.GetComponent<Rigidbody>().velocity = throwingPower * eyes.transform.forward;
  }
}
