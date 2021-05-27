using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Blocky_Spawner_s : MonoBehaviour
{
  [SerializeField] private GameObject blocky_prefab;

  // [SerializeField] private int direction;
  private static Dictionary<string, Vector3Int> dirMap = new Dictionary<string, Vector3Int>() {
    {"+x", new Vector3Int(1, 0, 0)},
    {"-x", new Vector3Int(-1, 0, 0)},
    {"+y", new Vector3Int(0, 1, 0)},
    {"-y", new Vector3Int(0, -1, 0)},
    {"+z", new Vector3Int(0, 0, 1)},
    {"-z", new Vector3Int(0, 0, -1)},
  };
  private static Dictionary<(string, string), string> relDirMap = new Dictionary<(string, string), string>() {
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
  private static List<string> relativeDirections = new List<string>() {"left", "right", "up", "down"};
  private static Dictionary<string, string> blockyDefMap = new Dictionary<string, string>();
  private static int GAPSIZE = 0;
  private List<GameObject> blockyList = new List<GameObject>();
  private List<Vector3> blockyPositions = new List<Vector3>();
  private string generalDirectionString;
  private string relativeDirectionString;
  private Vector3Int generalDirection;
  private Vector3Int relativeDirection;
  private int generalCount;
  private int relativeCount;
  private Vector3 generalOrigin = Vector3.zero;
  private Vector3 relativeOrigin = Vector3.zero;
  private bool relative = false;
  
  /* HARDCODED example for visualising:
      number small
      number big

      small = input()
      big = input()

      if (big > 2*small)
        output("really big")
      else 
        output("quite big")
      end if
  */
  public void setDirection(string dir) {
    dir = dir.Trim();
    
    if (!dirMap.ContainsKey(dir)) {
      Debug.LogError("Direction string: " + dir + " is not a valid direction string, format [+-][xyz], ex: +x\n Taken example direction instead");
      dir = "+x";
      return;
    }

    generalDirectionString = dir;
    generalDirection = dirMap[generalDirectionString];
    relative = false;
    if (relativeDirectionString == null) relativeDirectionString = generalDirectionString;
  }

  public void setRelativeDirection(string relDir) {
    if (!relativeDirections.Contains(relDir)) Debug.LogError("Relative direction: " + relDir + " is not a valid relative direction, [left, right, up, down]");

    relativeDirectionString= relDirMap[(relativeDirectionString, relDir)];

    relativeDirection = dirMap[relativeDirectionString];
    relativeCount = 1;
    relativeOrigin = blockyPositions[blockyPositions.Count-1];
    relative = true;
  }

  public void setBlockyDefs(string blockydefs) {
    // Parse this juicy string and form the definitions 0-0
    // So for .. to ..
    // blockyDefMap["Name"] = "blockyTextualRepresentation"
    blockyDefMap["DECL"] = "[\n(-1, 0, 1 | white)\n(1, 0, 1 | white)\n(1, 0, -1 | white)\n(-1, 0, -1 | white)\n(-1, 1, 1 | white)\n(0, 1, 1 | white)\n(1, 1, 1 | white)\n(1, 1, 0 | white)\n(-1, 1, 0 | white)\n(-1, 1, -1 | white)\n(0, 1, -1 | white)\n(1, 1, -1 | white)\n(1, -1, -1 | white)\n(0, -1, -1 | white)\n(-1, -1, -1 | white)\n(-1, -1, 0 | white)\n(-1, -1, 1 | white)\n(0, -1, 1 | white)\n(1, -1, 1 | white)\n(1, -1, 0 | white)\n]";
    blockyDefMap["ASS"] = "[\n(-1, -1, -1 | white)\n(1, -1, -1 | white)\n(2, -1, -1 | white)\n(2, -1, 1 | white)\n(1, -1, 1 | white)\n(-1, -1, 1 | white)\n(2, 1, 1 | white)\n(1, 1, 1 | white)\n(-1, 1, 1 | white)\n(-1, 1, -1 | white)\n(1, 1, -1 | white)\n(2, 1, -1 | white)\n";
    blockyDefMap["ADD"] = "[\n(-1, 0, 0 | green)\n(0, 0, 1 | green)\n(1, 0, 0 | green)\n(0, 0, -1 | green)\n(0, 0, 2 | green)\n(-2, 0, 0 | green)\n(2, 0, 0 | green)\n(0, 0, -2 | green)\n(0, 0, 0 | green)\n(0, -1, 0 | green)\n";
    blockyDefMap["IF"] = "[\n(-2, 0, 0 | white)\n(-1, 0, 0 | white)\n(0, 0, 0 | white)\n(1, 0, 0 | white)\n(2, 0, 0 | white)\n(0, 0, -1 | white)\n(-1, -1, -1 | yellow)\n(1, -1, -1 | yellow)\n(-1, -2, -2 | yellow)\n(1, -2, -2 | yellow)\n(0, -2, -2 | yellow)\n(0, -1, -1 | white)\n]";
    blockyDefMap["GT"] = "[\n(1, 0, 0 | blue)\n(0, 1, 0 | blue)\n(-1, 2, 0 | blue)\n(0, -1, 0 | blue)\n(-1, -2, 0 | blue)\n";
    blockyDefMap["DIV"] = "[\n(-2, -2, 0 | magenta)\n(-1, -1, 0 | magenta)\n(0, 0, 0 | magenta)\n(1, 1, 0 | magenta)\n(2, 2, 0 | magenta)\n";
    blockyDefMap["OUT"] = "[\n(-2, 0, 0 | cyan)\n(2, 0, 0 | cyan)\n(-1, 1, 0 | cyan)\n(1, 1, 0 | cyan)\n(0, 2, 0 | cyan)\n(1, -1, 0 | cyan)\n(-1, -1, 0 | cyan)\n(0, -2, 0 | cyan)\n";
  
  }

  void Start() {
    Blocky_s.SIZE = 5;
    setDirection("-x");
    setBlockyDefs("Definitions of blockys go here in JSON or somethin lol");
    createBlockyFromString(blockyDefMap["DECL"], getNextPosition());
    createBlockyFromString(blockyDefMap["DECL"], getNextPosition());
    createBlockyFromString(blockyDefMap["DECL"], getNextPosition());
    createBlockyFromString(blockyDefMap["DECL"], getNextPosition());
    createBlockyFromString(blockyDefMap["DECL"], getNextPosition());

    setRelativeDirection("up");
    createBlockyFromString(blockyDefMap["DECL"], getNextPosition());
    createBlockyFromString(blockyDefMap["DECL"], getNextPosition());
    createBlockyFromString(blockyDefMap["DECL"], getNextPosition());

    relative = false;
    createBlockyFromString(blockyDefMap["DECL"], getNextPosition());
    createBlockyFromString(blockyDefMap["DECL"], getNextPosition());
    createBlockyFromString(blockyDefMap["DECL"], getNextPosition());

    setRelativeDirection("right");
    createBlockyFromString(blockyDefMap["DECL"], getNextPosition());
    createBlockyFromString(blockyDefMap["DECL"], getNextPosition());
    createBlockyFromString(blockyDefMap["DECL"], getNextPosition());

    setRelativeDirection("left");
    createBlockyFromString(blockyDefMap["DECL"], getNextPosition());
    createBlockyFromString(blockyDefMap["DECL"], getNextPosition());
    createBlockyFromString(blockyDefMap["DECL"], getNextPosition());
    
    setRelativeDirection("down");
    createBlockyFromString(blockyDefMap["DECL"], getNextPosition());
    createBlockyFromString(blockyDefMap["DECL"], getNextPosition());
    createBlockyFromString(blockyDefMap["DECL"], getNextPosition());

  }

  // Update is called once per frame
  void Update() {
    
  }

  Vector3 getNextPosition() {
    Vector3 pos;
    if (relative) {
      pos = relativeOrigin + (Blocky_s.SIZE + GAPSIZE) * relativeCount * relativeDirection;
      relativeCount++;
    } else {
      pos = generalOrigin + (Blocky_s.SIZE + GAPSIZE) * generalCount * generalDirection;
      generalCount++;
    }
    return pos;
  }

  void createBlockyFromString(string blockyString, Vector3 blockpos) {
    List<(Vector3Int, Color)> tilePositionsColors = new List<(Vector3Int, Color)>();
    string[] blockySplit = blockyString.Split('\n');
    char[] brackets = {'(', ')'};
    char[] whiteSpace = {' ', '\t'};

    for (int i = 1; i < blockySplit.Length-1; i++) {
      string[] tileSplit = blockySplit[i].Trim(brackets).Split('|');
      string position = tileSplit[0];
      string color = tileSplit[1].Trim(brackets).Trim(whiteSpace);

      string[] coords = position.Split(',');
      Vector3Int tilepos = new Vector3Int(int.Parse(coords[0]), int.Parse(coords[1]), int.Parse(coords[2]));
      tilePositionsColors.Add((tilepos, Colours.StringToColor[color]));
    }

    // if (blockyPositions.Contains(blockpos)) Debug.LogError("Overlapping Blockys detected at position: " + blockpos.ToString());

    GameObject blocky = Instantiate(blocky_prefab, blockpos, Quaternion.identity);
    blocky.GetComponent<Blocky_s>().setTilePositions(tilePositionsColors);
    blocky.GetComponent<Blocky_s>().spawnTiles();
    blockyList.Add(blocky);
    blockyPositions.Add(blockpos);
  }

  void clearBlockys() {foreach (GameObject blocky in blockyList) { Destroy(blocky); }}
}
