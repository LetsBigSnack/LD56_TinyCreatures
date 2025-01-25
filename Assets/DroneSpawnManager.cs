using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSpawnManager : MonoBehaviour
{
    public static DroneSpawnManager Instance;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Canvas parent;
    [SerializeField] private GameObject dronePrefab;
    [SerializeField] private GameObject currentDrone;
    [SerializeField] private Collider2D flyZoneBoundry;
    
    private Vector2 target;
    public System.Action<Vector2> OnChangeTargetValue;

    private IEnumerator spawnNextDrone;

    public Canvas Parent
    {
        get { return parent; }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        CreateNewDrone();
    }

    private void OnDisable()
    {
        StopDrone();
    }

    public void CreateNewDrone()
    {
        spawnNextDrone = SpawnNextDrone();
        StartCoroutine(spawnNextDrone);
    }

    private void StopDrone()
    {
        if(currentDrone != null)
        {
            Destroy(currentDrone);
        }
        StopCoroutine(spawnNextDrone);
    }

    public void FindNewPoint()
    {
        Debug.Log("FindNewPoint");
        float randomX = Random.Range(flyZoneBoundry.bounds.min.x, flyZoneBoundry.bounds.max.x);
        float randomY = Random.Range(flyZoneBoundry.bounds.min.y, flyZoneBoundry.bounds.max.y);
        target = new Vector2(randomX, randomY);
        OnChangeTargetValue?.Invoke(target);
    }


    private IEnumerator SpawnNextDrone()
    {
        Debug.Log("SpawnedNewDrone");
        yield return new WaitForSeconds(Random.Range(0, 1));
        GameObject newDrone = Instantiate(dronePrefab, parent.transform);
        newDrone.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        currentDrone = newDrone;
        FindNewPoint();
    }

}
