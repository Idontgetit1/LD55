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
                    TexturePath = "res://Resources/Assets/Summons/FlameWolf.png"
                };
            case SummonType.Slime:
                return new Stats {
                    Health = 2,
                    AtkPower = 1,
                    AtkSpeed = 1,
                    MoveSpeed = 1,
                    AtkArea = 1
                };
            case SummonType.Spider:
                return new Stats {
                    Health = 1,
                    AtkPower = 1,
                    AtkSpeed = 1,
                    MoveSpeed = 1,
                    AtkArea = 1
                };
            case SummonType.Whatever:
                return new Stats {
                    Health = 4,
                    AtkPower = 1,
                    AtkSpeed = 1,
                    MoveSpeed = 1,
                    AtkArea = 1
                };
            case SummonType.Placeholder1:
                return new Stats {
                    Health = 5,
                    AtkPower = 1,
                    AtkSpeed = 1,
                    MoveSpeed = 1,
                    AtkArea = 1
                };
            case SummonType.Placeholder2:
                return new Stats {
                    Health = 6,
                    AtkPower = 1,
                    AtkSpeed = 1,
                    MoveSpeed = 1,
                    AtkArea = 1
                };
            default:
                return new Stats {
                    Health = 0,
                    AtkPower = 0,
                    AtkSpeed = 0,
                    MoveSpeed = 0,
                    AtkArea = 0
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
    public string TexturePath = null;
}