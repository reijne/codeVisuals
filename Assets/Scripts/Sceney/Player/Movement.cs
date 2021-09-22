using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
  [SerializeField] float moveSpeed;
  [SerializeField] float jumpHeight;
  [SerializeField] float sensitivity;
  [SerializeField] float verticalViewClampAngle;
  [SerializeField] Transform camTransform;
  [SerializeField] CharacterController charController;
  [SerializeField] Transform groundCheck;
  public static float GRAVITY = -9.81f;
  public static bool doInput = true;
  public static bool thirdPerson = false;
  public enum MovementType {running, flying};
  private float groundCheckDistance = 0.5f;
  private LayerMask groundCheckMask;
  private MovementType moveType = MovementType.flying;
  private Vector3 startPosition;
  private Vector3 velocity;
  private Quaternion startRotation;
  private float rotX = 0.0f; // rotation around the right/x axis
  private bool skipone = false;

  /// <summary> Start the player by setting the start position and rotation. </summary>
  private void Start() {
    groundCheckMask = LayerMask.GetMask("Default");
    Cursor.lockState = CursorLockMode.Locked;
    setStartPosition(transform.position, transform.rotation);
    setRotation();
    float calibrationSensitivity = 519915;
    sensitivity = (sensitivity * Screen.width * Screen.height) / calibrationSensitivity;
  }

  #region (Re)Setters
  /// <summary> Set the movementType according to the Sceney initialisation. </summary>
  public void setMovementType(MovementType moveType) {
    this.moveType = moveType;
  }

  /// <summary> Store the starting position and rotation, used for resetting the player </summary>
  public void setStartPosition(Vector3 position, Quaternion rotation) {
    startPosition = position;
    startRotation = rotation;
  }

  /// <summary> Reset the player to the starting position and rotation. </summary>
  public void resetPosition() {
    MovementType m = moveType;
    setMovementType(MovementType.flying);
    Debug.Log(String.Format("Resetting from{0}", transform.position));
    transform.position = startPosition;
    Debug.Log(String.Format("Resetting to{0}", transform.position));
    Debug.Log(String.Format("Resetting to{0}", startPosition));
    transform.rotation = startRotation;
    setMovementType(m);
    velocity = Vector3.zero;
    skipone = true;
  }

  /// <summary> Move the player to a position and make them look at the Lookat position. </summary>
  public void setDesiredPosition(Vector3 position, Vector3 lookAt) {
    Debug.Log(String.Format("Position: {0} Lookat {1}", position,lookAt));
    doInput = false;
    transform.position = position;
    camTransform.LookAt(lookAt);
    doInput = true;
  }

  /// <summary> Look at a spot while matching height so rotation is clean is the y axis. </summary>
  public void cleanLookAt(Vector3 lookAt) {
    setDesiredPosition(new Vector3(transform.position.x, lookAt.y, transform.position.z), lookAt);
  }

  /// <summary> Set the default rotation of the camera. </summary>
  public void setRotation() {
    Vector3 rot = camTransform.localRotation.eulerAngles;
    rotX = rot.x;
  }
  #endregion // (Re)Setters

  /// <summary> Handle input, movement and camera. </summary>
  private void Update() {
    handleInput();
    handleMovement();
    handleOutOfBounds();
    if (thirdPerson) offsetCamera();
  }

  /// <summary> Handle the movement based on the movement type. </summary>
  private void handleMovement() {
    if (skipone) {
      skipone = false;
    } else {
      if (doInput) {
        switch (moveType) {
          case MovementType.flying: doFlyingController(); break;
          default: doCharacterController(); break;
        }
        updateCameraRotation();
      }
    }
  }

  /// <summary> Reset the player once they go out of bounds </summary>
  private void handleOutOfBounds() {
    if (moveType == MovementType.running && transform.position.y < -2*Blocky_s.SIZE-2*SceneSpawner_s.BlockyBounds.extents.y) {
      resetPosition();
    }
  }

  /// <summary> Offset the camera behind the player for third person. </summary>
  private void offsetCamera() {
    Debug.Log("Wow dude its like third person");
    camTransform.position = transform.position - 2*camTransform.forward + Vector3.up;
  }

  /// <summary> Handle the specific keyboard inputs.  </summary>
  private void handleInput() {
    if (Input.GetKeyDown(KeyCode.T)) toggleMovementType();
    if (Input.GetKey(KeyCode.R)) resetPosition();
  }

  /// <summary> Move the player according to the horizontal and vertical inputs. </summary>
  private void doCharacterController() {
    if (charController.isGrounded && velocity.y < 0) velocity.y = 0f;
    float x = Input.GetAxis("Horizontal");
    float z = Input.GetAxis("Vertical");
    Vector3 move = transform.right * x + transform.forward * z;
    charController.Move(move * Time.deltaTime * moveSpeed);

    if (isGrounded() && Input.GetButtonDown("Jump")) {
      velocity.y += Mathf.Sqrt(jumpHeight * -2f * GRAVITY);
    }

    velocity.y += GRAVITY * Time.deltaTime;
    charController.Move(velocity * Time.deltaTime);
  }

  /// <summary> Check whether the player is currently standing on the ground. </summary>
  private bool isGrounded() {
    return Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundCheckMask);
  }

  /// <summary> Fly the player around according to the horizontal and vertical inputs. </summary>
  private void doFlyingController() {
    float dt = Time.deltaTime;
    float dx = Input.GetAxis("Horizontal") * moveSpeed * dt;
    float dz = Input.GetAxis("Vertical") * moveSpeed * dt;
    float dy = 0;

    dy = handleUpDown(dt);
    transform.position += transform.TransformDirection(new Vector3(dx, dy, dz));    
  }

  /// <summary> Fly the player up or down according to space and left shift respectively. </summary>
  private float handleUpDown(float dt) {
    if (Input.GetKey(KeyCode.Space)) return moveSpeed * dt;
    else if (Input.GetKey(KeyCode.LeftShift)) return -moveSpeed * dt;
    else return 0;
  }

  /// <summary> Toggle which type of movement is currently required. </summary>
  public void toggleMovementType() {
    switch (moveType) {
      case MovementType.flying : {
        moveType = MovementType.running;
        break;}
      case MovementType.running : {
        moveType = MovementType.flying;
        velocity = Vector3.zero;
        break;}
    }
  }

  /// <summary> Rotate the eyes of the player according to the mouse movements. </summary>
  private void updateCameraRotation() {
    float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
    float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
    rotX = Mathf.Clamp((rotX - mouseY), -verticalViewClampAngle, verticalViewClampAngle);
    camTransform.localRotation = Quaternion.Euler(rotX, 0, 0);
    transform.Rotate(Vector3.up * mouseX);
  }
}
