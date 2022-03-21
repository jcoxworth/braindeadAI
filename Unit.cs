using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Range(-1f, 1f)]
    public float speed = 0.0f;
    [Range(1f, 10f)]
    public float shootingSkill = 5f;
    [Range(0f, 1f)]
    public float bravery = 0.25f;
    [Range(0f, 1f)]
    public float cowardess = 0.25f;
    public Transform unitCenter;
    public int UnitHealth = 100;
    private int maxHealth;
    private AICharacterMovement aiMovement;
    public float healthPercentage = 1f;
    private float speedLoss = 0f;
    // Start is called before the first frame update
    private void OnEnable()
    {
        InitializeUnit();
    }
    public void InitializeUnit()
    {
        GetComponent<AggressiveAI>().enabled = true;
        GetComponent<CharacterAiming>().TurnAllRigsOn();
        GetComponent<Animator>().SetBool("Victory", false);
        //set the max health to whatever health was set before the unit was enabled
        maxHealth = UnitHealth;
        //set the movement speed, like setting a skill
        speedLoss = 0f;
        SetUnitSpeed();
        //speed loss will increase as the unit is damaged
        //set the center point for others to aim at 
        if (!unitCenter)
            unitCenter = transform;
        //Set the team
        if (transform.gameObject.CompareTag("A"))
            Units.A_Units.Add(gameObject);
        if (transform.gameObject.CompareTag("Z"))
            Units.Z_Units.Add(gameObject);
    }
    public void ReactivateUnit()
    {
        GetComponent<AggressiveAI>().enabled = true;
        GetComponent<CharacterAiming>().TurnAllRigsOn();
        GetComponent<Animator>().SetBool("Victory", false);
        //set the movement speed, like setting a skill
        speedLoss = 0f;
        SetUnitSpeed();
    }
    public void SetUnitSpeed()
    {
        speedLoss = (1 - healthPercentage) * -1.5f; //maximum speed loss is a quarter of normal full speed
        aiMovement = GetComponent<AICharacterMovement>();
        if (!aiMovement)//if there is no component just get out
            return;

        //clamp the agent speed os taht it doesn't get lower than 1
        float agentMoveSpeed = Mathf.Clamp(3.5f + speed + speedLoss, 1f, 4.5f);
        float animMoveSpeed = 1f + speed + speedLoss;

        aiMovement.SetMovementSpeed(animMoveSpeed, agentMoveSpeed);
    }

    public void ChangeHealth(int amount)
    {
        UnitHealth += amount;
        healthPercentage = UnitHealth / maxHealth;
        SetUnitSpeed();

        if (UnitHealth <= 0)
            UnitDie();
    }
    public void UnitVictory()
    {
        GetComponent<AggressiveAI>().enabled = false;
        GetComponent<CharacterAiming>().TurnAllRigsOff();
        GetComponent<Animator>().SetBool("Victory", true);
    }
    public void UnitDie()
    {
        RemoveUnitFromList();
        gameObject.SetActive(false);
        /*
        GetComponent<AggressiveAI>().enabled = false;
        GetComponent<CharacterAiming>().TurnAllRigsOff();
        GetComponent<Animator>().SetBool("Dead", true);

        GetComponent<AIDestination>().enabled = false;
        GetComponent<Sight>().enabled = false;
        GetComponent<CharacterController>().enabled = false;
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;

        */
    }
    private void RemoveUnitFromList()
    {
        if (transform.gameObject.CompareTag("A"))
        {
            if (Units.A_Units.Contains(gameObject))
                Units.A_Units.Remove(gameObject);

            if (!Units.A_Units_Dead.Contains(gameObject))
                Units.A_Units_Dead.Add(gameObject);
        }
        if (transform.gameObject.CompareTag("Z"))
        {
            if (Units.Z_Units.Contains(gameObject))
                Units.Z_Units.Remove(gameObject);

            if (!Units.Z_Units_Dead.Contains(gameObject))
                Units.Z_Units_Dead.Add(gameObject);
        }
    }
}
