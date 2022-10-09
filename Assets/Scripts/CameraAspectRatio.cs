using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAspectRatio : MonoBehaviour
{
     public float aspect_ratio_x=9f;
     public float aspect_ratio_y=16f;
 
     void Start() {
         fixaspect ();
     }
     void fixaspect(){
         float targetaspect = aspect_ratio_x / aspect_ratio_y;
         float windowaspect = (float)Screen.width / (float)Screen.height;
         float scaleheight = windowaspect / targetaspect;
         if(scaleheight>=1f) {// add pillarbox
             float scalewidth = 1.0f / scaleheight;            
             Rect rect = GetComponent<Camera>().rect;            
             rect.width = scalewidth;
             rect.height = 1.0f;
             rect.x = (1.0f - scalewidth) / 2.0f;
             rect.y = 0;            
             GetComponent<Camera>().rect = rect;
         }
         else {
             GetComponent<Camera>().rect=new Rect(0,0,1,1);
         }
         //Camera.main.aspect = aspect_ratio_x / aspect_ratio_y;
     }
}
