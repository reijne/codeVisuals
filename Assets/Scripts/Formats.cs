using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Formats {
  using variableMap = System.Collections.Generic.Dictionary<string, string>;
  using Milestones = List<Stone>;
  [Serializable]
  public class Stone {
    public string snippet;
    public variableMap vmap;
  }
 
  public class MileStones {
    public List<Stone> milestoneList;
  }
}



