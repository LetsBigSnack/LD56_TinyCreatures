using Data;
using UnityEngine;

public class Creature
{
    
    
    
    [Header("Stats")]
    private string _creatureName;
    private int _creatureGeneration;
    private int _currentHealth;
    private int _maxHealth;
    private int _creatureWins;
    
    private CreatureStats _creatureStats;
    private CreatureRepresentation _representation;

    public string CreatureName
    {
        get => _creatureName;
        set => _creatureName = value;
    }

    public int CreatureGeneration
    {
        get => _creatureGeneration;
        set => _creatureGeneration = value;
    }

    public int CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = value;
    }

    public int MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }

    public CreatureStats CreatureStats
    {
        get => _creatureStats;
        set => _creatureStats = value;
    }

    public CreatureRepresentation Representation
    {
        get => _representation;
        set => _representation = value;
    }
    
    public int CreatureWins
    {
        get => _creatureWins;
        set => _creatureWins = value;
    }

    //TODO: make it a static Class Method and not a Object Method
    private string[] prefixes = { "Xeno", "Proto", "Neuro", "Hydro", "Pyro", "Astro", "Bio", "Cryo", "Phyto", "Electro" };
    private string[] middleParts = { "ter", "gon", "pho", "ri", "no", "mo", "plu", "lo", "zan", "rek", "dar", "val" };
    private string[] suffixes = { "ium", "on", "us", "or", "ic", "ex", "is", "ax", "um", "ox" };


    public Creature(int generation, int maxHealth, CreatureStats creatureStats, CreatureRepresentation representation)
    {
        _creatureName = GenerateRandomName();
        _maxHealth = maxHealth;
        _currentHealth = _maxHealth;
        _creatureStats = creatureStats;
        _representation = representation;
        
    }
    
    
    public int TakeDamage(float damage)
    {
        float finalDamage = damage * (1 - (_creatureStats.Defense / (_creatureStats.Defense  + 200)));
        finalDamage = Mathf.Max(1, finalDamage);
        finalDamage = Mathf.RoundToInt(finalDamage);
        _currentHealth -= Mathf.RoundToInt(finalDamage);
        _currentHealth = Mathf.Max(0, _currentHealth);

        return (int)finalDamage;
    }
    
    public string GenerateRandomName()
    {
        string prefix = prefixes[Random.Range(0, prefixes.Length)];
        string middle = middleParts[Random.Range(0, middleParts.Length)];
        string suffix = suffixes[Random.Range(0, suffixes.Length)];

        return prefix + middle + suffix;
    }
    
}

