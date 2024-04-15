using System.Collections.Generic;

public static partial class TypeStats
{
    public static Stats GetStats(SummonType type) {
        switch (type) {
            case SummonType.FlameWolf:
                return new Stats {
                    Health = 5,
                    AtkPower = 7,
                    AtkEvery = 5,
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
                    Health = 3,
                    AtkPower = 1,
                    AtkEvery = 3,
                    MoveSpeed = 1,
                    AtkArea = 1,
                    ManaCost = 10,
                    TexturePath = "res://Resources/Assets/Summons/Slime4.png",
                    Name = "Slime",
                    Description = "A slime. It's slimy.\nIf killed, slows enemy.",
                    Code = new RuneCode("adsswds")
                };
            case SummonType.Slimeloon:
                return new Stats {
                    Health = 5,
                    AtkPower = 5,
                    AtkEvery = 5,
                    MoveSpeed = 1,
                    AtkArea = 1,
                    ManaCost = 15,
                    TexturePath = "res://Resources/Assets/Summons/Slimeloon.png",
                    Name = "Slimeloon",
                    Description = "A slime with a balloon. It's Floaty.\n15% Chance to not be hit by an attack.",
                    Code = new RuneCode("wswsawd"),
                    BaseScale = 0.5f,
                    HeightOffset = -35f,
                    BobbingStrength = 1.5f,
                    BobbingSpeed = 2f,
                    BobbingPause = 0f
                };
            case SummonType.Tree:
                return new Stats {
                    Health = 20,
                    AtkPower = 3,
                    AtkEvery = 5,
                    MoveSpeed = 1,
                    AtkArea = 1,
                    ManaCost = 50,
                    TexturePath = "res://Resources/Assets/Summons/Tree.png",
                    Name = "Tree",
                    Description = "A tree. It's woody.\nHeals the Summon in front of it for 1 health every 3 seconds.",
                    Code = new RuneCode("ddawdss"),
                    Mirrored = true
                };
            case SummonType.GrassBoi:
                return new Stats {
                    Health = 5,
                    AtkPower = 5,
                    AtkEvery = 1,
                    MoveSpeed = 1,
                    AtkArea = 1,
                    ManaCost = 25,
                    TexturePath = "res://Resources/Assets/Summons/GrassBoi.png",
                    Name = "Grass Boi",
                    Description = "A grass boi. It's grassy.\nSummons in front gain +3 ATK.",
                    Code = new RuneCode("adwdsdssa"),
                    Mirrored = false
                };
            case SummonType.IceMouse:
                return new Stats {
                    Health = 6,
                    AtkPower = 3,
                    AtkEvery = 1,
                    MoveSpeed = 1,
                    AtkArea = 1,
                    ManaCost = 50,
                    TexturePath = "res://Resources/Assets/Summons/IceMouse.png",
                    Name = "Ice Mouse",
                    Description = "An ice mouse. It's icy.\n5% Chance to freeze the enemy for 3 seconds.",
                    Code = new RuneCode("dadawdsd"),
                    Mirrored = true
                };
            case SummonType.MagmaPuddle:
                return new Stats {
                    Health = 10,
                    AtkPower = 5,
                    AtkEvery = 1,
                    MoveSpeed = 1,
                    AtkArea = 1,
                    ManaCost = 80,
                    TexturePath = "res://Resources/Assets/Summons/MagmaPuddle.png",
                    Name = "Magma Puddle",
                    Description = "A magma puddle. It's hot.\nCauses the first 3 enemies to burn for 3 seconds. Hurts enemy for 1 HP if hit.",
                    Code = new RuneCode("sswwdadsd"),
                    Mirrored = true
                };
            case SummonType.Cherry:
                return new Stats {
                    Health = 15,
                    AtkPower = 1,
                    AtkEvery = 1,
                    MoveSpeed = 1,
                    AtkArea = 1,
                    ManaCost = 100,
                    TexturePath = "res://Resources/Assets/Summons/Cherry.png",
                    Name = "Cherry",
                    Description = "A cherry. It's fruity.\nSummons behind will eat for +5 HP and +5 ATK.",
                    Code = new RuneCode("ddaadada"),
                    Mirrored = true
                };
            case SummonType.Bat:
                return new Stats {
                    Health = 8,
                    AtkPower = 5,
                    AtkEvery = 1,
                    MoveSpeed = 1,
                    AtkArea = 1,
                    ManaCost = 100,
                    TexturePath = "res://Resources/Assets/Summons/Bat.png",
                    Name = "Bat",
                    Description = "A bat. It's batty.\nIncreases power of the summon in front of it by 1. Leeches 1 HP every attack.",
                    Code = new RuneCode("wswswsadw"),
                    Mirrored = true,
                    BobbingStrength = 1.5f,
                    BobbingSpeed = 2f,
                    BobbingPause = 0f
                };
            default:
                return new Stats {
                    Health = 0,
                    AtkPower = 0,
                    AtkEvery = 0,
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
    public int AtkEvery;
    public int MoveSpeed;
    public int AtkArea;
    public int ManaCost;
    public string TexturePath = null;
    public string Name = null;
    public string Description;
    public RuneCode Code;
    public float BaseScale = 1f;
    public bool Mirrored = false;
    public float HeightOffset = 0f;
    public float BobbingStrength = 0.25f;
    public float BobbingSpeed = 20f;
    public float BobbingPause = 1f;
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