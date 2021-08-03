using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorEnemy_s : MonoBehaviour
{
  [SerializeField] Transform player;
  [SerializeField] float viewRange;
  private bool canSeePlayer = false;

  private void Start() {
    player = GameObject.FindGameObjectWithTag("Player").transform;
  }
  private void Update() {
    setCanSeePlayer();
    conditionalLook();
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
}
