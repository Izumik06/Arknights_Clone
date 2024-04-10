using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Izumik
{
    public class Enemy : MonoBehaviour
    {
        public bool moveLeft;
        public bool isWaiting;

        public string enemyType;
        public Attacktype attacktype;

        public Node targetNode;
        public List<Node> roots;
        public List<WaitNode> waitNodes;

        public int weight;
        public int curhp;
        public int maxhp;
        public int atk;
        public int def;
        public int sdef;
        public int edef;
        public int eresistance;
        public float attackspeed;
        public float speed;
        public float range;
        public float posError;
        public void Findpath(List<Node> targets, Node spawnPoint)
        {
            roots = new List<Node>();

            FathFinder fathFinder = GetComponent<FathFinder>();

            roots.Add(spawnPoint);
            for (int i = 0; i < targets.Count; i++)
            {
                List<Node> fath = fathFinder.PathFinding(roots[roots.Count - 1], targets[i]);
                for(int j = 0; j < fath.Count; j++)
                {
                    roots.Add(fath[j]);
                }
            }
        }
        private void Start()
        {
            StartCoroutine(Move());
        }
        IEnumerator Move()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            for(int i = 0; i < roots.Count; i++)
            {
                isWaiting = false;
                while(Vector2.Distance(new Vector2(roots[i].x, roots[i].y), new Vector2(transform.position.x, transform.position.z)) >= 0.02)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(roots[i].x, transform.position.y, roots[i].y), speed * 0.01f);
                    moveLeft = (new Vector3(roots[i].x, transform.position.y, roots[i].y) - transform.position).x < 0;
                    yield return new WaitForSeconds(0.02f);
                }
                WaitNode waitNode = waitNodes.Find(_ => _.x == roots[i].x && _.y == roots[i].y);
                if (waitNode != null)
                {
                    isWaiting = true;
                    yield return new WaitForSeconds(waitNode.time);
                }
                else
                {
                    yield return null;
                }
            }
        }
    }
    
    public enum Attacktype
    {
        melee, ranged
    }
}
