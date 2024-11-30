using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JyEnemyScriptRanged : MonoBehaviour
{
    [SerializeField]
    private float detectionRadius = 100f;
    private Transform player;
    private NavMeshAgent agent;
    [SerializeField]
    private Material patrollingColour;
    [SerializeField]
    private Material chasingColour;
    [SerializeField]
    private float patrolTimer = 10f;
    private float currentTimer = 0f;

    [Header("Bullet Settings")]
    public GameObject bulletPrefab; // Drag your bullet prefab here
    public Transform firePoint;     // Where bullets are fired from
    public float bulletSpeed = 10f;
    public float bulletLifetime = 5f; // Max time bullet exists if it doesn't hit
    public float firingInterval = 3f;

    [SerializeField] public int NumberOfBullets = 5;
    public List<MonoBehaviour> currentNumberOfBullets = new List<MonoBehaviour>();

    List<Transform> waypointsList = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        GameObject waypointsCluster = NPCWaypoints.Instance.GetComponent<NPCWaypoints>().NPCWaypointsCluster;

        foreach(Transform t in waypointsCluster.transform)
        {
            waypointsList.Add(t);
        }
        GetComponent<Renderer>().material = patrollingColour;
        Vector3 firstPosition = waypointsList[Random.Range(0, waypointsList.Count)].position;
        agent.SetDestination(firstPosition);
    }
    // Update is called once per frame
    void Update()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        if(distanceFromPlayer < detectionRadius) //chase state
        {
            Debug.Log("jy Enter attacking state");
            GetComponent<Renderer>().material = chasingColour;
            if (player!=null && currentNumberOfBullets.Count < NumberOfBullets && !isSpawning)                                                                                                                                                                         
            {
                StartCoroutine(Attack());
            }
        }
        else //patrol state
        {
            currentTimer += Time.deltaTime;
            if(currentTimer >= patrolTimer)
            {
                Debug.Log(currentTimer + "Patrol timer");
                Debug.Log("jy Enter patrolling state");
                GetComponent<Renderer>().material = patrollingColour;
                agent.SetDestination(waypointsList[Random.Range(0, waypointsList.Count)].position);
                currentTimer = 0f;
            }
        }

        //clear bullets
        List<MonoBehaviour> toRemove = new List<MonoBehaviour>();
        foreach (MonoBehaviour bullet in currentNumberOfBullets)
        {
            toRemove.Add(bullet);
        }
        // Remove all entities marked for removal
        foreach (MonoBehaviour bullet in toRemove)
        {
            currentNumberOfBullets.Remove(bullet);
        }
        toRemove.Clear();
    }

    private bool isSpawning = false;
    private IEnumerator Attack()
    {
        isSpawning = true;
        OrientTowardsTarget(player);

        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        currentNumberOfBullets.Add(bulletScript);

        // Add a trail renderer to the bullet if it doesn't already have one
        if (!bullet.TryGetComponent(out TrailRenderer trail))
        {
            TrailRenderer newTrail = bullet.AddComponent<TrailRenderer>();
            newTrail.time = 0.5f; // How long the trail lasts
            newTrail.startWidth = 0.1f;
            newTrail.endWidth = 0f;
            newTrail.material = new Material(Shader.Find("Sprites/Default")) { color = Color.red };
        }

        // Launch the bullet towards the player
        Vector3 direction = (player.position - firePoint.position).normalized;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb == null) rb = bullet.AddComponent<Rigidbody>(); // Add Rigidbody if not present
        rb.useGravity = false; // No gravity for ranged bullets
        rb.velocity = direction * bulletSpeed;

        // Destroy the bullet once it reaches the target
        Destroy(bullet, bulletLifetime); // Failsafe: destroy after lifetime


        yield return new WaitForSeconds(firingInterval);
        isSpawning = false;
    }



    private void OrientTowardsTarget(Transform target)
    {
        if (target == null) return;

        // Calculate the direction to the target
        Vector3 direction = (target.position - transform.position).normalized;

        // Ignore the Y-axis for a flat rotation (optional)
        direction.y = 0;

        // Calculate the rotation to look at the target
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate towards the target (optional for smoother effect)
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

}
