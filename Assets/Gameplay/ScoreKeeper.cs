using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    The ScoreKeeper takes incoming judgements and processes them.
    It is in charge of tracking score throughout a round.
*/
public class ScoreKeeper : MonoBehaviour
{
    // Configuration
    [Tooltip("The maximum score a user can earn.")]
    public int maxScore = 1000000;
    [Tooltip("The weight of a perfect hit.")]
    public float PerfectWeight = 2.0f;
    [Tooltip("The weight of a great hit.")]
    public float GreatWeight = 1f;
    [Tooltip("The weight of a good hit.")]
    public float GoodWeight = 0.5f;
    [Tooltip("Weight of a miss, points are deducted")]
    public float MissWeight = 0f;

    // Values calculated at runtime.
    private float pointsPerPerfect = 0;
    private float pointsPerGreat = 0;
    private float pointsPerGood = 0;
    private float pointsPerMiss = 0;
    public float currentScore = 0;
    public Rank currentRank = Rank.PreScore;
    private int notesJudged = 0;
    private int totalNotes;

    // Entrypoint for new judgements delivered by the NoteControllers.
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
            case Judgement.Miss:
                currentScore += pointsPerMiss;
                break;
        }
        UpdateRank();
    }

    // Updates rank based on current score and maximum score available at present time
    void UpdateRank() {
        float percentageThroughSong = (float)notesJudged/(float)totalNotes;

        if (percentageThroughSong < .1f) {
            return;
        }

        float maxPointsAtPosition = maxScore*percentageThroughSong;
        float currentPercentageRank = currentScore/maxPointsAtPosition;

        // This is roughly based on a wiki article I found about Beatmania scoring.
        // TODO at some point make this more accurate to how Jubeat specifically is scored
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

    // Point for the EventDispatcher to inform the ScoreKeeper how many notes are in the upcoming song
    public void ReportNoteCount(int notes) {
        totalNotes = notes;
        float pointsPerNote = (float)maxScore/((float)totalNotes*PerfectWeight);
        pointsPerPerfect = pointsPerNote*PerfectWeight;
        pointsPerGreat = pointsPerNote*GreatWeight;
        pointsPerGood = pointsPerNote*GoodWeight;
        pointsPerMiss = pointsPerNote*MissWeight*-1f;
    }
}
