using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class pkratten_PoseDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public TextMeshProUGUI text;
    public pkratten_HandPose handPose;

    // Update is called once per frame
    void Update()
    {
        text.text = Enum.GetName(typeof(HandPoseName), handPose.currentPose.name);
    }
}
