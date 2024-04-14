using System.Collections.Generic;

public static partial class TypeStats
{
    public static Stats GetStats(SummonType type) {
        switch (type) {
            case SummonType.FlameWolf:
                return new Stats {
                    Health = 3,
                    AtkPower = 1,
                    AtkSpeed = 1,
                    MoveSpeed = 1,
                    AtkArea = 1,
                    ManaCost = 25,
                    TexturePath = "res://Resources/Assets/Summons/FlameWolf2.png",
                    Name = "Flame Wolf",
                    Description = "A wolf made of fire. It's hot.\nCauses the enemy to burn for 3 seconds.",
                    Code = new RuneCode("wsadw")
                };
            case SummonType.Slime:
                return new Stats {
                    Health = 2,
                    AtkPower = 1,
                    AtkSpeed = 1,
                    MoveSpeed = 1,
                    AtkArea = 1,
                    ManaCost = 10,
                    TexturePath = "res://Resources/Assets/Summons/Slime4.png",
                    Name = "Slime",
                    Description = "A slime. It's slimy.\nSlows the enemy for 3 seconds.",
                    Code = new RuneCode("adsswds")
                };
            case SummonType.Slimeloon:
                return new Stats {
                    Health = 1,
                    AtkPower = 1,
                    AtkSpeed = 1,
                    MoveSpeed = 1,
                    AtkArea = 1,
                    ManaCost = 10,
                    TexturePath = "res://Resources/Assets/Summons/Slimeloon.png",
                    Name = "Slimeloon",
                    Description = "A slime with a balloon. It's slimy.\n50% Chance to not be hit by an attack.",
                    Code = new RuneCode("wswsawd")
                };
            case SummonType.Tree:
                return new Stats {
                    Health = 4,
                    AtkPower = 1,
                    AtkSpeed = 1,
                    MoveSpeed = 1,
                    AtkArea = 1,
                    ManaCost = 50,
                    TexturePath = "res://Resources/Assets/Summons/Tree.png",
                    Name = "Tree",
                    Description = "A tree. It's woody.\nHeals the Summon in front of it for 1 health every 2 seconds.",
                    Code = new RuneCode("ddawdss")
                };
            case SummonType.GrassBoi:
                return new Stats {
                    Health = 5,
                    AtkPower = 1,
                    AtkSpeed = 1,
                    MoveSpeed = 1,
                    AtkArea = 1,
                    ManaCost = 75,
                    TexturePath = "res://Resources/Assets/Summons/GrassBoi.png",
                    Name = "Grass Boi",
                    Description = "A grass boi. It's grassy.\nIncreases the attack power of the summon in front of it by 1.",
                    Code = new RuneCode("adwdsdssa")
                };
            case SummonType.IceMouse:
                return new Stats {
                    Health = 6,
                    AtkPower = 1,
                    AtkSpeed = 1,
                    MoveSpeed = 1,
                    AtkArea = 1,
                    ManaCost = 100,
                    TexturePath = "res://Resources/Assets/Summons/IceMouse.png",
                    Name = "Ice Mouse",
                    Description = "An ice mouse. It's icy.\n5% Chance to freeze the enemy for 3 seconds.",
                    Code = new RuneCode("dadawdsd")
                };
            case SummonType.MagmaPuddle:
                return new Stats {
                    Health = 7,
                    AtkPower = 1,
                    AtkSpeed = 1,
                    MoveSpeed = 1,
                    AtkArea = 1,
                    ManaCost = 125,
                    TexturePath = "res://Resources/Assets/Summons/MagmaPuddle.png",
                    Name = "Magma Puddle",
                    Description = "A magma puddle. It's hot.\nCauses the enemy to burn for 3 seconds.",
                    Code = new RuneCode("sswwdadsd")
                };
            default:
                return new Stats {
                    Health = 0,
                    AtkPower = 0,
                    AtkSpeed = 0,
                    MoveSpeed = 0,
                    AtkArea = 0,
                    ManaCost = 0,
                };
        }
    }
}

public partial class Stats {
    public int Health;
    public int AtkPower;
    public int AtkSpeed;
    public int MoveSpeed;
    public int AtkArea;
    public int ManaCost;
    public string TexturePath = null;
    public string Name = null;
    public string Description;
    public RuneCode Code;
}

public partial class RuneCode {
    public List<RuneType> runes = new List<RuneType>();
    
    // Constructor to take wasd string and convert it to RuneType
    public RuneCode(string wasd) {
        foreach (char c in wasd) {
            switch (c) {
                case 'w':
                    runes.Add(RuneType.Up);
                    break;
                case 'a':
                    runes.Add(RuneType.Left);
                    break;
                case 's':
                    runes.Add(RuneType.Down);
                    break;
                case 'd':
                    runes.Add(RuneType.Right);
                    break;
            }
        }
    }
}