using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneController_s : MonoBehaviour
{
  [SerializeField] Interaction playerInteraction;
  [SerializeField] UserInterface_s userInterface;
  [SerializeField] Movement playerMovement;
  [SerializeField] SceneSpawner_s sceneSpawner;

  /// <summary> Create a new Sceney instance using the showeydefintion. </summary>
  public void createSceney(string showeyJSON) {
    sceneSpawner.initFromJSON(showeyJSON);
  }

  // TODO add a movement type parser and set this in Player>Movement

  /// <summary> Update the Sceney instance to show the labeled traversal visually. </summary>
  public void updateSceney(string labeledTraversal) {
    Debug.Log("updateing the scene");
    sceneSpawner.clearScene();
    sceneSpawner.parseLabeledTraversal(labeledTraversal);
    playerMovement.setMovementType(Movement.MovementType.flying);
  }

  public void updateErrors(string errors) {
    Debug.Log("updateing the errors");
    playerInteraction.interType = Interaction.interactionType.throwing;
    playerMovement.setMovementType(Movement.MovementType.running);
    sceneSpawner.clearEnemies();
    sceneSpawner.parseErrors(errors);
    playerInteraction.resetHealth();
    Movement.doInput = true;
    userInterface.setIcon("heart");
  }

  public void updateBranches(string branches) {
    Debug.Log("updateing the branches");
    playerInteraction.interType = Interaction.interactionType.shooting;
    playerMovement.setMovementType(Movement.MovementType.running);
    sceneSpawner.clearCollectables();
    sceneSpawner.parseBranches(branches);
    userInterface.setIcon("collectable");
  }
}
// movement
// errorenemies