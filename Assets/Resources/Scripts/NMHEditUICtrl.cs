using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NMHEditUICtrl : MonoBehaviour
{
    public NMHSequence Sequence;

    public InputField[] InputGene;

    public Dropdown GeneDrop;

    public List<string> strGeneList;

    float[] GeneLimitMax;
    float[] GeneLimitMin;

    public int nCurrentSelGene = 0;

	void Start ()
    {
        initializeObs();

    }
	
	void Update ()
    {
		
	}

    void initializeObs()
    {
        GeneLimitMax = new float[16];
        GeneLimitMin = new float[16];

        GeneLimitMax[0] = 1.5f;
        GeneLimitMax[1] = 1.5f;
        GeneLimitMax[2] = 0.2f;
        GeneLimitMax[3] = 0.2f;
        GeneLimitMax[4] = 0.2f;
        GeneLimitMax[5] = 0.2f;
        GeneLimitMax[6] = 2f;
        GeneLimitMax[7] = 0.33f;
        GeneLimitMax[8] = 0.33f;
        GeneLimitMax[9] = 0.33f;
        GeneLimitMax[10] = 0.33f;
        GeneLimitMax[11] = 0.33f;
        GeneLimitMax[12] = 0.33f;

        for (int i = 13; i < 16; i++)
        {
            GeneLimitMax[i] = 360f;
        }

        for(int i = 0; i < 16; i++)
        {
            GeneLimitMin[i] = GeneLimitMax[i] * -1f;
        }

        GeneDrop.AddOptions(strGeneList);

        for (int i = 0; i < 2; i++)
        {
            InputGene[i].text = Sequence.ChildSpaceShip[nCurrentSelGene].RocketGene[i].y.ToString();

            (InputGene[2 + i * 2].text) = Sequence.ChildSpaceShip[nCurrentSelGene].RocketEngineGene[i].x.ToString();
            (InputGene[3 + i * 2].text) = Sequence.ChildSpaceShip[nCurrentSelGene].RocketEngineGene[i].z.ToString();
        }

        InputGene[6].text = Sequence.ChildSpaceShip[nCurrentSelGene].TankGene.y.ToString();

        for (int i = 0; i < 3; i++)
        {
            (InputGene[7 + i * 2].text) = Sequence.ChildSpaceShip[nCurrentSelGene].OrbiterEngineGene[i].x.ToString();
            InputGene[8 + i * 2].text = Sequence.ChildSpaceShip[nCurrentSelGene].OrbiterEngineGene[i].z.ToString();

            (InputGene[13 + i].text) = Sequence.ChildSpaceShip[nCurrentSelGene].AngleGene[i].ToString();
        }

        InputGene[nCurrentSelGene].transform.Find("Placeholder").GetComponent<Text>().text = InputGene[nCurrentSelGene].text;
    }

    public void Back()
    {
        this.gameObject.SetActive(false);
    }

    public void SelectGene(int _nNum)
    {
        nCurrentSelGene = GeneDrop.value;

        for(int i = 0; i < 16; i ++)
        {
            InputGene[i].text = "";
        }

        for (int i = 0; i < 2; i++)
        {
            InputGene[i].text = Sequence.ChildSpaceShip[nCurrentSelGene].RocketGene[i].y.ToString();

            (InputGene[2 + i * 2].text) = Sequence.ChildSpaceShip[nCurrentSelGene].RocketEngineGene[i].x.ToString();
            (InputGene[3 + i * 2].text) = Sequence.ChildSpaceShip[nCurrentSelGene].RocketEngineGene[i].z.ToString();
        }

        InputGene[6].text = Sequence.ChildSpaceShip[nCurrentSelGene].TankGene.y.ToString();

        for (int i = 0; i < 3; i++)
        {
            (InputGene[7 + i * 2].text) = Sequence.ChildSpaceShip[nCurrentSelGene].OrbiterEngineGene[i].x.ToString();
            InputGene[8 + i * 2].text = Sequence.ChildSpaceShip[nCurrentSelGene].OrbiterEngineGene[i].z.ToString();

            (InputGene[13 + i].text) = Sequence.ChildSpaceShip[nCurrentSelGene].AngleGene[i].ToString();
        }

        for (int i = 0; i < 16; i++)
        {
            InputGene[nCurrentSelGene].transform.Find("Placeholder").GetComponent<Text>().text = InputGene[nCurrentSelGene].text;
        }

        Debug.Log("Gene " + nCurrentSelGene);
    }

    public void CheckInputs()
    {
        for(int i = 0; i < 16; i ++)
        {
            if (InputGene[i].text != "")
            {
                if (float.Parse(InputGene[i].text) > GeneLimitMax[i])
                {
                    InputGene[i].text = GeneLimitMax[i].ToString();
                }

                if (float.Parse(InputGene[i].text) < GeneLimitMin[i])
                {
                    InputGene[i].text = GeneLimitMin[i].ToString();
                }
            }
        }
    }


    public void SetGene()
    {
        bool bIsNone = false;


        for (int i = 0; i < 16; i++)
        {
            if (InputGene[i].text != "")
            {
                bIsNone = false;
            }
            else
            {
                bIsNone = true;
            }
        }

        if (bIsNone == false)
        {
            for (int i = 0; i < 2; i++)
            {
                Sequence.ChildSpaceShip[nCurrentSelGene].RocketGene[i].y = float.Parse(InputGene[i].text);

                Sequence.ChildSpaceShip[nCurrentSelGene].RocketEngineGene[i].x = float.Parse(InputGene[2 + i * 2].text);
                Sequence.ChildSpaceShip[nCurrentSelGene].RocketEngineGene[i].z = float.Parse(InputGene[3 + i * 2].text);
            }

            Sequence.ChildSpaceShip[nCurrentSelGene].TankGene.y = float.Parse(InputGene[6].text);

            for (int i = 0; i < 3; i++)
            {
                Sequence.ChildSpaceShip[nCurrentSelGene].OrbiterEngineGene[i].x = float.Parse(InputGene[7 + i * 2].text);
                Sequence.ChildSpaceShip[nCurrentSelGene].OrbiterEngineGene[i].z = float.Parse(InputGene[8 + i * 2].text);

                Sequence.ChildSpaceShip[nCurrentSelGene].AngleGene[i] = float.Parse(InputGene[13 + i].text);
            }



            foreach (Transform child in Sequence.ChildSpaceShip[nCurrentSelGene].transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            Sequence.putPrefabToSelGameObj(nCurrentSelGene);


            //Sequence.ChangeNoticeBoard();
        }
    }
}
