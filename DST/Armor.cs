using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    public class Armor:Prefab
    {
        public Armor(string name="",double defense= 0.6,double maxdurability=1200):base(name)
        {
            this.defense = defense;
            this.maxdurability = this.curdurability = maxdurability;
            this.AddTag("armor");
            this.AddTag("reusable");
            this.getdurabilityfn = GetDurability;
        }
        public Armor(Armor other):base(other)
        {
            this.defense = other.defense;
            this.maxdurability = other.maxdurability;
            this.curdurability = other.curdurability;
        }
        public bool IsBroken()
        {
            return curdurability <= 0;
        }
        public double GetDurability(Prefab who)  //耐久度
        {
            var my = (Armor)who;
            return my.curdurability / my.maxdurability;
        }
        public double defense;  //吸收60%的伤害
        public double curdurability; //直接扣耐久
        public double maxdurability;
    }
}
