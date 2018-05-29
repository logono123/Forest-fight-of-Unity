using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemymove : MonoBehaviour {
    float TranslateSpeed = 0.1f;
    float TranslateSpeedTime = 0.1f;
    void Update()
    {
        TranslateSpeedTime += 0.1f;

        transform.Translate(Vector3.forward * TranslateSpeed);
        if (TranslateSpeedTime > 150.0f)
        {
            transform.Rotate(0, 180, 0);
            TranslateSpeedTime = 0.1f;
        }
    }


}
