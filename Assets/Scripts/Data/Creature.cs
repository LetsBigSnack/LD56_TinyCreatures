using System;
using Data;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Creature
{
    
    [Header("Stats")]
    private string _creatureName;

    private BigDecimal _creatureGeneration;
    private BigDecimal _currentHealth;
    private BigDecimal _maxHealth;
    private BigDecimal _creatureWins;
    private CreatureStats _creatureStats;
    private CreatureRepresentation _representation;

    public string CreatureName
    {
        get => _creatureName;
        set => _creatureName = value;
    }

    public BigDecimal CreatureGeneration
    {
        get => _creatureGeneration;
        set => _creatureGeneration = value;
    }
    
    [JsonIgnore]
    public BigDecimal CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = value;
    }

    public BigDecimal MaxHealth
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
    
    public BigDecimal CreatureWins
    {
        get => _creatureWins;
        set => _creatureWins = value;
    }

    //TODO: make it a static Class Method and not a Object Method
    private string[] prefixes = { "Xeno", "Proto", "Neuro", "Hydro", "Pyro", "Astro", "Bio", "Cryo", "Phyto", "Electro" };
    private string[] middleParts = { "ter", "gon", "pho", "ri", "no", "mo", "plu", "lo", "zan", "rek", "dar", "val" };
    private string[] suffixes = { "ium", "on", "us", "or", "ic", "ex", "is", "ax", "um", "ox" };


    public Creature(BigDecimal generation, BigDecimal maxHealth, CreatureStats creatureStats, CreatureRepresentation representation)
    {
        _creatureName = GenerateRandomName();
        _creatureGeneration = generation;
        _maxHealth = maxHealth;
        _currentHealth = _maxHealth;
        _creatureStats = creatureStats;
        _representation = representation;
        _creatureWins = 0;
    }
    
    
    public BigDecimal TakeDamage(BigDecimal damage)
    {
        BigDecimal finalDamage = damage * (1 - (_creatureStats.Defense / (_creatureStats.Defense  + 200)));
        finalDamage = BigDecimal.Max(1, finalDamage);
        finalDamage = finalDamage.Round(0);
        _currentHealth = _currentHealth - finalDamage;
        _currentHealth = BigDecimal.Max(0, _currentHealth);

        return finalDamage;
    }
    
    public string GenerateRandomName()
    {
        string prefix = prefixes[Random.Range(0, prefixes.Length)];
        string middle = middleParts[Random.Range(0, middleParts.Length)];
        string suffix = suffixes[Random.Range(0, suffixes.Length)];

        return prefix + middle + suffix;
    }

    public bool IsNull()
    {
        return _creatureName == null;
    }
    
}

