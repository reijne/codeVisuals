using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockyController_s : MonoBehaviour
{
  [SerializeField] private GameObject showeyInterface;
  [SerializeField] private GameObject blockyInterface;
  [SerializeField] private Blocky_s showBlocky;
  [SerializeField] private Blocky_s createBlocky;
  [SerializeField] private SquareHandler squareHandler;
  [SerializeField] private InputField createBlockyNameField;
  [SerializeField] private Button sizeButton;
  [SerializeField] private Text sizeText;
  [SerializeField] private Button addBlockyButton;
  [SerializeField] private Button removeSelectedButton;
  [SerializeField] public Dropdown blockySelector;
  [SerializeField] private Text blockySelectorLabel;
  [SerializeField] private HeightHandler heightHandler;
  private Vector3 showBlockyStandardPosition;

  public static Dictionary<string, List<(Vector3Int, Color)>> nameToTilesMap = new Dictionary<string, List<(Vector3Int, Color)>>();

  private void Start() {
    showBlockyStandardPosition = showBlocky.transform.position;
  }
  #region Button Logic
  public void cycleBlockySize() {
    sizeText.text = Maps.blockySizeMap[sizeText.text];
    Blocky_s.SIZE = int.Parse(sizeText.text);
  }
  
  public void addBlocky() {
    switchInterfaces();
  }

  public void removeSelected() {
    if (blockySelectorLabel.text == "") return;
    nameToTilesMap.Remove(blockySelector.options[blockySelector.value].text);
    blockySelector.options.RemoveAt(blockySelector.value);
    if (nameToTilesMap.Count == 0) {
      sizeButton.interactable = true;
      blockySelectorLabel.text = "";
    } else {
      onBlockySelection(blockySelector);
    }
    showBlocky.removeTiles();

  }

  public void saveBlockyReturn() {
    if (createBlockyNameField.text == "") { 
      // No name given for the created blocky
      createBlockyNameField.placeholder.GetComponent<Text>().text = "Blocky Name Here... REQUIRED!!!";
    } else if (nameToTilesMap.ContainsKey(createBlockyNameField.text)) {
      // Name is already taken
      createBlockyNameField.text = "";
      createBlockyNameField.placeholder.GetComponent<Text>().text = "Blocky Name Already Used!";
    } else {
      saveNewBlocky(createBlockyNameField.text);
      sizeButton.interactable = false;
    }
  }
  #endregion

  public void onBlockySelection(Dropdown selector) {
    showBlocky.tilePosCols = nameToTilesMap[selector.options[selector.value].text];
    showBlocky.spawnTiles();
    Vector3 oldpos = showBlocky.transform.position;
    showBlocky.transform.position = showBlockyStandardPosition + new Vector3(0, Blocky_s.SIZE, Blocky_s.SIZE + (Blocky_s.SIZE * 2)); 
  }

  #region Adding a new Blocky
  private void saveNewBlocky(string blockyName) {
    // add blocky to da dict
    nameToTilesMap[createBlockyNameField.text] = createBlocky.tilePosCols;
    // clear the createBlocky
    clearCreateBlocky();
    // add blockyName to da dropdown 
    addNewOption();
    // return to showeyinterface
    switchInterfaces();
    // select the last added blockyName in the dropdown
    selectBlocky(blockyName);
  }

  private void clearCreateBlocky() {
    squareHandler.clearTiles();
  }

  private void addNewOption() {
    blockySelector.ClearOptions();
    List<string> newOptions = new List<string>();
    foreach (string key in nameToTilesMap.Keys) {
      newOptions.Add(key);
    }
    blockySelector.AddOptions(newOptions);
  }

  private void selectBlocky(string blockyName) {
    for (int i = 0; i < blockySelector.options.Count; i++) {
      if (blockySelector.options[i].text == blockyName) {

      } 
    }
    blockySelector.value = blockySelector.options.Count - 1;
    onBlockySelection(blockySelector);
  }
  #endregion

  private void switchInterfaces() {
    showeyInterface.SetActive(!showeyInterface.activeSelf);
    blockyInterface.SetActive(!blockyInterface.activeSelf);
    squareHandler.spawnSelectSquares();
    heightHandler.updateHeightSlider();
    createBlockyNameField.text = "";
    createBlockyNameField.placeholder.GetComponent<Text>().text = "Blocky Name Here...";
  }
}