using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController_s : MonoBehaviour
{
  [SerializeField] GameObject blocky_prefab;
  private ShoweyDefinition showdef;
  private List<(string, string)> catNodeStack = new List<(string, string)>();
  private List<(Vector3Int, Vector3)> dirPosStack = new List<(Vector3Int, Vector3)>();
  private Vector3 spawnPoint = Vector3Int.zero;
  // private Vector3Int generalDirection;
  private Vector3Int currentDirection;

  public void init() {
    currentDirection = SceneMaps.str2dir[showdef.vars.sign + showdef.vars.genDir];
    //  = generalDirection;
    Blocky_s.SIZE = showdef.vars.blockySize;
  }

  private void parseLabels(string labels) {
    string[] labelList = labels.Split('\n');
    foreach (string label in labelList) {
      if (label[0] == '_') {
        // Debug.Log("child: " + label);
        // Child label
        string[] parts = label.Split('_');
        string operation = parts[1];
        string typ = parts[2];
        string child = parts[3];
        if (operation == "in") {
          setChildPath(typ, child);
        } else if (operation == "out") {
          backtrack();
          // pop the direction and position from the stack and set them to the current 
        }
      } else {
        // Node label
        string[] parts = label.Trim().Split('_');
        string operation = parts[0];
        string category = parts[1];
        string node = parts[2];
        if (operation == "in") {
          spawnNode(category, node);
          // Debug.Log("adding" + (category, node));
          catNodeStack.Add((category, node));
        } else if (operation == "out") {
          catNodeStack.RemoveAt(catNodeStack.Count-1);
          // Debug.Log("removing" + (category, node));
        }

      }
    }
  }

  private void spawnNode(string category, string node) {
    string blockyName = showdef.categoryNodeMap[category].nodes[node].blockyName;
    if (blockyName == Node_s.skipKeyword) return;
    Debug.Log("Spawnpoint before adding:  @" + spawnPoint.x + spawnPoint.y + spawnPoint.z);
    spawnPoint += currentDirection * Blocky_s.SIZE;
    GameObject blockyInstance = Instantiate(blocky_prefab, spawnPoint, Quaternion.identity);
    Blocky_s blockyScript = blockyInstance.GetComponent<Blocky_s>();
    blockyScript.setTilePositions(showdef.blockyMap[blockyName]);
    blockyScript.spawnTiles();
    Debug.Log("SPAWNED :: " + blockyName + " @" + spawnPoint.x + spawnPoint.y + spawnPoint.z);
  }

  private void setChildPath(string typ, string child) {
    // Store the current direction and position
    dirPosStack.Add((currentDirection, spawnPoint));
    Debug.Log("Spawnpoint added to stack: " + spawnPoint.x + spawnPoint.y + spawnPoint.z);
    
    // Gather the new direction 
    string curdir = SceneMaps.dir2str[currentDirection];
    (string category, string node) = catNodeStack[catNodeStack.Count-1];

    if (!showdef.categoryNodeMap[category].nodes[node].children.ContainsKey(typ + "_" + child)) {
      return;
    }
    string reldir = showdef.categoryNodeMap[category].nodes[node].children[typ + "_" + child].relativeDirection;
    
    if (reldir == Child_s.noChangeKeyword) return;
    
    // Set the new direction
    string newdir = SceneMaps.relDirMap[(curdir, reldir)];
    currentDirection = SceneMaps.str2dir[newdir];
    
    // set the new position
    // LR x, UD y, BF z
  }

  private void backtrack() {
    (Vector3Int oldDirection, Vector3 oldPosition) = dirPosStack[dirPosStack.Count-1];
    currentDirection = oldDirection;
    spawnPoint = oldPosition;
    Debug.Log("Set spawnpoint back to: " + spawnPoint.x + spawnPoint.y + spawnPoint.z);
    dirPosStack.RemoveAt(dirPosStack.Count-1); 
  }

  private void Start() {
    showdef = ShoweyDefinition.fromSerialise("{\"vars\":{\"sign\":\"+\",\"genDir\":\"x\",\"camMode\":\"user\",\"camDir\":\"NE\",\"blockySize\":3},\"blockyDefinitions\":[{\"name\":\"FUNC\",\"tilePositions\":[{\"x\":-1,\"y\":0,\"z\":-1},{\"x\":-1,\"y\":0,\"z\":0},{\"x\":-1,\"y\":0,\"z\":1},{\"x\":0,\"y\":0,\"z\":1},{\"x\":1,\"y\":0,\"z\":1},{\"x\":0,\"y\":0,\"z\":0},{\"x\":0,\"y\":0,\"z\":-1},{\"x\":1,\"y\":0,\"z\":0},{\"x\":1,\"y\":0,\"z\":-1}],\"tileColors\":[{\"r\":1.0,\"g\":1.0,\"b\":1.0,\"a\":1.0},{\"r\":1.0,\"g\":1.0,\"b\":1.0,\"a\":1.0},{\"r\":1.0,\"g\":1.0,\"b\":1.0,\"a\":1.0},{\"r\":1.0,\"g\":1.0,\"b\":1.0,\"a\":1.0},{\"r\":1.0,\"g\":1.0,\"b\":1.0,\"a\":1.0},{\"r\":0.0,\"g\":1.0,\"b\":1.0,\"a\":1.0},{\"r\":1.0,\"g\":1.0,\"b\":1.0,\"a\":1.0},{\"r\":1.0,\"g\":1.0,\"b\":1.0,\"a\":1.0},{\"r\":1.0,\"g\":1.0,\"b\":1.0,\"a\":1.0}]},{\"name\":\"PLUS\",\"tilePositions\":[{\"x\":0,\"y\":0,\"z\":0},{\"x\":-1,\"y\":0,\"z\":0},{\"x\":0,\"y\":0,\"z\":-1},{\"x\":1,\"y\":0,\"z\":0},{\"x\":0,\"y\":0,\"z\":1}],\"tileColors\":[{\"r\":0.0,\"g\":1.0,\"b\":0.0,\"a\":1.0},{\"r\":0.0,\"g\":1.0,\"b\":0.0,\"a\":1.0},{\"r\":0.0,\"g\":1.0,\"b\":0.0,\"a\":1.0},{\"r\":0.0,\"g\":1.0,\"b\":0.0,\"a\":1.0},{\"r\":0.0,\"g\":1.0,\"b\":0.0,\"a\":1.0}]},{\"name\":\"I\",\"tilePositions\":[{\"x\":0,\"y\":0,\"z\":0},{\"x\":0,\"y\":-1,\"z\":0},{\"x\":0,\"y\":1,\"z\":0}],\"tileColors\":[{\"r\":1.0,\"g\":0.0,\"b\":0.0,\"a\":1.0},{\"r\":1.0,\"g\":0.0,\"b\":0.0,\"a\":1.0},{\"r\":1.0,\"g\":0.0,\"b\":0.0,\"a\":1.0}]}],\"categoryList\":[{\"categoryName\":\"Program\",\"nodes\":[{\"nodeName\":\"program\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Stmt_statements\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]}]},{\"categoryName\":\"Parameter\",\"nodes\":[{\"nodeName\":\"parameter\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"str_datatype\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"str_id\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]}]},{\"categoryName\":\"Block\",\"nodes\":[{\"nodeName\":\"block\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Stmt_statements\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]}]},{\"categoryName\":\"Expr\",\"nodes\":[{\"nodeName\":\"funCall\",\"blockyName\":\"FUNC\",\"children\":[{\"childName\":\"str_id\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Expr_args\",\"relativeDirection\":\"left\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"divExpr\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Expr_lhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Expr_rhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"eqExpr\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Expr_lhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Expr_rhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"boolExpr\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Boolean_boolean\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"modExpr\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Expr_lhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Expr_rhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"idExpr\",\"blockyName\":\"I\",\"children\":[{\"childName\":\"str_id\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"minExpr\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Expr_lhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Expr_rhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"gtExpr\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Expr_lhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Expr_rhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"powExpr\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Expr_lhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Expr_rhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"gteExpr\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Expr_lhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Expr_rhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"andExpr\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Expr_lhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Expr_rhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"addExpr\",\"blockyName\":\"PLUS\",\"children\":[{\"childName\":\"Expr_lhs\",\"relativeDirection\":\"left\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Expr_rhs\",\"relativeDirection\":\"right\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"numExpr\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"int_number\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"lteExpr\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Expr_lhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Expr_rhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"listExpr\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Expr_items\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"ltExpr\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Expr_lhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Expr_rhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"strExpr\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"str_string\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"mulExpr\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Expr_lhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Expr_rhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"notExpr\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Expr_expr\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"orExpr\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Expr_lhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Expr_rhs\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]}]},{\"categoryName\":\"Type\",\"nodes\":[{\"nodeName\":\"t_list\",\"blockyName\":\"skip\",\"children\":[]},{\"nodeName\":\"t_bool\",\"blockyName\":\"skip\",\"children\":[]},{\"nodeName\":\"t_str\",\"blockyName\":\"skip\",\"children\":[]},{\"nodeName\":\"t_num\",\"blockyName\":\"skip\",\"children\":[]}]},{\"categoryName\":\"Boolean\",\"nodes\":[{\"nodeName\":\"b_true\",\"blockyName\":\"skip\",\"children\":[]},{\"nodeName\":\"b_false\",\"blockyName\":\"skip\",\"children\":[]}]},{\"categoryName\":\"Stmt\",\"nodes\":[{\"nodeName\":\"whileStmt\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Expr_cond\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Block_block\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"decl\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Type_datatype\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"str_id\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"returnStmt\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Expr_expr\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"exprStmt\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Expr_expr\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"ifStmt\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Expr_cond\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Block_block\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"funDef\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Type_datatype\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"str_id\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Parameter_parameters\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Block_block\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"outputStmt\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Expr_expr\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"inputStmt\",\"blockyName\":\"skip\",\"children\":[]},{\"nodeName\":\"ifElseStmt\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"Expr_cond\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Block_thenBlock\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Block_elseBlock\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"assStmt\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"str_id\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Expr_expr\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]},{\"nodeName\":\"repeatStmt\",\"blockyName\":\"skip\",\"children\":[{\"childName\":\"int_iter\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}},{\"childName\":\"Block_block\",\"relativeDirection\":\"noChange\",\"offset\":{\"x\":0,\"y\":0,\"z\":0}}]}]}]}");

    init();
    parseLabels("in_Program_program\n_in_Stmt_statements\n in_Stmt_exprStmt\n_in_Expr_expr\nin_Expr_funCall\n_in_Expr_args\nin_Expr_idExpr\nout_Expr_idExpr\n_out_Expr_args\nout_Expr_funCall\n_out_Expr_expr\nout_Stmt_exprStmt\n in_Stmt_exprStmt\n_in_Expr_expr\nin_Expr_funCall\n_in_Expr_args\nin_Expr_idExpr\nout_Expr_idExpr\n_out_Expr_args\nout_Expr_funCall\n_out_Expr_expr\nout_Stmt_exprStmt\n_out_Stmt_statements\nout_Program_program");
 }
}
