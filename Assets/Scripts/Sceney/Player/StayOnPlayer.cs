using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayOnPlayer : MonoBehaviour
{
  [SerializeField] Transform player;
  /// <summary> Set the position equal to the player. </summary>
  private void Update() {
    transform.position = player.position;
  }
}
