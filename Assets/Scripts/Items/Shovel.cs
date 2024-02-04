using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shovel : Item
{
    SoilTile thisCurrentTile;
    ParticleSystem dirtParticles;
    bool shoveling;
    public override void Initialize(Vector2 startPosition, itemType type)
    {
        visualPrefab = Resources.Load("Items/shovel") as GameObject;
        animTriggerName = "pick-up-shovel";
        dirtParticles = Object.Instantiate(Resources.Load("DirtParticles") as GameObject).GetComponent<ParticleSystem>();


        base.Initialize(startPosition, type);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (shoveling)
        {
            if(BlackBoard.soldierAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "ShovelFinish")
            {
                Dig();
                shoveling = false;
                Camera.main.orthographicSize = 10;
            }
        }
    
    }

    public override void Interact(InputAction.CallbackContext context)
    {
        base.Interact(context);

        //this is the current tile the player is standing on

        //if tile is flower: dig flower
        if(BlackBoard.currentTile == null || (BlackBoard.currentTile.flower == null && BlackBoard.currentTile.dug))
        {
            //do nothing, there is no tile
            return;
        }

        if (!BlackBoard.currentTile.dug)
        {
            thisCurrentTile = BlackBoard.currentTile;
            BlackBoard.soldierAnimator.SetTrigger("Shovel");

            shoveling = true;
            BlackBoard.shovelPause = true;
            GlobalSfxPlayer.instance.Play(SFX.Shovel);
        }
        else if (BlackBoard.currentTile.flower.GetFlowerGrowState() >= FlowerGrowState.Seedling)
        {
            thisCurrentTile = BlackBoard.currentTile;
            BlackBoard.soldierAnimator.SetTrigger("HardShovel");
            Camera.main.orthographicSize = 5;

            shoveling = true;
            BlackBoard.shovelPause = true;
            GlobalSfxPlayer.instance.Play(SFX.Shovel);
        }
    }

    public void Dig()
    {

        //check if the tile has a flower
        if (thisCurrentTile && thisCurrentTile.flower != null)
        {
            thisCurrentTile.Harvest();
        }

        //if it doesn't have a flower, check if the ground is already ground, if it's not, make it
        if (thisCurrentTile && !thisCurrentTile.dug)
        {
            //dig it
            thisCurrentTile.DigTile();
            thisCurrentTile.PlantSeed();
            ScoreManager.instance.IncreaseRageMeter(0.1f);
        }
        
        dirtParticles.transform.position = BlackBoard.currentTile.transform.position;
        dirtParticles.Play();
        BlackBoard.shovelPause = false;
        thisCurrentTile = null;
    }
}
