using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    public class Creature:Prefab
    {
        public Creature(string name = "", double maxhealth = 100, double damage = 10, double defense = 0,bool canfight=false) : base(name)
        {
            tags.Add("creature");
            this.maxhealth = this.currenthealth = maxhealth;
            this.damage = damage<0?-damage:damage;
            this.defense = defense % 100;
            this.fightskills = new Dictionary<string, double>();
            this.sarcasmskill = new Dictionary<string, string>();
            this.misskills = new List<string>();

            if (canfight)
            {
                this.fightskills.Add("攻击了", -damage);
                this.misskills.Add("闪避了");
                tags.Add("fightable");
            }
        }
        public void UpdataHealth(double healthdelta)
        {
            if (healthdelta > 0)
                currenthealth = ( currenthealth + healthdelta > maxhealth ) ? maxhealth : ( currenthealth + healthdelta );
            else
                currenthealth = ( currenthealth + healthdelta < 0 ) ? 0 : ( currenthealth + healthdelta );
        }
        public bool IsDead()
        {
            return (int)currenthealth <= 0;
        }
        private static void Fight(Creature attacker,Creature victim,Queue<string> process)
        {
            if (attacker.fightskills.Count <= 0 && attacker.sarcasmskill.Count<=0) { return; } //无法攻击,也无法嘲讽
            Random rand = new Random();
            if(attacker.fightskills.Count==0 || attacker.sarcasmskill.Count>0 && rand.Next(100) < 30 ) //攻击不存在，攻击存在嘲讽也存在且抽到嘲讽
            {
                int sarcasmindex = rand.Next(attacker.sarcasmskill.Count);
                var sarcasmkav = attacker.sarcasmskill.ElementAt(sarcasmindex);
                string str = attacker.name + sarcasmkav.Key + victim.name + sarcasmkav.Value;
                process.Enqueue(str);
            }
            else  // 攻击存在，没抽到嘲讽或嘲讽不存在
            {
                int fightindex = rand.Next(attacker.fightskills.Count);
                var fightkav = attacker.fightskills.ElementAt(fightindex);
                if(fightkav.Value>0) //对自己使用
                {
                    process.Enqueue(attacker.name + fightkav.Key);
                    attacker.UpdataHealth(fightkav.Value);
                }
                else
                {
                    string str = attacker.name + fightkav.Key + victim.name;
                    if (victim.misskills.Count>0 && rand.Next(100)<30) //存在闪避且抽到
                    {
                        int missindex = rand.Next(victim.misskills.Count);
                        var misskav = victim.misskills.ElementAt(missindex);
                        process.Enqueue(str);
                        process.Enqueue("***"+victim.name + misskav + attacker.name + "的攻击"+"***");
                    }
                    else //没能闪避，扣血
                    {
                        if(victim.HasTag("traveler")&&Form1.traveler.armor!=null)
                        {
                            var traveler = (Traveler) victim;
                            traveler.armor.curdurability -= fightkav.Value;
                            if(traveler.armor.IsBroken())
                            {
                                traveler.RemoveArmor();
                            }
                        }

                        int demage = (int)(( victim.defense - 100)*0.01 * fightkav.Value); //负负得正
                        process.Enqueue(str + "，造成了" + demage + "的伤害");
                        victim.UpdataHealth(-demage);
                    }
                }
            }
        }
        public static Queue<string> Fight(Creature first,Creature later)
        {
            if (!( first.HasTag("fightable") && later.HasTag("fightable") )) { return null; }

            Queue<string> process = new Queue<string>();
            process.Enqueue("~~~!!! " + first.name + "与" + later.name + "的战斗 !!!~~~");
            while (true)
            {
                Fight(first, later, process);
                if (later.IsDead())
                {
                    process.Enqueue("~~~!!! "+first.name+"打败了"+later.name+" !!!~~~");
                    break;
                }
                Fight(later, first, process);
                if (first.IsDead())
                {
                    process.Enqueue("~~~!!! "+later.name + "打败了" + first.name+" !!!~~~");
                    break;
                }
            }
            return process;
        }
        public string[] GetFightString(Creature first,Creature later)
        {
            Queue<string> process = Fight(first, later);
            return process.ToArray();
        }
        //为了方便显示要用int
        public double maxhealth;
        public double currenthealth;
        public double damage;
        public double defense;
        public Dictionary<string, double> fightskills; // 大于0 回血 小于等于0 攻击 
        public List<string> misskills; //抽到这个时可以miss别人一次攻击
        public Dictionary<string,string> sarcasmskill; //嘲讽
        
    }
}
