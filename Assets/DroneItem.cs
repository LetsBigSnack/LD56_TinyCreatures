using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneItem : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector2 target;
    [SerializeField] private float wiggleRoom;
    [SerializeField] private GameObject sparkleEffect;
    private bool onBreak;

    private void OnEnable()
    {
        DroneSpawnManager.Instance.OnChangeTargetValue += MoveTowardsNewTarget;
    }

    private void OnDisable()
    {
        DroneSpawnManager.Instance.OnChangeTargetValue -= MoveTowardsNewTarget;
    }

    public void OnClick()
    {
        DroneSpawnManager.Instance.CreateNewDrone();
        GameObject newSparkleEffect = Instantiate(sparkleEffect, DroneSpawnManager.Instance.Parent.transform);
        newSparkleEffect.transform.position = transform.position;
        Destroy(gameObject);
    }

    void FixedUpdate()
    { 
        if(target != null && !onBreak) {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), target, step);

            if (Vector2.Distance(transform.position, target) < wiggleRoom)
            {
                StartCoroutine(takeABreak());
            }
        }
    }


    private void MoveTowardsNewTarget(Vector2 newTarget)
    {
        target = newTarget;
    }

    private IEnumerator takeABreak()
    {
        onBreak = true;
        yield return new WaitForSeconds(6f);
        onBreak = false;
        DroneSpawnManager.Instance.FindNewPoint();
    }
}
