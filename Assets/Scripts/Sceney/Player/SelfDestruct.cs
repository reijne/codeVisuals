using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
  public static float frequency = 1;
  public float lifeTime;
  /// <summary> Multiply the lifetime with the frequency of fixedupdate. </summary>
  private void Awake() {
    lifeTime = frequency * 50;  
  } 

  /// <summary> Remove one tick from the lifetime, destroy if no lifetime left. </summary>
  private void FixedUpdate() {
    if (lifeTime-- <= 0) {
      Destroy(this.gameObject);
      Destroy(this);
    }
  }
}
