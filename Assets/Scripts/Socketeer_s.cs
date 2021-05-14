using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.IO;

public class Socketeer_s : MonoBehaviour
{
  public static bool doSocket = false;
  int local_port = 1500;
  IPAddress address = IPAddress.Parse("127.0.0.1");
  TcpListener listener;
  Stream s;
  StreamReader sr;
  Socket soc;
  bool firstFrame = true;
  // Start is called before the first frame update
  void Start() {
    if (doSocket) {
      Debug.Log("start of start");
      listener = new TcpListener(address, local_port);
      Debug.Log("Created a new TCP listener");
      listener.Start();
      Debug.Log("Going to wait for a socket connection.");
      soc = listener.AcceptSocket(); // blocks
      Debug.Log("Found connection");
      s = new NetworkStream(soc);
      Debug.Log("Made a stream for communication");
      sr = new StreamReader(s);
      Debug.Log("Made a stream Reader");
    }
    // s.Close();
    // soc.Close();
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
    if (doSocket) {
      if (s.CanRead) {
        Debug.Log("RECEIVED: " + sr.ReadLine());
        s.Close();
        soc.Close();
      } 
    }
  }
}
