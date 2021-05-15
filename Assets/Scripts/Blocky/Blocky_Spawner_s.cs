using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocky_Spawner_s : MonoBehaviour
{
  public GameObject blocky_prefab;
  
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
    List<Vector3Int> tilepos = new List<Vector3Int>();
    tilepos.Add(new Vector3Int(-1, 0, 1));
    tilepos.Add(new Vector3Int(-1, 0, -1));
    tilepos.Add(new Vector3Int(1, 0, -1));
    tilepos.Add(new Vector3Int(1, 0, 1));
    tilepos.Add(new Vector3Int(-1, 1, 1));
    tilepos.Add(new Vector3Int(1, 1, 1));
    tilepos.Add(new Vector3Int(-1, 1, -1));
    tilepos.Add(new Vector3Int(1, 1, -1));
    tilepos.Add(new Vector3Int(0, 1, 1));
    tilepos.Add(new Vector3Int(-1, 1, 0));
    tilepos.Add(new Vector3Int(1, 1, 0));
    tilepos.Add(new Vector3Int(0, 1, -1));
    tilepos.Add(new Vector3Int(-1, -1, -1));
    tilepos.Add(new Vector3Int(-1, -1, 0));
    tilepos.Add(new Vector3Int(-1, -1, 1));
    tilepos.Add(new Vector3Int(0, -1, 1));
    tilepos.Add(new Vector3Int(1, -1, 1));
    tilepos.Add(new Vector3Int(1, -1, 0));
    tilepos.Add(new Vector3Int(1, -1, -1));
    tilepos.Add(new Vector3Int(0, -1, -1));

    GameObject centre = GameObject.Instantiate(blocky_prefab, new Vector3(0,0,0), Quaternion.identity);
    Blocky_s decl = centre.GetComponent<Blocky_s>();
    decl.setTilePositions(tilepos);

    GameObject next = GameObject.Instantiate(blocky_prefab, new Vector3((Blocky_s.SIZE+1),0,0), Quaternion.identity);
    Blocky_s decl2 = next.GetComponent<Blocky_s>();
    decl2.setTilePositions(tilepos);

    List<Vector3Int> ass = new List<Vector3Int>();
	  ass.Add(new Vector3Int(1, 0, 1)); 
	  ass.Add(new Vector3Int(0, 0, 1)); 
	  ass.Add(new Vector3Int(-1, 0, 1));  
	  ass.Add(new Vector3Int(-1, 0, -1)); 
	  ass.Add(new Vector3Int(0, 0, -1));  
	  ass.Add(new Vector3Int(1, 0, -1)); 

    GameObject ass1 = GameObject.Instantiate(blocky_prefab, new Vector3(2*(Blocky_s.SIZE+1),0,0), Quaternion.identity);
    Blocky_s bass1 = ass1.GetComponent<Blocky_s>();
    bass1.setTilePositions(ass); 

    GameObject ass2 = GameObject.Instantiate(blocky_prefab, new Vector3(3*(Blocky_s.SIZE+1),0,0), Quaternion.identity);
    Blocky_s bass2 = ass2.GetComponent<Blocky_s>();
    bass2.setTilePositions(ass); 

    List<Vector3Int> tilepos2 = new List<Vector3Int>();
    tilepos2.Add(new Vector3Int(-2, 0, 0));
    tilepos2.Add(new Vector3Int(-1, 0, 0));
    tilepos2.Add(new Vector3Int(0, 0, 0));
    tilepos2.Add(new Vector3Int(0, 0, 1));
    tilepos2.Add(new Vector3Int(1, 1, 1));
    tilepos2.Add(new Vector3Int(2, 1, 1));
    tilepos2.Add(new Vector3Int(0, 0, -1));
    tilepos2.Add(new Vector3Int(1, -1, -1));
    tilepos2.Add(new Vector3Int(2, -1, -1));

    GameObject right = GameObject.Instantiate(blocky_prefab, new Vector3(4*(Blocky_s.SIZE+1),0,0), Quaternion.identity);
    Blocky_s b2 = right.GetComponent<Blocky_s>();
    b2.setTilePositions(tilepos2);

    List<Vector3Int> outy = new List<Vector3Int>();
    outy.Add(new Vector3Int(0, -2, 0));
	  outy.Add(new Vector3Int(-1, -1, 0));
	  outy.Add(new Vector3Int(0, -1, 1));
	  outy.Add(new Vector3Int(1, -1, 0));
	  outy.Add(new Vector3Int(0, -1, -1));
	  outy.Add(new Vector3Int(-2, 0, 0));
	  outy.Add(new Vector3Int(-1, 0, 1));
	  outy.Add(new Vector3Int(0, 0, 2));
	  outy.Add(new Vector3Int(1, 0, 1));
	  outy.Add(new Vector3Int(2, 0, 0));
	  outy.Add(new Vector3Int(1, 0, -1));
	  outy.Add(new Vector3Int(0, 0, -2));
	  outy.Add(new Vector3Int(-1, 0, -1));

    GameObject out1 = GameObject.Instantiate(blocky_prefab, new Vector3(5*(Blocky_s.SIZE+1),0,0), Quaternion.identity);
    Blocky_s bout1 = out1.GetComponent<Blocky_s>();
    bout1.setTilePositions(outy);

    GameObject out2 = GameObject.Instantiate(blocky_prefab, new Vector3(6*(Blocky_s.SIZE+1),0,0), Quaternion.identity);
    Blocky_s bout2 = out2.GetComponent<Blocky_s>();
    bout2.setTilePositions(outy);

  }

  // Update is called once per frame
  void Update() {
    
  }
}
