using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [Header("Representation")]
    [SerializeField] private string creatureName;
    [SerializeField] private int creatureGeneration;
    [SerializeField] private SpriteRenderer headRenderer;
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private SpriteRenderer legsRenderer;
    [SerializeField] private SpriteRenderer armsRenderer;
    
    [SerializeField] private Color headColor;
    [SerializeField] private Color bodyColor;
    [SerializeField] private Color legsColor;
    [SerializeField] private Color armsColor;
    
    
    [Header("Stats")]
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private float speed;
    [SerializeField] private float attack;
    [SerializeField] private float defense;
    [SerializeField] private float dexterity;
    [SerializeField] private int powerLevel;

    // Getters and Setters for Representation
    
    public string CreatureName
    {
        get { return creatureName; }
        set { creatureName = value; }
    }

    public int CreatureGeneration
    {
        get { return creatureGeneration; }
        set { creatureGeneration = value; }
    }

    public SpriteRenderer HeadRenderer
    {
        get { return headRenderer; }
        set { headRenderer = value; }
    }

    public SpriteRenderer BodyRenderer
    {
        get { return bodyRenderer; }
        set { bodyRenderer = value; }
    }

    public SpriteRenderer LegsRenderer
    {
        get { return legsRenderer; }
        set { legsRenderer = value; }
    }

    public SpriteRenderer ArmsRenderer
    {
        get { return armsRenderer; }
        set { armsRenderer = value; }
    }

    public Color HeadColor
    {
        get { return headColor; }
        set { headColor = value; }
    }

    public Color BodyColor
    {
        get { return bodyColor; }
        set { bodyColor = value; }
    }

    public Color LegsColor
    {
        get { return legsColor; }
        set { legsColor = value; }
    }

    public Color ArmsColor
    {
        get { return armsColor; }
        set { armsColor = value; }
    }

    // Getters and Setters for Stats

    public int CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public float Attack
    {
        get { return attack; }
        set { attack = value; }
    }

    public float Defense
    {
        get { return defense; }
        set { defense = value; }
    }

    public float Dexterity
    {
        get { return dexterity; }
        set { dexterity = value; }
    }

    public int PowerLevel
    {
        get { return powerLevel; }
        set { powerLevel = value; }
    }
    
    // Method to set the creature's stats
    public void SetStats(int health, float speed, float attack, float defense, float dexterity)
    {
        this.maxHealth = health;
        this.currentHealth = health;
        this.speed = speed;
        this.attack = attack;
        this.defense = defense;
        this.dexterity = dexterity;
        CalculatePowerLevel();
    }

    public void SetColor(Color headColor, Color bodyColor, Color legsColor, Color armsColor)
    {
        this.headColor = headColor;
        this.bodyColor = bodyColor;
        this.legsColor = legsColor;
        this.armsColor = armsColor;
    }

    // Method to assign the creature's sprite parts
    public void SetSprites(Sprite head, Sprite body, Sprite legs, Sprite arms)
    {
        headRenderer.sprite = head;
        headRenderer.color = headColor;
        bodyRenderer.sprite = body;
        bodyRenderer.color = bodyColor;
        legsRenderer.sprite = legs;
        legsRenderer.color = legsColor;
        armsRenderer.sprite = arms;
        armsRenderer.color = legsColor;
    }

    // Example method to calculate the power level based on stats
    private void CalculatePowerLevel()
    {
        powerLevel = Mathf.RoundToInt((speed + attack + defense + dexterity) / 4);
    }
}

