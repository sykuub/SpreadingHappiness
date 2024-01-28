using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalGirlController : MonoBehaviour
{
	[SerializeField] public bool isAngryAtStart;

	private float cooldownShootingTimer;
    private float directionChangeInterval = 5f;
    private float directionTimer = 5f;
    private float movementSpeed = 0.01f;
    private int x_angle = 0;
    private int y_angle = 0;

    // Each magical girl type has one angrystate and one happystate
    // that inherit from these two abstract classes.
    protected AbstractAngryState angryState;
	protected AbstractHappyState happyState;

	// we're using a State DP. Each state has 2 methods: Move() and Shoot()
	// Happy States will be shooting good bullets to help the MC
	// Angry States will be shooting stuff for the MC to avoid
	protected AbstractMagicalGirlState magicalGirlState;
	
	// Rigidbody2D. Do not set in Editor.
	private Rigidbody2D rigid2d;
	
	// ************************************************************************
	
	public void TurnHappy()
	{
		// DO STUFF HERE
		magicalGirlState = happyState;
		
		// Resets the firing cooldown.
		cooldownShootingTimer = magicalGirlState.CooldownTimeBeforeShooting;
	}

    void ChangeDirection()
    {
        x_angle = UnityEngine.Random.Range(-180, 180);
        y_angle = UnityEngine.Random.Range(-180, 180);
    }
    // ************************************************************************

	public void Awake()
	{
		// The Magical girl unity-object should also have one
		// angry state component and one happy state component
		angryState = GetComponent<AbstractAngryState>();
		happyState = GetComponent<AbstractHappyState>();

		if (isAngryAtStart)
			magicalGirlState = angryState;
		else magicalGirlState = happyState;
	}

    // Start is called before the first frame update
    void Start()
    {
        cooldownShootingTimer = magicalGirlState.CooldownTimeBeforeShooting;
        rigid2d = GetComponent<Rigidbody2D>();
		
		if (isAngryAtStart)
			magicalGirlState = angryState;
		else magicalGirlState = happyState;

        ChangeDirection();
        directionTimer = directionChangeInterval;
    }

	// ************************************************************************
	
    // Update is called once per frame
    void Update()
    {        
        cooldownShootingTimer -= Time.deltaTime;
		if (cooldownShootingTimer < 0)
		{
			// Shoots once the timer is over.
			magicalGirlState.Shoot();
			cooldownShootingTimer += magicalGirlState.CooldownTimeBeforeShooting;
		}

        this.transform.position += new Vector3(1 * x_angle, 1 * y_angle, 1) * movementSpeed * Time.deltaTime;

        directionTimer -= Time.deltaTime;
        if(directionTimer <= 0)
        {
            ChangeDirection();
            directionTimer = directionChangeInterval;
        }
    }
}
