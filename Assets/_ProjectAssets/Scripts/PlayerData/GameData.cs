using Newtonsoft.Json;
using System;

[Serializable]
public class GameData
{
    int seasonNumber;
    DateTime seasonEnds;
    int levelBaseExp;
    int levelBaseScaler;
    int respinPrice;
    int glassOfMilkPrice;
    int jugOfMilkPrice;

    public bool HasSeasonEnded => DateTime.UtcNow > SeasonEnds;

    public int SeasonNumber
    {
        get
        {
            return seasonNumber;
        }
        set
        {
            seasonNumber = value;
        }
    }

    public DateTime SeasonEnds
    {
        get
        {
            return seasonEnds;
        }
        set
        {
            seasonEnds = value;
        }
    }

    public int LevelBaseExp
    {
        get
        {
            return levelBaseExp;
        }
        set
        {
            levelBaseExp = value;
        }
    }

    public int LevelBaseScaler
    {
        get
        {
            return levelBaseScaler;
        }
        set
        {
            levelBaseScaler = value;
        }
    }

    public int RespinPrice
    {
        get
        {
            return respinPrice;
        }
        set
        {
            respinPrice = value;
        }
    }

    public int GlassOfMilkPrice
    {
        get
        {
            return glassOfMilkPrice;
        }
        set
        {
            glassOfMilkPrice = value;
        }
    }

    public int JugOfMilkPrice
    {
        get
        {
            return jugOfMilkPrice;
        }
        set
        {
            jugOfMilkPrice = value;
        }
    }
}
