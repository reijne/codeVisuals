using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maps
{
  // Dictionary to cycle between the directions 
  public static Dictionary<string, string> genDirMap = new Dictionary<string, string>() {
    {"x", "y"},
    {"y", "z"},
    {"z", "x"}
  };

  // Dictionary to cycle between the camera modes
  public static Dictionary<string, string> camModeMap = new Dictionary<string, string>() {
    {"static", "kinematic"},
    {"kinematic", "user"},
    {"user", "static"}
  };

  // Dictionary to cycle between camera directions
  public static Dictionary<string, string> camDirMap = new Dictionary<string, string>() {
    {"N", "NE"},
    {"NE", "E"},
    {"E", "SE"},
    {"SE", "S"},
    {"S", "SW"},
    {"SW", "W"},
    {"W", "NW"},
    {"NW", "N"}
  };

  // Dictionary giving the position to the sign indicator for a given direction
  public static Dictionary<string, Vector3> signPositionMap = new Dictionary<string, Vector3>() {
    {"x", new Vector3(0.5f, 0f, 0f)},
    {"y", new Vector3(0f, 0.5f, 0f)},
    {"z", new Vector3(0f, 0f, 0.5f)},
  };

  // Dictionary with offsets for the directions when in user camera mode
  public static Dictionary<string, Vector3> camUserOffset = new Dictionary<string, Vector3>() {
    {"x", new Vector3(0, 1, 0)},
    {"y", new Vector3(0, 0, -1)},
    {"z", new Vector3(0, 1, 0)}
  };

  // Dictionary for the directions relative to the general direction
  public static Dictionary<(string, string), Vector3Int> relativeDirectionMap = new Dictionary<(string, string), Vector3Int>() {
    {("+x", "N"),  new Vector3Int(0, 1, 0)},
    {("+x", "NE"), new Vector3Int(0, 1, -1)},
    {("+x", "E"),  new Vector3Int(0, 0, -1)},
    {("+x", "SE"), new Vector3Int(0, -1, -1)},
    {("+x", "S"),  new Vector3Int(0, -1, 0)},
    {("+x", "SW"), new Vector3Int(0, -1, 1)},
    {("+x", "W"),  new Vector3Int(0, 0, 1)},
    {("+x", "NW"), new Vector3Int(0, 1, 1)},

    {("-x", "N"),  new Vector3Int(0, 1, 0)},
    {("-x", "NE"), new Vector3Int(0, 1, 1)},
    {("-x", "E"),  new Vector3Int(0, 0, 1)},
    {("-x", "SE"), new Vector3Int(0, -1, 1)},
    {("-x", "S"),  new Vector3Int(0, -1, 0)},
    {("-x", "SW"), new Vector3Int(0, -1, -1)},
    {("-x", "W"),  new Vector3Int(0, 0, -1)},
    {("-x", "NW"), new Vector3Int(0, 1, -1)},


    {("+y", "N"),  new Vector3Int(0, 0, -1)},
    {("+y", "NE"), new Vector3Int(1, 0, -1)},
    {("+y", "E"),  new Vector3Int(1, 0, 0)},
    {("+y", "SE"), new Vector3Int(1, 0, 1)},
    {("+y", "S"),  new Vector3Int(0, 0, 1)},
    {("+y", "SW"), new Vector3Int(-1, 0, 1)},
    {("+y", "W"),  new Vector3Int(-1, 0, 0)},
    {("+y", "NW"), new Vector3Int(-1, 0, -1)},

    {("-y", "N"),  new Vector3Int(0, 0, -1)},
    {("-y", "NE"), new Vector3Int(-1, 0, -1)},
    {("-y", "E"),  new Vector3Int(-1, 0, 0)},
    {("-y", "SE"), new Vector3Int(-1, 0, 1)},
    {("-y", "S"),  new Vector3Int(0, 0, 1)},
    {("-y", "SW"), new Vector3Int(1, 0, 1)},
    {("-y", "W"),  new Vector3Int(1, 0, 0)},
    {("-y", "NW"), new Vector3Int(1, 0, -1)},


    {("+z", "N"),  new Vector3Int(0, 1, 0)},
    {("+z", "NE"), new Vector3Int(1, 1, 0)},
    {("+z", "E"),  new Vector3Int(1, 0, 0)},
    {("+z", "SE"), new Vector3Int(1, -1, 0)},
    {("+z", "S"),  new Vector3Int(0, -1, 0)},
    {("+z", "SW"), new Vector3Int(-1, -1, 0)},
    {("+z", "W"),  new Vector3Int(-1, 0, 0)},
    {("+z", "NW"), new Vector3Int(-1, 1, 0)},

    {("-z", "N"),  new Vector3Int(0, 1, 0)},
    {("-z", "NE"), new Vector3Int(-1, 1, 0)},
    {("-z", "E"),  new Vector3Int(-1, 0, 0)},
    {("-z", "SE"), new Vector3Int(-1, -1, 0)},
    {("-z", "S"),  new Vector3Int(0, -1, 0)},
    {("-z", "SW"), new Vector3Int(1, -1, 0)},
    {("-z", "W"),  new Vector3Int(1, 0, 0)},
    {("-z", "NW"), new Vector3Int(1, 1, 0)},
  };
}
