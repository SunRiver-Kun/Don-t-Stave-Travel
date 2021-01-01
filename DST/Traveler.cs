using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    public class Traveler:Creature
    {
        public Backpack backpack;
        public Weapon weapon = null;
        public Armor armor = null ;
        public Hat hat = null;
        private double orginaldamage;

        public double maxsanity, maxhunger, currentsanity, currenthunger;
        public double hungerrate = -1, sanityrate = 0;
        public double sanitybonus = 0;  //回san时用的/扣
        public Traveler(string name,double maxhealth,double maxsanity,double maxhunger,double damage=10,double defense=0):base(name,maxhealth,damage,defense)   
        {
            this.maxsanity = this.currentsanity = maxsanity;
            this.maxhunger = this.currenthunger = maxhunger;

            this.backpack = new Backpack(6);
            this.orginaldamage = damage;
            this.fightskills.Add("使用小拳拳，攻击了",-damage);
            this.misskills.Add("闪避了");
            this.tags.Add("fightable"); //想战斗就加这个标签

            this.tags.Add("human");
            this.tags.Add("takeable");
            this.tags.Add("traveler"); //唯一的旅行者标签，来判断玩家

        }
        public void Eat(double hungerdelta = 10, double healthdelta=0,double sanitydelta=0)
        {
            this.UpdataHealth(hungerdelta);
            this.UpdataSanity(sanitydelta);
            this.UpdataHunger(hungerdelta);
        }
        public void Eat(Food food)
        {
            Eat(food.hungerdelta, food.healthdelta, food.sanitydelta);
        }
      
        public void UpdataSanity(double sanitydelta)
        {
            if (sanitydelta > 0)
                currentsanity = ( currentsanity + sanitydelta > maxsanity ) ? maxsanity : ( currentsanity + sanitydelta );
            else
                currentsanity = ( currentsanity + sanitydelta < 0 ) ? 0 : ( currentsanity + sanitydelta );
        }
        public void UpdataSanity(TimeSpan time,double rata=1)
        {
            double delta = rata * time.TotalHours * 5 * ( sanityrate + sanitybonus );
            UpdataSanity(delta);
        }
        public void UpdataHunger(double hungerdelta)  //hunger==0 -> delta health
        {
            if (hungerdelta > 0)
                currenthunger = ( currenthunger + hungerdelta > maxsanity ) ? maxsanity : ( currenthunger + hungerdelta );
            else
            {
                if(currenthunger + hungerdelta > 0)
                {
                    currenthunger += hungerdelta;
                }
                else
                {
                    double delta = ( currenthunger + hungerdelta )*3;  // 
                    currenthunger = 0;
                    UpdataHealth(delta); 
                    if(IsDead())
                    {
                        g_baseresult.GotoResult(Form1.traveler.name + "死于饿死");
                    }
                }
            }
        }
        public void UpdataHunger(TimeSpan time,double rate = 1)
        {
            double delta = rate * time.TotalHours * 5 * ( hungerrate );  //-
            UpdataHunger(delta);
        }
        //--------------------
        public void UpdataSanity(object sender, EventArgs e)  //这个是专门给bonus用的,用timer_Sanitybonus刷新
        {
            UpdataSanity(sanityrate + sanitybonus);
            if(this.hat!=null)
            {
                if (this.hat.usetime > 0) 
                {
                    this.hat.usetime--;
                }
                else
                {
                    RemoveHat();
                }
            }
        }   
        public void UpdataHunger(object sender,EventArgs e) //刷新
        {
            UpdataHunger(hungerrate); //扣啊扣
        }
        //---------------------
        public void SetWeapon(Weapon weapon)
        {
            this.weapon = weapon;
            this.fightskills.Clear();
            this.fightskills = weapon.fightskills;
            this.sarcasmskill.Clear();
            this.sarcasmskill = weapon.sarcasmskill;
            this.misskills.Clear();
            this.misskills = weapon.misskills;
        }
        public void RemoveWeapon()
        {
            this.weapon = null;
            this.fightskills.Clear();
            this.fightskills.Add("使用小拳拳，攻击了",- this.orginaldamage);
            this.sarcasmskill.Clear();
            this.misskills.Clear();
            this.misskills.Add("闪避了");
        }
        public void SetArmor(Armor armor)
        {
            this.armor = armor;
            this.defense = armor.defense;
        }
        public void RemoveArmor()
        {
            this.armor = null;
            this.defense = 0;
        }
        public void SetHat(Hat hat)
        {
            this.hat = hat;
            this.sanitybonus += hat.sanitybonus;
        }
        public void RemoveHat()
        {
            this.sanitybonus -= this.hat.sanitybonus;
            this.hat = null;            
        }
        public void ReSet() //重新开始游戏使用
        {
            this.armor = null;
            this.hat = null;
            this.weapon = null;

            this.currenthealth = this.maxhealth;
            this.currenthunger = this.maxhunger;
            this.currentsanity = this.maxsanity;

            this.hungerrate = -1;
            this.sanityrate = 0;
            this.sanitybonus = 0;  //回san时用的/扣
            this.damage = 1;
            this.defense = 0;

            this.backpack.Clear();
            this.backpack.Resize(6);

            this.damage = this.orginaldamage;
            this.fightskills.Clear();
            this.misskills.Clear();
            this.sarcasmskill.Clear();
            this.tags.Clear();

            this.fightskills.Add("使用小拳拳，攻击了", -damage);
            this.misskills.Add("闪避了");
            this.tags.Add("fightable"); //想战斗就加这个标签
        
            this.tags.Add("human");
            this.tags.Add("takeable");
            this.tags.Add("traveler"); //唯一的旅行者标签，来判断玩家
        }
    }
}
