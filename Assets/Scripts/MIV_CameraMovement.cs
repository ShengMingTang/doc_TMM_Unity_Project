using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MIV_CameraMovement : MonoBehaviour
{
    // will read "{LoadTracePredix}/{transformName}.csv"
    public string traceFile = "";
    
    protected FileInfo traceFileInfo = null;
    protected StreamReader reader = null;
    protected string text = " ";
    protected bool moveable = true;
    protected string transformName = null;

    // Start is called before the first frame update
    void Start()
    {
        transformName = transform.name;
        traceFileInfo = new FileInfo(traceFile);
        reader = traceFileInfo.OpenText();
        text = reader.ReadLine(); // skip the first line
    }

    // Update is called once per frame
    void Update()
    {
        // if(moveable){
        //     ReadAndMove();
        // }
    }

    // return true then valid, false otherwise
    public bool ReadAndMove(){
        if(!moveable) {return moveable;}
        if(text != null){
            text = reader.ReadLine();
            if(text == null){
                moveable = false;
                print($"{transformName} finish moving");
            }
            else{
                text = text.Replace(" ", "");
                string[] subs = text.Split(",");
                // read row
                MoveCamera(float.Parse(subs[1]),float.Parse(subs[2]),float.Parse(subs[3]),float.Parse(subs[4]),float.Parse(subs[5]),float.Parse(subs[6]));
            }
            return true;
        }
        return moveable;
    }

    void MoveCamera(float x, float y, float z, float yaw, float pitch, float roll){
        print($"{transformName} move to {x},{y},{z},{yaw},{pitch},{roll}");
        transform.localPosition = new Vector3(x, y, z);
        transform.localRotation = Quaternion.Euler(yaw, pitch, roll);
        return;
    }

}
