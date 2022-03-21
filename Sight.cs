using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    public Transform headTransform;
    public Transform eyeTarget;
    private Vector3 eyeTargetPosition;
    private float idleLookX = 0f;
    private Vector3 focusLookPosition;

    public enum SightState { idle, focus}
    public SightState currentSightState = SightState.idle;
    public List<GameObject> seenEnemies = new List<GameObject>();
    public GameObject FocusedEnemy;
    private enum Team { A, Z}
    private Team myTeam = Team.Z;
    public bool EnemyInView = false;
    public string lastSeenObject = "";
    float maxSightAngle = 60f;
    // Start is called before the first frame update
    void Start()
    {
        InitializeSight();
    }

    // Update is called once per frame
    void Update()
    {
       // ManageSightState();

        switch (currentSightState)
        {
            case SightState.idle:
                eyeTargetPosition = transform.position + transform.up + transform.forward * 5f + transform.right * idleLookX;//idleLookPosition;
                break;
            case SightState.focus:
                eyeTargetPosition = focusLookPosition;
                break;
        }
        eyeTarget.position = eyeTargetPosition;// Vector3.Slerp(eyeTarget.position, eyeTargetPosition, Time.deltaTime * 10f);
    }
    public void InitializeSight()
    {
        GetTeam();
        FocusStraightAhead();
        GetNewIdleLookPosition();
        StartCoroutine(IdleLookAround());
        StartCoroutine(SeeEnemies());
    }
    private void OnDisable()
    {
        //Debug.Log("sight is dead");
        seenEnemies.Clear();
    }
    private void GetTeam()
    {
        if (gameObject.CompareTag("A"))
            myTeam = Team.A;
        else
            myTeam = Team.Z;
    }
    private IEnumerator SeeEnemies()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(Random.Range(0.25f,1f));
            RayCastEnemies();
            FocusedEnemy = GetClosestEnemy();
            EnemyInView = RayCastFocusedEnemy();
        }
    }
    private void ManageSightState()
    {
        if (EnemyInView)
        {
            focusLookPosition = FocusedEnemy.transform.GetComponent<Unit>().unitCenter.position;
            currentSightState = SightState.focus;
        }
        else
            currentSightState = SightState.idle;
    }
    
    private void RayCastEnemies()
    {
        //WeedOutSeenEnemies();

        if (myTeam == Team.Z)
            RayCastTeam("A", Units.A_Units);
        else
            RayCastTeam("Z", Units.Z_Units);
    }
   
    private GameObject GetClosestEnemy()
    {
        //remove dead enemies from list of seen enemies
        for (int j = 0; j < seenEnemies.Count; j++)// (GameObject e in seenEnemies)
        {
            if (!seenEnemies[j].activeSelf)
                seenEnemies.Remove(seenEnemies[j]);
        }
        GameObject _closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        for (int i = 0; i < seenEnemies.Count; i++)// (GameObject e in seenEnemies)
        {
            float testedDistance = Vector3.Distance(transform.position, seenEnemies[i].transform.position);
            if (testedDistance < closestDistance)
            {
                closestDistance = testedDistance;
                _closestEnemy = seenEnemies[i];
            }
        }
        return _closestEnemy;
    }
    private void RayCastTeam(string teamName, List<GameObject> polledList)
    {
        RaycastHit hit;
        for (int i = 0; i < polledList.Count; i++) //(GameObject e in polledList)
        {
            Vector3 directionToEnemy = polledList[i].transform.GetComponent<Unit>().unitCenter.position - headTransform.position;
            float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

            float distanceToEnemy = Vector3.Distance(polledList[i].transform.GetComponent<Unit>().unitCenter.position, headTransform.position);
            //testing only angle will make it so that units can be snuck up on, i.e. this unit can only see, not "hear"
            //TO DO: make it test based on distance so that they can't be snuck up on. Only a "guarding" type behaviour should be able to be snuck up on
            if (angleToEnemy > maxSightAngle && distanceToEnemy > 5f)//make it so that the unit won't stand there stupidly as an enemy is standing right behind them
                continue;

            Ray ray = new Ray(headTransform.position, directionToEnemy);

            if (Physics.Raycast(ray, out hit))
            {
                lastSeenObject = hit.transform.gameObject.name;
                if (hit.transform.gameObject.CompareTag(teamName))
                {
                    if (!seenEnemies.Contains(polledList[i]))
                    {
                        seenEnemies.Add(polledList[i]);
                    }
                }
            }
        }
    }
    private bool RayCastFocusedEnemy()
    {
        if (!FocusedEnemy)
            return false;

        RaycastHit hit;
        Vector3 directionToEnemy = FocusedEnemy.transform.GetComponent<Unit>().unitCenter.position - headTransform.position;
        float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);
        //we don't want the enemy to be considered in view if it's angle is too large, this will make the unit run and turn around until it can aim naturally
        if (angleToEnemy > maxSightAngle)
            return false;
        Ray ray = new Ray(headTransform.position, directionToEnemy);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject == FocusedEnemy)
                return true;
        }
        return false;
    }
    private IEnumerator IdleLookAround()
    {
        while(enabled)
        {
            yield return new WaitForSeconds(1f);
            GetNewIdleLookPosition();
        }
    }
    private void FocusStraightAhead()
    {
        focusLookPosition = headTransform.position + transform.forward * 5f;
    }
    public void SetNewFocus(Vector3 pos)
    {
        focusLookPosition = pos;
    }
    private void GetNewIdleLookPosition()
    {
        idleLookX = Random.Range(-5f, 5f);
            
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(eyeTarget.position, 0.25f);
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position + transform.up + transform.forward * 5f + transform.right * idleLookX, 0.5f);

    }
}
