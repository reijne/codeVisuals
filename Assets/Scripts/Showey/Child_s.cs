using System.Collections;
using System.Collections.Generic;
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
  public string relativeDirection;
  public Vector3Int offset = new Vector3Int(0, 0, 0);

  public void onDirectionSelect() {
    relativeDirection = childDropdown.options[childDropdown.value].text;
  }

  public void setName(string name) {
    childName = name;
    childNameText.text = name;
  }

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

  private int clampToInt(string value) {
    int numeric = 0;
    int.TryParse(value, out numeric);
    return numeric;
  }
}
