using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject target;
    public float speed;
    private Vector3 direction;

    private void Start()
    {
        target = GameObject.Find("Player");
        direction = target.transform.position - transform.position;
    }
    private void Update()
    {
        if(Vector3.Distance(transform.position, target.transform.position) > 0.1f)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
