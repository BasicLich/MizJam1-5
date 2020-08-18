using System;
using System.Collections.Generic;
using System.Text;

using static MizJam1.Unit.Stats;

namespace MizJam1.Unit
{
    public class UnitClass
    {
       /* public readonly Dictionary<string, UnitClass> UnitClasses = new Dictionary<string, UnitClass>() 
        {
            ["Warrior"] = new UnitClass("Warrior", (Attack, Defense), new Dictionary<Stats, ushort>() {[] })
            };*/


        public UnitClass(string name, (Stats, Stats) oppositeStats, Dictionary<Stats, ushort> defaultStatsValue)
        {
            Name = name;
            OppositeStats = oppositeStats;
            DefaultStatsValue = defaultStatsValue;
        }

        public string Name { get; set; }
        public (Stats, Stats) OppositeStats { get; set; }
        public Dictionary<Stats, ushort> DefaultStatsValue { get; set; }

        public Dictionary<Stats, ushort> GetStats(ushort leftOppositeStatValue)
        {
            Dictionary<Stats, ushort> stats = new Dictionary<Stats, ushort>();

            foreach (var stat in DefaultStatsValue)
            {
                stats.Add(stat.Key, stat.Value);
            }
            stats.Add(OppositeStats.Item1, leftOppositeStatValue);
            stats.Add(OppositeStats.Item2, (ushort)(7 - leftOppositeStatValue));

            return stats;
        }
    }
}
