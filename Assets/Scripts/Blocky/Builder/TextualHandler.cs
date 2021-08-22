// UI element to show the textual representation of the designed Blocky.
// Author: Youri Reijne

using UnityEngine;
using UnityEngine.UI;

public class TextualHandler : MonoBehaviour
{
  [SerializeField] private InputField textualRepresentation;
  [SerializeField] private Blocky_s blocky;
  public static bool focusText = false;
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
    if (focusText) textualRepresentation.Select();
  }
}
