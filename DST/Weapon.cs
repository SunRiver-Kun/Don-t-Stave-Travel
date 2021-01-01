using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    public class Weapon:Prefab
    {
        public Weapon(string name="",double damage=10,int usetime=1):base(name)
        {
            this.damage = damage;
            this.maxusetime = this.curusetime =usetime;
            this.fightskills = new Dictionary<string, double>();
            this.misskills = new List<string>();
            this.sarcasmskill = new Dictionary<string, string>();

            this.getdurabilityfn = GetDurability;
            this.fightskills.Add("使用" + name + "攻击了", damage);
            this.AddTag("weapon");
            this.AddTag("reusable");
        }
        public Weapon(Weapon other):base(other)
        {
            this.damage = other.damage;
            this.maxusetime = other.maxusetime;
            this.curusetime = other.curusetime;
            this.fightskills = new Dictionary<string, double>(other.fightskills);
            this.misskills = new List<string>(other.misskills);
            this.sarcasmskill = new Dictionary<string, string>(other.sarcasmskill);
        }
        public double GetDurability(Prefab who)
        {
            var my = (Weapon) who;
            return (double) my.curusetime / my.maxusetime;
        }
        public void AddFightskill(string name,double damege)
        {//attacker.name + fightkav.Key + victim.name+"造成了"+fightskills.Values+"的伤害"
            this.fightskills.Add(name, damege);
        }
        public void AddMisskill(string str)
        {//victim.name + misskav + attacker.name + "的攻击"
            this.misskills.Add(str);
        }
        public void AddSarskill(string to,string action)
        {//attacker.name + sarcasmkav.Key + victim.name + sarcasmkav.Value;
            this.sarcasmskill.Add(to, action);
        }
        public bool IsBroken()
        {
            return curusetime <= 0;
        }
        public double damage;
        public int maxusetime,curusetime;
        //用的时候直接添加给Creater,切换的时候再却掉
        public Dictionary<string, double> fightskills; // 大于0 回血 小于等于0 攻击 
        public List<string> misskills; //抽到这个时可以miss别人一次攻击
        public Dictionary<string, string> sarcasmskill; //嘲讽
    }
}
