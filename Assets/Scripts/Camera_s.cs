using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_s : MonoBehaviour
{
  public static Transform target;
  public float smoothTime = 0.2f;
  public Vector3 offset; 
  // TODO calculate this using information from what we actually need to visualise
  void FixedUpdate()
  {
    if (target) {
      Vector3 desiredPos = target.position + offset;
      transform.position = Vector3.Lerp(transform.position, desiredPos, smoothTime);
      this.transform.LookAt(target);
    }
  }
}
