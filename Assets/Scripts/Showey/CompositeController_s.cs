// Gathers all the information from the components of showey to Save and Load from and to the interface.
// Author: Youri Reijne

using SFB;
using System.IO;
using System.Collections.Generic;
using UnityEngine;


public class CompositeController_s : MonoBehaviour
{
  [SerializeField] ShoweyController_s showeyController;
  [SerializeField] BlockyController_s blockyController;
  [SerializeField] MappyController_s mappyController;

  // private void Start() {
  //   loadShoweyDefinition();
  // }

  /// <summary> Create a new showey definition using the flattened abstract tree of a language. </summary>
  public void createShowey(string abstractTree) {
    mappyController.createCategoryMap(abstractTree);
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
    // Debug.Log(toSerialize()); // Why was this still on ;-;
  }

  public void loadShoweyDefinition() {loadShoweyDefinition(false);}  

  /// <summary> Load in a Serialized Showey Definition through file. </summary>
  public void loadShoweyDefinition(bool onlyImport) {
    // Open file
    var paths = StandaloneFileBrowser.OpenFilePanel("Load Showey Definition", "", "show", false);
    if (paths.Length != 0 && paths[0].Length != 0) {
      StreamReader reader = new StreamReader(paths[0]);
      if (onlyImport) importBlockys(reader.ReadLine());
      else loadShoweyDefinition(reader.ReadLine());
    }
  }

  /// <summary> Import solely the blockys from a Serialized Showey definition. </summary>
  public void importBlockys(string json) {
    ShoweyDefinition showdef = ShoweyDefinition.fromSerialise(json);
    Blocky_s.SIZE = showdef.vars.blockySize;
    blockyController.loadBlockyMap(showdef.blockyMap);
  }

  /// <summary> Load in a Serialized Showey Definition into the interface. </summary>
  public void loadShoweyDefinition(string json) {
    ShoweyDefinition showdef = ShoweyDefinition.fromSerialise(json);
    showeyController.loadShoweyVars(showdef.vars);
    showeyController.activateCameraDirection();
    blockyController.loadBlockyMap(showdef.blockyMap);
    mappyController.loadCategoryMap(showdef.categoryNodeMap);
  }

  /// <summary> Save a Serialized Showey Definition through a pop-up save window. </summary>
  public void saveShoweyDefinition() {
    string json = toSerialize();
    // Save file
    var path = StandaloneFileBrowser.SaveFilePanel("Save Showey Definition", "", "showdef", "show");
    // var path = EditorUtility.SaveFilePanel("Save Showey Definition", "", "showdef", "show");
    if (path.Length != 0) {
      StreamWriter writer = new StreamWriter(path, false);
      writer.WriteLine(json);
      writer.Close();
    }
  }

  /// <summary> Make a serializable showey definition with all the information from the interface. </summary>
  public string toSerialize() {
    SeriShoweyDefinition showdef = new SeriShoweyDefinition();
    showdef.vars = new ShoweyVars(showeyController.sign, showeyController.genDir, 
                                  showeyController.camMode, showeyController.camDir, Blocky_s.SIZE);
    showdef.blockyDefinitions = makeBlockyDefinitions();
    showdef.categoryList = makeCategoryList();
    return showdef.serialize();
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
