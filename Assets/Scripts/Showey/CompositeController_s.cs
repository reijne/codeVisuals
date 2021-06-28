// Gathers all the information from the different controllers to create the textual definition of Showey.
// Author: Youri Reijne

using System;
using System.Collections.Generic;
using UnityEngine;

public class CompositeController_s : MonoBehaviour
{
  [SerializeField] ShoweyController_s showeyController;
  [SerializeField] BlockyController_s blockyController;
  [SerializeField] MappyController_s mappyController;

  private void Start() {
    // toString();
    toSerialize();
  }

  /// <summary> Create a string from all the components. </summary>
  public string toString() {
    string composite = "";
    composite += showeyController.toString();
    composite += blockyController.toString();
    composite += mappyController.toString();
    Debug.Log(composite);
    return composite;
  }

  private void FixedUpdate() {
    // toString();
    Debug.Log(toSerialize());
  }

  /// <summary> Make a serializable showey definition with all the information from the interface. </summary>
  public string toSerialize() {
    SeriShoweyDefinition showdef = new SeriShoweyDefinition();
    showdef.sign = showeyController.sign;
    showdef.genDir = showeyController.genDir;
    showdef.camMode = showeyController.camMode;
    showdef.camDir = showeyController.camDir;

    showdef.blockyDefinitions = makeBlockyDefinitions();
    showdef.categoryList = makeCategoryList();
    return JsonUtility.ToJson(showdef);
  }

  /// <summary> Create a list of Blocky definitions from the created artifacts. </summary>
  private List<BlockyDefinition> makeBlockyDefinitions() {
    List<BlockyDefinition> blockyDefinitions = new List<BlockyDefinition>();
    foreach (string name in blockyController.blockyMap.Keys) {
      List<(Vector3Int, Color)> tiles = blockyController.blockyMap[name];
      BlockyDefinition newBlocky = new BlockyDefinition();
      newBlocky.name = name;
      newBlocky.tilePositions = new List<Vector3Int>();
      newBlocky.tileColors = new List<Color>();
      
      foreach ((Vector3Int pos, Color col) in tiles) {
        newBlocky.tilePositions.Add(pos);
        newBlocky.tileColors.Add(col);
      }

      blockyDefinitions.Add(newBlocky);
    }
    return blockyDefinitions;
  }

  /// <summary> Create the list of Categories with the respective nodes. </summary>
  private List<Category> makeCategoryList() {
    List<Category> categoryList = new List<Category>();
    foreach (string categoryName in mappyController.categories) {
      Category newCategory = new Category();
      newCategory.categoryName = categoryName;
      newCategory.nodes = makeNodes(categoryName);
      categoryList.Add(newCategory); 
    }
    return categoryList;
  }

  /// <summary> Create a list of serializable nodes for a given category. </summary>
  private List<Node> makeNodes(string categoryName) {
    List<Node> nodes = new List<Node>();
    foreach (GameObject node in mappyController.categoryNodeMap[categoryName]) {
      Node_s nodeClass = node.GetComponent<Node_s>();
      Node newNode = new Node();
      newNode.nodeName = nodeClass.nodeName;
      newNode.blockyName = nodeClass.blockyName;
      newNode.children = makeChildren(nodeClass.children);
      nodes.Add(newNode);
    }
    return nodes;
  }

  /// <summary> Create a list of serializable children from the given list of gameobjects. </summary>
  private List<Child> makeChildren(List<GameObject> childrenGameObjects) {
    List<Child> children = new List<Child>();
    foreach(GameObject child in childrenGameObjects) {
      Child_s childClass = child.GetComponent<Child_s>();
      Child newChild = new Child();
      newChild.childName = childClass.childName;
      newChild.relativeDirection = childClass.relativeDirection;
      newChild.offset = childClass.offset;
      children.Add(newChild);
    }
    return children;
  }
}
