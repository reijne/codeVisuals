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
  [SerializeField] GameObject collectable_prefab;
  public static Vector3 firstSpawn;
  public static Dictionary<int, Vector3> nodePositions = new Dictionary<int, Vector3>();
  public static List<int> fallingBlocks = new List<int>();
  public static ShoweyDefinition showdef;
  public static Bounds BlockyBounds;
  private List<(string, string)> catNodeStack = new List<(string, string)>();
  private List<(Vector3Int, Vector3)> dirPosStack = new List<(Vector3Int, Vector3)>();
  private Vector3 spawnPoint = Vector3Int.zero;
  private List<GameObject> blockies = new List<GameObject>();
  private List<GameObject> enemies = new List<GameObject>();
  private List<GameObject> collectables = new List<GameObject>();
  private List<Vector3> spawns = new List<Vector3>(); 
  private List<(int, string)> errorList = new List<(int, string)>();
  private Vector3Int currentDirection;
  private bool isPlayerPositioned = false;
  private string currentLabels = "";
  private int nodeID = 0;

  /// <summary> Initialise the spawner with a showeydefinition from json. </summary>
  public void initFromJSON(string serialisedShoweyDefinition) {
    showdef = ShoweyDefinition.fromSerialise(serialisedShoweyDefinition);
    currentDirection = SceneMaps.str2dir[showdef.vars.sign + showdef.vars.genDir];
    Blocky_s.SIZE = showdef.vars.blockySize;

    Movement.thirdPerson = false;
    if (showdef.vars.camMode == "third person") {
      Movement.thirdPerson = true;
    }
  }

  /// <summary> Open a file panel to select a showeydefinition file and load it. </summary>
    public void initFromFile() {
    string[] paths = StandaloneFileBrowser.OpenFilePanel("Load Showey Definition", "", "show", false);
    if (paths.Length != 0 && paths[0].Length != 0) {
      string path = paths[0];
      StreamReader reader = new StreamReader(path);
      initFromJSON(reader.ReadLine());
    }
  }

  /// <summary> Clear the scene by removing all gameobjects and reinitialising the variables. </summary>
  public void clearScene() {
    catNodeStack = new List<(string, string)>();
    dirPosStack = new List<(Vector3Int, Vector3)>();
    spawnPoint = Vector3Int.zero;
    foreach (GameObject blocky in blockies) Destroy(blocky);
    blockies = new List<GameObject>();
    spawns = new List<Vector3>();
    nodePositions = new Dictionary<int, Vector3>();
    BlockyBounds = new Bounds();
    isPlayerPositioned = false;
    nodeID = 0;
    System.GC.Collect();
  }

  /// <summary> Clear all the enemies in the scene. </summary>
  public void clearEnemies() {
    foreach (GameObject enemy in enemies) Destroy(enemy);
    enemies = new List<GameObject>();
    System.GC.Collect();
  }

  public void clearCollectables() {
    foreach (GameObject collectable in collectables) Destroy(collectable);
    collectables = new List<GameObject>();
    System.GC.Collect();
  }

  #region Parsing
  /// <summary> Parse the labeled traversal of the AST containing nodes and children. </summary>
  public void parseLabeledTraversal(string labels) {
    currentLabels = labels;
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
    createBounds();
    if (showdef.vars.camMode == "kinematic") spawnCamera();
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
      nodeID++;
    } else if (operation == "out") {
      catNodeStack.RemoveAt(catNodeStack.Count-1);
    }
  }

  /// <summary> Parse the errors consisting of nodeID and msg. Spawn error enemies on locs. </summary>
  public void parseErrors(string errors) {
    string[] errorSplit = errors.Split('\n');
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

  /// <summary> Parse the branches and make them falling blocks, spawn collectable at the end. </summary>
  public void parseBranches(string branches) {
    fallingBlocks = new List<int>();
    string[] branchesSplit = branches.Split('|');
    string fallers = branchesSplit[0];
    string tails = branchesSplit[1];
    storeFallingBlocks(fallers);
    parseTails(tails);
  }

  /// <summary> Store the blocks that are fallers. </summary>
  private void storeFallingBlocks(string fallers) {
    if (fallers == "") return;
    string[] fallIDlist = fallers.Split(',');
    foreach (string fallID in fallIDlist) 
      addFallingBlock(int.Parse(fallID));
  }

  /// <summary> Add a falling block. </summary>
  public void addFallingBlock(int id) {
    fallingBlocks.Add(id);
  }

  /// <summary> Parse the end of every branch to spawn a colectable. </summary>
  private void parseTails(string tails) {
    if (tails == "") return;
    string[] tailSplit = tails.Split(',');
    UserInterface_s.maxCollected = 0;
    foreach (string loc in tailSplit) {
      spawnCollectable(int.Parse(loc));
      UserInterface_s.maxCollected++;
    }
  }
  #endregion // Parsing

  #region Spawning
  /// <summary> Get an existing node position to ensure spawning when a node is skipped in visuals. </summary>
  private int getExistingLoc(int loc) {
    while (!nodePositions.ContainsKey(loc)) {
      loc--;
      if (loc == 0) break;
    }
    return loc;
  }
  
  /// <summary> Spawn a node into the scene using the showeyDefinition</summary>
  private void spawnNode(string category, string node) {
    if (!showdef.categoryNodeMap.ContainsKey(category)) {
      Debug.LogError(String.Format("Category {0} not found in showeydefinition", category));
      return;
    }
    if (!showdef.categoryNodeMap[category].nodes.ContainsKey(node)) {
      Debug.LogError(String.Format("Node {0} not found in showeydefinition", node));
      return;
    }
    string blockyName = showdef.categoryNodeMap[category].nodes[node].blockyName;
    if (blockyName == Node_s.skipKeyword) return;
    incrementSpawnpoint();
    if (!isPlayerPositioned) spawnPlayer();
    instantiateNode(blockyName);
  }

  /// <summary> Instantiate a node in the scene, as Blocky. </summary>
  private void instantiateNode(string blockyName) {
    GameObject blockyInstance = Instantiate(blocky_prefab, spawnPoint, Quaternion.identity);
    Blocky_s blockyScript = blockyInstance.GetComponent<Blocky_s>();
    Quaternion rotation = Quaternion.LookRotation(currentDirection);
    blockyScript.nodeID = nodeID;
    blockyScript.setTilePositions(showdef.blockyMap[blockyName]);
    blockyScript.spawnTiles();
    blockyInstance.transform.rotation = rotation;
    blockies.Add(blockyInstance);
    spawns.Add(spawnPoint);
  }

  /// <summary> Spawn the player in the scene at the correct location. </summary>
  private void spawnPlayer() {
    Vector3 lookAt = Blocky_s.SIZE*currentDirection + Vector3.up;
    Debug.Log(String.Format("Spawning player at{0}", spawnPoint + Blocky_s.SIZE * Vector3.up));
    player.setDesiredPosition(spawnPoint + Blocky_s.SIZE * Vector3.up, spawnPoint + lookAt);
    player.setStartPosition(player.transform.position, player.transform.rotation);
    isPlayerPositioned = true;
  }

  /// <summary> Create the bounding box of all Blockies. </summary>
  private void createBounds() {
    BlockyBounds = new Bounds();
    foreach (GameObject blocky in blockies) BlockyBounds.Encapsulate(blocky.transform.position);
  }

  /// <summary> Spawn the camera using the showdef.camdir relative direction. </summary>
  private void spawnCamera() {
    Vector3 offset = Maps.relativeDirectionMap[(SceneMaps.dir2str[currentDirection], showdef.vars.camDir)];
    float dist = Mathf.Max(BlockyBounds.max.x, BlockyBounds.max.y, BlockyBounds.max.z);
    player.setDesiredPosition(BlockyBounds.center + dist*offset, BlockyBounds.center);
    player.setMovementType(Movement.MovementType.flying);
    player.setStartPosition(player.transform.position, player.transform.rotation);
  }

  /// <summary> Spawn in an error enemy on a node location </summary>
  public void spawnErrorEnemy(int nodeID) {
    if (nodePositions[nodeID] != null) spawnErrorEnemy(nodePositions[nodeID]);
  }

  /// <summary> Spawn in an error enemy on the specified position. </summary>
  private void spawnErrorEnemy(Vector3 pos) {
    GameObject enemy = Instantiate(errorEnemy_prefab, pos, Quaternion.identity);
    enemies.Add(enemy);
  }

  /// <summary> Spawn a collectable at a node position. </summary>
  public void spawnCollectable(int nodeID) {
    if (nodePositions[nodeID] != null) spawnCollectable(nodePositions[nodeID]);
  }

  /// <summary> Spawn a collectable at a given vector3 position. </summary>
  private void spawnCollectable(Vector3 pos) {
    pos = getOpenPos(pos, Vector3.up);
    GameObject collectable = Instantiate(collectable_prefab, pos, Quaternion.identity);
    collectables.Add(collectable);
  }

  /// <summary> Set the desired direction and offset based on showdef. </summary>
  private void setChildPath(string typ, string child) {
    dirPosStack.Add((currentDirection, spawnPoint)); 
    string curdir = SceneMaps.dir2str[currentDirection];
    (string category, string node) = catNodeStack[catNodeStack.Count-1];

    if (!showdef.categoryNodeMap[category].nodes[node].children.ContainsKey(typ + "_" + child))
      return;

    Child childClass = showdef.categoryNodeMap[category].nodes[node].children[typ + "_" + child];
    if (childClass.relativeDirection == Child_s.noChangeKeyword) return;

    curdir = setNewDirection(curdir, childClass);
    setNewPosition(curdir, childClass);
  }

  /// <summary> Set the new direction based on previous and showdef relative dir. </summary>
  private string setNewDirection(string curdir, Child childClass) {
    string newdir = SceneMaps.relDirMap[(curdir, childClass.relativeDirection)];
    currentDirection = SceneMaps.str2dir[newdir];
    curdir = SceneMaps.dir2str[currentDirection];
    return curdir;
  }

  /// <summary> Set the new position based on the previous and showdef relative offset. </summary>
  private void setNewPosition(string curdir, Child childClass) {
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
    spawnPoint = getOpenPos(spawnPoint, currentDirection);
  }

  /// <summary> Get an open Blocky spot, in the current direction. </summary>
  private Vector3 getOpenPos(Vector3 pos, Vector3 dir) {
    pos += dir * Blocky_s.SIZE;
    while (spawns.Contains(pos)) pos += dir * Blocky_s.SIZE;
    return pos;
  }

  /// <summary> Pop the direction and position from the stack and restore them. </summary>
  private void backtrack() {
    (Vector3Int oldDirection, Vector3 oldPosition) = dirPosStack[dirPosStack.Count-1];
    currentDirection = oldDirection;
    spawnPoint = oldPosition;
    dirPosStack.RemoveAt(dirPosStack.Count-1); 
  }
  #endregion // Spawning
}
