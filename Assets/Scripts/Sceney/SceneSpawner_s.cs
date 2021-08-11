using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using SFB;

public class SceneSpawner_s : MonoBehaviour
{
  [SerializeField] Movement player;
  [SerializeField] GameObject blocky_prefab;
  [SerializeField] GameObject errorEnemy_prefab;
  public static Vector3 firstSpawn;
  public static Dictionary<int, Vector3> nodePositions = new Dictionary<int, Vector3>();
  private ShoweyDefinition showdef;
  private List<(string, string)> catNodeStack = new List<(string, string)>();
  private List<(Vector3Int, Vector3)> dirPosStack = new List<(Vector3Int, Vector3)>();
  private Vector3 spawnPoint = Vector3Int.zero;
  private List<GameObject> blockys = new List<GameObject>();
  private List<GameObject> enemies = new List<GameObject>();
  private List<Vector3> spawns = new List<Vector3>(); 
  private List<(int, string)> errorList = new List<(int, string)>();
  private Vector3Int currentDirection;
  private bool isPlayerPositioned = false;
  private int nodeID = 0;
  // private void Start() {
  //   initFromFile("D:\\School\\master_software_engineering\\Thesis\\Puzzle\\src\\Puzzle\\serialised.show");
  //   parseLabeledTraversal("in-Program-program\n-in-list[Stmt]-statements\nin-Stmt-decl\n-in-Type-datatype\nin-Type-t_num\r\n    out-Type-t_num\n-out-Type-datatype\nout-Stmt-decl\n-out-Stmt-statements\nout-Program-program");
  //   spawnErrorEnemy(2);
  //   spawnErrorEnemy(1);
  // }

  /// <summary> Initialise the spawner with a showeydefinition from json. </summary>
  public void initFromJSON(string serialisedShoweyDefinition) {
    showdef = ShoweyDefinition.fromSerialise(serialisedShoweyDefinition);
    currentDirection = SceneMaps.str2dir[showdef.vars.sign + showdef.vars.genDir];
    Blocky_s.SIZE = showdef.vars.blockySize;
    // Debug.Log("Initialised using JSON, blocky suze:: " + Blocky_s.SIZE);
  }

  /// <summary> Open a file panel to select a showeydefinition file. </summary>
    public void initFromFile() {
    string[] paths = StandaloneFileBrowser.OpenFilePanel("Load Showey Definition", "", "show", false);
    if (paths.Length != 0 && paths[0].Length != 0) {
      initFromFile(paths[0]);
    }
  }

  /// <summary> Initialise the spawner with a showeydefinition from file. </summary>
  public void initFromFile(string path) {
    // Debug.Log(path);
    StreamReader reader = new StreamReader(path);
    showdef = ShoweyDefinition.fromSerialise(reader.ReadLine());
    currentDirection = SceneMaps.str2dir[showdef.vars.sign + showdef.vars.genDir];
    Blocky_s.SIZE = showdef.vars.blockySize;
  }

  /// <summary> Clear the scene by removing all gameobjects and reinitialising the variables. </summary>
  public void clearScene() {
    catNodeStack = new List<(string, string)>();
    dirPosStack = new List<(Vector3Int, Vector3)>();
    spawnPoint = Vector3Int.zero;
    foreach (GameObject blocky in blockys) Destroy(blocky);
    blockys = new List<GameObject>();
    spawns = new List<Vector3>();
    nodePositions = new Dictionary<int, Vector3>();
    nodeID = 0;
    System.GC.Collect();
  }

  /// <summary> Clear all the enemies in the scene. </summary>
  public void clearEnemies() {
    foreach (GameObject enemy in enemies) Destroy(enemy);
    enemies = new List<GameObject>();
    System.GC.Collect();
  }

  #region Parsing
  /// <summary> Parse the labeled traversal of the AST containing nodes and children. </summary>
  public void parseLabeledTraversal(string labels) {
    if (labels == "") return;
    string[] labelList = labels.Split('\n');
    if (labelList.Length == 0) return;
    foreach (string label in labelList) {
      // Debug.Log(label);
      if (label == "") continue;
      if (label[0] == '-') {
        parseChild(label);
      } else {
        parseNode(label);
      }
    }
  }

  /// <summary> Parse the label of a child, containing the operation, type and name of the child. </summary>
  private void parseChild(string childLabel) {
    string[] parts = childLabel.Split('-');
    string operation = parts[1];
    string typ = parts[2];
    string child = parts[3];
    if (operation == "in") {
      setChildPath(typ, child);
    } else if (operation == "out") {
      backtrack();
    }
  }

  /// <summary> Parse the label of a node, containing the operation, category and name of the node. </summary>
  private void parseNode(string nodeLabel) {
    string[] parts = nodeLabel.Trim().Split('-');
    string operation = parts[0];
    string category = parts[1];
    string node = parts[2];
    if (operation == "in") {
      spawnNode(category, node);
      catNodeStack.Add((category, node));
      nodePositions[nodeID] = spawnPoint;
      Debug.Log(String.Format("New node {2} position added ID{0} POS{1}", nodeID, nodePositions[nodeID], node));
      nodeID++;
    } else if (operation == "out") {
      catNodeStack.RemoveAt(catNodeStack.Count-1);
    }
    // TODO remember which nodeID : spawned places
    
  }

