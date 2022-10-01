using UnityEngine;
using UnityEngine.AI;

//TODO Noch nutzen und kommentieren?
//Not Used ?!
public class EnemyController1 : MonoBehaviour
{
    public GameObject player;
    public Transform target;
    private NavMeshAgent nav;
    public float howClose;
    private float dist;
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        
    }

    void Update()
    {
        dist = Vector3.Distance(target.position, transform.position);
        if (dist <= howClose)
        {
            nav.SetDestination(target.position);
            transform.LookAt(player.transform);



        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            Destroy(this.gameObject);
        }
    }
}