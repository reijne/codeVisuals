using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController_s : MonoBehaviour
{
  [SerializeField] Interaction playerInteraction;
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
    
  }

  public void updateErrors(string errors) {
    Debug.Log("updateing the errors");
    sceneSpawner.clearEnemies();
    sceneSpawner.parseErrors(errors);
    playerInteraction.resetHealth();
    Movement.doInput = true;
  }
}
// movement
// errorenemies