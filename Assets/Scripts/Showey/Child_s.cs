// Child of a node, containing name and relative direction as well as the controls to select these.
// Author: Youri Reijne

using UnityEngine;
using UnityEngine.UI;

public class Child_s : MonoBehaviour
{
  [SerializeField] private Text childNameText;
  [SerializeField] private Dropdown childDropdown;
  [SerializeField] private InputField LRinput;
  [SerializeField] private InputField UDinput;
  [SerializeField] private InputField BFinput;
  public string childName;
  public string relativeDirection = "noChange";
  public Vector3Int offset = new Vector3Int(0, 0, 0);

  /// <summary> Set the name of the child, should be called upon instantiation. </summary>
  public void setName(string name) {
    childName = name;
    childNameText.text = name;
  }

  /// <summary> Initialise the relative direction. </summary>
  private void Awake() {
    onDirectionSelect();
  }

  /// <summary> Set the relative direction to the selection. </summary>
  public void onDirectionSelect() {
    relativeDirection = childDropdown.options[childDropdown.value].text;
  }

  /// <summary> Update the offset of the child and the interface text inputs. </summary>
  public void updateOffset(string direction) {
    if (direction == "leftright") {
      offset.x = clampToInt(LRinput.text);
      LRinput.text = offset.x.ToString();
    } else if (direction == "updown") {
      offset.y = clampToInt(UDinput.text);
      UDinput.text = offset.y.ToString();
    } else if (direction == "backfront") {
      offset.z = clampToInt(BFinput.text);
      BFinput.text = offset.z.ToString();
    }
  }

  /// <summary> TryParse the input as an integer or return standard value 0. </summary>
  private int clampToInt(string value) {
    int numeric = 0;
    int.TryParse(value, out numeric);
    return numeric;
  }

  /// <summary> Contain all the information of a child into a string. </summary>
  public string toString() {
    return childName + "-" + relativeDirection + "-(" + offset.x + "," + offset.y + "," + offset.z + ")";
  }
}
