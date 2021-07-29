using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController_s : MonoBehaviour
{
  [SerializeField] Camera_s player;
  [SerializeField] SceneSpawner_s sceneSpawner;

  /// <summary> Create a new Sceney instance using the showeydefintion. </summary>
  public void createSceney(string showeyJSON) {
    sceneSpawner.initFromJSON(showeyJSON);
  }

  /// <summary> Update the Sceney instance to show the labeled traversal visually. </summary>
  public void updateSceney(string labeledTraversal) {
    sceneSpawner.clearScene();
    sceneSpawner.parseLabeledTraversal(labeledTraversal);
  }
}
