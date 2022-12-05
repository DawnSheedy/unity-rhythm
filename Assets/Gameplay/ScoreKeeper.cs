using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public int maxScore = 1000000;
    public float PerfectWeight = 0.5f;
    public float GreatWeight = 0.3f;
    public float GoodWeight = 0.2f;
    private float pointsPerPerfect = 0;
    private float pointsPerGreat = 0;
    private float pointsPerGood = 0;
    public float currentScore = 0;
    public Rank currentRank = Rank.PreScore;
    private int notesJudged = 0;
    private int totalNotes;
    // Start is called before the first frame update

    public void ProcessJudgement(Judgement judgement) {
        notesJudged++;
        switch(judgement) {
            case Judgement.Perfect:
                currentScore += pointsPerPerfect;
                break;
            case Judgement.Great:
                currentScore += pointsPerGreat;
                break;
            case Judgement.Good:
                currentScore += pointsPerGood;
                break;
        }
        UpdateRank();
    }

    void UpdateRank() {
        float percentageThroughSong = (float)notesJudged/(float)totalNotes;

        if (percentageThroughSong < .1f) {
            return;
        }

        float maxPointsAtPosition = maxScore*percentageThroughSong;
        float currentPercentageRank = currentScore/maxPointsAtPosition;

        if (currentPercentageRank >= 0.9f) {
            currentRank = Rank.SSS;
        } else if (currentPercentageRank >= 0.8f) {
            currentRank = Rank.SS;
        } else if (currentPercentageRank >=0.7f) {
            currentRank = Rank.S;
        } else if (currentPercentageRank >=0.6f) {
            currentRank = Rank.AAA;
        } else if (currentPercentageRank >=0.5f) {
            currentRank = Rank.AA;
        } else if (currentPercentageRank >=0.4f) {
            currentRank = Rank.A;
        } else if (currentPercentageRank >=0.3f) {
            currentRank = Rank.B;
        } else if (currentPercentageRank >=0.2f) {
            currentRank = Rank.C;
        } else if (currentPercentageRank >=0.1f) {
            currentRank = Rank.D;
        } else {
            currentRank = Rank.F;
        }
    }

    public void ReportNoteCount(int notes) {
        totalNotes = notes;
        float pointsPerNote = (float)maxScore/((float)totalNotes*PerfectWeight);
        pointsPerPerfect = pointsPerNote*PerfectWeight;
        pointsPerGreat = pointsPerNote*GreatWeight;
        pointsPerGood = pointsPerNote*GoodWeight;
    }
}
