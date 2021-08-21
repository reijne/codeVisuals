using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_s : MonoBehaviour
{
  private void OnCollisionEnter(Collision other) {
    if (other.transform.tag == "Player") {
      Debug.Log("Wew we hit the player.");
    }
  }
}
