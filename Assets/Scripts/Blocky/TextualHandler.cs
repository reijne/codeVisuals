using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextualHandler : MonoBehaviour
{
  private void Start() {
    // Tile guan = new Tile("((1,1,1) | red)");
  }
  [SerializeField] private InputField textualRepresentation;
  [SerializeField] private Blocky_s blocky;
  private bool showTextual = false;
  public void toggleTextualRepresentation() {
    showTextual = !showTextual;
  }
  private void updateTextualRepresentation() {
    if (!showTextual)  {
      textualRepresentation.text = "";
    }
    else if (blocky.toString() != textualRepresentation.text) {
      textualRepresentation.text = blocky.toString();
    }
  }

  private void FixedUpdate() {
    updateTextualRepresentation();
  }
}
