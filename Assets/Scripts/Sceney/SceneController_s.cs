using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneController_s : MonoBehaviour
{
  [SerializeField] Interaction playerInteraction;
  [SerializeField] UserInterface_s userInterface;
  [SerializeField] GameObject userInterfaceObject;
  [SerializeField] GameObject menu;
  [SerializeField] Movement playerMovement;
  [SerializeField] SceneSpawner_s sceneSpawner;
  private void Start() {
    sceneSpawner.initFromFile();
    updateSceney("in-Program-program\n-in-list[Stmt]-statements\nin-Stmt-decl\n-in-Type-datatype\nin-Type-t_num\r\n    out-Type-t_num\n-out-Type-datatype\nout-Stmt-decl\nin-Stmt-assStmt\n-in-Expr-expr\nin-Expr-inputExpr\r\n    out-Expr-inputExpr\n-out-Expr-expr\nout-Stmt-assStmt\nin-Stmt-ifElseStmt\n-in-Expr-cond\nin-Expr-modExpr\n-in-Expr-lhs\nin-Expr-idExpr\nout-Expr-idExpr\n-out-Expr-lhs\n-in-Expr-rhs\nin-Expr-numExpr\nout-Expr-numExpr\n-out-Expr-rhs\nout-Expr-modExpr\n-out-Expr-cond\n-in-list[Stmt]-thenBlock\nin-Stmt-outputStmt\n-in-Expr-expr\nin-Expr-boolExpr\n-in-Boolean-boolean\nin-Boolean-b_true\r\n    out-Boolean-b_true\n-out-Boolean-boolean\nout-Expr-boolExpr\n-out-Expr-expr\nout-Stmt-outputStmt\n-out-Stmt-thenBlock\n-in-list[Stmt]-elseBlock\nin-Stmt-outputStmt\n-in-Expr-expr\nin-Expr-boolExpr\n-in-Boolean-boolean\nin-Boolean-b_false\r\n    out-Boolean-b_false\n-out-Boolean-boolean\nout-Expr-boolExpr\n-out-Expr-expr\nout-Stmt-outputStmt\n-out-Stmt-elseBlock\nout-Stmt-ifElseStmt\n-out-Stmt-statements\nout-Program-program");
    // updateErrors("6|Expected a bool, got unexpected type\n4|Input expr not yet evaluated\n7|Modulo requires number arguments on both sides");
    updateBranches("10,11,12|12");
  }

  /// <summary> Create a new Sceney instance using the showeydefintion. </summary>
  public void createSceney(string showeyJSON) {
    sceneSpawner.initFromJSON(showeyJSON);
  }

  // TODO add a movement type parser and set this in Player>Movement

  /// <summary> Update the Sceney instance to show the labeled traversal visually. </summary>
  public void updateSceney(string labeledTraversal) {
    Debug.Log("updateing the scene");
    Debug.Log(labeledTraversal);
    Debug.Log("End of labeled traversal");
    sceneSpawner.clearScene();
    sceneSpawner.parseLabeledTraversal(labeledTraversal);
  }

  public void updateErrors(string errors) {
    Debug.Log("updateing the errors");
    Debug.Log(errors);
    Debug.Log("End of errors");
    playerInteraction.interType = Interaction.interactionType.throwing;
    setMovement();
    sceneSpawner.clearEnemies();
    sceneSpawner.parseErrors(errors);
    playerInteraction.resetHealth();
    Movement.doInput = true;
    userInterfaceObject.SetActive(true);
    userInterface.setIcon("heart");
  }

  public void updateBranches(string branches) {
    Debug.Log("updateing the branches");
    Debug.Log(branches);
    Debug.Log("end of branches");
    playerInteraction.interType = Interaction.interactionType.shooting;
    setMovement();
    sceneSpawner.clearCollectables();
    sceneSpawner.parseBranches(branches);
    userInterfaceObject.SetActive(true);
    userInterface.setIcon("collectable");
  }

  private void setMovement() {
    if (sceneSpawner.showdef.vars.camMode != "kinematic")
      playerMovement.setMovementType(Movement.MovementType.running);
  }

  public void quit() {
    Application.Quit();
  }

  public void resume() {
    menu.SetActive(false);
  }

  private void Update() {
    if (Input.GetKeyDown(KeyCode.Escape)) menu.SetActive(!menu.activeSelf);
    if (menu.activeSelf) Cursor.lockState = CursorLockMode.None;
    else Cursor.lockState = CursorLockMode.Locked;
  }
}
// movement
// errorenemies