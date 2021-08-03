using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowCube_s : MonoBehaviour
{
  [SerializeField] GameObject explosion;
  public static float frequency = 50;
  public float lifeTime;
  /// <summary> Multiply the lifetime with the frequency of fixedupdate. </summary>
  private void Awake() {
    lifeTime = lifeTime * frequency;  
  } 

  /// <summary> Remove one tick from the lifetime, destroy if no lifetime left. </summary>
  private void FixedUpdate() {
    if (lifeTime-- <= 0) {
      Destroy(this.gameObject);
      Destroy(this);
    }
  }

  private void OnCollisionEnter(Collision other) {
    GameObject boom = Instantiate(explosion, transform.position, Quaternion.identity);
    Destroy(boom, 2f);
  }
}
