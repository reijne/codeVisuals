using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Target {
  void hit(Collision other, Transform culprit);
}

public class ErrorEnemy_s : MonoBehaviour, Target
{
  [SerializeField] Transform player;
  [SerializeField] GameObject throwCube_prefab;
  [SerializeField] float viewRange;
  [SerializeField] float firerate;
  [SerializeField] float firePower;
  [SerializeField] GameObject eye1;
  [SerializeField] GameObject eye2;
  private bool canSeePlayer = false;
  private float nextShot;
  private int health = 3;

  private void Start() {
    player = GameObject.FindGameObjectWithTag("Player").transform;
    nextShot = Time.time + 2 * (1f / firerate);
  }
  private void Update() {
    setCanSeePlayer();
    conditionalLook();
    conditionalShoot();
  }

  private void setCanSeePlayer() {
    RaycastHit hit;
    Vector3 dest = player.position - transform.position;
    if (Physics.Raycast(transform.position, dest, out hit, viewRange)) {
      if (hit.transform.tag == "Player") canSeePlayer = true;
      else canSeePlayer = false;
    } else canSeePlayer = false;
  }

  private void conditionalLook() {
    if (!canSeePlayer) return;
    else {
      transform.LookAt(player);
    }
  }

  private void conditionalShoot() {
    if (canSeePlayer && Time.time >= nextShot) {
      if (Random.Range(0, 2) == 0) {
        Vector3 fireDirection = (player.position - transform.position).normalized;
        GameObject cube = Instantiate(throwCube_prefab, transform.position + fireDirection, transform.rotation);
        cube.GetComponent<Rigidbody>().velocity = firePower * fireDirection;
      }
      nextShot = Time.time + 1f/firerate;
    }
  }

  public void hit(Collision other, Transform culprit) {
    Debug.Log("We hit the fucker");
    switch (health) {
      case 3: Destroy(eye1); break;
      case 2: Destroy(eye2); break;
      default: {
        Destroy(this.gameObject);
        Destroy(this);
        break;
      }
    }
    health--;
  }
}
