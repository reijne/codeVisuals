using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;

struct CreateUpdate {
  public string method;
  public string param;
  public bool close;
}

public class Socketeer_s : MonoBehaviour
{
  [SerializeField] CompositeController_s compositeController;
  [SerializeField] SceneController_s sceneController;
  public static bool doSocket = true;
  private static string request = "";
  private static Thread creator_thread;
  private static Thread reader_thread;
  private static Mutex mut = new Mutex();
  int local_port = 530;
  IPAddress address = IPAddress.Parse("127.0.0.1");
  TcpListener listener;
  static Stream s;
  static StreamReader sr;
  Socket soc;
  bool isSocketCreated = false;
  bool isReaderAlive = false;
  
  // Start is called before the first frame update
  void Start() {
    if (doSocket) {
      creator_thread = new Thread(new ThreadStart(createSocket));
      creator_thread.Start();
    }
    // s.Close();
    // soc.Close();
  }

  void createSocket() {
    // Debug.Log("Creating a socket");
    listener = new TcpListener(address, local_port);
    // Debug.Log("Created a new TCP listener");
    listener.Start();
    // Debug.Log("Going to wait for a socket connection.");
    soc = listener.AcceptSocket(); // blocks
    // Debug.Log("Found connection");
    s = new NetworkStream(soc);
    // Debug.Log("Made a stream for communication");
    sr = new StreamReader(s);
    // Debug.Log("Made a stream Reader");
    isSocketCreated = true;
  }

  // IEnumerator createSocket() {
  //   soc = listener.AcceptSocket(); // blocks
  //   Debug.Log("Found connection");
  //   s = new NetworkStream(soc);
  //   Debug.Log("Made a stream for communication");
  //   sr = new StreamReader(s);
  //   Debug.Log("Made a stream Reader");
  //   firstFrame = false;
  //   yield return null;
  // } 
  void Update()
  {
    if (isSocketCreated && !isReaderAlive) {
      isReaderAlive = true;
      reader_thread = new Thread(new ThreadStart(readSocket));
      reader_thread.Start();
    }
    // if (firstFrame) {
    //   StartCoroutine("createSocket");
    //   soc = listener.AcceptSocket(); // blocks
    //   Debug.Log("Found connection");
    //   s = new NetworkStream(soc);
    //   Debug.Log("Made a stream for communication");
    //   sr = new StreamReader(s);
    //   Debug.Log("Made a stream Reader");
    //   firstFrame = false;
    // } else 
    Debug.Log(String.Format("Current request:: {0}", request));
    if (request != "") {
      // Debug.Log("Received:: >" + request + "<");
      CreateUpdate cu = JsonUtility.FromJson<CreateUpdate>(request);
      handleRequest(cu);
      // sr.DiscardBufferedData();
      mut.WaitOne();
      request = "";
      mut.ReleaseMutex();
      // Debug.Log("set the request back to null");
      if (cu.close) {
        // Debug.Log("Closed the connection");
        s.Close();
        soc.Close();
      } 
    }
    
    // if (Input.GetKeyDown(KeyCode.Escape)) {
    //   s.Close();
    //   soc.Close();
    // }
  }

  // private async void readSocket() {
  //   if (s.CanRead) {
  //     string request;
  //     await request = sr.ReadLineAsync();
            
  //     if (request != "") {
  //       Debug.Log("Received:: " + request);
  //       CreateUpdate cu = JsonUtility.FromJson<CreateUpdate>(request);
  //       handleRequest(cu);
  //       sr.DiscardBufferedData();
  //       if (cu.close) {
  //         Debug.Log("Closed the connection");
  //         s.Close();
  //         soc.Close();
  //       } 
  //     }
  //     // else {
  //     //   Byte[] buffer = Encoding.ASCII.GetBytes("received");
  //     //   s.Write(buffer, 0, 0);
  //     // }
  //   }
  // }
  private void readSocket() {
    while (doSocket) {
      if (s.CanRead && request == "") {
        string requestLine = sr.ReadLine();
        if (requestLine == null || requestLine == "") continue;
        mut.WaitOne();
        request = requestLine;
        mut.ReleaseMutex();
        Debug.Log("Set the request to " + requestLine);
        // else {
        //   Byte[] buffer = Encoding.ASCII.GetBytes("received");
        //   s.Write(buffer, 0, 0);
        // }
      }
    }
    s.Close();
    soc.Close();
  }

  void handleRequest(CreateUpdate cu) {
    Debug.Log(cu.method);
    switch (cu.method) {
      case "createShowey": compositeController.createShowey(cu.param);          break;
      case "updateShowey": compositeController.loadShoweyDefinition(cu.param);  break;
      case "createSceney": sceneController.createSceney(cu.param);              break;
      case "updateSceney": sceneController.updateSceney(cu.param);              break;
      case "updateErrors": sceneController.updateErrors(cu.param);              break;
      default: Debug.LogError("Unknown request received by socket."); break;
    }
  }
  private void OnApplicationQuit() {
    reader_thread.Abort();
    s.Close();
    soc.Close();
  }
}