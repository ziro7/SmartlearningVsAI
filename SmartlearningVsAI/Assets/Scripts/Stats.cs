﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour, IDamageable {

	[Header("Stats")]
	[SerializeField] private float critChance = 0.05f;
	[SerializeField] private int level = 1; //Max 5
	[SerializeField] private float mana = 317f;
	[SerializeField] private float manaGainPrTick = 10f;
	[SerializeField] private float health = 570f;
	[SerializeField] private float baseDamage = 58f;
	//[SerializeField] private float xpNextLevel = 100f;

	IDie objectToDie;

	private float currentHealth;
	private float currentMana;
	float currentHealthPct;
	float currentManaPct;

	// Hvor meget skal være eksponeret?
	public float CritChance { get { return critChance; } }
	public float BaseDamage { get { return baseDamage; } }

	// Creates an event that take a float and is being called when health or mana is changed.
	// I don't need to create a delegate first as i use the generic action delegate here.
	public event Action<float> OnHealthChanged;
	public event Action<float> OnManaChanged;


	private void Awake()
	{
		currentHealth = health;
		currentMana = mana;
	}
	   	 
	private void Start()
	{
		// Finds the first (only) scripts that implements IDie interface.
		objectToDie = gameObject.GetComponent<IDie>();
	}

	private void Update()
	{
		RegenMana();
	}

	private void RegenMana()
	{
		if (currentMana == mana)
		{
			return;
		}
		if (manaGainPrTick > (mana - currentMana))
		{
			currentMana += (mana - currentMana);
		}
		else
		{
			currentMana += manaGainPrTick;
		}
	}

	public void TakeDamage(float damageAmount)
	{
		// Reduced the health with the damage taken.
		currentHealth -= damageAmount;
		currentHealthPct = currentHealth / health;

		if (currentHealth <= 0)
		{
			objectToDie.Die();
			currentHealth = 0;
		}

		// Calls the event saying that the health changed and gives a new percentage
		// to whomever is registered for the info.
		OnHealthChanged(currentHealthPct);
				

	}

	public void ManaUsed(float manaUsed)
	{
		// Reduced the mana with the amount used.
		currentMana += manaUsed;
		currentManaPct = currentMana / mana;

		// Calls the event saying that the health changed and gives a new percentage
		// to whomever is registered for the info.
		OnManaChanged(currentManaPct);
	}

	public void LevelUp()
	{
		if(level < 5)
		{
			level++;
			baseDamage *= 1.60f;
			health *= 1.73f;
			mana *= 1.49f;
			currentHealth = health;
			currentMana = mana;

			// Calls the events saying that the health and mana changed and gives a new percentage
			OnHealthChanged(currentHealthPct);
			OnManaChanged(currentManaPct);
		}
	}

	public void Respawn()
	{
		currentHealth = health;
		currentMana = mana;

		// Calls the events saying that the health and mana changed and gives a new percentage
		OnHealthChanged(currentHealthPct);
		OnManaChanged(currentManaPct);
	}
}



