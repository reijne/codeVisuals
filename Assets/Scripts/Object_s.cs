using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_s : MonoBehaviour
{
  private Material mat;

  private void Start() {
    mat = GetComponent<Renderer>().material;
    setColor("yellow");
    setSize(3);
  }
  public void setSize(float size) {
    this.transform.localScale = new Vector3(size, size, size);
  }

  public void setColor(string color) {
    if (color == "magenta") {
      mat.color = Color.magenta;
    } else if (color == "cyan") {
      mat.color = Color.cyan;
    } else if (color == "yellow") {
      mat.color = Color.yellow;
    } else {
      mat.color = Color.clear;
    }
  }
}
