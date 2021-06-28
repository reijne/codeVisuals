using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoweyController_s : MonoBehaviour
{
  [SerializeField] private int directionSize;
  [SerializeField] private Text plusMinusText;
  [SerializeField] private Text xyzText;
  [SerializeField] private Text camModeText;
  [SerializeField] private Text camDirText;
  [SerializeField] private GameObject generalDirectionIndicator;
  [SerializeField] private GameObject signIndicator;
  [SerializeField] private GameObject cameraIndicator;
  [SerializeField] private GameObject cameraDirectionControls;
  public string sign = "+";
  public string genDir = "x";
  public string camMode = "user";
  public string camDir = "NE";

  // Dictionary giving the scale of the direction indicator for a given direction
  private Dictionary<string, Vector3> genDirScaleMap;

  // Dictionary giving scale to the sign indicator for a given direction
  private Dictionary<string, Vector3> signScaleMap;

  #region Setup
  /// <summary> Setup the initial state of the maps, buttons and visual representation </summary>
  private void Start() {
    createMaps();
    updateButtons();
    updateVisualRepresentation();
  }

  /// <summary> Load the 
  public void loadShoweyVars(string sign, string genDir, string camMode, string camDir) {
    this.sign = sign;
    this.genDir = genDir;
    this.camMode = camMode;
    this.camDir = camDir;
    updateButtons();
    updateVisualRepresentation();
  }

  /// <summary> Create the scaling maps given the size of the direction visualisation </summary>
  private void createMaps() {
    genDirScaleMap = new Dictionary<string, Vector3>() {
      {"x", new Vector3(directionSize, 1, 1)},
      {"y", new Vector3(1, directionSize, 1)},
      {"z", new Vector3(1, 1, directionSize)}
    };

    float signSize = 1.2f / directionSize;
    signScaleMap = new Dictionary<string, Vector3>() {
      {"x", new Vector3(signSize, 1.2f, 1.2f)},
      {"y", new Vector3(1.2f, signSize, 1.2f)},
      {"z", new Vector3(1.2f, 1.2f, signSize)},
    };
  }
  #endregion

  #region Updaters
  /// <summary> Update the visual indication of the direction </summary> 
  private void updateVisualRepresentation() {
    generalDirectionIndicator.transform.localScale = genDirScaleMap[genDir];
    signIndicator.transform.localScale = signScaleMap[genDir];
    cameraIndicator.transform.localScale = signScaleMap[genDir];
    signIndicator.transform.localPosition = sign == "+" ? Maps.signPositionMap[genDir] : -Maps.signPositionMap[genDir];
    updateCameraIndicatorPosition();
  }

  /// <summary> Update the position of the indicator of the camera according to the chosen direction. </summary>
  private void updateCameraIndicatorPosition() {
    if (camMode == "user") {
      cameraIndicator.transform.localPosition = -signIndicator.transform.localPosition + Maps.camUserOffset[genDir];
    } else {
      cameraIndicator.transform.localPosition = Mathf.Sqrt(directionSize) * (Vector3) Maps.relativeDirectionMap[(sign + genDir, camDir)];
      // cameraIndicator.transform.localPosition = signIndicator.transform.localPosition + Maps.camUserOffset[genDir];
    }
  }

  /// <summary> Update the visual representation if the size is changed </summary>
  private void FixedUpdate() {
    if (genDirScaleMap["x"].x != directionSize) {
      setTextureSize();
      createMaps();
      updateVisualRepresentation();
    }
  }

  /// <summary> Set the texture repeating tile size such that it holds for the directionSize. </summary>
  private void setTextureSize() {
    if (genDir == "y") {
      generalDirectionIndicator.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1, directionSize);
    } else {
      generalDirectionIndicator.GetComponent<Renderer>().material.mainTextureScale = new Vector2(directionSize, 1);
    }
  }
  #endregion
  
  #region ButtonLogic
  /// <summary> Toggle the sign from positive to negative and vice versa. </summary> 
  public void toggleSign() {
    sign = sign == "+" ? "-" : "+";
    updateButtons();
    updateVisualRepresentation();
  }

  /// <summary> Cycle the general direction from X -> Y -> Z </summary> 
  public void cycleXYZ() {
    genDir = Maps.genDirMap[genDir];
    setTextureSize();
    updateButtons();
    updateVisualRepresentation();
  }

  /// <summary> Cycle the camera mode from static -> kinematic -> user </summary> 
  public void cycleCameraMode() {
    camMode = Maps.camModeMap[camMode];

    if (camMode == "user") {
      cameraDirectionControls.SetActive(false);
    } else {
      cameraDirectionControls.SetActive(true);
    }

    updateButtons();
    updateVisualRepresentation();
  }

  /// <summary> Cycle the camera directions from N -> NE -> E -> SE -> S -> SW -> W -> NW </summary>
  public void cycleCameraDirection() {
    camDir = Maps.camDirMap[camDir];
    updateButtons();
    updateVisualRepresentation();
  }

  /// <summary> Update the text of the buttons to represent the current variables </summary> 
  private void updateButtons() {
    plusMinusText.text = sign;
    xyzText.text = genDir;
    camModeText.text = camMode;
    camDirText.text = camDir;
  }
  #endregion

  /// <summary> Contain all the valuable information for the ShoweyController in a string. </summary>
  public string toString() {
    string showeyInfo = "";
    showeyInfo = "sign=\"+\";\ngenDir=\"x\";\ncamMode=\"user\";\ncamDir=\"NE\";\n";
    return showeyInfo; 
  }
}
