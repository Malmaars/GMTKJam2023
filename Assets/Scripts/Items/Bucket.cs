using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

public class Bucket : Item
{
    GameObject waterCircle;
    GameObject splashAnimation;

    float cooldownTimer;
    public override void Initialize(Vector2 startPosition, itemType type)
    {
        visualPrefab = Resources.Load("Items/bucket") as GameObject;
        animTriggerName = "pick-up-bucket";

        waterCircle = Object.Instantiate(Resources.Load("Items/waterCircle") as GameObject);
        waterCircle.SetActive(false);
        splashAnimation = Object.Instantiate(Resources.Load("Items/WaterSplash") as GameObject);
        splashAnimation.SetActive(false);

        base.Initialize(startPosition, type);

        //water or something idk
    }

    Vector2 previousInput;
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //gradually make the water circle smaller, till it reaches the minimum size
        //for now the min size is 1.5

        float shrinkSpeed = 1f;

        if (waterCircle.transform.localScale.x > 1.5f)
            waterCircle.transform.localScale = new Vector3(waterCircle.transform.localScale.x - shrinkSpeed * Time.deltaTime, waterCircle.transform.localScale.y - shrinkSpeed * Time.deltaTime, waterCircle.transform.localScale.z - shrinkSpeed * Time.deltaTime);

        Vector2 inputDirection = InputDistributor.inputActions.Player.Move.ReadValue<Vector2>();
        if (inputDirection != Vector2.zero)
            previousInput = inputDirection;

        waterCircle.transform.position = BlackBoard.playerPosition + previousInput.normalized * 2;

        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;
    }

    public override void Interact(InputAction.CallbackContext context)
    {
        base.Interact(context);

        if (cooldownTimer > 0)
            return;

        //this is the current tile the player is standing on

        //if you click fast, it's big aoe, if you hold, the aoe gets smaller
        waterCircle.SetActive(true);
        waterCircle.transform.localScale = new Vector3(8, 8, 8);

        //get the player direction and keep the circle in that direction, with a distance of 2
        Vector2 inputDirection = InputDistributor.inputActions.Player.Move.ReadValue<Vector2>();
        waterCircle.transform.position = BlackBoard.playerPosition + inputDirection.normalized * 2;

        //water or something idk
    }

    public override void Release(InputAction.CallbackContext context)
    {
        base.Release(context);

        if (cooldownTimer > 0)
            return;

        //physics circle to check for tiles to water, based on the circle
        waterCircle.SetActive(false);

        ////if tile is flower: dig flower
        //if (BlackBoard.currentTile == null || BlackBoard.currentTile.flower == null)
        //{
        //    //do nothing, there is no flower
        //    return;
        //}

        LayerMask mask = (1 << 6);

        Collider2D[] overlappingTiles = Physics2D.OverlapCircleAll(waterCircle.transform.position, waterCircle.transform.localScale.x / 2, mask);
        
        foreach(Collider2D col in overlappingTiles)
        {
            col.gameObject.GetComponent<SoilTile>().ApplyWater();
        }

        Debug.Log(overlappingTiles.Length);

        //trigger the water splash animation
        splashAnimation.transform.position = waterCircle.transform.position + new Vector3(0, waterCircle.transform.localScale.x / 4, 0);
        splashAnimation.transform.localScale = waterCircle.transform.localScale;
        splashAnimation.SetActive(true);
        splashAnimation.GetComponent<Animator>().Play("WaterSplash");

        cooldownTimer = 2f;

        //check if the hit colliders are all tiles
        //BlackBoard.currentTile.ApplyWater();
    }
}
