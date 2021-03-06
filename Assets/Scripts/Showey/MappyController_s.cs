// Controller for the mapping of AST components to visual artifacts
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MappyController_s : MonoBehaviour
{
  [SerializeField] private Text categoryText;
  [SerializeField] private Button nextCategoryButton;
  [SerializeField] private Button prevCategoryButton;
  [SerializeField] private GameObject nodePrefab;
  [SerializeField] private Transform nodeSpawnPoint;
  [SerializeField] private RectTransform nodeScrollView;
  [SerializeField] private Scrollbar nodeScrollBar;
  public Dictionary<string, List<GameObject>> categoryNodeMap = new Dictionary<string, List<GameObject>>();
  public List<string> categories = new List<string>();
  private int currentCategoryIndex = 0;
  private Vector3 nodeSpawnOffset = new Vector3(0,0,0);

  /// <summary> Parse the intermediate representation containing an AST definition</summary>
  public void createCategoryMap(string astNodes) {
    string[] lines = astNodes.Split('\n');
    string category = "";
    List<string> childrenNames = new List<string>();
    string nodeName = "";
    foreach (string line in lines) {
      if (line.Contains("category")) { // new category
        category = line.Split(' ')[0];
        categories.Add(category);
        categoryNodeMap[category] = new List<GameObject>();
        nodeSpawnOffset = new Vector3(0,0,0);
      } else if (line.Contains("{}")) { // node without children
        categoryNodeMap[category].Add(createNode(line.Trim(new char[]{'{','}'})));
      } else if (line.Contains("{")) { // start of node with children
        nodeName = line.Trim('{');
      } else if (line.Contains("}")) { // end of node with children
        categoryNodeMap[category].Add(createNode(nodeName, childrenNames));
        childrenNames = new List<string>();
      } else { // child
        childrenNames.Add(line);
      }
    }
    currentCategoryIndex = categories.Count-1;
    cycleCategory(true);
  }

  /// <summary> overload createCategoryMap with the dictionary of Category_D type </summary>
  public void loadCategoryMap(Dictionary<string, Category_D> loadCategoryMap) {
    if (categories.Count > 0) clearCategories();
    categoryNodeMap = new Dictionary<string, List<GameObject>>();
    categories = new List<string>();
    foreach (string categoryName in loadCategoryMap.Keys) {
      nodeSpawnOffset = new Vector3(0,0,0);
      categories.Add(categoryName);
      categoryNodeMap[categoryName] = new List<GameObject>();
      foreach (string nodeName in loadCategoryMap[categoryName].nodes.Keys) {
        categoryNodeMap[categoryName].Add(createNode(nodeName, loadCategoryMap[categoryName].nodes[nodeName]));
      }
    }
    currentCategoryIndex = categories.Count-1;
    cycleCategory(true);
  }

  /// <summary> Clear the category list and map, with all the existing GameObjects. </summary>
  private void clearCategories() {
    foreach(string categoryName in categoryNodeMap.Keys) {
      foreach (GameObject node in categoryNodeMap[categoryName]) Destroy(node);
    }
    categoryNodeMap = new Dictionary<string, List<GameObject>>();
    categories = new List<string>();
  }

  /// <summary> Cycle through the possible categories and show their encompassed nodes and children. </summary>
  public void cycleCategory(bool next) {
    if (categories.Count == 0) return;
    foreach(GameObject node in categoryNodeMap[categories[currentCategoryIndex]]) {
      node.SetActive(false);
    }

    if (next) currentCategoryIndex = (currentCategoryIndex + 1) % categories.Count;
    else      currentCategoryIndex = (categories.Count+(currentCategoryIndex - 1)) % categories.Count;
    categoryText.text = categories[currentCategoryIndex];

    foreach(GameObject node in categoryNodeMap[categories[currentCategoryIndex]]) {
      node.SetActive(true);
    }
    nodeScrollBar.value = 1;
  }

  /// <summary> Create a new node using a name (and a list of children names).</summary>
  public GameObject createNode(string name, List<string> childrenNames=null) {
    GameObject node = Instantiate(nodePrefab, nodeSpawnPoint.position + nodeSpawnOffset, Quaternion.identity);
    Node_s node_s = node.GetComponent<Node_s>();
    node_s.setName(name);
    node.transform.SetParent(nodeScrollView);
    if (childrenNames != null) {
      foreach (string childname in childrenNames) {
        node_s.addChild(childname);
      }
      nodeSpawnOffset += new Vector3(0, -(1+childrenNames.Count)*25, 0);
    } else {
      nodeSpawnOffset += new Vector3(0, -25, 0);
    }
    node.SetActive(false);
    return node;
  }

  /// <summary> Create a new node using a name Node_D type, used for loading in a definition.</summary>
  public GameObject createNode(string name, Node_D node) {
    GameObject nodeGameObject = Instantiate(nodePrefab, nodeSpawnPoint.position + nodeSpawnOffset, Quaternion.identity);
    Node_s node_s = nodeGameObject.GetComponent<Node_s>();
    node_s.setName(name);
    node_s.setBlockyName(node.blockyName);
    nodeGameObject.transform.SetParent(nodeScrollView);

    foreach (string childname in node.children.Keys) {
      Child child = node.children[childname];
      node_s.addChild(childname, child.relativeDirection, child.offset);
    }
    
    nodeSpawnOffset += new Vector3(0, -(1+node.children.Count)*25, 0);
    nodeGameObject.SetActive(false);
    return nodeGameObject;
  }

  /// <summary> Contain all the valuable information for the MappyController in a string. </summary>
  public string toString() {
    string mapInfo = "";
    foreach (string category in categories) {
      mapInfo += category + " category";
      foreach (GameObject node in categoryNodeMap[category]) {
        mapInfo += "\n" + node.GetComponent<Node_s>().toString();
      }
      mapInfo += "\n";
    }
    return mapInfo;
  }
}
