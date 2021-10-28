using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_s : MonoBehaviour
{
  /// <summary> Upon player collision, add it to the score and remove the collectable. </summary>
  private void OnTriggerEnter(Collider other) {
    if (other.transform.tag == "Player") {
      Debug.Log("Wew we hit the player.");
      GameObject ui = GameObject.FindGameObjectWithTag("UserInterface");
      if (ui != null) ui.GetComponent<UserInterface_s>().increaseCollected();
      Destroy(this.gameObject);
      Destroy(this);
    }
  }
}