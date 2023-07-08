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
            if (!targetFlower)
            {
                _state = CrowState.Fleaing;
                StartCoroutine(FlyAway());
                return;
            }

            float distanceToTarget = Vector3.Distance(gameObject.transform.position,
                targetFlower.transform.position);

            float scaleRate = distanceToTarget / 4;
            transform.localScale = new Vector3(1 + scaleRate, 1 + scaleRate, 1 + scaleRate);
            
            transform.position = Vector2.Lerp(transform.position, targetFlower.transform.position,
                flySpeed / 2.5f * Time.deltaTime);
            
            if (distanceToTarget < 0.1f)
            {
                _state = CrowState.Sitting;
                targetFlower.CrowEnter();
            }
        }
        else if (_state == CrowState.Sitting)
        {
            if (!targetFlower || targetFlower.GetFlowerState() != FlowerState.Normal )
            {
                _state = CrowState.Fleaing;
                StartCoroutine(FlyAway());
                return;
            }
            
            for (int i = 0; i < BlackBoard.allItems.Count; i++)
            {
                Item item = BlackBoard.allItems[i];
                
                float distanceToTarget = Vector3.Distance(gameObject.transform.position,
                    item.rb.transform.position);

                if (distanceToTarget > scareDistance)
                    continue;

                if (item.rb.velocity.magnitude < 0.2f)
                    continue;
                    
                _state = CrowState.Fleaing;
                StartCoroutine(FlyAway());
            }
        }
    }

    private IEnumerator FlyAway()
    {
        targetFlower.CrowLeave();
        body.gravityScale = -1f;
        int dir = Random.Range(0, 2);
        if (dir == 0)
            body.AddForce(Vector2.right * (flySpeed * 80));
        if (dir == 1)
            body.AddForce(Vector2.left * (flySpeed * 80));
        
        float rate = flySpeed / 2.5f;
        for (float i = 0; i < 3; i += 1.0f / 60)
        {
            transform.localScale = new Vector3(1 + i * rate, 1 + i * rate, 1 + i * rate); 
            yield return new WaitForSeconds(1.0f / 60);
        }
        
        Destroy(gameObject);
    }
}