  /// <summary> Parse the errors consisting of nodeID and msg. </summary>
  public void parseErrors(string errors) {
    Debug.Log(String.Format("In parseErrors, errors : {0}", errors));
    string[] errorSplit = errors.Split('\n');
    Debug.Log(errorSplit);
    foreach (string error in errorSplit) {
      if (error == "") continue;
      string[] parts = error.Split('|');
      int location = int.Parse(parts[0]);
      string msg = parts[1];
      location = getExistingLoc(location);
      errorList.Add((location, msg));
      spawnErrorEnemy(location);
    }
  }

  private int getExistingLoc(int loc) {
    while (!nodePositions.ContainsKey(loc)) {
      loc--;
      if (loc == 0) break;
    }
    return loc;
  }
  #endregion // Parsing

  #region Spawning
  /// <summary> Spawn a node into the scene using the showeyDefinition</summary>
  private void spawnNode(string category, string node) {
    // Debug.Log(category + " - "+ node);
    string blockyName = showdef.categoryNodeMap[category].nodes[node].blockyName;
    if (blockyName == Node_s.skipKeyword) return;
    // Debug.Log("Spawnpoint before adding:  @" + spawnPoint.x + spawnPoint.y + spawnPoint.z);
    incrementSpawnpoint();
    if (!isPlayerPositioned) {
      spawnPlayer();
    }
    GameObject blockyInstance = Instantiate(blocky_prefab, spawnPoint, Quaternion.identity);
    Blocky_s blockyScript = blockyInstance.GetComponent<Blocky_s>();
    blockyScript.setTilePositions(showdef.blockyMap[blockyName]);
    blockyScript.spawnTiles();
    // Debug.Log("SPAWNED :: " + blockyName + " @" + spawnPoint.x + spawnPoint.y + spawnPoint.z);
    blockys.Add(blockyInstance);
    spawns.Add(spawnPoint);
  }

  /// <summary> Spawn the player in the scene at the correct location. </summary>
  private void spawnPlayer() {
    Vector3Int up = Blocky_s.SIZE*SceneMaps.str2dir[SceneMaps.relDirMap[(SceneMaps.dir2str[currentDirection], "up")]];
    // Debug.Log(String.Format("up vector :: {0}", up));
    Vector3 lookAt = Blocky_s.SIZE*currentDirection + up;
    // Debug.Log(String.Format("lookAt first part :: {0}", Blocky_s.SIZE*currentDirection));
    // Debug.Log(String.Format("lookAt vector :: {0}", lookAt));
    player.setDesiredPosition(spawnPoint + up, spawnPoint + lookAt);
    player.cleanLookAt(spawnPoint + lookAt);
    isPlayerPositioned = true;
  }

  /// <summary> Spawn in an error enemy on a node location </summary>
  private void spawnErrorEnemy(int nodeID) {
    Debug.Log(String.Format("Spawning enemy on nodeID :: {0}", nodeID));
    if (nodePositions[nodeID] != null) spawnErrorEnemy(nodePositions[nodeID]);
  }

  /// <summary> Spawn in an error enemy on the specified position. </summary>
  private void spawnErrorEnemy(Vector3 pos) {
    Debug.Log(String.Format("Spawning enemy @ {0}", pos));
    GameObject enemy = Instantiate(errorEnemy_prefab, pos, Quaternion.identity);
    enemies.Add(enemy);
  }

  // TODO CLEAN THIS SHIT UP
  private void setChildPath(string typ, string child) {
    // Store the current direction and position
    // Debug.Log("Spawnpoint added to stack: " + spawnPoint.x + spawnPoint.y + spawnPoint.z);
    dirPosStack.Add((currentDirection, spawnPoint)); 
    // Gather the new direction 
    string curdir = SceneMaps.dir2str[currentDirection];
    (string category, string node) = catNodeStack[catNodeStack.Count-1];

    if (!showdef.categoryNodeMap[category].nodes[node].children.ContainsKey(typ + "_" + child))
      return;

    Child childClass = showdef.categoryNodeMap[category].nodes[node].children[typ + "_" + child];
    if (childClass.relativeDirection == Child_s.noChangeKeyword) return;

    // Debug.Log(childClass.relativeDirection);
    // Set the new direction
    string newdir = SceneMaps.relDirMap[(curdir, childClass.relativeDirection)];
    currentDirection = SceneMaps.str2dir[newdir];
    
    curdir = SceneMaps.dir2str[currentDirection];
    // set the new position
    // LR x, UD y, BF z
    string left = SceneMaps.relDirMap[(curdir, "left")];
    Vector3Int leftDir = SceneMaps.str2dir[left];

    string up = SceneMaps.relDirMap[(curdir, "up")];
    Vector3Int upDir = SceneMaps.str2dir[up];

    spawnPoint += leftDir * childClass.offset.x 
               + upDir * childClass.offset.y 
               + currentDirection * childClass.offset.z;
  }

  /// <summary> Increment the spawnpoint in the current direction to the open spot. </summary>
  private void incrementSpawnpoint() {
    spawnPoint += currentDirection * Blocky_s.SIZE;
    while (spawns.Contains(spawnPoint)) spawnPoint += currentDirection * Blocky_s.SIZE;
  }

  /// <summary> Pop the direction and position from the stack and restore them. </summary>
  private void backtrack() {
    (Vector3Int oldDirection, Vector3 oldPosition) = dirPosStack[dirPosStack.Count-1];
    currentDirection = oldDirection;
    spawnPoint = oldPosition;
    // Debug.Log("Set spawnpoint back to: " + spawnPoint.x + spawnPoint.y + spawnPoint.z);
    dirPosStack.RemoveAt(dirPosStack.Count-1); 
  }
  #endregion // Spawning
}
