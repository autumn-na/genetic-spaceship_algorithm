using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NMHUICtrl : MonoBehaviour
{
    public NMHSequence Sequence;

    public GameObject NextGenerationObj;
    public GameObject LaunchObj;
    public GameObject EndObj;
    public GameObject PauseObj;
    public GameObject ResumeObj;

    public Button EditGeneButton;

    public GameObject EditUIObj;

    public Text[] Texts;

    public float currentTimeScale = 1f;

	void Start ()
    {
        currentTimeScale = 1f;

    }
	
	void Update ()
    {
		
	}

    public void NextGenerationButton()
    {
        Sequence.CreateGenome();
        NextGenerationObj.GetComponent<Button>().interactable = false;
        LaunchObj.GetComponent<Button>().interactable = true;
        EditGeneButton.interactable = true;
    }

    public void LaunchButton()
    {
        Sequence.LaunchSpaceship();
        LaunchObj.GetComponent<Button>().interactable = false;
        EditGeneButton.interactable = false;
    }

    public void EndButton()
    {
        Application.LoadLevel(0);
    }

    public void EditGene()
    {
        EditUIObj.SetActive(true);
    }
    
    public void ShowUIToggle(bool _bIsOn)
    {
        if(_bIsOn)
        {
            NextGenerationObj.SetActive(true);
            LaunchObj.SetActive(true);
            EndObj.SetActive(true);

            Debug.Log("On UI");
        }
        else if(!_bIsOn)
        {
            NextGenerationObj.SetActive(false);
            LaunchObj.SetActive(false);
            EndObj.SetActive(false);
        }
    }

    public void ShowNotificationToggle(bool _bIsOn)
    {
        if(_bIsOn)
        {
            for(int i = 0; i < 12; i ++)
            {
                Texts[i].gameObject.SetActive(true);
            }
        }
        else if(!_bIsOn)
        {
            for (int i = 0; i < 12; i++)
            {
                Texts[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetFixedDeltaTIme(float fPercent)
    {
        //Time.fixedDeltaTime = fPercent * Time.deltaTime;
        Time.timeScale = fPercent;
        currentTimeScale = fPercent;

        Time.fixedDeltaTime = 0.02f / Time.timeScale;
    }

    public void Pause()
    {
        Time.timeScale = 0;

        PauseObj.SetActive(false);
        ResumeObj.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = currentTimeScale;

        PauseObj.SetActive(true);
        ResumeObj.SetActive(false);
    }
}
