using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareHandler : MonoBehaviour
{
  public static int height = 0;
  public GameObject selectsquare;
  public GameObject blocky;
  public static List<GameObject> squares = new List<GameObject>();
  public static List<Vector3Int> aliveGridPositions = new List<Vector3Int>(); 
  public Canvas screen;
  private Vector3 origin;
  private int sizeMem;
  private int heightMem;

  private void Start() {
    spawnSelectSquares();
  }
  public void spawnSelectSquares() {
    clearSquares();
    float width = screen.GetComponent<RectTransform>().rect.width;
    // float fieldSize = width * 0.40f;
    // origin = new Vector3(0, -0.5f*width - 0.5f*fieldSize, 0);
    float tileSize = (width * 0.4f) / Blocky_Spawner_s.BLOCKSIZE; 
    float BorderedTileSize = tileSize * 0.95f;
    int radius = (int) (Blocky_Spawner_s.BLOCKSIZE-1) / 2;
    sizeMem = Blocky_Spawner_s.BLOCKSIZE;
    heightMem = height;

    Vector3 offset;
    for (int z = -radius; z <= radius; z++) {
      for (int x = -radius; x <= radius; x++) {
        offset = new Vector3(x*tileSize, z*tileSize, 0);
        GameObject ss = GameObject.Instantiate(selectsquare, this.transform.position+offset, Quaternion.identity);
        ss.transform.SetParent(this.transform);
        Vector3Int gridpos = new Vector3Int(x, height, z);
        SelectSquare_s sscomponent = ss.GetComponent<SelectSquare_s>();
        sscomponent.gridpos = gridpos;
        if (aliveGridPositions.Contains(gridpos)) sscomponent.clickSelectSquare();
        ss.GetComponent<RectTransform>().sizeDelta = new Vector2(BorderedTileSize, BorderedTileSize); 
        squares.Add(ss);
      }
    }
  }

  private void clearSquares() {
    if (squares.Count <= 0) return;

    foreach (GameObject ss in squares) {
      Destroy(ss);
    }
  }

  public void clearTiles() {
    if (aliveGridPositions.Count <= 0) return;
    
    aliveGridPositions = new List<Vector3Int>();
    spawnSelectSquares();
    spawnTiles();
  }

  private void clearOutOfBounds() {
    List<Vector3Int> inBoundsGridpos = new List<Vector3Int>();
    int radius = (Blocky_Spawner_s.BLOCKSIZE - 1) / 2;
    
    foreach (Vector3Int gridpos in aliveGridPositions) {
      if (Mathf.Abs(gridpos.x) > radius || Mathf.Abs(gridpos.y) > radius || Mathf.Abs(gridpos.z) > radius) continue;
      inBoundsGridpos.Add(gridpos);
    }

    aliveGridPositions = inBoundsGridpos;
  }

  private void FixedUpdate() {
    if (sizeMem != Blocky_Spawner_s.BLOCKSIZE || heightMem != height) {
      if (sizeMem > Blocky_Spawner_s.BLOCKSIZE) {
        clearOutOfBounds();
        spawnTiles();
      }
      spawnSelectSquares();
    }
  }

  public void addGridPos(Vector3Int gridpos) {
    if (aliveGridPositions.Contains(gridpos)) return;
    
    aliveGridPositions.Add(gridpos);
    spawnTiles();
  }

  public void removeGridPos(Vector3Int gridpos) {
    aliveGridPositions.Remove(gridpos);
    spawnTiles();
  }

  public void spawnTiles() {
    blocky.GetComponent<Blocky_s>().setTilePositions(aliveGridPositions);
  }
}