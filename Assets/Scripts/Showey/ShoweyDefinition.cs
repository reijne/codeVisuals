using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary> An entire Showey definition used within unity to map nodes and children to the 3D world. </summary>
public class ShoweyDefinition {
  public string sign = "+";
  public string genDir = "x";
  public string camMode = "user";
  public string camDir = "NE";
  public Dictionary<string, List<(Vector3Int, Color)>> blockyMap;
  public Dictionary<string, Category_D> categoryNodeMap = new Dictionary<string, Category_D>();

  
  /// <summary> From a JSON create the showey definition workable within unity. </summary>
  public static ShoweyDefinition fromSerialise(string json) {
    SeriShoweyDefinition serShow = JsonUtility.FromJson<SeriShoweyDefinition>(json);
    ShoweyDefinition showdef = new ShoweyDefinition();
    showdef.sign = serShow.sign;
    showdef.genDir = serShow.genDir;
    showdef.camMode = serShow.camMode;
    showdef.camDir = serShow.camDir;

    showdef.blockyMap = makeBlockyMap(serShow.blockyDefinitions);
    showdef.categoryNodeMap = makeCategoryMap(serShow.categoryList);
    return showdef;
  }
  
  /// <summary> Create a mapping from the blocky name to their coloured tiles they 
  private static Dictionary<string, List<(Vector3Int, Color)>> makeBlockyMap(List<BlockyDefinition> blockyDefinitions) {
    Dictionary<string, List<(Vector3Int, Color)>> blockyMap = new Dictionary<string, List<(Vector3Int, Color)>>();
    foreach (BlockyDefinition blockydef in blockyDefinitions) {
      List<(Vector3Int, Color)> tiles = new List<(Vector3Int, Color)>();
      for (int i = 0; i < blockydef.tilePositions.Count; i++) {
        tiles.Add((blockydef.tilePositions[i], blockydef.tileColors[i]));
      }
      blockyMap[blockydef.name] = tiles;
    } 
    return blockyMap;
  }

  private static Dictionary<string, Category_D> makeCategoryMap(List<Category> categoryList) {
    Dictionary<string, Category_D> categoryMap = new Dictionary<string, Category_D>();
    foreach (Category cat in categoryList) {
      Category_D newCat = new Category_D();
      newCat.nodes = makeNodeMap(cat.nodes);
      categoryMap[cat.categoryName] = newCat;
    }
    return categoryMap;
  }

  private static Dictionary<string, Node_D> makeNodeMap(List<Node> nodes) {
    Dictionary<string, Node_D> nodeMap = new Dictionary<string, Node_D>();
    foreach (Node node in nodes) {
      Node_D newNode = new Node_D();
      newNode.blockyName = node.blockyName;
      newNode.children = makeChildMap(node.children);
      nodeMap[node.nodeName] = newNode;
    }
    return nodeMap;
  }

  private static Dictionary<string, Child> makeChildMap(List<Child> children) {
    Dictionary<string, Child> childMap = new Dictionary<string, Child>();
    foreach (Child child in children) {
      childMap[child.childName] = child;
    }
    return childMap;
  }
}

#region Dictionary definitions used in-Unity for quick lookup
public class Category_D {
  public Dictionary<string, Node_D> nodes;
}
public class Node_D {
  public string blockyName;
  public Dictionary<string, Child> children;
}
#endregion

#region Serializable definition of every component of Showey
/// <summary> An entire Showey definition contained in a serializable class. </summary>
[Serializable] public class SeriShoweyDefinition {
  public string sign;
  public string genDir;
  public string camMode;
  public string camDir;
  public List<BlockyDefinition> blockyDefinitions;
  public List<Category> categoryList;
}

/// <summary> A serializable Blocky definition containing the name, and tile positions with colours. </summary>
[Serializable] public struct BlockyDefinition {
  public string name;
  public List<Vector3Int> tilePositions;
  public List<Color> tileColors;
}

/// <summary> A serializable Category definition, which is the heighest level of the mapping. </summary>
[Serializable] public struct Category {
  public string categoryName;
  public List<Node> nodes;
} 

/// <summary> A serializable Node definition containing name, Blocky to which its supposed to map and its children. </summary>
[Serializable] public struct Node {
  public string nodeName;
  public string blockyName;
  public List<Child> children;
} 

/// <summary> A serializable Child definition with its name, relative direction and offset. </summary>
[Serializable] public struct Child {
  public string childName;
  public string relativeDirection;
  public Vector3Int offset;
} 
#endregion