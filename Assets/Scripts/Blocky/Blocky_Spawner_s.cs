using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocky_Spawner_s : MonoBehaviour
{
  [SerializeField] private GameObject blocky_prefab;
  // [SerializeField] private int direction;
  private List<GameObject> blockyList = new List<GameObject>();
  private Vector3 offset;
  
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
  void Start() {
    Blocky_s.SIZE = 5;
    offset = new Vector3(Blocky_s.SIZE+1, 0, 0);
    // DECL
    createBlockyFromString("[\n(-1, 0, 1 | white)\n(1, 0, 1 | white)\n(1, 0, -1 | white)\n(-1, 0, -1 | white)\n(-1, 1, 1 | white)\n(0, 1, 1 | white)\n(1, 1, 1 | white)\n(1, 1, 0 | white)\n(-1, 1, 0 | white)\n(-1, 1, -1 | white)\n(0, 1, -1 | white)\n(1, 1, -1 | white)\n(1, -1, -1 | white)\n(0, -1, -1 | white)\n(-1, -1, -1 | white)\n(-1, -1, 0 | white)\n(-1, -1, 1 | white)\n(0, -1, 1 | white)\n(1, -1, 1 | white)\n(1, -1, 0 | white)\n]");
    
    // ASS
    createBlockyFromString("[\n(-2, 1, 1 | white)\n(-1, 1, 1 | white)\n(0, 1, 1 | white)\n(2, 1, 1 | white)\n(-2, 1, -1 | white)\n(-1, 1, -1 | white)\n(0, 1, -1 | white)\n(2, 1, -1 | white)\n(-2, -1, -1 | white)\n(-1, -1, -1 | white)\n(0, -1, -1 | white)\n(2, -1, -1 | white)\n(-2, -1, 1 | white)\n(-1, -1, 1 | white)\n(0, -1, 1 | white)\n(2, -1, 1 | white)\n");
    // ADD
    createBlockyFromString("[\n(-1, 0, 0 | green)\n(0, 0, 1 | green)\n(1, 0, 0 | green)\n(0, 0, -1 | green)\n(0, 0, 2 | green)\n(-2, 0, 0 | green)\n(2, 0, 0 | green)\n(0, 0, -2 | green)\n(0, 0, 0 | green)\n(0, -1, 0 | green)\n(0, 2, 0 | green)\n");

    // IF
    createBlockyFromString("[\n(-2, 0, 0 | white)\n(-1, 0, 0 | white)\n(0, 0, 0 | white)\n(1, 0, 0 | white)\n(2, 0, 0 | white)\n(0, 0, -1 | white)\n(-1, -1, -1 | yellow)\n(1, -1, -1 | yellow)\n(-1, -2, -2 | yellow)\n(1, -2, -2 | yellow)\n(0, -2, -2 | yellow)\n(0, -1, -1 | white)\n]");
    // GT
    createBlockyFromString("[\n(1, 0, 0 | blue)\n(0, 1, 0 | blue)\n(-1, 2, 0 | blue)\n(0, -1, 0 | blue)\n(-1, -2, 0 | blue)\n");
    
    offset = new Vector3(3.6f, -1, -1);
    // ASS
    createBlockyFromString("[\n(-2, 1, 1 | white)\n(-1, 1, 1 | white)\n(0, 1, 1 | white)\n(2, 1, 1 | white)\n(-2, 1, -1 | white)\n(-1, 1, -1 | white)\n(0, 1, -1 | white)\n(2, 1, -1 | white)\n(-2, -1, -1 | white)\n(-1, -1, -1 | white)\n(0, -1, -1 | white)\n(2, -1, -1 | white)\n(-2, -1, 1 | white)\n(-1, -1, 1 | white)\n(0, -1, 1 | white)\n(2, -1, 1 | white)\n");
    // DIV
    offset = new Vector3(3, -1, -2);
    createBlockyFromString("[\n(-2, -2, 0 | magenta)\n(-1, -1, 0 | magenta)\n(0, 0, 0 | magenta)\n(1, 1, 0 | magenta)\n(2, 2, 0 | magenta)\n");
  
    offset = new Vector3(4.5f, 0, 0);
    // OUT
    createBlockyFromString("[\n(-2, 0, 0 | cyan)\n(2, 0, 0 | cyan)\n(-1, 1, 0 | cyan)\n(1, 1, 0 | cyan)\n(0, 2, 0 | cyan)\n(1, -1, 0 | cyan)\n(-1, -1, 0 | cyan)\n(0, -2, 0 | cyan)\n");
  
  }

  // Update is called once per frame
  void Update() {
    
  }

  void createBlockyFromString(string blockyString) {
    List<(Vector3Int, Color)> tilePositionsColors = new List<(Vector3Int, Color)>();
    string[] blockySplit = blockyString.Split('\n');
    char[] brackets = {'(', ')'};
    char[] whiteSpace = {' ', '\t'};

    for (int i = 1; i < blockySplit.Length-1; i++) {
      Debug.Log(blockySplit[i]);
      string[] tileSplit = blockySplit[i].Trim(brackets).Split('|');
      string position = tileSplit[0];
      string color = tileSplit[1].Trim(brackets).Trim(whiteSpace);

      string[] coords = position.Split(',');
      Vector3Int tilepos = new Vector3Int(int.Parse(coords[0]), int.Parse(coords[1]), int.Parse(coords[2]));
      Debug.Log(color);
      tilePositionsColors.Add((tilepos, Colours.StringToColor[color]));
    }

    GameObject blocky = Instantiate(blocky_prefab, Vector3.zero + blockyList.Count * offset, Quaternion.identity);
    blocky.GetComponent<Blocky_s>().setTilePositions(tilePositionsColors);
    blocky.GetComponent<Blocky_s>().spawnTiles();
    blockyList.Add(blocky);
  }
}
