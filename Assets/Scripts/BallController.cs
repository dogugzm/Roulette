using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private Transform wheelCenter;

    [SerializeField] private Wheel.WheelPocketManager pocketManager;

    [Header("Settings")] [SerializeField] private float orbitRadius = 1f;
    [SerializeField] private float orbitSpeed = 50f;
    [SerializeField] private float startDelay = 3f;
    [SerializeField] private float jumpHeight = 0.3f;
    [SerializeField] private float jumpDuration = 0.5f;
    [SerializeField] private int randomJumps = 3;

    private bool isJumping = false;

    public async Task RunBallRoutineAsync(CancellationToken cancellationToken)
    {
        float elapsed = 0f;
        float angle = 0f;

        //orbit first
        while (elapsed < startDelay)
        {
            cancellationToken.ThrowIfCancellationRequested();
            angle += orbitSpeed * Time.deltaTime;
            float rad = angle * Mathf.Deg2Rad;
            transform.position = wheelCenter.position + new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * orbitRadius;
            elapsed += Time.deltaTime;
            await Task.Yield();
        }

        // get target
        Transform targetPocket = pocketManager.GetPocketTransform(2);
        if (targetPocket == null)
        {
            Debug.LogWarning("Target pocket not found!");
            return;
        }

        Debug.Log("Targeting pocket: " + targetPocket.name);

        // go if closer
        float orbitTargetDistance = 4f;
        while (Vector3.Distance(transform.position, targetPocket.position) > orbitTargetDistance)
        {
            cancellationToken.ThrowIfCancellationRequested();
            angle += orbitSpeed * Time.deltaTime;
            float rad = angle * Mathf.Deg2Rad;
            transform.position = wheelCenter.position + new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * orbitRadius;
            await Task.Yield();
        }

        for (int i = 0; i < randomJumps; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await JumpToDynamicTargetAsync(targetPocket, 0.6f, cancellationToken);
        }

        await JumpToDynamicTargetAsync(targetPocket, 0f, cancellationToken);

        if (targetPocket != null)
        {
            transform.SetParent(targetPocket, true);
        }
    }


    private async Task JumpToDynamicTargetAsync(Transform target, float offsetRange,
        CancellationToken cancellationToken)
    {
        isJumping = true;
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        Vector3 randomOffset = offsetRange > 0f
            ? new Vector3(UnityEngine.Random.Range(-offsetRange, offsetRange), 0f,
                UnityEngine.Random.Range(-offsetRange, offsetRange))
            : Vector3.zero;

        while (elapsed < jumpDuration)
        {
            cancellationToken.ThrowIfCancellationRequested();
            float t = elapsed / jumpDuration;
            Vector3 currentTarget = target.position + randomOffset;
            float yOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            transform.position = Vector3.Lerp(startPos, currentTarget, t) + Vector3.up * yOffset;
            elapsed += Time.deltaTime;
            await Task.Yield();
        }

        transform.position = target.position + randomOffset;
        isJumping = false;
    }
}