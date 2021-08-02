using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMaps {
  /// <summary> Takes a direction string and gives back a directional vector.  </summary>
  public static Dictionary<string, Vector3Int> str2dir = new Dictionary<string, Vector3Int>() {
    {"+x", new Vector3Int(1, 0, 0)},
    {"-x", new Vector3Int(-1, 0, 0)},
    {"+y", new Vector3Int(0, 1, 0)},
    {"-y", new Vector3Int(0, -1, 0)},
    {"+z", new Vector3Int(0, 0, 1)},
    {"-z", new Vector3Int(0, 0, -1)},
  };

  /// <summary> Takes a directional vectory and gives back the string representation. </summary>
  public static Dictionary<Vector3Int, string> dir2str = new Dictionary<Vector3Int, string>() {
    {new Vector3Int(1, 0, 0), "+x"},
    {new Vector3Int(-1, 0, 0), "-x"},
    {new Vector3Int(0, 1, 0), "+y"},
    {new Vector3Int(0, -1, 0), "-y"},
    {new Vector3Int(0, 0, 1), "+z"},
    {new Vector3Int(0, 0, -1), "-z"},
  };

  /// <summary> Takes a tuple of string (current, relative) direction </summary>
  public static Dictionary<(string, string), string> relDirMap = new Dictionary<(string, string), string>() {
    {("+x", "left"), "+z"},
    {("+x", "right"), "-z"},
    {("+x", "up"), "+y"},
    {("+x", "down"), "-y"},
    {("-x", "left"), "-z"},
    {("-x", "right"), "+z"},
    {("-x", "up"), "+y"},
    {("-x", "down"), "-y"},

    {("+y", "left"), "+z"},
    {("+y", "right"), "-z"},
    {("+y", "up"), "-x"},
    {("+y", "down"), "+x"},
    {("-y", "left"), "-z"},
    {("-y", "right"), "+z"},
    {("-y", "up"), "+x"},
    {("-y", "down"), "-x"},

    {("+z", "left"), "-x"},
    {("+z", "right"), "+x"},
    {("+z", "up"), "+y"},
    {("+z", "down"), "-y"},
    {("-z", "left"), "+x"},
    {("-z", "right"), "-x"},
    {("-z", "up"), "+y"},
    {("-z", "down"), "-y"},
  };
}