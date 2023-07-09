using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    private Animator _animator;
    private float _size = 2;
    private SpriteRenderer _sr;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if (body.velocity.x > 0)
            _sr.flipX = false;
        else if (body.velocity.x < 0)
            _sr.flipX = true;

        if (_state == CrowState.Targeting)
        {
            if (targetFlower == null)
            {
                _state = CrowState.Fleaing;
                StartCoroutine(FlyAway());
                return;
            }
            
            if (transform.position.x > targetFlower.transform.position.x)
                _sr.flipX = true;
            else if (transform.position.x < targetFlower.transform.position.x)
                _sr.flipX = false;

            var targetPos3 = targetFlower.transform.position;
            Vector2 targetPos2 = new Vector2(targetPos3.x, targetPos3.y);
            
            float distanceToTarget = Vector3.Distance(gameObject.transform.position,
                targetPos2);

            float scaleRate = _size + (distanceToTarget / 4);
            transform.localScale = new Vector3(1 + scaleRate, 1 + scaleRate, 1 + scaleRate);

            transform.position = Vector2.Lerp(transform.position, targetPos2,
                (flySpeed / 4.5f / (distanceToTarget / 4)) * Time.deltaTime);

            if (distanceToTarget < 0.1f)
            {
                _state = CrowState.Sitting;
                _animator.SetTrigger("crow-sit");
                targetFlower.CrowEnter();
            }
        }
        else if (_state == CrowState.Sitting)
        {
            if (targetFlower == null || targetFlower.GetFlowerState() != FlowerState.Normal)
            {
                _state = CrowState.Fleaing;
                StartCoroutine(FlyAway());
                return;
            }
            
            float distanceToPlayer = Vector3.Distance(gameObject.transform.position,
                BlackBoard.playerPosition);

            if (distanceToPlayer <= scareDistance)
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
        _animator.SetTrigger("crow-scare");
        new WaitForSeconds(.1f / 60);
        
        targetFlower.CrowLeave();
        body.gravityScale = -1f;
        int dir = Random.Range(0, 2);
        if (dir == 0)
            body.AddForce(Vector2.right * (flySpeed * 80));
        if (dir == 1)
            body.AddForce(Vector2.left * (flySpeed * 80));
        
        _animator.SetTrigger("crow-fly");
        
        float rate = _size + (flySpeed / 2.5f);
        for (float i = 0; i < 3; i += 1.0f / 60)
        {
            transform.localScale = new Vector3(1 + i * rate, 1 + i * rate, 1 + i * rate); 
            yield return new WaitForSeconds(1.0f / 60);
        }

        Destroy(gameObject);
    }
}