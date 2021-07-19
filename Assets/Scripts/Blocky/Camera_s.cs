// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Camera_s : MonoBehaviour
// {
//   public enum CameraMode {Static, Kinematic, User, Debug};
//   public enum CameraDirection {N, NE, E, SE, S, SW, W, NW};
//   public static GameObject target;
//   public static CameraMode camMode = CameraMode.Debug;
//   public static CameraDirection camDir = CameraDirection.NE;
//   public static int forwardbackward = 0;
//   public float smoothTime = 0.2f;
//   public Vector3 offset; 
//   private Vector3 scaledOffset = new Vector3(1, 1, 1);
//   // TODO calculate this using information from what we actually need to visualise

//   public static void setCameraMode(CameraMode mode) {
//     camMode = mode;
//   }

//   public static void setCamerDirection(CameraDirection dir) {
//     camDir = dir;
//   }
//   void FixedUpdate()
//   {
//     if (target) {
//       moveLookAtTarget();
//     }
//   }

//   private void moveLookAtTarget() {
//     scaledOffset = offset;
//     scaledOffset.x *= target.transform.localScale.x;
//     scaledOffset.y *= target.transform.localScale.y;
//     scaledOffset.z *= target.transform.localScale.z;
//     Vector3 desiredPos = target.transform.position + scaledOffset;
//     transform.position = Vector3.Lerp(transform.position, desiredPos, smoothTime);
//     this.transform.LookAt(target.transform);
//   }
// }
// // << Hehe