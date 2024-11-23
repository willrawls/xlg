using System.Text.Json;
using MetX.Standard.Library.Encryption;
    
namespace Roller001.App_Code;

public class RollerAState
{
    // ReSharper disable once InconsistentNaming
    public int THAC0 { get; set; } = 12;
    public int TargetAc { get; set; } = 0;
    
    public bool HasGiantStrength { get; set; } = true;
    public bool HasBerserk { get; set; } = false;
    public bool HasBless { get; set; } = false;
    public bool HasHaste { get; set; } = false;
    
    public int HandsAttacks { get; set; } = 3;
    public int HandsToHit { get; set; } = 3;
    public int HandsDice { get; set; } = 4;
    public int HandsSides { get; set; } = 6;
    public int HandsPlus { get; set; } = 2;
    
    public int Cestis3Attacks { get; set; } = 1;
    public int Cestis3ToHit { get; set; } = 7;
    public int Cestis3Dice { get; set; } = 3;
    public int Cestis3Sides { get; set; } = 8;
    public int Cestis3Plus { get; set; } = 8;
    
    public int Cestis2Attacks { get; set; } = 1;
    public int Cestis2ToHit { get; set; } = 6;
    public int Cestis2Dice { get; set; } = 3;
    public int Cestis2Sides { get; set; } = 8;
    public int Cestis2Plus { get; set; } = 7;
    
    public int BiteAttacks { get; set; } = 3;
    public int BiteToHit { get; set; } = 0;
    public int BiteDice { get; set; } = 1;
    public int BiteSides { get; set; } = 4;
    public int BitePlus { get; set; } = 0;
    
    public int DamageMultiplier { get; set; } = 2;
    public int AttacksMultiplier { get; set; } = 1;


    public static RollerAState Load(string? filePath)
    {
        SuperRandom.FillSaltShaker(SuperRandom.GenerateRegularSalt());

        if (!File.Exists(filePath))
        {
            return new RollerAState();
        }

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<RollerAState>(json);
    }

    public void Save(string? filePath)
    {
        SuperRandom.FillSaltShaker(SuperRandom.GenerateRegularSalt());

        var json = JsonSerializer.Serialize(this);
        File.WriteAllText(filePath, json.ToString());
    }

    public void HasGiantStrength_Click()
    {
        HasGiantStrength = !HasGiantStrength;
        DamageMultiplier = HasGiantStrength ? 2 : 1;
    }

    public void HasHaste_Click()
    {
        HasHaste = !HasHaste;
        AttacksMultiplier = HasHaste ? 2 : 1;
    }

    public void HasBless_Click()
    {
        HasBless = !HasBless;
    }

    public void HasBerserk_Click()
    {
        HasBerserk = !HasBerserk;
        if (HasBerserk)
        {
            HandsAttacks++;
            Cestis3Attacks++;
            BiteAttacks++;
        }
        else
        {
            HandsAttacks = 4;
            Cestis3Attacks = 1;
            Cestis2Attacks = 1;
            BiteAttacks = 4;
        }
    }

    public int MinimumHandDamage()
    {
        return ((HandsAttacks * HandsDice) + (HandsPlus + (HasBless ? 1 : 0)) * HandsAttacks) * DamageMultiplier * AttacksMultiplier;
    }

    public int MinimumCestis3Damage()
    {
        return ((Cestis3Attacks * Cestis3Dice) + (Cestis3Plus + (HasBless ? 1 : 0)) * Cestis3Attacks) * DamageMultiplier * AttacksMultiplier;
    }

    public int MinimumCestis2Damage()
    {
        return ((Cestis2Attacks * Cestis2Dice) + (Cestis2Plus + (HasBless ? 1 : 0)) * Cestis2Attacks) * DamageMultiplier * AttacksMultiplier;
    }

    public int ShowDamageMultiplier()
    {
        return DamageMultiplier;
    }

    public int ShowAttacksMultiplier()
    {
        return AttacksMultiplier;
    }


    public string RollD20(RollerAState state, int bonus, out RollState rollState)
    {
        rollState = new RollState();
        var roll = (int)SuperRandom.NextRoll(1, 20);
        rollState.natural20 = roll == 20;
        rollState.natural1 = roll == 1;
        rollState.hitRoll = roll + bonus + (HasBless ? 1 : 0);
        if (rollState.natural1)
        {
            rollState.hitRoll = 1;
            return "Natural 1";
        }
        
        if (rollState.natural20)
        {
            rollState.didHit = true;
            return $"Nat 20+{bonus}={rollState.hitRoll}";
        }
        
        var target = state.THAC0 - state.TargetAc;
        if (rollState.hitRoll >= target)
        {
            rollState.didHit = true;
            return $"{roll}+{bonus}={rollState.hitRoll}";
        }

        var missText = $"{roll}+{bonus}={rollState.hitRoll} (Miss)";
        _alreadyMissed = true;
        return missText;
    }

    public string Target()
    {
        return $"Target is {THAC0 - TargetAc}";
    }

    public string CalculateDamage(int dice, int sides, int bonusToDamage, RollState rollState)
    {
        if (!rollState.didHit)
        {
            return "";
        }
        if (rollState.natural1)
        {
            rollState.hitRoll = -1;
            return "Skip next";
        }

        var damageRoll = (int)SuperRandom.NextRoll(dice, sides);

        var baseDamage = damageRoll + bonusToDamage;
        rollState.damage = baseDamage
                           * (HasGiantStrength ? 2 : 1)
                           * (rollState.natural20 ? 2 : 1);
        _totalDamage += rollState.damage;
        return rollState.damage.ToString();
    }

    public int MinimumDamage()
    {
        return (MinimumHandDamage() + MinimumCestis3Damage() + MinimumCestis2Damage());
    }

    private static bool _alreadyMissed;
    private static int _totalDamage;
    public string PreRoll()
    {
        _alreadyMissed = false;
        _totalDamage = 0;
        return Target();
    }

    public string PostRoll()
    {
        return $"Total damage is {_totalDamage}";
    }
}

public class RollState
{
    public bool natural20;
    public bool natural1;
    public int hitRoll;
    public bool didHit;
    public int damage;
}

public class FairnessState
{
    public List<int> Number;

    public FairnessState()
    {
        Number = new List<int>(21);
        for(int i = 0; i < 21; i++)
            Number.Add(i);

        for (int i = 0; i < 10000; i++)
        {
            var roll = (int)SuperRandom.NextRoll(1, 20);
            Number[roll]++;
        }
    }

    public int Average()
    {
        int total = 0;
        for (int i = 0; i < 21; i++)
            total += Number[i];
        return total / 20;
    }

    public int Min()
    {
        return Number.Where(n => n > 0).Min();
    }

    public int Max()
    {
        return Number.Where(n => n > 0).Max();
    }
}