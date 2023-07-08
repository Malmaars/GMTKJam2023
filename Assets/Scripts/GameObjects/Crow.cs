using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CrowState
{
    Targeting, Sitting, Fleaing 
}

public class Crow : MonoBehaviour
{
    [Header("References")]
    public Flower targetFlower;
    public Player player;
    public Rigidbody2D body;
    
    [Header("Variables")]
    public int flySpeed = 4;
    public int scareDistance = 2;

    private CrowState _state;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_state == CrowState.Targeting)
        {
            float distanceToTarget = Vector3.Distance(gameObject.transform.position,
                targetFlower.transform.position);

            float scaleRate = distanceToTarget / 4;
            transform.localScale = new Vector3(1 + scaleRate, 1 + scaleRate, 1 + scaleRate);

            transform.position = Vector2.Lerp(transform.position, targetFlower.transform.position,
                flySpeed / 2.5f * Time.deltaTime);
            
            if (distanceToTarget < 0.1f)
            {
                _state = CrowState.Sitting;
            }
        }
        else if (_state == CrowState.Sitting)
        {
            float distanceToTarget = Vector3.Distance(gameObject.transform.position,
                player.transform.position);
            
            if (distanceToTarget < scareDistance)
            {
                _state = CrowState.Fleaing;
                StartCoroutine(FlyAway());
            }
        }

    }

    private IEnumerator FlyAway()
    {
        body.gravityScale = -1f;
        body.AddForce(Vector2.right * (flySpeed * 80));
        
        float rate = flySpeed / 2.5f;
        for (float i = 0; i < 3; i += 1.0f / 60)
        {
            transform.localScale = new Vector3(1 + i * rate, 1 + i * rate, 1 + i * rate); 
            yield return new WaitForSeconds(1.0f / 60);
        }
        
        Destroy(gameObject);
    }
}