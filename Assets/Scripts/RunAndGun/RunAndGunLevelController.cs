using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class RunAndGunLevelController : LevelManager
{
    [SerializeField] private Timer timer;
    [SerializeField] private CinemachineVirtualCamera cmn;
    [SerializeField] private Text score;
    
    public override void ShowGameOverWindow()
    {
        cmn.m_Follow = cmn.transform;
        base.ShowGameOverWindow();
        timer.StopTimer();
    }

    protected override void ShowWinWindow()
    {
        timer.StopTimer();
        winWindow.SetActive(true);
        character.enabled = false;
        score.text = $"Your score is: {timer.timerText.text}";
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")) ShowWinWindow();
    }
}
