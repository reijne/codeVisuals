using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ref: https://learn.unity.com/tutorial/controlling-unity-camera-behaviour#5fc3f6a3edbc2a459f91c6ae
// ref: https://forum.unity.com/threads/flying-character.34783/
// ref: https://answers.unity.com/questions/29741/mouse-look-script.html
// ref: https://forum.unity.com/threads/how-to-correctly-setup-3d-character-movement-in-unity.981939/#post-6379746
public class FlyingMovement : MonoBehaviour
{
  public static Vector3 startPos;
  public bool disabled = true;
  [SerializeField] private Button toggleMovementButton;
  [SerializeField] private float speed = 20;
  [SerializeField] private float updateSpeed = 10;
  [SerializeField] private float sensitivity = 100f;
  [SerializeField] private float clampAngle = 80f;
  private float rotY = 0.0f; // rotation around the up/y axis
  private float rotX = 0.0f; // rotation around the right/x axis
  private bool moving = false;
  private Vector3 desiredPos;
  private Vector3 desiredLookat;
  
  /// <summary> Set the starting position, move to it, lock the cursor and colour the movement button. </summary>
  void Start()
  {
    startPos = this.transform.position;
    if (toggleMovementButton) {
      toggleMovementButton.image.color = Color.gray;
    }
    moveToStart();
  }

  /// <summary> Handle movement. </summary>
  void FixedUpdate() {
    if (moving) {
      if (!Input.GetKey(KeyCode.LeftControl)) {
        this.transform.position = desiredPos;
        this.transform.LookAt(desiredLookat);
        setRotation();
      }
      moving = false;
      return;
    }
    if (disabled) return;
    if (!Input.GetKey(KeyCode.LeftAlt) && !moving) {
      float mouseX = Input.GetAxis("Mouse X");
      float mouseY = -Input.GetAxis("Mouse Y");

      rotY += mouseX * sensitivity * Time.deltaTime;
      rotX += mouseY * sensitivity * Time.deltaTime;

      rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

      Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
      transform.rotation = localRotation;
    }
      
    float dt = Time.deltaTime;
    float dy =  0;
    if(Input.GetKey(KeyCode.Space)) {
      dy = speed * dt;
    }
    if(Input.GetKey(KeyCode.LeftShift)) {
      dy -= speed * dt;
    }
    float dx = Input.GetAxis("Horizontal") * dt * speed;
    float dz= Input.GetAxis("Vertical") * dt * speed;
    transform.position += transform.TransformDirection(new Vector3(dx, dy, dz));
    // cc.Move(transform.TransformDirection(new Vector3(dx, dy, dz)));
  }

  /// <summary> Handle specified inputs. </summary>
  private void Update() {
    if (Input.GetKeyDown(KeyCode.T)) toggleMovement();
    if (Input.GetKeyDown(KeyCode.R)) moveToStart();
    if (!disabled && Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2)) disabled = true;
    TextualHandler.focusText = !disabled;
    updateMovementButton();
  }

  /// <summary> Update the colour of the movement button. </summary>
  private void updateMovementButton() {
    if (toggleMovementButton == null) return;
    if (disabled) {
      toggleMovementButton.image.color = Color.gray;
    } else {
      toggleMovementButton.image.color = Color.white;
    }
  }

  /// <summary> Set the rotation of the camera. </summary>
  public void setRotation() {
    Vector3 rot = transform.localRotation.eulerAngles;
    rotY = rot.y;
    rotX = rot.x;
  }

  /// <summary> Set the desired position and target. </summary>
  public void moveCam(Vector3 pos, Vector3 target) {
    moving = true;
    desiredPos = pos;
    desiredLookat = target;
  }

  /// <summary> Move back to the starting position. </summary>
  public static void moveToStart() {
    if (startPos != null) {
      FlyingMovement ft = GameObject.FindWithTag("MainCamera").GetComponent<FlyingMovement>();
      ft.moveCam(startPos, Vector3.zero);
    }
  }

  /// <summary> Toggle the movement. </summary>
  public void toggleMovement() {
    disabled = !disabled;
    if (disabled) Cursor.lockState = CursorLockMode.None;
    else Cursor.lockState = CursorLockMode.Confined;
  }
}
