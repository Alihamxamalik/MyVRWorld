using DG.Tweening;
using UnityEngine;

public class ShootingTarget : MonoBehaviour
{
    public Transform startPoition;
    public Transform endPoition;
    public float time = 5f;
    public bool isMoving = true;

    public void StartMoving(Transform start,Transform end) { 
    
        if (start != null) startPoition = start;
        if (end != null) endPoition = end;
        isMoving = true;

        transform.DORotate(new Vector3(0, 0, 0), 1f);
        transform.DOMove(endPoition.position, time).OnComplete(() => {

            transform.DOKill();
            Destroy(gameObject);
        });

    }



}
