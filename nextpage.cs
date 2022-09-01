using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class nextpage : MonoBehaviour
{
    public GameObject spo1;
    public GameObject sco;
    public GameObject spo2;
    public GameObject spo3;
    // Start is called before the first frame update
    void Start()
    {
        sco.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void chatingFiled(){
        spo1.SetActive(false);
        spo2.SetActive(false);
        spo3.SetActive(false);
        sco.SetActive(true);
    }
}
