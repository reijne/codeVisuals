using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocky_Spawner_s : MonoBehaviour
{
  public GameObject blocky_prefab;
  // Start is called before the first frame update
  void Start()
  {
    GameObject blocky_start = Object.Instantiate(blocky_prefab);
    blocky_start.GetComponent<Blocky_s>().setName("start");
    GameObject blocky_program = Object.Instantiate(blocky_prefab, new Vector3(0, 0, 1), Quaternion.identity);
    blocky_program.GetComponent<Blocky_s>().setName("program");
    GameObject blocky_end = Object.Instantiate(blocky_prefab, new Vector3(0, 0, 2), Quaternion.identity);
    blocky_end.GetComponent<Blocky_s>().setName("end");
    Debug.Log("done");
  }

  // Update is called once per frame
  void Update()
  {
    
  }
}
