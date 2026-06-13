using System.Collections;
using UnityEngine;

public class HangerScript : MonoBehaviour
{
    public GameObject shootingTargetPrefab;
    public TargetType targetType = TargetType.Infinite;
    public int numberOfTargets = 5;
    public float spawnDelay = 3f;

    public Transform startPoint, endPoint;
    public void Start()
    {
        if (startPoint != null && endPoint != null)
        {
            StartCoroutine(spwawnRoutine());
        }
    }

    IEnumerator spwawnRoutine()
    {

        if (targetType == TargetType.finite)
        {
            if (numberOfTargets > 0)
            {
                numberOfTargets--;
                ShootingTarget shootingTarget = Instantiate(shootingTargetPrefab, transform.position, Quaternion.identity).GetComponent<ShootingTarget>();
                shootingTarget.StartMoving(startPoint, endPoint);
                yield return new WaitForSeconds(spawnDelay);
                StartCoroutine(spwawnRoutine());
            }
        }
        else {
            ShootingTarget shootingTarget = Instantiate(shootingTargetPrefab, transform.position, Quaternion.identity).GetComponent<ShootingTarget>();
            shootingTarget.StartMoving(startPoint, endPoint);
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(spwawnRoutine());

        }
    }

}

public enum TargetType
{
    Infinite,finite
}