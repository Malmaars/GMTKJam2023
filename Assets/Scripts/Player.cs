using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    InputManager inputManager;
    PlayerInputActions playerInput;

    Rigidbody2D body;

    float horizontal;
    float vertical;
    public float maxSpeed;

    public float runSpeed = 20.0f;
    public float brakingMultiplier;

    public float DashDistance;
    bool dashing;
    float dashTimer;
    public float dashTime;

    //float dashCooldown;
    //public float dashCooldownLength;

    public float throwForce;

    Vector2 movingDirection;

    public Transform holdParent;

    Item currentItem;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInputActions();
        InputDistributor.inputActions = playerInput;
        inputManager = new InputManager(
        new InputAction[] {
            playerInput.Player.Move,
            playerInput.Player.Fire,
            playerInput.Player.Interact,
            playerInput.Player.PickUpThrow,
        });
        BlackBoard.OnAwake();

        body = GetComponent<Rigidbody2D>();

        //inputManager.AddActionToInput(InputDistributor.inputActions.Player.Focus, forwardTarget.ZoomIn);
        inputManager.AddActionToInput(InputDistributor.inputActions.Player.Interact, Dash);
        inputManager.AddActionToInput(InputDistributor.inputActions.Player.PickUpThrow, PickUp);

        BlackBoard.SpawnItem(new Vector2(1, 1), itemType.shovel);
    }

    private void OnEnable()
    {
        inputManager.WhenEnabled();
    }

    private void OnDisable()
    {
        inputManager.WhenDisabled();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Move()
    {
        Vector2 inputDirection = InputDistributor.inputActions.Player.Move.ReadValue<Vector2>();
        horizontal = inputDirection.x;
        vertical = inputDirection.y;
        movingDirection = inputDirection;

        //Debug.Log("X input = " + horizontal + ", Y Input =" + vertical);

        if (!dashing)
        {
            if (horizontal != 0 || vertical != 0) // Check for diagonal movement
            {
                // limit movement speed diagonally, so you move at 70% speed
                //horizontal = moveLimiter;
                // vertical = moveLimiter;

                //instead of directly changing the velocity, let's add some velocity to the object, slowly (or fast idk)
                body.velocity += new Vector2(horizontal * runSpeed, vertical * runSpeed);

                if (body.velocity.magnitude > maxSpeed)
                {
                    body.velocity = body.velocity.normalized * maxSpeed;
                }
            }

            else
            {
                if (body.velocity.magnitude > 0.1f)
                {
                    body.velocity = body.velocity * brakingMultiplier;
                }

                else
                {
                    body.velocity = Vector2.zero;
                }
            }
        }
        else
        {
            //maybe a timer to set dashing to false?
            dashTimer -= Time.deltaTime;

            if(dashTimer < 0)
            {
                dashing = false;
            }
        }

        
    }

    public void Dash(InputAction.CallbackContext context)
    {
        //dash towards the direction you're walking
        //Since we're moving the player with velocity, I'll just add force

        Debug.Log("DASH");

        body.AddForce(movingDirection.normalized * DashDistance, ForceMode2D.Impulse);

        dashing = true;
        dashTimer = dashTime;

        
    }

    public void PickUp(InputAction.CallbackContext context)
    {
        //if the player picks something up, add the interact function of the object to the interact button, otherwise it should be Dash

        //1: search if the player is standing close enough to an object
        //2: pick up the object
        //3: Change the button action from dash to interact

        //I think it's safe to assume each item has the same pickup range
        for (int i = 0; i < BlackBoard.allItems.Count; i++)
        {
            //check each location with the player, if one is close enough, select that one and break the loop
            if(Vector2.Distance(transform.position, BlackBoard.allItems[i].visual.transform.position) < 1)
            {
                //pick up this item
                currentItem = BlackBoard.allItems[i];

                currentItem.visual.transform.SetParent(holdParent);
                currentItem.visual.transform.localPosition = Vector2.zero;
                currentItem.rb.isKinematic = true;

                //set the interaction button action to the function of the item
                inputManager.ClearAllActionsFromInput(InputDistributor.inputActions.Player.Interact);
                inputManager.AddActionToInput(InputDistributor.inputActions.Player.Interact, currentItem.Interact);

                //remove this function from the pickup button, and change it to throw
                inputManager.ClearAllActionsFromInput(InputDistributor.inputActions.Player.PickUpThrow);
                inputManager.AddActionToInput(InputDistributor.inputActions.Player.PickUpThrow, Throw);

                break;
            }
        }
    }

    public void Throw(InputAction.CallbackContext context)
    {
        //throw the object you're holding to the direction you're walking. Maybe hold to throw farther

        //1: throw the object
        //2: Change the button action from interact to dash

        if (currentItem == null)
        {
            Debug.LogError("No item in hand, can't Throw");
        }

        else
        {
            currentItem.visual.transform.SetParent(null);
            currentItem.rb.isKinematic = false;
            currentItem.rb.AddForce(movingDirection * throwForce, ForceMode2D.Impulse);
        }

        //set the interaction button action to the function of the item
        inputManager.ClearAllActionsFromInput(InputDistributor.inputActions.Player.Interact);
        inputManager.AddActionToInput(InputDistributor.inputActions.Player.Interact, Dash);

        //remove this function from the pickup button, and change it to throw
        inputManager.ClearAllActionsFromInput(InputDistributor.inputActions.Player.PickUpThrow);
        inputManager.AddActionToInput(InputDistributor.inputActions.Player.PickUpThrow, PickUp);
    }


    //Deze functie is overbodig sinds we de functie van de objecten zelf gebruiken?
    //public void Interact(InputAction.CallbackContext context)
    //{

    //}
}
