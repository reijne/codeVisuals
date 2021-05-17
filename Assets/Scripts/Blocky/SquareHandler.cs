using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareHandler : MonoBehaviour
{
  public static int height = 0;
  public static List<GameObject> squares = new List<GameObject>();
  public static Color currentColor = Color.white;
  [SerializeField] private GameObject selectsquare_prefab;
  [SerializeField] private Blocky_s blocky;
  [SerializeField] private Canvas screen;
  private Vector3 origin;
  private int sizeMem;
  private int heightMem;

  private void Start() {
    spawnSelectSquares();
  }
  public void spawnSelectSquares() {
    clearSquares();
    float width = Screen.width;
    float tileSize = (width * 0.4f) / Blocky_s.SIZE; 
    float BorderedTileSize = tileSize * 0.95f;
    int radius = (int) (Blocky_s.SIZE-1) / 2;
    sizeMem = Blocky_s.SIZE;
    heightMem = height;

    Vector3 offset; // TODO clean this shit up pleasae
    for (int z = -radius; z <= radius; z++) {
      for (int x = -radius; x <= radius; x++) {
        offset = new Vector3(x*tileSize, z*tileSize, 0);
        GameObject ss = GameObject.Instantiate(selectsquare_prefab, this.transform.position+offset, Quaternion.identity);
        ss.transform.SetParent(this.transform);
        Vector3Int gridpos = new Vector3Int(x, height, z);
        SelectSquare_s sscomponent = ss.GetComponent<SelectSquare_s>();
        sscomponent.init();
        sscomponent.gridpos = gridpos;
        Color squareColor = blocky.getColor(gridpos);
        if (squareColor != Color.gray) {
          sscomponent.desiredColor = squareColor;
          sscomponent.clickSelectSquare();
        } 
        ss.GetComponent<RectTransform>().sizeDelta = new Vector2(BorderedTileSize, BorderedTileSize); 
        squares.Add(ss);
      }
    }
  }

  private void clearSquares() {
    foreach (GameObject ss in squares) {
      Destroy(ss);
    }
  }

  public void clearTiles() {
    blocky.removeTiles();
    blocky.removeTilePositions();
    spawnSelectSquares();
  }

  private void FixedUpdate() {
    if (sizeMem != Blocky_s.SIZE || heightMem != height) {
      if (sizeMem > Blocky_s.SIZE) {
        blocky.removeTilesOutsideBlock();
        blocky.spawnTiles();
      }
      spawnSelectSquares();
    }
  }

  public void addGridPos(Vector3Int gridpos, Color col) {
    blocky.addTile(gridpos, col);
    blocky.spawnTiles(true);
  }
  public void removeGridPos(Vector3Int gridpos) {
    blocky.removeTile(gridpos);
    blocky.spawnTiles(true);
  }
}