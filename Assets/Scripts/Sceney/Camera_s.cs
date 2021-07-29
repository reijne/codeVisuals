using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Camera_s : MonoBehaviour
{
  [SerializeField] float moveSpeed;
  [SerializeField] float jumpHeight;
  [SerializeField] float sensitivity;
  [SerializeField] float verticalViewClampAngle;
  [SerializeField] Transform camTransform;
  [SerializeField] CharacterController charController;
  [SerializeField] Transform groundCheck;
  public static float GRAVITY = -9.81f;
  private float groundCheckDistance = 0.25f;
  private LayerMask groundCheckMask;
  private enum MovementType {running, flying};
  private MovementType moveType = MovementType.flying;
  private Vector3 startPosition;
  private Vector3 velocity;
  private Quaternion startRotation;
  private Vector3 desiredPosition;
  private Vector3 desiredLookat;
  private bool isKinematic = false;
  private bool isInputDisabled = true;
  private float rotY = 0.0f; // rotation around the up/y axis
  private float rotX = 0.0f; // rotation around the right/x axis

  /// <summary> Start the player by setting the start position and rotation. </summary>
  private void Start() {
    groundCheckMask = LayerMask.GetMask("Default");
    Cursor.lockState = CursorLockMode.Locked;
    setStartPosition(transform.position, transform.rotation);
    // setDesiredPosition(new Vector3(0, 10, 0), new Vector3(1, 10, 0));
    setRotation();
    Debug.Log(this.transform.position);
  }
  public void setStartPosition(Vector3 position, Quaternion rotation) {
    startPosition = position;
    startRotation = rotation;
  }

  public void setDesiredPosition(Vector3 position, Vector3 lookAt) {
    isInputDisabled = true;
    desiredPosition = position;
    desiredLookat = lookAt;
  }

  public void setRotation() {
    Vector3 rot = camTransform.localRotation.eulerAngles;
    rotY = rot.y;
    rotX = rot.x;
  }

  public void moveTo() {
    // transform.position += (transform.TransformDirection(desiredPosition) / 100);
    transform.position = desiredPosition;
    transform.LookAt(desiredLookat);
    // if (transform.position == desiredPosition) isInputDisabled = false;
    isInputDisabled = false;
  }

  private void Update() {
    handleInput();
    if (!isInputDisabled) {
      switch (moveType) {
        case MovementType.flying: doFlyingController(); break;
        default: doCharacterController(); break;
      }
      updateCameraRotation();
    } else {
      Debug.Log("moving through script");
      Debug.Log(transform.position);
      moveTo();
    }
  }

  private void handleInput() {
    if (Input.GetKeyDown(KeyCode.T)) toggleMovementType();
    if (Input.GetKeyDown(KeyCode.R)) transform.rotation = Quaternion.identity;
    if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
  }

  private void doCharacterController() {
    if (charController.isGrounded && velocity.y < 0) velocity.y = 0f;
    float x = Input.GetAxis("Horizontal");
    float z = Input.GetAxis("Vertical");
    Vector3 move = transform.right * x + transform.forward * z;
    // Vector3 move = transform.TransformDirection(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    charController.Move(move * Time.deltaTime * moveSpeed);

    if (isGrounded() && Input.GetButtonDown("Jump")) {
      velocity.y += Mathf.Sqrt(jumpHeight * -2f * GRAVITY);
      Debug.Log("jumping");
    }

    velocity.y += GRAVITY * Time.deltaTime;
    charController.Move(velocity * Time.deltaTime);
  }

  /// <summary> Check whether the player is currently standing on the ground. </summary>
  private bool isGrounded() {
    return Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundCheckMask);
  }

  // private void updateCameraRotation() {
  //   getMouseRotation(sensitivity, verticalViewClampAngle);
  //   // transform.rotation = getMouseRotation(sensitivity, verticalViewClampAngle);
  //   // camTransform.rotation = transform.rotation;
  // }

  private void doFlyingController() {
    float dt = Time.deltaTime;
    float dx = Input.GetAxis("Horizontal") * moveSpeed * dt;
    float dz = Input.GetAxis("Vertical") * moveSpeed * dt;
    float dy = 0;

    dy = handleUpDown(dt);
    transform.position += transform.TransformDirection(new Vector3(dx, dy, dz));
    // if (moveType == MovementType.flying) {
    // } else {
    //   if (charController.isGrounded && Input.GetKeyDown(KeyCode.Space)) {
    //     dy = jumpHeight * dt;
    //     transform.position += new Vector3(dx, dy, dz);
    //   } 
    // }
    // Debug.Log(dx + "," + dy + "," + dz);
    
  }

  private float handleUpDown(float dt) {
    if (Input.GetKey(KeyCode.Space)) return moveSpeed * dt;
    else if (Input.GetKey(KeyCode.LeftShift)) return -moveSpeed * dt;
    else return 0;
  }

  public void toggleMovementType() {
    if (moveType == MovementType.flying) {
      moveType = MovementType.running;
    } else if (moveType == MovementType.running) {
      moveType = MovementType.flying;
    }
    Debug.Log("movement::" + moveType);
  }

  private void updateCameraRotation() {
    float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
    float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
    rotX = Mathf.Clamp((rotX - mouseY), -verticalViewClampAngle, verticalViewClampAngle);
    camTransform.localRotation = Quaternion.Euler(rotX, 0, 0);
    transform.Rotate(Vector3.up * mouseX);
  }
}
