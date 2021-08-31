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
  
  /// <summary> Toggle the showing of textual Blocky. </summary>
  public void toggleTextualRepresentation() {
    showTextual = !showTextual;
  }

  /// <summary> Update the textual representation with the current Blocky state. </summary>
  private void updateTextualRepresentation() {
    if (!showTextual)  {
      textualRepresentation.text = "";
    }
    else if (blocky.toString() != textualRepresentation.text) {
      textualRepresentation.text = blocky.toString();
    }
  }

  /// <summary> Update the text and select it when it is focused. </summary>
  private void FixedUpdate() {
    updateTextualRepresentation();
    if (focusText) textualRepresentation.Select();
  }
}
