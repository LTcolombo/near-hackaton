using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    // Update is called once per frame
    public void DoSwitch(string value)
    {
        SceneManager.LoadScene(value);
    }
}
