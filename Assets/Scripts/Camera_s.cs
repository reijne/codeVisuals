using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_s : MonoBehaviour
{
  public static GameObject target;
  public float smoothTime = 0.2f;
  public Vector3 offset; 
  private Vector3 scaledOffset = new Vector3(1, 1, 1);
  // TODO calculate this using information from what we actually need to visualise
  void FixedUpdate()
  {
    if (target) {
      scaledOffset = offset;
      scaledOffset.x *= target.transform.localScale.x;
      scaledOffset.y *= target.transform.localScale.y;
      scaledOffset.z *= target.transform.localScale.z;
      Vector3 desiredPos = target.transform.position + scaledOffset;
      transform.position = Vector3.Lerp(transform.position, desiredPos, smoothTime);
      this.transform.LookAt(target.transform);
    }
  }
}
