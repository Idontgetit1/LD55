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
                    TexturePath = "res://Resources/Assets/Summons/FlameWolf.png"
                };
            case SummonType.Slime:
                return new Stats {
                    Health = 2,
                    AtkPower = 1,
                    AtkSpeed = 1,
                    MoveSpeed = 1,
                    AtkArea = 1,
                    ManaCost = 10,
                };
            case SummonType.Spider:
                return new Stats {
                    Health = 1,
                    AtkPower = 1,
                    AtkSpeed = 1,
                    MoveSpeed = 1,
                    AtkArea = 1,
                    ManaCost = 10,
                };
            case SummonType.Whatever:
                return new Stats {
                    Health = 4,
                    AtkPower = 1,
                    AtkSpeed = 1,
                    MoveSpeed = 1,
                    AtkArea = 1,
                    ManaCost = 50,
                };
            case SummonType.Placeholder1:
                return new Stats {
                    Health = 5,
                    AtkPower = 1,
                    AtkSpeed = 1,
                    MoveSpeed = 1,
                    AtkArea = 1,
                    ManaCost = 75,
                };
            case SummonType.Placeholder2:
                return new Stats {
                    Health = 6,
                    AtkPower = 1,
                    AtkSpeed = 1,
                    MoveSpeed = 1,
                    AtkArea = 1,
                    ManaCost = 100,
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
}