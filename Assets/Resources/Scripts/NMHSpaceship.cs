using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NMHSpaceship : MonoBehaviour
{
    public Vector3[] RocketEngineGene;
    public Vector3[] OrbiterEngineGene;

    public Vector3 TankGene;
    public Vector3[] RocketGene;

    public float[] AngleGene;

    public GameObject[] RocketObj;
    public GameObject[] RocketEngineObj;
    public GameObject OrbiterObj;
    public GameObject[] OrbiterEngineObj;
    public GameObject TankObj;

    public Rigidbody BodyRig;

    public float fOrbiterEngineForce = 30.0f;
    public float fRocketEngineForce = 30.0f;

    public float fHeight;
    public float fDistance;
    public float fFitness;

    public Vector3 FirstVec3;
    public Vector3 LastVec3;

    public bool isRocketForce = false;
    public bool isOrbiterForce = false;
    public bool isUp = true;
    public bool isAlive = true;

    void Start ()
    {
        InitializeObjs();
    }
	
	void Update ()
    {
        ControlSpaceship();
        CheckHeight();
    }

    void InitializeObjs()
    {
        isRocketForce = false;

        BodyRig = GetComponent<Rigidbody>();

        RocketEngineGene = new Vector3[2];
        OrbiterEngineGene = new Vector3[3];
        RocketGene = new Vector3[2];

        AngleGene = new float[3];

        RocketObj = new GameObject[2];
        RocketEngineObj = new GameObject[2];
        OrbiterEngineObj = new GameObject[3];

        FirstVec3 = transform.localPosition;
    }

    void ControlSpaceship()
    {

        RocketForceSpaceship();
        OrbiterForceSpaceship();
    }

    void RocketForceSpaceship()
    {
        if (isRocketForce)
        {
            BodyRig.AddForceAtPosition(RocketEngineObj[0].transform.up * fRocketEngineForce, RocketEngineObj[0].transform.position, ForceMode.Force);
            BodyRig.AddForceAtPosition(RocketEngineObj[1].transform.up * fRocketEngineForce, RocketEngineObj[1].transform.position, ForceMode.Force);
        }
    }

    public void StopRocketForceSpaceship()
    {
        isRocketForce = false;

        Debug.Log("Rocket Force Stopeed!");

        isOrbiterForce = true;

        //DetachRocket();
    }

    //void DetachRocket()
    //{

    //}

    void OrbiterForceSpaceship()
    {
        if (isOrbiterForce)
        {
            BodyRig.AddForceAtPosition(OrbiterEngineObj[0].transform.up * fOrbiterEngineForce, OrbiterEngineObj[0].transform.localPosition, ForceMode.Force);
            BodyRig.AddForceAtPosition(OrbiterEngineObj[1].transform.up * fOrbiterEngineForce, OrbiterEngineObj[1].transform.localPosition, ForceMode.Force);
            BodyRig.AddForceAtPosition(OrbiterEngineObj[2].transform.up * fOrbiterEngineForce, OrbiterEngineObj[2].transform.localPosition, ForceMode.Force);
        }
    }

    public void StopOrbiterForceSpaceship()
    {
        isOrbiterForce = false;

        Debug.Log("Orbiter Force Stopeed!");
    }

    void CheckHeight()
    {
        if(BodyRig.velocity.y < 0 && isUp)
        {
            isUp = false;
            fHeight = transform.localPosition.y;
        }

        if(transform.localPosition.y < 5 && isAlive)
        {
            isAlive = false;
            LastVec3 = transform.localPosition;

            fDistance = Vector3.Distance(FirstVec3, LastVec3);

            fFitness = fHeight + fDistance;
        }
    }
}
