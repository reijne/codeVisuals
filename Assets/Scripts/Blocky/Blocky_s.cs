using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using tile = (Vector3Int position, string color); 


public class Blocky_s : MonoBehaviour
{
  public static int SIZE = 3; // For spawning consistency, all the blockys are the same size i.e. static
  [SerializeField] GameObject tile_prefab;
  private List<GameObject> tiles = new List<GameObject>();
  public List<(Vector3Int, Color)> tilePosCols = new List<(Vector3Int, Color)>();
  
  /// <summary> Set the positions for the 3D tiles in the blocky. </summary>
  public void setTilePositions(List<(Vector3Int, Color)> tilePosCols) {
    this.tilePosCols = tilePosCols;
  }

  /// <summary> Spawn the 3D tiles from which the Blocky is made up, into the scene.  </summary>
  public void spawnTiles() {
    if (tiles.Count > 0) removeTiles();

    foreach ((Vector3Int tilepos, Color col) in tilePosCols) {
      spawnTile(tilepos, col);
    }
  }
  
  /// <summary> Spawn a 3D colored tile at the desired position. </summary>
  private void spawnTile(Vector3Int tilepos, Color col) {
    if (istileOutsideBlock(tilepos)) {
        Debug.LogError("Tile position: " + tilepos + " outside of block with size" + SIZE);
        return;
      }

      GameObject tile = GameObject.Instantiate(tile_prefab);
      tile.transform.SetParent(this.transform);
      tile.transform.position = this.transform.position + tilepos;
      tile.GetComponent<Renderer>().material.color = col;
      tiles.Add(tile);
  }

  /// <summary> Destroy the 3D gameobject tile on a specific position. </summary>
  private void despawnTile(Vector3Int tilepos) {
    foreach (GameObject tile in tiles) {
      if (tile.transform.position == tilepos) {
        tiles.Remove(tile);
        Destroy(tile);
        break;
      }
    }
  }

  /// <summary> Remove all the 3D tiles from the blocky </summary>
  public void removeTiles() {
    foreach (GameObject tile in tiles) {
      GameObject.Destroy(tile);
    }
  }

  /// <summary> Remove the list of positions for the 3D tiles. </summary>
  public void removeTilePositions() {
    tilePosCols = new List<(Vector3Int, Color)>();
  }

  /// <summary> Remove the 3D tiles that are outside the bounds of the blocky. </summary>
  public void removeTilesOutsideBlock() {
    List<(Vector3Int, Color)> inBounds = new List<(Vector3Int, Color)>();
    foreach ((Vector3Int tilepos, Color col) in tilePosCols) {
      if (istileOutsideBlock(tilepos)) continue;
      inBounds.Add((tilepos, col));
    }
    tilePosCols = inBounds;
    spawnTiles();
  }

  /// <summary> Return if the tile is outside of the blocky bounds. </summary>
  public bool istileOutsideBlock(Vector3Int tilepos) {
    int limit = Mathf.FloorToInt(SIZE / 2);
    return (Mathf.Abs(tilepos.x) > limit || 
            Mathf.Abs(tilepos.y) > limit ||
            Mathf.Abs(tilepos.z) > limit);
  }

  /// <summary> Get a textual representation of a Blocky. </summary>
  public string toString() {
    string textrep = "[";
    foreach ((Vector3Int tilePos, Color col) in tilePosCols) {
      textrep += "\n\t(" + tilePos.x.ToString() + ", " 
                         + tilePos.y.ToString() + ", " 
                         + tilePos.z.ToString() +  " | " 
                         + Colours.ColorToString[col] + ")"; 
    }
    return textrep + "\n]";
  }

  /// <summary> Add a new tile position to the list of the Blocky. </summary>
  public void addTile(Vector3Int tilepos, Color col) {
    if (containsTilepos(tilepos)) return;
    tilePosCols.Add((tilepos, col));
    spawnTile(tilepos, col);
  }

  /// <summary> Remove the tile at the selected position. </summary>
  public void removeTile(Vector3Int gridpos) {
    foreach ((Vector3Int tilepos, Color col) in tilePosCols) {
      if (tilepos == gridpos) {
        tilePosCols.Remove((tilepos, col));
        despawnTile(tilepos);
        break;
      }
    }
  }

  /// <summary> Return if the current position already contains a 3D tile. </summary>
  public bool containsTilepos(Vector3Int gridpos) {
    foreach ((Vector3Int tilepos, _) in tilePosCols) {
      if (tilepos == gridpos) return true;
    }
    return false;
  }

  /// <summary> Get the color of a 3D tile at a specified position, or default gray for no tile. </summary>
  public Color getColor(Vector3Int gridpos) {
    foreach ((Vector3Int tilepos, Color col) in tilePosCols) {
      if (tilepos == gridpos) return col;
    }
    return Color.gray;
  }
}