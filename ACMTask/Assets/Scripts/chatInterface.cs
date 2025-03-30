using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class chatInterface : MonoBehaviour
{
    public GameObject chatPanel;
    ScrollRect scrollRect;
    InputField inputField;
    GameObject content;
    Vector2 offset= new Vector2(0,30f);
    public GameObject textPrefab; 
    // Start is called before the first frame update
    void Start()
    {
        scrollRect=GetComponent<ScrollRect>();    
        inputField=chatPanel.transform.GetChild(2).GetComponent<InputField>();
        content=chatPanel.transform.GetChild(0).GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return)){
            chatPanel.SetActive(!chatPanel.activeSelf);
        }
        if(Input.GetKeyDown(KeyCode.LeftAlt)){
            Debug.Log("H");
            GameObject newText=Instantiate(textPrefab,content.transform);
            //newText.GetComponent<RectTransform>().localPosition=offset;
            newText.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,-50f);
            newText.GetComponent<Text>().text=inputField.text;
            inputField.text="";
            for(int i=0;i<content.transform.childCount-1;i++){
                content.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition+=offset;
            }
        }
    }
}
