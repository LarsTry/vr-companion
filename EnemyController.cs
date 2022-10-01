using UnityEngine;
using UnityEngine.AI;

namespace MyStuff.MyScripts
{
    //TODO Noch nutzen und kommentieren?
    //Not Used ?!
    public class EnemyController : MonoBehaviour
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
        public void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("RightHand") || other.gameObject.CompareTag("LeftHand"))
            {
                Destroy(this.gameObject);
            }
        }
    }
}
