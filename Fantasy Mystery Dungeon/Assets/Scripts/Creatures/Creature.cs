using UnityEngine;

public class Creature : MonoBehaviour, IHealth
{
	[SerializeField] protected CreatureData data;
	public CreatureType creatureType { get { return data.creatureType; } }
	protected int maxHealth { get { return data.baseHealth * Level * 2 + 2; } }
	protected int attack  { get { return data.baseAttack * Level + 2; } }
	protected int defense { get { return data.baseDefense * Level + 2; } }

	protected int bonusHealth  = 0;
	protected int bonusAttack  = 0;
	protected int bonusDefense = 0;

	public int MaxHealth
	{
		get
		{
			return maxHealth + bonusHealth;
		}
	}
	public int TotalAttack
	{
		get
		{
			return attack + bonusAttack;
		}
	}
	public int TotalDefense
	{
		get
		{
			return defense + bonusDefense;
		}
	}

	protected int health;
	public int Health { get { return health; } }

	private int level = 2;
	public int Level { get { return level; } }
	private void Start ()
	{
		health = MaxHealth;
	}
	
	private void Update ()
	{
	    if (health <= 0)
		{
			Die();
		}
		else
		{
			TakeDamage(1, DamageTypes.Raw);
		}
	}

	#region IHealth
	protected int GetTypeMultiplier(DamageTypes damType)
	{
		if( (creatureType == CreatureType.Human  && damType == DamageTypes.Magic) ||
			(creatureType == CreatureType.Beast  && damType == DamageTypes.Physical) ||
			(creatureType == CreatureType.Undead && damType == DamageTypes.Holy))
		{
			return 2;
		}
		else
		{
			return 1;
		}
	}

	public void TakeDamage(int amount, DamageTypes damType)
	{
		//Amount should be (power * totalAttack)
		if (amount < 0)
		{
			Debug.LogWarning("Damage amount must be non-negative.");
			return;
		}

		if (damType == DamageTypes.Raw)
		{
			health = Mathf.Clamp(health - amount, 0, MaxHealth);
		}
		else
		{
			int newAmount = Mathf.RoundToInt(amount / TotalDefense);
			newAmount *= GetTypeMultiplier(damType);
			if (newAmount > 0 && health > 0)
			{
				health = Mathf.Clamp(health - newAmount, 0, MaxHealth);
			}
		}
	}

	public void Heal(int amount)
	{
		if(amount > 0 && health > 0 && health < MaxHealth)
		{
			health = Mathf.Clamp(health + amount, 0, MaxHealth);
		}
	}

	public void Die()
	{
		Debug.Log(gameObject.name + " has died.");
		gameObject.SetActive(false);
	}
	#endregion IHealth

    protected void DealDamage(IHealth target)
	{
		int power = 5;
		target.TakeDamage(power * TotalAttack, DamageTypes.Physical);
	}
}
