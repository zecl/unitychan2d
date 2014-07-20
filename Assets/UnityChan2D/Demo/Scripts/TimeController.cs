using UnityEngine;
using System.Collections;
using UniRx;

public class TimeController : ObservableMonoBehaviour
{
    public int time;
    [SceneName]
    public string nextLevel;

    public GUIText timer;

    public override void Awake()
    {
        this.UpdateAsObservable()
            .Subscribe(_ => {
                int remainingTime = time - Mathf.FloorToInt(Time.timeSinceLevelLoad * 2.5f);

                if (0 <= remainingTime)
                {
                    timer.text = remainingTime.ToString("000");
                }
                else
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    if (player)
                    {
                        StartCoroutine(LoadNextLevel());
                        enabled = false;
                    }
                }
            });

        base.Awake();
    }

    private IEnumerator LoadNextLevel()
    {
        var player = GameObject.FindGameObjectWithTag("Player");

        if (player)
        {
            player.SendMessage("TimeOver", SendMessageOptions.DontRequireReceiver);
        }

        yield return new WaitForSeconds(3);

        Application.LoadLevel(nextLevel);
    }
}
