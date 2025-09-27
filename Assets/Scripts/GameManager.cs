using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Wheel;

public class GameManager : MonoBehaviour
{
    public BallController ballController;
    private bool spinning = false;

    [ContextMenu("Spin")]
    public async void Spin()
    {
        ballController.RunBallRoutineAsync(destroyCancellationToken);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !spinning)
        {
            spinning = true;
            Spin();
        }
    }
}