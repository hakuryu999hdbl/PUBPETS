using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class language : MonoBehaviour
{

    Text Text;


    public string J;
    public string C_1;
    public string C_2;
    public string E;
    public string K;

    void Start()
    {
        Text=GetComponent<Text>();

        switch (PlayerPrefs.GetInt("language"))
        {
            case 0:
                Text.text = J;
                break;
            case 1:
                Text.text = C_1;
                break;
            case 2:
                Text.text = C_2;
                break;
            case 3:
                Text.text = E;
                break;
            case 4:
                Text.text = K;
                break;
        }
    }
}
