using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    public static int count=0;
    public static Chat instance;
    void Awake() => instance = this;
    public RectTransform chatContent;
    public Text ChatText;
    public ScrollRect ChatScrollRect;
    public void ShowMessage(string date){
        if(count==4){
             ChatText.text=ChatText.text==""? date:"\n"+date;
        }else{
            ChatText.text+=ChatText.text==""? date:"\n"+date;
            count++;
        }
        Fit(ChatText.GetComponent<RectTransform>());
        Fit(chatContent);
        Invoke("ScrollDelay",0.03f);
    }
    void Fit(RectTransform Rect)=>LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);

    void ScrollDelay()=>ChatScrollRect.verticalScrollbar.value=0;
}
