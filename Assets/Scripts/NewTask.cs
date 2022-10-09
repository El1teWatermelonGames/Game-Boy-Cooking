using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTask : MonoBehaviour
{
    public Vector3 Pos1, Pos2, Pos3;
    public bool active1, active2, active3;
    public int key1 = -1, key2 = -1, key3 = -1;
    public GameObject EmptyIconField;
    public GameObject[] Icons;
    public GameObject[] UIElements; //0 = Fries, 1 = Ice Cream, 2 = Muffin
    public float waitLength, MinWait, maxWait;
    public PlayerController PC;

    bool ableToCreateNewTask;

    IEnumerator delayedNewTask(){
        Debug.Log("Function: delayedNewTask ran");
        waitLength = Random.Range(MinWait, maxWait);
        Debug.Log("waitLength now equals: "+waitLength);
        yield return new WaitForSeconds(waitLength);
        newTask();
    }

    public void newTask(){
        if(key1 != -1 && key2 != -1 && key3 != -1) {
            PC.reasonForLoss = "You took too long to finish the orders";
            PC.GameActive = false;
            Debug.Log("game over: "+PC.reasonForLoss);
        }
        int newKey = Random.Range(0, 3);
        Debug.Log("New Key: "+newKey);
        Vector3 SpawnPos = new Vector3 (0,0,0);
        int PosKey = 0;
        if(active1 == false){
            SpawnPos = Pos1;
            key1 = newKey;
            active1 = true;
            PosKey=0;
        } else if(active2 == false){
            SpawnPos = Pos2;
            key2 = newKey;
            active2 = true;
            PosKey=1;
        } else if(active3 == false){
            SpawnPos = Pos3;
            key3 = newKey;
            active3 = true;
            PosKey=2;
        }
        Icons[PosKey] = Instantiate(UIElements[newKey], SpawnPos, Quaternion.identity);
        Debug.Log("New Task - Key: "+newKey+"PosKey: "+PosKey);
        StartCoroutine(delayedNewTask());
    }

    public void removeTask(int key){
        Debug.Log("Ran Remove task, task to remove has key: "+key);
        if(key1==key){
            active1=false;
            key1=-1;
            Destroy(Icons[0]);
            Icons[0]=EmptyIconField;
            PC.score++;
        } else if(key2==key){
            active2=false;
            key2=-1;
            Destroy(Icons[1]);
            Icons[1]=EmptyIconField;
            PC.score++;
        } else if(key3==key){
            active3=false;
            key3=-1;
            Destroy(Icons[2]);
            Icons[2]=EmptyIconField;
            PC.score++;
        } else {
            PC.reasonForLoss="Served the wrong product!";
            PC.GameActive=false;
            Debug.Log("game over: "+PC.reasonForLoss);
        }
    }

    void clearTasks(){
        active1 = false;
        active2 = false;
        active3 = false;
        key1 = -1;
        key2 = -1;
        key3 = -1;
        if(Icons[0] != EmptyIconField) Destroy(Icons[0]);
        if(Icons[1] != EmptyIconField) Destroy(Icons[1]);
        if(Icons[2] != EmptyIconField) Destroy(Icons[2]);
        Icons[0] = EmptyIconField;
        Icons[1] = EmptyIconField;
        Icons[2] = EmptyIconField;
    }
}
