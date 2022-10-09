using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Vector3 ItemOffset;
    public bool holdingItem;
    public float raycastLength;
    public GameObject holdingItemObject;
    public int currentItemKey;

    Vector2 Movement;
    Rigidbody2D rb;

    public RaycastHit2D RaycastHitUp, RaycastHitDown;
    public GameObject CurrrentInteractable;

    public GameObject[] ItemPresetObjects;
    public int score;

    public NewTask TaskManager;

    public bool GameActive;

    public TMP_Text infoBox;

    [TextArea(5,10)]
    public string starting, whilePlaying, endWrap1, endWrap2;

    public AudioSource MusicEmit;
    public AudioSource[] SFXRStack; // 0 = pickup, 1 = affect, 2 = output
    public bool ableToStart=true;
    public string reasonForLoss;

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        infoBox.text = starting;
    }

    void Update()
    {
        if(GameActive==true){
            RaycastHitUp = Physics2D.Raycast(transform.position, Vector2.up, raycastLength);
            RaycastHitDown = Physics2D.Raycast(transform.position, Vector2.down, raycastLength);

            if(RaycastHitUp.collider != null){
                CurrrentInteractable = RaycastHitUp.collider.gameObject;
            } else CurrrentInteractable = null;

            if(RaycastHitDown.collider != null){
                CurrrentInteractable = RaycastHitDown.collider.gameObject;
            } else CurrrentInteractable = null;

            Movement.x = Input.GetAxisRaw("Horizontal");
            Movement.y = Input.GetAxisRaw("Vertical");

            if(Input.GetKeyDown(KeyCode.E)){
                Debug.Log("Interact key pressed");
                if(CurrrentInteractable!=null){
                    if(CurrrentInteractable.tag=="pBox"){
                        Debug.Log("Used Potatoe box");
                        NewItem(0, -1);
                        SFXRStack[0].Play();
                    }
                    else if(CurrrentInteractable.tag=="bBox"){
                        Debug.Log("Used Bowel box");
                        NewItem(1, -1);
                        SFXRStack[0].Play();
                    }
                    else if(CurrrentInteractable.tag=="dBox"){
                        Debug.Log("Used Dough box");
                        NewItem(2, -1);
                        SFXRStack[0].Play();
                    }
                    else if(CurrrentInteractable.tag=="kCounter"){
                        Debug.Log("Used Knife Counter");
                        NewItem(3, 0);
                        SFXRStack[1].Play();
                    }
                    else if(CurrrentInteractable.tag=="Oven"){
                        Debug.Log("Used Oven");
                        NewItem(5, 2);
                        SFXRStack[1].Play();
                    }
                    else if(CurrrentInteractable.tag=="Freezer"){
                        Debug.Log("Used Freezer");
                        NewItem(4, 1);
                        SFXRStack[1].Play();
                    }
                    else if(CurrrentInteractable.tag=="out"){
                        Debug.Log("Used Output");
                        OutputItem(currentItemKey);
                        SFXRStack[2].Play();

                        int selec = Random.Range(0,2);
                        if(selec==0) TaskManager.MinWait = TaskManager.MinWait - (TaskManager.MinWait * Random.Range(0.1f, 0.2f));
                        if(selec==1) TaskManager.maxWait = TaskManager.maxWait - (TaskManager.maxWait * Random.Range(0.1f, 0.2f));
                    }
                }
            }

            if(holdingItemObject != null){
                holdingItemObject.transform.position = transform.position + ItemOffset;
            }
        } if(GameActive==false){
            if(ableToStart==true){
                if(Input.GetKeyDown(KeyCode.Space)){
                    // Game starts in here
                    Debug.Log("Game Started");

                    TaskManager.newTask();
                    infoBox.text = whilePlaying;

                    GameActive=true;
                    ableToStart=false;
                }
            } else{ // Game ending code here
                Movement.x = 0;
                Movement.y = 0;
                infoBox.text = reasonForLoss + "\n\n" + endWrap1 + score + endWrap2;
                if(Input.GetKeyDown(KeyCode.R)){
                    SceneManager.LoadScene("GameScene");
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)){
            if(MusicEmit.mute==false) MusicEmit.mute=true;
            else MusicEmit.mute=false;
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + Movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void NewItem(int newItemKey, int requiredItemKey){
        if(requiredItemKey==-1){
            ClearCurrentItem();
            holdingItemObject = Instantiate(ItemPresetObjects[newItemKey], transform.position + ItemOffset, Quaternion.identity);
            holdingItem = true;
            currentItemKey=newItemKey;
        }
        else if(requiredItemKey==currentItemKey){
            ClearCurrentItem();
            holdingItemObject = Instantiate(ItemPresetObjects[newItemKey], transform.position + ItemOffset, Quaternion.identity);
            holdingItem = true;
            currentItemKey=newItemKey;
        }
    }

    void OutputItem(int itemKey){
        TaskManager.removeTask(itemKey-=3);
        ClearCurrentItem();
        Debug.Log($"Output an item, score is now {score}");
    }

    void ClearCurrentItem(){
        holdingItem = false;
        Destroy(holdingItemObject);
        holdingItemObject = null;
        holdingItem = false;
        currentItemKey = -5;
    }
}

/*

----- Item Key List -----

0 | Potatoe
1 | Bowl
2 | Dough
3 | Chips
4 | Ice Cream
5 | Cake

*/