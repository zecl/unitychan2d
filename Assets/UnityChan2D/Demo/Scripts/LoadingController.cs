using UnityEngine;
using System.Collections;
using UniRx;

public class LoadingController : ObservableMonoBehaviour
{
    [SceneName]
    public string nextLevel;

    new public IEnumerator Start()
    {
        yield return new WaitForSeconds(3);

        Application.LoadLevel(nextLevel);
    }
}
