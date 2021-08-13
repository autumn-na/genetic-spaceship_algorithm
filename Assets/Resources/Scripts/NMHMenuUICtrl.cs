using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NMHMenuUICtrl : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(1280, 720, true);
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void StartEvolution()
    {
        Application.LoadLevel(1);
    }

    public void LoadDate()
    {

    }

    public void End()
    {
        Application.Quit();
    }
}
