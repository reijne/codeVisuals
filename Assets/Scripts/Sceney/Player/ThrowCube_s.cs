using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowCube_s : MonoBehaviour
{
  [SerializeField] GameObject explosion;
  [SerializeField] AudioSource ears;
  [SerializeField] AudioClip explode;
  public static float frequency = 50;
  public float lifeTime;
  /// <summary> Multiply the lifetime with the frequency of fixedupdate. </summary>
  private void Awake() {
    lifeTime = lifeTime * frequency;
    ears = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();  
  } 

  /// <summary> Remove one tick from the lifetime, destroy if no lifetime left. </summary>
  private void FixedUpdate() {
    if (lifeTime-- <= 0) {
      Destroy(this.gameObject);
      Destroy(this);
    }
  }

  /// <summary> On collision, hit the target using the target interface. </summary>
  private void OnCollisionEnter(Collision other) {
    GameObject boom = Instantiate(explosion, transform.position, Quaternion.identity);
    Destroy(boom, 2f);
    Target t = other.gameObject.GetComponent(typeof(Target)) as Target;
    if (t != null) t.hit(other, transform);
    ears.clip = explode;
    ears.Play();
  }
}
