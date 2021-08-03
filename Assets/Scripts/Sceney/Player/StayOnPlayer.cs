using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayOnPlayer : MonoBehaviour
{
  [SerializeField] Transform player;
  private void Update() {
    transform.position = player.position;
  }
}
