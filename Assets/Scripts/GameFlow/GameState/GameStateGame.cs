using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStateGame : GameState
{
    public GameObject gameUI;
    [SerializeField] private TextMeshProUGUI fishCount;
    [SerializeField] private TextMeshProUGUI scoreCount;
    [SerializeField] private AudioClip gameLoopMusic;
    public override void Construct()
    {
        GameManager.Instance.motor.ResumePlayer();
        GameManager.Instance.ChangeCamera(GameCamera.Game);

        GameStats.Instance.OnCollectFish += OnCollectFish;
        GameStats.Instance.OnScoreChange += OnScoreChange;
        gameUI.SetActive(true);

        AudioManager.Instance.PlayMusicWithXFade(gameLoopMusic, 0.5f);
    }
    private void OnScoreChange(float scoreAmount)
    {
        scoreCount.text = scoreAmount.ToString("000000");
    }
    private void OnCollectFish(int fishAmount)
    {
        fishCount.text = fishAmount.ToString("000");
    }
    public override void Destruct()
    {
        gameUI.SetActive(false);

        GameStats.Instance.OnCollectFish -= OnCollectFish;
        GameStats.Instance.OnScoreChange -= OnScoreChange;

    }
    public override void UpdateState()
    {
        GameManager.Instance.worldGeneration.ScanPosition();
        GameManager.Instance.sceneChunkGeneration.ScanPosition();
    }
}
