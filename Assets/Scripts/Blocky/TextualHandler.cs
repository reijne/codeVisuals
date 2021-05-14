using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextualHandler : MonoBehaviour
{
  public InputField textualRepresentation;
  private string textrep = "";
  private bool showTextual = false;
  public void toggleTextualRepresentation() {
    showTextual = !showTextual;
    
  }
  private void updateTextualRepresentation() {
    if (!showTextual) return;

    textrep = "[";
    foreach (Vector3Int gridpos in SquareHandler.aliveGridPositions) {
      textrep += "\n\t(" + gridpos.x.ToString() + ", " + gridpos.y.ToString() + ", " + gridpos.z.ToString() + ")"; 
    }
    textrep += "\n]";

    if (textrep != textualRepresentation.text) textualRepresentation.text = textrep;
  }

  private void FixedUpdate() {
    updateTextualRepresentation();
  }
}
