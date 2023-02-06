using Character.Stats;
using DiContainerLibrary.DiContainer;
using General.Enums;
using General.State;
using Implementation.Data;
using UnityEngine;

public class Spear : StateForMovement
{
    /// <summary>
    /// Gets or sets enemy data.
    /// </summary>
    [InjectDiContainter]
    private IGameInformation gameInformation { get; set; }

    private int damage { get; set; }

    private float movementSpeed { get; set; }

    private int horizontalDirection { get; set; }

    private int verticalDirection { get; set; }

	private PoolObjectKey key { get; set; }

	//protected override void Initialization_State()
	//{
	//    base.Initialization_State();
	//    controller.SwapState(this);
	//}

	private void OnEnable()
	{
		base.Initialization_State();
		controller.SwapState(this);
	}

	public override void Update_State()
	{
		base.Update_State();
		if (controller.ActiveStateMovement != this && !gameInformation.StopMovement)
		{
			controller.SwapState(this);
		}
	}

	public override void WhileActive_State()
    {
        base.WhileActive_State();
        rigBody.velocity = new Vector3(horizontalDirection * movementSpeed, verticalDirection * movementSpeed);
		if (gameInformation.StopMovement)
		{
			controller.EndState(this);
		}
    }

    public void SetValues(float movementSpeed, int damage, DirectionEnum verticalDirection, DirectionEnum horizontalDirection, PoolObjectKey key)
    {
        this.damage = damage;
        this.movementSpeed = movementSpeed;
        this.horizontalDirection = (int)horizontalDirection;
        this.verticalDirection = (int)verticalDirection;
		this.key = key;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.gameObject.GetComponent<CharacterTakeDamage>();

        if (collision.gameObject.tag == "Ground")
        {
			//Destroy(this.gameObject);
			ObjectPooler.pooler.PushObject(gameObject, key);
        }
        else if (character != null)
        {
            character.TakeDamage(damage);
			//Destroy(this.gameObject);
			ObjectPooler.pooler.PushObject(gameObject, key);
		}
    }
}
