using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveAI : MonoBehaviour
{
    Sight sight;
    AIDestination aiDestination;
    CharacterAiming characterAiming;
    Gun gun;
    public bool isTeamA = true;
    public float actionTime;
    [Range(0f, 3f)] public float Base_AggressiveTime = 3f;
    [Range(0f, 3f)] public float Base_RetreatTime = 3f;

    [Range(0f, 3f)] public float AggressiveTime = 3f;
    [Range(0f, 3f)] public float RetreatTime = 3f;

    public int tacticalCycles = 0;
    public enum AggressiveState { searching, attacking, retreating}
    public AggressiveState currentState = AggressiveState.searching;
    
    private List<GameObject> coverTransformsInRange = new List<GameObject>();
    public Transform coverTransform;
    // Start is called before the first frame update
    void OnEnable()
    {
        InitializeAggressive();
    }
    public void InitializeAggressive()
    {
        isTeamA = gameObject.CompareTag("A");
        sight = GetComponent<Sight>();
        aiDestination = GetComponent<AIDestination>();
        characterAiming = GetComponent<CharacterAiming>();
        gun = GetComponentInChildren<Gun>();
        AggressiveTime = Base_AggressiveTime;
        RetreatTime = Base_RetreatTime;
        //cycle strategy sets the cover area, the aggressive time, and the retreat time
        tacticalCycles = 0;
        CycleStrategy();
    }
    // Update is called once per frame
    void Update()
    {
        ManageActionTime();
        MangeMovement();
        ManageState();
        ManageWeaponFiring();
    }
    void ManageWeaponFiring()
    {
        if (characterAiming.isAiming)
        {
            gun.Fire();
        }
    }
    void ManageState()
    {
        if (sight.EnemyInView)
        {
            if (actionTime > RetreatTime)
                currentState = AggressiveState.attacking;
            else
                currentState = AggressiveState.retreating;
        }
        else
        {
            if (actionTime > RetreatTime)
                currentState = AggressiveState.searching;
            else
                currentState = AggressiveState.retreating;
        }
    }
    void MangeMovement()
    {
        switch (currentState)
        {
            case AggressiveState.attacking:
                if (sight.FocusedEnemy)
                    Aggressive();
                else
                    currentState = AggressiveState.searching;
                break;
            case AggressiveState.searching:
                Searching();
                break;
            case AggressiveState.retreating:
                Retreating();
                break;
        }
    }
    void Aggressive()
    {
        if (Vector3.Distance(transform.position, sight.FocusedEnemy.transform.position) > 10f)
        {
            characterAiming.isAiming = false;
            aiDestination.SetTarget(sight.FocusedEnemy.transform.position);
            sight.currentSightState = Sight.SightState.focus;
            sight.SetNewFocus(sight.FocusedEnemy.transform.GetComponent<Unit>().unitCenter.position);
        }
        else
        {
            characterAiming.isAiming = true;
            aiDestination.SetTarget(transform.position + transform.forward);
            sight.currentSightState = Sight.SightState.focus;
            sight.SetNewFocus(sight.FocusedEnemy.transform.GetComponent<Unit>().unitCenter.position);
        }
    }
    void Searching()
    {
        if (sight.FocusedEnemy)
        {
            aiDestination.SetTarget(sight.FocusedEnemy.transform.position);
        }
        else
        {
            //if you dont' have an enemy in mind, just run to the center of where the enemies are
            //TO DO: maybe make this only happen if the unit has a lot of bravery?

            if (isTeamA)
                aiDestination.SetTarget(FindCenterOfGroup._ZCenter);
            else
                aiDestination.SetTarget(FindCenterOfGroup._ACenter);
        }

        characterAiming.isAiming = false;
        sight.currentSightState = Sight.SightState.idle;
    }
    void Retreating()
    {
        characterAiming.isAiming = false;
        sight.currentSightState = Sight.SightState.idle;
        if (!coverTransform)
            coverTransform = GetRandomCoverInRange(transform);
            if (!coverTransform)
                coverTransform = transform;
        aiDestination.SetTarget(coverTransform.position);
    }
    void ManageActionTime()
    {
        if (actionTime > 0f)
        {
            actionTime -= Time.deltaTime;
        }
        else
        {
            CycleStrategy();
        }
    }
    public void ChangeHealth(int amount)
    {
        actionTime = RetreatTime;
    }
    void CycleStrategy()
    {
        //make the unit more aggressive as time passes
        AggressiveTime = Base_AggressiveTime + Random.Range(-1f, 1f) + tacticalCycles;
        RetreatTime = Base_RetreatTime + Random.Range(-1f, 1f);
        //this is where we cycle back again, and maybe the unit will have more action time or less action time depending on their performance
        actionTime = RetreatTime + AggressiveTime;
        //during this cycle, we can find a new cover area
        coverTransform = GetRandomCoverInRange(transform); //we can put the range in there later if we want
        tacticalCycles ++;
    }
    private Transform GetRandomCoverInRange(Transform unit, float range = 30f)
    {
        coverTransformsInRange.Clear();
        for (int i = 0; i <  Units.CoverAreas.Count; i++)
        {
            if (Vector3.Distance(Units.CoverAreas[i].transform.position, unit.position) < range)
            {
                coverTransformsInRange.Add(Units.CoverAreas[i]);
            }
        }

        //This just gets a cover object, like a box or a wall, the next code will get the position
        GameObject randomCoverArea = transform.gameObject;
        if (coverTransformsInRange.Count > 0)
        {
            randomCoverArea = coverTransformsInRange[Random.Range(0, coverTransformsInRange.Count)];
            //If we are taking cover from an enemy, we would want to be on the opposite side of a wall from them
            //this is where we get that opposite side

            if (sight.FocusedEnemy)
                return randomCoverArea.GetComponent<Cover>().GetCoverArea(sight.FocusedEnemy.transform);//the cover has the code where we find teh opposite side
            else
                return randomCoverArea.GetComponent<Cover>().transform;
        }
        else
        {
            //make them run away to their friends if they can't find cover
            if (isTeamA)
                return FindCenterOfGroup._AcenterTransform;
            else
                return FindCenterOfGroup._ZcenterTransform;
        }
        

    }
    
}
