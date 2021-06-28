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

  /// <summary> Select standard if none selected. </summary>
  private void FixedUpdate() {
    if (nodeDropdownLabel.text == "" && nodeDropdown.options.Count > 0) {
      nodeDropdown.value = 1;
      onBlockySelection();
    }
    makeNodeOptions();
  }

  /// <summary> Make the static dropdown options for the Blocky visualisation and set . </summary>
  private void makeNodeOptions() {
    bool blockyStillExists = false;
    if (nodeOptions.Count != blockyController.blockySelector.options.Count+1) {
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
      Debug.Log("reset the name");
      Debug.Log(blockyName);
      blockyName = skipKeyword;
      Debug.Log(blockyName);
      Debug.Log("/n");
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

  /// <summary> Contain all the information about the node into a string. </summary>
  public string toString() {
    string nodeInfo = nodeName + ":" + blockyName + "{";
    foreach (GameObject child in children) {
      nodeInfo += "\n" + child.GetComponent<Child_s>().toString();
    }
    return nodeInfo + "}";
  }
}
