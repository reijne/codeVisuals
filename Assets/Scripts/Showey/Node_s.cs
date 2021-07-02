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
  public static string skipKeyword = "skip";
  public string nodeName;
  public string blockyName;
  public List<GameObject> children = new List<GameObject>();
  private static List<Dropdown.OptionData> nodeOptions = new List<Dropdown.OptionData>() {new Dropdown.OptionData("skip")};

  /// <summary> Gather the blockyController and set the options on instantiate. </summary>
  private void Awake() {
    if (blockyController == null) blockyController = GameObject.FindWithTag("BlockyController").GetComponent<BlockyController_s>();
    makeNodeOptions();
  }

  /// <summary> Set the name of the node. </summary>
  public void setName(string name) {
    nodeName = name;
    nodeNameText.text = name;
  }

  /// <summary> Set the blockyname and select it in the dropdown. </summary>
  public void setBlockyName(string name) {
    if (name == skipKeyword) return;
    blockyName = name;
    int i;
    for (i = 0; i < nodeDropdown.options.Count; i++) {
      if (nodeDropdown.options[i].text == blockyName) break;
    }
    nodeDropdown.value = i;
    onBlockySelection();
  } 

  /// <summary> Select standard if none selected. </summary>
  private void FixedUpdate() {
    if (nodeDropdownLabel.text == "" && nodeDropdown.options.Count > 0) {
      nodeDropdown.value = 0;
      onBlockySelection();
    }
    makeNodeOptions();
  }

  /// <summary> Make the static dropdown options for the Blocky visualisation and set . </summary>
  private void makeNodeOptions() {
    bool blockyStillExists = true;
    if (nodeOptions.Count != blockyController.blockySelector.options.Count+1) {
      blockyStillExists = false;
      nodeOptions = new List<Dropdown.OptionData>(){new Dropdown.OptionData(skipKeyword)};
      foreach (Dropdown.OptionData blocky in blockyController.blockySelector.options) {
        nodeOptions.Add(blocky);
        if (blocky.text == blockyName) blockyStillExists = true;
      }
    }

    if (nodeDropdown.options != nodeOptions) {
      nodeDropdown.options = nodeOptions;
    }

    if (blockyName != skipKeyword && !blockyStillExists) {
      blockyName = skipKeyword;
      nodeDropdown.value = 0;
      onBlockySelection();
    }
  }

  /// <summary> When a Blocky is chosen as representation set the BlockyName. </summary>
  public void onBlockySelection() {
    blockyName = nodeDropdown.options[nodeDropdown.value].text;
    nodeDropdownLabel.text = blockyName;
  }

  /// <summary> Instantiate the child, set its position, parent transform and name. </summary>
  public void addChild(string name) {
    GameObject child = Instantiate(childPrefab);
    child.transform.SetParent(this.transform);
    child.transform.position = this.transform.position + new Vector3(10, -(1+children.Count)*25, 0);
    child.GetComponent<Child_s>().setName(name);
    children.Add(child);
  }

  public void addChild(string name, string relativeDirection, Vector3Int offset) {
    GameObject child = Instantiate(childPrefab);
    child.transform.SetParent(this.transform);
    child.transform.position = this.transform.position + new Vector3(10, -(1+children.Count)*25, 0);
    Child_s childClass = child.GetComponent<Child_s>();
    childClass.setName(name);
    childClass.setDirection(relativeDirection);
    childClass.setOffset(offset);
    children.Add(child);
  }

  // TODO addChild overload with Child type

  /// <summary> Contain all the information about the node into a string. </summary>
  public string toString() {
    string nodeInfo = nodeName + ":" + blockyName + "{";
    foreach (GameObject child in children) {
      nodeInfo += "\n" + child.GetComponent<Child_s>().toString();
    }
    return nodeInfo + "}";
  }
}
