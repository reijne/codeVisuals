using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Text_Blocky_s : MonoBehaviour
{
  public TextMeshPro textMesh;
  public Material mat;
  public Text variablesText;
  public string variablesSnapshot;
  private bool isHighlighted = false; 

  private void Start() {
    mat = GetComponent<Renderer>().material;
  }
  public void setName(string name) {
    textMesh.SetText(name);
  }

  public void setSize(float x, float y, float z) {
    this.transform.localScale += new Vector3(x, y, z);
  }

  public void setVariables(string vars) {
    variablesSnapshot = vars;
  }

  public void highlight(bool doHighlight) {
    isHighlighted = doHighlight;
    
    if (isHighlighted) {
      mat.color = Color.white;
      // Camera_s.target = this.gameObject;
    }
  }

  private void OnMouseDown() {
    highlight(!isHighlighted);
    variablesText.text = variablesSnapshot;
  }
}
