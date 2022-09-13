using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MIV_Main : MonoBehaviour
{
    // will read LoadTracePredix/{cameraName}.csv
    public string LoadTracePredix = "../Trace";
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject cameraPrefab;
    public int numOfCameras = 2;
    public string cameraNamePrefix = "sv";
    public int width = 1080;
	public int height = 720;
    public string outputDir = "../Output";
    public string RGBImageFormat = "rgb_{0}_{1}.png";
	public string DImageFormat = "d_{0}_{1}.raw"; 
    
    private int frameIdx = 0;
    private List<GameObject> sourceCameras;
    // Start is called before the first frame update
    void Start()
    {
        //check if directory doesn't exit
        if(!Directory.Exists(outputDir)){
            //if it doesn't, create it
            Directory.CreateDirectory(outputDir);
        }
        Time.timeScale = 0;
        sourceCameras = new List<GameObject>();
        for(int i = 0; i < numOfCameras; i++){
            GameObject camera = Instantiate(cameraPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            camera.transform.parent = transform;
            // camera initial potition and orientation
            camera.transform.position = new Vector3(3*(i), 0.0f, -10.0f);
            camera.transform.rotation = Quaternion.Euler(0.0f,0.0f,0.0f);
            camera.name = cameraNamePrefix + i;
            sourceCameras.Add(camera);
        }
        Debug.Log($"{sourceCameras.Count} cameras instantiated");
        for(int i = 0; i < sourceCameras.Count; i++){
            sourceCameras[i].GetComponent<MIV_CameraMovement>().traceFile = Path.Combine(LoadTracePredix, sourceCameras[i].name + ".csv");
            sourceCameras[i].GetComponent<MIV_Capture>().setWidthHeight(width, height);
            // sourceCameras[i].GetComponent<MIV_Capture>().setImageFormat(Path.Combine(outputDir, RGBImageFormat), Path.Combine(outputDir, DImageFormat));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // [YuanChun begin]
        StartCoroutine(DoItFrameByFrame());
        // [YuanChun end]
        // [YuanChun begin]
        // print(sourceCameras.Count);
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     // Time.timeScale = 0;
        //     for(int i = 0; i < sourceCameras.Count; i++){
        //         Capture(i, frameIdx);
        //     }
        //     frameIdx++;
        //     Time.timeScale = 1;
        // }  
        // DoItFrameByFrame();
        // [YuanChun end] 
    }

    // [YuanChun begin]
    public IEnumerator DoItFrameByFrame(){
        // Time.timeScale = 0;
        yield return null;
        bool toCont = false;
        for(int i = 0; i < sourceCameras.Count; i++){
            bool hasNext = Capture(i, frameIdx);
            toCont = toCont || hasNext;
        }
        frameIdx++;
        Time.timeScale = 1;
        // toCont
    }
    // [YuanChun end] 
    
    public bool Capture(int cameraNum, int frameNum)
    {
        string sourceCameraName = sourceCameras[cameraNum].name;
        bool hasNext = sourceCameras[cameraNum].GetComponent<MIV_CameraMovement>().ReadAndMove();
        if(hasNext) {
            sourceCameras[cameraNum].GetComponent<MIV_Capture>().OnCameraChange();
            var sRGBImg = string.Format(RGBImageFormat, "{0}", frameIdx);
            var sDImg = string.Format(DImageFormat, "{0}", frameIdx);
            sourceCameras[cameraNum].GetComponent<MIV_Capture>().setImageFormat(
                Path.Combine(outputDir, sRGBImg), 
                Path.Combine(outputDir, sDImg)
            );
            sourceCameras[cameraNum].GetComponent<MIV_Capture>().Capture(sourceCameras[cameraNum].name);
        }
        return hasNext;
    }
}
