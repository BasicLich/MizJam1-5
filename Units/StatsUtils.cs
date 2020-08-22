using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1.Units
{
    public static class StatsUtils
    {
        public static string GetName(Stats stat)
        {
            switch (stat)
            {
                case Stats.MaxHealth:return "HP";
                case Stats.Attack:return "ATT";
                case Stats.Defense: return "DEF";
                case Stats.Range: return "RNG";
                case Stats.Magic: return "MAG";
                case Stats.MagicDefense: return "MDEF";
                case Stats.Speed: return "SPD";
            }
            return null;
        }
    }
}
