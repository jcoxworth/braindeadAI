using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Match : MonoBehaviour
{
    public string winLoseMessage = "";
    public enum MatchStatus { preMatch, inProgress, finished}
    public MatchStatus currentMatchStatus = MatchStatus.inProgress;

    public GameObject preMatchUI;
    public GameObject postMatchUI;

    public Slider teamSlider;
    public TMP_Text A_PointsTXT;
    public TMP_Text Z_PointsTXT;

    public TMP_Text WinLoseMessageTXT;
    public TMP_Text A_AwardedPointsTXT;
    public TMP_Text Z_AwardedPointsTXT;

    private int A_Points = 0;
    private int Z_Points = 0;
    private int A_AwardedPoints = 0;
    private int Z_AwardedPoints = 0;
    // Start is called before the first frame update
    void Start()
    {
        ContinueToNextMatch();
    }
    // Update is called once per frame
    void Update()
    {
        ManageMatchUI();
    }
    public void ManageMatchUI()
    {
        switch(currentMatchStatus)
        {
            case MatchStatus.preMatch:
                preMatchUI.SetActive(true);
                postMatchUI.SetActive(false);
                break;
            case MatchStatus.inProgress:
                preMatchUI.SetActive(false);
                postMatchUI.SetActive(false);
                CheckMatchStatus();
                break;
            case MatchStatus.finished:
                preMatchUI.SetActive(false);
                postMatchUI.SetActive(true);
                break;
        }
    }

    //These are methods for the UI buttons to call
    public void ContinueToNextMatch()
    {
        AddPointsToTeams();
        SetPreMatchUI();
        currentMatchStatus = MatchStatus.preMatch;
    }
    public void StartMatch()
    {
        currentMatchStatus = MatchStatus.inProgress;
    }
    public void FinishMatch()
    {
        currentMatchStatus = MatchStatus.finished;
    }
    //Finish Methods for match


    public void SetPreMatchUI()
    {
        A_PointsTXT.text = A_Points.ToString();
        Z_PointsTXT.text = Z_Points.ToString();
    }
    public void CheckMatchStatus()
    {
        if (Units.Z_Units.Count < 1)
        {
            AwardPoints(true);
            FinishMatch();
        }

        if (Units.A_Units.Count < 1)
        {
            AwardPoints(false);
            FinishMatch();
        }
    }
    public void AwardPoints(bool AteamWon)
    {
        int MatchReward = 500;


        if (AteamWon)
        {
            winLoseMessage = "A Team Wins!";
            //To do: put a match reward in there
            A_AwardedPoints = 0;
            A_AwardedPoints += MatchReward;
            foreach(GameObject g in Units.A_Units)
            {
                g.transform.GetComponent<Unit>().UnitVictory();
                A_AwardedPoints += 100;
            }

            //Reward Team Z for killing some of team A
            Z_AwardedPoints = 0;
            foreach(GameObject g in Units.A_Units_Dead)
            {
                Z_AwardedPoints += 20;
            }
        }
        else
        {
            winLoseMessage = "Z Team Wins!";
            Z_AwardedPoints = 0;
            Z_AwardedPoints += MatchReward;
            foreach (GameObject g in Units.Z_Units)
            {
                g.transform.GetComponent<Unit>().UnitVictory();
                Z_AwardedPoints += 100;
            }


            //Reward Team A for killing some of team Z
            A_AwardedPoints = 0;
            foreach (GameObject g in Units.Z_Units_Dead)
            {
                A_AwardedPoints += 20;
            }
        }

        WinLoseMessageTXT.text = winLoseMessage;
        A_AwardedPointsTXT.text = A_AwardedPoints.ToString();
        Z_AwardedPointsTXT.text = Z_AwardedPoints.ToString();

    }
    public void AddPointsToTeams()
    {
        A_Points += A_AwardedPoints;
        A_AwardedPoints = 0;
        Z_Points += Z_AwardedPoints;
        Z_AwardedPoints = 0;


    }
}
