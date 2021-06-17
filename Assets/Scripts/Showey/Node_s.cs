using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node_s : MonoBehaviour
{
  [SerializeField] private BlockyController_s blockyController;
  [SerializeField] private Text nodeNameText; 
  [SerializeField] private Dropdown nodeDropdown;
  [SerializeField] private Text nodeDropdownLabel;
  [SerializeField] private GameObject childPrefab;
  public string nodeName;
  public string blockyName;
  public List<GameObject> children = new List<GameObject>();

  /// <summary> Gather the blockyController and set the options on instantiate. </summary>
  private void Awake() {
    if (blockyController == null) blockyController = GameObject.FindWithTag("BlockyController").GetComponent<BlockyController_s>();
    if (blockyController.blockySelector.options != nodeDropdown.options) {
      nodeDropdown.options = blockyController.blockySelector.options;
    }
  }

  /// <summary> Set the name of the node. </summary>
  public void setName(string name) {
    nodeName = name;
    nodeNameText.text = name;
  }

  /// <summary> Select standard if none selected. </summary>
  private void FixedUpdate() {
    if (nodeDropdownLabel.text == "" && nodeDropdown.options.Count > 0) {
      nodeDropdown.value = 1;
      onBlockySelection();
    }
  }

  /// <summary> When a Blocky is chosen as representation set the BlockyName</summary>
  public void onBlockySelection() {
    blockyName = nodeDropdown.options[nodeDropdown.value].text;
    nodeDropdownLabel.text = blockyName;
  }

  /// <summary> </summary>
  public void addChild(string name) {
    // Instantiate
    GameObject child = Instantiate(childPrefab);
    // set parent and position
    child.transform.SetParent(this.transform);
    child.transform.position = this.transform.position + new Vector3(10, -(1+children.Count)*25, 0);
    // set name
    child.GetComponent<Child_s>().setName(name);
    children.Add(child);
  }

  /// <summary> </summary>

}
