using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameCanvas : MonoBehaviour
{
    public Text _levelText;
    void Start()
    {
        _levelText.text = "Level " + (SceneController.Instance.CurrentScene - 1).ToString();
    }
}
