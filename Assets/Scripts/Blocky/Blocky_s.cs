using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using tile = (Vector3Int position, string color); 


public class Blocky_s : MonoBehaviour
{
  public static int SIZE; // For ease of use all the blockys are the same size i.e. static var
  private List<GameObject> tiles = new List<GameObject>();
  public List<Vector3Int> tilePositions = new List<Vector3Int>();
  
  public void setTilePositions(List<Vector3Int> tilePositions) {
    this.tilePositions = tilePositions;
    spawnTiles();
  }

  public void spawnTiles() {
    if (tiles.Count > 0) removeTiles();

    foreach (Vector3Int tilepos in tilePositions) {
      if (tileOutsideBlock(tilepos)) {
        Debug.LogError("Tile position: " + tilepos + " outside of block with size" + SIZE);
        continue;
      }
      GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
      tile.transform.SetParent(this.transform);
      tile.transform.position = this.transform.position + tilepos;
      tiles.Add(tile);
    }
  }

  public void removeTiles() {
    foreach (GameObject tile in tiles) {
      GameObject.Destroy(tile);
    }
  }

  public void removeTilePositions() {
    tilePositions = new List<Vector3Int>();
  }

  public bool tileOutsideBlock(Vector3Int tilepos) {
    int limit = Mathf.FloorToInt(SIZE / 2);
    return (Mathf.Abs(tilepos.x) > limit || 
            Mathf.Abs(tilepos.y) > limit ||
            Mathf.Abs(tilepos.z) > limit);
  }

  public string toString() {
    string textrep = "[";
    foreach (Vector3Int tilePos in tilePositions) {
      textrep += "\n\t(" + tilePos.x.ToString() + ", " + tilePos.y.ToString() + ", " + tilePos.z.ToString() + ")"; 
    }
    return textrep + "\n]";
  }

  public void addTile(Vector3Int tilepos) {
    if (tilePositions.Contains(tilepos)) return;
    tilePositions.Add(tilepos);
    spawnTiles();
  }

  public void removeTile(Vector3Int tilepos) {
    tilePositions.Remove(tilepos);
    spawnTiles();
  }
}

public class Tile {
  private Vector3Int position {get;}
  public Color color {get;}
  public Tile(Vector3Int position, Color color) {
    this.position = position;
    this.color = color;
  }

  public Tile(string posAndColor) {
    string[] split = posAndColor.Split('|');
    Debug.Log(split[0]);
    Debug.Log(split[1]);
    // (x, y, z | "c")
  }

  public string toString() {
    return "(" + position.x.ToString() + "," + position.y.ToString() + "," + position.z.ToString() 
          + "|" + Colours.ColorToString[color] + ")";
  }  
}