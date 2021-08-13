using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NMHSequence : MonoBehaviour
{
    public GameObject UppetTextObj;
    Text UpperText;

    public GameObject[] NoticeTextObj;
    Text[] NoticeText;

    public GameObject[] ResultTextObj;
    Text[] ResultText;

    public GameObject NextGenerationObj;

    public Text ParentNoticeText;

    //여기까지 UI
    //////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////

    public GameObject ChildSpaceshipEmptyObj;
    public GameObject ChildSpaceShipPrefab;

    public GameObject OrbiterPrefab;
    public GameObject OrbiterEnginePrefab;
    public GameObject ExternalTankPrefab;
    public GameObject RocketPrefab;
    public GameObject RocketEnginePrefab;

    GameObject[] ChildSpaceShipObj;
    public NMHSpaceship[] ChildSpaceShip;
    GameObject[] ParentSpaceShipObj;
    NMHSpaceship[] ParentSpaceShip;

    int nGeneration = 0;

    float[] fGenefitnessRank;
    int[] nRank;

    float fIntensity;

    bool bisFirst = true;

	public int nSelGene = 0;
	public bool bFollow = false;

	private Vector3 InitCamPosVec3;

	void Awake ()
    {
        InitializeObjs();
    }

	void Update ()
    {
        ShowResult();
        CheckAllAlive();

		if (bFollow)
		{
			Camera.main.transform.position = ChildSpaceShipObj[nSelGene].transform.position + new Vector3(0f, 10f, -10f);
		}
    }

    void InitializeObjs()
    {
        UpperText = UppetTextObj.GetComponent<Text>();

        if (bisFirst)
        {
            UpperText.text = "";
        }

        NoticeText = new Text[10];

        for (int i = 0; i < 10; i++)
        {
            NoticeText[i] = NoticeTextObj[i].GetComponent<Text>();
        }

        ResultText = new Text[10];

        for (int i = 0; i < 10; i++)
        {
            ResultText[i] = ResultTextObj[i].GetComponent<Text>();
        }

        ChildSpaceShipObj = new GameObject[10];
        ChildSpaceShip = new NMHSpaceship[10];

        ParentSpaceShipObj = new GameObject[2];
        ParentSpaceShip = new NMHSpaceship[2];

        nRank = new int[10];
        fGenefitnessRank = new float[10];

        for (int i = 0; i < 10; i++)
        {
            ChildSpaceShipObj[i] = Instantiate(ChildSpaceShipPrefab, new Vector3(30 * i, 16, 0), Quaternion.identity, ChildSpaceshipEmptyObj.transform);
            ChildSpaceShip[i] = ChildSpaceShipObj[i].GetComponent<NMHSpaceship>();
            // ChildSpaceShip[i].
        }

        for (int i = 0; i < 2; i++)
        {
            ParentSpaceShipObj[i] = Instantiate(ChildSpaceShipPrefab, new Vector3(30 * i, 16, 0), Quaternion.identity);
            ParentSpaceShip[i] = ChildSpaceShipObj[i].GetComponent<NMHSpaceship>();
		}

		InitCamPosVec3 = Camera.main.transform.position;
	}

    void InitializeSpaceship()
    {
        for (int i = 0; i < 10; i++)
        {
            ChildSpaceShipObj[i].transform.localPosition = new Vector3(30 * i, 16, 0);
            ChildSpaceShipObj[i].transform.localRotation = Quaternion.identity;

            ChildSpaceShip[i].BodyRig.useGravity = false;

            ChildSpaceShip[i].BodyRig.velocity = new Vector3(0, 0, 0);
            ChildSpaceShip[i].BodyRig.angularVelocity = new Vector3(0, 0, 0);

            ChildSpaceShip[i].isRocketForce = false;
            ChildSpaceShip[i].isOrbiterForce = false;
            ChildSpaceShip[i].isUp = true;
            ChildSpaceShip[i].isAlive = true;

            foreach (Transform child in ChildSpaceShip[i].transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    public void CreateGenome()
    {
        if (!bisFirst)
        {
            nGeneration++;

            InitializeSpaceship();
        }

        GamebleGenome();

        ChangeNoticeBoard();
        HideResult();

        putPrefabToGameObj();

        bisFirst = false;
    }

    void GamebleGenome()
    {
        if(bisFirst)
        {
            for(int i = 0; i < 10; i ++)
            {
                for (int j_Rnum = 0; j_Rnum < 2; j_Rnum++)
                {
                    if (j_Rnum == 0)
                    {
                        ChildSpaceShip[i].RocketGene[j_Rnum].x = -0.8f;
                    }
                    else
                    {
                        ChildSpaceShip[i].RocketGene[j_Rnum].x = 0.8f; //좌우 나눠 -1.6f, 1.6f
                    }
                    ChildSpaceShip[i].RocketGene[j_Rnum].y = UnityEngine.Random.Range(-1.5f, 1.5f);
                    ChildSpaceShip[i].RocketGene[j_Rnum].z = 0f;
                }

                ChildSpaceShip[i].TankGene.x = 0f;
                ChildSpaceShip[i].TankGene.y = UnityEngine.Random.Range(-2f, 2f);
                ChildSpaceShip[i].TankGene.z = 1.5f;

                for (int j_REnum = 0; j_REnum < 2; j_REnum++)
                {
                    ChildSpaceShip[i].RocketEngineGene[j_REnum].x = UnityEngine.Random.Range(-0.2f, 0.2f);
                    ChildSpaceShip[i].RocketEngineGene[j_REnum].y = -1.1f;
                    ChildSpaceShip[i].RocketEngineGene[j_REnum].z = UnityEngine.Random.Range(-0.2f, 0.2f);
                }

                for (int j_OEnum = 0; j_OEnum < 3; j_OEnum++)
                {
                    ChildSpaceShip[i].OrbiterEngineGene[j_OEnum].x = UnityEngine.Random.Range(-0.33f, 0.33f);
                    ChildSpaceShip[i].OrbiterEngineGene[j_OEnum].y = -1.1f;
                    ChildSpaceShip[i].OrbiterEngineGene[j_OEnum].z = UnityEngine.Random.Range(-0.33f, 0.33f);
                }

                for(int j_nAngleNum = 0; j_nAngleNum < 3; j_nAngleNum++)
                {
                    ChildSpaceShip[i].AngleGene[j_nAngleNum] = UnityEngine.Random.Range(-90f, 90f);

                    if(j_nAngleNum == 1)
                    {
                        ChildSpaceShip[i].AngleGene[j_nAngleNum] = UnityEngine.Random.Range(0f, 360f);
                    }
                }

                Debug.Log("Generation 0 Gambled");
            }
        }
        else if(!bisFirst)
        {
            SetIntensity();

            for (int i = 0; i < 10; i++)
            {
                CrossoverRocket(i);
                CrossoverTank(i);
                CrossoverREngine(i);
                CrossoverOEngine(i);
                CrossoverAngle(i);

            }
        }
    }

    void SetIntensity()
    {
        fIntensity = fGenefitnessRank[0] / fGenefitnessRank[9] / 100f;

        if(fIntensity <= 0.02f)
        {
            fIntensity = 0.02f;
        }
        else if(fIntensity >= 1f)
        {
            fIntensity = 1f;
        }

        Debug.Log(fIntensity);
    }

    void CrossoverRocket(int i)
    {
        int tmp = UnityEngine.Random.Range(0, 2);

        for (int j_nRnum = 0; j_nRnum < 2; j_nRnum++)
        {
            ChildSpaceShip[i].RocketGene[j_nRnum].y = Mathf.Clamp(ParentSpaceShip[tmp].RocketGene[j_nRnum].y + (- 1.5f + UnityEngine.Random.Range(0f, 3.0f)) * fIntensity, -1.5f, 1.5f);

        }
    }

    void CrossoverTank(int i)
    {
        int tmp = UnityEngine.Random.Range(0, 2);
        ChildSpaceShip[i].TankGene.y = Mathf.Clamp(ParentSpaceShip[tmp].TankGene.y - (2f + UnityEngine.Random.Range(0f, 4f)) * fIntensity, -2f, 2f);
    }

    void CrossoverREngine(int i)
    {
        int tmp = UnityEngine.Random.Range(0, 2);

        for (int j_REnum = 0; j_REnum < 2; j_REnum++)
        { 
            ChildSpaceShip[i].RocketEngineGene[j_REnum].x = Mathf.Clamp(ParentSpaceShip[tmp].RocketEngineGene[j_REnum].x + (- 0.2f + UnityEngine.Random.Range(0f, 0.4f)) * fIntensity, -0.2f, 0.2f);
            ChildSpaceShip[i].RocketEngineGene[j_REnum].y = -1.1f;
            ChildSpaceShip[i].RocketEngineGene[j_REnum].z = Mathf.Clamp(ParentSpaceShip[tmp].RocketEngineGene[j_REnum].z + (-0.2f + UnityEngine.Random.Range(0f, 0.4f)) * fIntensity, -0.2f, 0.2f);
        }
    }

    void CrossoverOEngine(int i)
    {
        int tmp = UnityEngine.Random.Range(0, 2);

        for (int j_OEnum = 0; j_OEnum < 3; j_OEnum++)
        {
            ChildSpaceShip[i].OrbiterEngineGene[j_OEnum].x = Mathf.Clamp(ParentSpaceShip[tmp].OrbiterEngineGene[j_OEnum].x + (-0.33f + UnityEngine.Random.Range(0f, 0.66f)) * fIntensity, -0.33f, 0.33f);
            ChildSpaceShip[i].OrbiterEngineGene[j_OEnum].y = -1.1f;
            ChildSpaceShip[i].OrbiterEngineGene[j_OEnum].z = Mathf.Clamp(ParentSpaceShip[tmp].OrbiterEngineGene[j_OEnum].z + (-0.33f + UnityEngine.Random.Range(0f, 0.66f)) * fIntensity, -0.33f, 0.33f);
        }
    }

    void CrossoverAngle(int i)
    {
        int tmp = UnityEngine.Random.Range(0, 2);

        for (int j_nAngleNum = 0; j_nAngleNum < 3; j_nAngleNum++)
        {
            ChildSpaceShip[i].AngleGene[j_nAngleNum] = Mathf.Clamp(ParentSpaceShip[tmp].AngleGene[j_nAngleNum] + (-90f + UnityEngine.Random.Range(0f, 180f)) * fIntensity, -90f, 90f);

            if (j_nAngleNum == 1)
            {
                ChildSpaceShip[i].AngleGene[j_nAngleNum] = Mathf.Clamp(ParentSpaceShip[tmp].AngleGene[j_nAngleNum] + (-180f + UnityEngine.Random.Range(180f, 360f)) * fIntensity, 0f, 360f);
            }
        }
    }

    public void putPrefabToGameObj()
    {
        for (int i = 0; i < 10; i++)
        {
            ChildSpaceShip[i].OrbiterObj = Instantiate(OrbiterPrefab, ChildSpaceShip[i].transform);
            ChildSpaceShip[i].OrbiterObj.transform.localPosition = new Vector3(0, 0, 0);
            ChildSpaceShip[i].OrbiterObj.transform.localRotation = Quaternion.identity;

            ChildSpaceShip[i].TankObj = Instantiate(ExternalTankPrefab, ChildSpaceShip[i].OrbiterObj.transform);
            ChildSpaceShip[i].TankObj.transform.localPosition = ChildSpaceShip[i].TankGene;
            ChildSpaceShip[i].TankObj.transform.localRotation = Quaternion.identity;

            for (int j_rocketnum = 0; j_rocketnum < 2; j_rocketnum++)
            {
                ChildSpaceShip[i].RocketObj[j_rocketnum] = Instantiate(RocketPrefab, ChildSpaceShip[i].TankObj.transform);
                ChildSpaceShip[i].RocketObj[j_rocketnum].transform.localPosition = ChildSpaceShip[i].RocketGene[j_rocketnum];
                ChildSpaceShip[i].RocketObj[j_rocketnum].transform.localRotation = Quaternion.identity;

                ChildSpaceShip[i].RocketEngineObj[j_rocketnum] = Instantiate(RocketEnginePrefab, ChildSpaceShip[i].RocketObj[j_rocketnum].transform);
                ChildSpaceShip[i].RocketEngineObj[j_rocketnum].transform.localPosition = ChildSpaceShip[i].RocketEngineGene[j_rocketnum];
                ChildSpaceShip[i].RocketEngineObj[j_rocketnum].transform.localRotation = Quaternion.identity;
            }

            for (int j_orbiternum = 0; j_orbiternum < 3; j_orbiternum++)
            {
                ChildSpaceShip[i].OrbiterEngineObj[j_orbiternum] = Instantiate(OrbiterEnginePrefab, ChildSpaceShip[i].OrbiterObj.transform);
                ChildSpaceShip[i].OrbiterEngineObj[j_orbiternum].transform.localPosition = ChildSpaceShip[i].OrbiterEngineGene[j_orbiternum];
                ChildSpaceShip[i].OrbiterEngineObj[j_orbiternum].transform.localRotation = Quaternion.identity;
            }

            //ChildSpaceShip[i].gameObject.AddComponent<BoxCollider>();
            //ChildSpaceShip[i].GetComponent<BoxCollider>().isTrigger = true;
            //ChildSpaceShip[i].GetComponent<BoxCollider>().center = new Vector3(0, 0, 1);
            //ChildSpaceShip[i].GetComponent<BoxCollider>().size = new Vector3(5, 20, 3);

            ChildSpaceShip[i].transform.localRotation = Quaternion.Euler(ChildSpaceShip[i].AngleGene[0], ChildSpaceShip[i].AngleGene[1], ChildSpaceShip[i].AngleGene[2]);
        }
    }

       public void putPrefabToSelGameObj(int i)
        {
                ChildSpaceShip[i].OrbiterObj = Instantiate(OrbiterPrefab, ChildSpaceShip[i].transform);
                ChildSpaceShip[i].OrbiterObj.transform.localPosition = new Vector3(0, 0, 0);
                ChildSpaceShip[i].OrbiterObj.transform.localRotation = Quaternion.identity;

                ChildSpaceShip[i].TankObj = Instantiate(ExternalTankPrefab, ChildSpaceShip[i].OrbiterObj.transform);
                ChildSpaceShip[i].TankObj.transform.localPosition = ChildSpaceShip[i].TankGene;
                ChildSpaceShip[i].TankObj.transform.localRotation = Quaternion.identity;
                ChildSpaceShip[i].TankObj.transform.localScale = ChildSpaceShip[9 / (i + 1)].TankObj.transform.localScale;

                for (int j_rocketnum = 0; j_rocketnum < 2; j_rocketnum++)
                {
                    ChildSpaceShip[i].RocketObj[j_rocketnum] = Instantiate(RocketPrefab, ChildSpaceShip[i].TankObj.transform);
                    ChildSpaceShip[i].RocketObj[j_rocketnum].transform.localPosition = ChildSpaceShip[i].RocketGene[j_rocketnum];
                    ChildSpaceShip[i].RocketObj[j_rocketnum].transform.localRotation = Quaternion.identity;
                    ChildSpaceShip[i].RocketObj[j_rocketnum].transform.localScale = ChildSpaceShip[9 / (i + 1)].RocketObj[j_rocketnum].transform.localScale;

                    ChildSpaceShip[i].RocketEngineObj[j_rocketnum] = Instantiate(RocketEnginePrefab, ChildSpaceShip[i].RocketObj[j_rocketnum].transform);
                    ChildSpaceShip[i].RocketEngineObj[j_rocketnum].transform.localPosition = ChildSpaceShip[i].RocketEngineGene[j_rocketnum];
                    ChildSpaceShip[i].RocketEngineObj[j_rocketnum].transform.localRotation = Quaternion.identity;
                    ChildSpaceShip[i].RocketEngineObj[j_rocketnum].transform.localScale = ChildSpaceShip[9 / (i + 1)].RocketEngineObj[j_rocketnum].transform.localScale;
        }

                for (int j_orbiternum = 0; j_orbiternum < 3; j_orbiternum++)
                {
                    ChildSpaceShip[i].OrbiterEngineObj[j_orbiternum] = Instantiate(OrbiterEnginePrefab, ChildSpaceShip[i].OrbiterObj.transform);
                    ChildSpaceShip[i].OrbiterEngineObj[j_orbiternum].transform.localPosition = ChildSpaceShip[i].OrbiterEngineGene[j_orbiternum];
                    ChildSpaceShip[i].OrbiterEngineObj[j_orbiternum].transform.localRotation = Quaternion.identity;
                    ChildSpaceShip[i].OrbiterEngineObj[j_orbiternum].transform.localScale = ChildSpaceShip[9 / (i + 1)].OrbiterEngineObj[j_orbiternum].transform.localScale;
        }

                ChildSpaceShip[i].transform.localRotation = Quaternion.Euler(ChildSpaceShip[i].AngleGene[0], ChildSpaceShip[i].AngleGene[1], ChildSpaceShip[i].AngleGene[2]);

        Debug.Log("put");
    }

    public void ChangeNoticeBoard()
    {
        UpperText.text = "Generation : " + nGeneration;

        //for (int i_GeneNum = 0; i_GeneNum < 1; i_GeneNum++)
        {
            NoticeText[0].text =
            "Gene " + nSelGene + " : " + "\n" +
            "RocketLeftY(" + Math.Round(ChildSpaceShip[nSelGene].RocketGene[0].y, 2) + ") " + "\n" +
			"RocketRightY(" + Math.Round(ChildSpaceShip[nSelGene].RocketGene[1].y, 2) + ") " + "\n" +
			"TankY(" + Math.Round(ChildSpaceShip[nSelGene].TankGene.y, 2) + ") " + "\n" +
			"REngineLeft(" + Math.Round(ChildSpaceShip[nSelGene].RocketEngineGene[0].x, 2) + ", " + Math.Round(ChildSpaceShip[nSelGene].RocketEngineGene[0].z, 2) + ") " + "\n" +
			"REngineRight(" + Math.Round(ChildSpaceShip[nSelGene].RocketEngineGene[1].x, 2) + ", " + Math.Round(ChildSpaceShip[nSelGene].RocketEngineGene[1].z, 2) + ") " + "\n" +
			"OEngine0(" + Math.Round(ChildSpaceShip[nSelGene].OrbiterEngineGene[0].x, 2) + ", " + Math.Round(ChildSpaceShip[nSelGene].OrbiterEngineGene[0].z, 2) + ") " + "\n" +
			"OEngine1(" + Math.Round(ChildSpaceShip[nSelGene].OrbiterEngineGene[1].x, 2) + ", " + Math.Round(ChildSpaceShip[nSelGene].OrbiterEngineGene[1].z, 2) + ") " + "\n" +
			"OEngine2(" + Math.Round(ChildSpaceShip[nSelGene].OrbiterEngineGene[2].x, 2) + ", " + Math.Round(ChildSpaceShip[nSelGene].OrbiterEngineGene[2].z, 2) + ") " + "\n" +
			"Angle(" + (int)ChildSpaceShip[nSelGene].AngleGene[0] + ", " + (int)ChildSpaceShip[nSelGene].AngleGene[1] + ", " + (int)ChildSpaceShip[nSelGene].AngleGene[2] + ") ";
        }
    }

    void ShowResult()
    {
        for(int i = 0; i < 10; i ++)
        {
            if(ChildSpaceShip[i].isAlive == false)
            {
                ResultText[i].text = "Gene " + i + " : Height : " + ChildSpaceShip[i].fHeight + ", Distance : " + ChildSpaceShip[i].fDistance + ", Fitness : " + ChildSpaceShip[i].fFitness;
            }
        }
    }

    void HideResult()
    {
        for (int i = 0; i < 10; i++)
        {
            ResultText[i].text = "Gene " + i + " : Height : " + ", Distance : "+ ", Fitness : ";
        }
    }

    void SetParents()
    {
        for(int i = 0; i < 10; i ++)
        {
            fGenefitnessRank[i] = ChildSpaceShip[i].fFitness;

            nRank[i] = i;
        }

        for (int i = 0; i < 9; i ++)
        {
            for(int j = 0; j < 9 - i; j++)
            {
                if(fGenefitnessRank[j] < fGenefitnessRank[j + 1])
                {
                    float tmp = fGenefitnessRank[j];
                    fGenefitnessRank[j] = fGenefitnessRank[j + 1];
                    fGenefitnessRank[j + 1] = tmp;

                    int nTmp = nRank[j];
                    nRank[j] = nRank[j + 1];
                    nRank[j + 1] = nTmp;
                }
            }
        }
        
        for(int i = 0; i < 2; i ++)
        {

            for (int j_AngleNum = 0; j_AngleNum < 3; j_AngleNum++)
            {
                ParentSpaceShip[i].AngleGene[j_AngleNum] = ChildSpaceShip[nRank[i]].AngleGene[j_AngleNum];
            }

            for (int j_OEnum = 0; j_OEnum < 3; j_OEnum++)
            {
                ParentSpaceShip[i].OrbiterEngineGene[j_OEnum] = ChildSpaceShip[nRank[i]].OrbiterEngineGene[j_OEnum];
            }

            for(int j_REnum = 0; j_REnum < 2; j_REnum++)
            {
                ParentSpaceShip[i].RocketEngineGene[j_REnum] = ChildSpaceShip[nRank[i]].RocketEngineGene[j_REnum];
            }

            for (int j_Rnum = 0; j_Rnum < 2; j_Rnum++)
            {
                ParentSpaceShip[i].RocketGene[j_Rnum] = ChildSpaceShip[nRank[i]].RocketGene[j_Rnum];
            }

            ParentSpaceShip[i].TankGene = ChildSpaceShip[nRank[i]].TankGene;
        }

        ParentNoticeText.text = "Parent Gene 0 : Gene " + nRank[0] + ", Fitness : " + fGenefitnessRank[0] + " (BEST)" + "\n" +
                                "Parent Gene 0 : Gene " + nRank[1] + ", Fitness : " + fGenefitnessRank[1];

    }


    public void LaunchSpaceship()
    {
        for(int i = 0; i < 10; i ++)
        {
            ChildSpaceShip[i].isRocketForce = true;
            ChildSpaceShip[i].BodyRig.useGravity = true;
        }

        Invoke("CallStopRocket", 5.0f);
        Invoke("CallStopOrbiter", 10.0f);
    }

    void CallStopRocket()
    {
        for(int i = 0; i < 10; i ++)
        {
            ChildSpaceShip[i].StopRocketForceSpaceship();
        }
    }

    void CallStopOrbiter()
    {
        for (int i = 0; i < 10; i++)
        {
            ChildSpaceShip[i].StopOrbiterForceSpaceship();
        }
    }

    void CheckAllAlive()
    {
        int cnt = 0;

        for (int i = 0; i < 10; i++)
        {
            if(ChildSpaceShip[i].isAlive)
            {
                break;
            }
            else
            {
                cnt++;
            }
        }
        if(cnt == 10)
        {
            NextGenerationObj.GetComponent<Button>().interactable = true;
            SetParents();
        }
    }

	public void PrevSelGene()
	{
		nSelGene = Math.Max(nSelGene - 1, 0);
		ChangeNoticeBoard();
	}

	public void NextSelGene()
	{
		nSelGene = Math.Min(nSelGene + 1, 9);
		ChangeNoticeBoard();
	}

	public void ToggleFollow()
	{
		bFollow = !bFollow;

		if ( !bFollow )
		{
			Camera.main.transform.position = InitCamPosVec3;
		}
	}
}
