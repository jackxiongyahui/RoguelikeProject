using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Loader1在main caream下，因此会在第一时间调用
public class Loader1 : MonoBehaviour {

    public GameObject gameManager1;

    private void Awake()
    {
        if(GameManager1.Instance==null)
        {
            GameObject.Instantiate(gameManager1);
        }     
    }


}
