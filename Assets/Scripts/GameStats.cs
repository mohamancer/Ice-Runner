using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance { get { return instance; } }
    private static GameStats instance;

    private void Awake()
    {
        instance = this;
    }
    // Score
    public float score;
    public float highScore;
    public float distanceModifier = 1.5f;

    // Fish
    public int totalFish;
    public int FishCollectedThisSession;
    public float pointsPerFish = 10.0f;
    public AudioClip fishCollectSoundSFX; 

    //  Internal cooldown
    private float lastScoreUpdate;
    private float scoreUpdateDelta = 0.2f;

    // Action
    public Action<int> OnCollectFish;
    public Action<float> OnScoreChange;

    private void Update()
    {
        float s = GameManager.Instance.motor.transform.position.z * distanceModifier;
        s += FishCollectedThisSession * pointsPerFish;
        if (s > score)
        {
            score = s;
            if (Time.time - lastScoreUpdate > scoreUpdateDelta) 
            {
                lastScoreUpdate = Time.time;
                OnScoreChange?.Invoke(score);
            }
                
        }

    }
    public void CollectFish()
    {
        FishCollectedThisSession++;
        OnCollectFish?.Invoke(FishCollectedThisSession);
        AudioManager.Instance.PlaySFX(fishCollectSoundSFX, 0.7f);
    }

    public void ResetSession()
    {
        score = FishCollectedThisSession = 0;

        OnCollectFish?.Invoke(FishCollectedThisSession);
        OnScoreChange?.Invoke(score);
    }
}
