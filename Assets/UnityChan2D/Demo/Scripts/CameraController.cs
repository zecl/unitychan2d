using System;
using System.Collections;
using UnityEngine;
using UniRx;
using System.Threading;

[RequireComponent(typeof(Camera))]
public class CameraController : ObservableMonoBehaviour
{
    public Transform target;
    public Transform stopPosition;

    [SceneName]
    public string nextLevel;

    private Camera m_camera;

    public override void Awake()
    {
        this.LateUpdateAsObservable()
            .Subscribe(_ =>
            {
                var right = m_camera.ViewportToWorldPoint(Vector2.right);
                var center = m_camera.ViewportToWorldPoint(Vector2.one * 0.5f);

                if (center.x < target.position.x)
                {
                    var pos = m_camera.transform.position;

                    if (Math.Abs(pos.x - target.position.x) >= 0.0000001f)
                    {
                        m_camera.transform.position = new Vector3(target.position.x, pos.y, pos.z);
                    }
                }

                if (stopPosition.position.x - right.x < 0)
                {
                    StartCoroutine(INTERNAL_Clear());
                    enabled = false;
                }
            });

        base.Awake();
        m_camera = GetComponent<Camera>();
    }

    private IEnumerator INTERNAL_Clear()
    {
        var player = GameObject.FindGameObjectWithTag("Player");

        if (player)
        {
            player.SendMessage("Clear", SendMessageOptions.DontRequireReceiver);
        }

        yield return new WaitForSeconds(3);

        Application.LoadLevel(nextLevel);
    }
}
