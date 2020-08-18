using System;
using System.Collections.Generic;
using System.Text;

using static MizJam1.Unit.Stats;

namespace MizJam1.Unit
{
    public class Unit
    {
        public Unit(string name, UnitClass unitClass, bool enemy)
        {
            Name = name;
            UnitClass = unitClass;
            Stats = new Dictionary<Stats, ushort>();
            Enemy = enemy;
        }

        public Dictionary<Stats, ushort> Stats;

        public string Name { get; set; }
        public UnitClass UnitClass { get; set; }
        public bool Enemy { get; set; }

        private ushort health;
        public ushort Health { get { return health; } set { if (value < Stats[MaxHealth]) health = value; else health = Stats[MaxHealth]; } }
    }
}
