using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    public class g_prefabs
    {
        //没有示例，全部static，第一次使用前先调用InitPrefabs
        public static Form1 MainForm;
        public static void SetMainForm(Form1 form) { MainForm = form; }
        //背包类的就不用加了，直接改数据就行
        static string[] material = new string[]
        {// Prefab  tags.Add("meterial")
            "木材","树枝","干草","蜘蛛丝","蜘蛛腺","猪皮","燧石","石头","金块","冰","齿轮","花瓣","牛毛","兔毛","莎草纸","木炭"
        };
        static string[] tool = new string[]
        { //string name , int usetime
            "斧头 3","鹤嘴锄 2","火把 1","篝火 1","石篝火 20","钓鱼竿 3","烹饪锅 6"
        };
        static string[] weapon = new string[]
        {//string name , double damage, int usetime
            "长矛 34 5","火腿棒 56 20","猎枪 80 8","回力标 30 10","影刀 80 3"
        };
        static string[] armor = new string[]
        {//string name ,double defense , double maxdurability
            "草甲 0.6 1200","木甲 0.8 2000","大理石甲 0.85 3000","影甲 0.85 4000","橄榄球头盔 0.85 2000"
        };
        static string[] hat = new string[]
        { //
            "花环 5 100","草帽 1 100","高礼帽 3 200"
        };

        static string[] food = new string[]
        {//string name="", double hungerdelta = 10, double healthdelta = 0 , double sanitydelta =0
            "大肉 15 2 -5 25 3 0 cookedmeat","小肉 10 1 -5 15 2 0 cookedsmallmeat","怪物肉 15 -10 -20 20 -3 -15 cookedmonstermeat",
            "胡萝卜 10 4 0 15 3 0 carrot_cooked","鱼 20 10 5 25 10 10 fish_cooked","浆果 10 3 0 13 1 0 berries_cooked",
            "end 0 0 0",
            "炖肉汤 100 15 20","鱼排 30 40 20","肉丸 40 10 12","怪物千层饼 30 -20 -10","失败料理 0 0 0"
        };
        //static string[] cookfood_meat = new string[]
        //{

        //}

        ////static string[][] foodrecipe = new string[][] {
        ////    new string[]{ "炖肉汤" ,"bonestew","大肉 3 小肉 1","大肉 4","大肉 3 怪物肉 1"},
        ////    new string[]{ "鱼排","fishsticks","树枝 1 鱼 1 胡萝卜 2"},
        ////    new string[]{ "肉丸","meatballs","有肉 其他"},
        ////    new string[]{ "怪物千层饼","monsterlasagna","怪物肉 2 其他 2"},

        ////};

        static string[] goods = new string[]
        {// Prefab,  tags.Add("tradeable")
            "金块","红宝石","绿宝石","蓝宝石","紫宝石","黑宝石","彩色宝石"
        };

        private static readonly Dictionary<string, Prefab> prefabs = new Dictionary<string, Prefab>(); //除下面几种以外的物品
        private static readonly Dictionary<string, Tool> tools = new Dictionary<string, Tool>();
        private static readonly Dictionary<string, Weapon> weapons = new Dictionary<string, Weapon>();
        private static readonly Dictionary<string, Armor> armors = new Dictionary<string, Armor>();
        private static readonly Dictionary<string, Food> foods = new Dictionary<string, Food>();
        private static readonly Dictionary<string, Hat> hats = new Dictionary<string, Hat>();
        public static void InitPrefabs()
        {
            foreach (string name in material)
            {
                Prefab prefab = new Prefab(name);
                prefab.AddTag("material");
                prefab.AddTag("stackable");//可堆叠
                prefabs.Add(name, prefab);
            }
            foreach(string v in tool)
            {
                string[] str = v.Split();
                string name = str[0];
                int num = Convert.ToInt32(str[1]);
                Tool prefab = new Tool(name,num);
                SetToolUSe(prefab);
                tools.Add(name, prefab);
            }
            foreach(string v in weapon)
            {
                var traveler = Form1.traveler;
                string[] str = v.Split();
                string name = str[0];
                double damage = Convert.ToDouble(str[1]);
                int usetime = Convert.ToInt32(str[2]);
                Weapon prefab = new Weapon(name,damage,usetime);
                SetWeaponSkills(prefab);

                prefab.usefn1 = (Prefab who) => {
                    var weapon = (Weapon) who;
                    traveler.SetWeapon(weapon);
                    weapon.curusetime--;
                    MainForm.TB_weapon.Text = weapon.name;
                    if (weapon.IsBroken())
                    {
                        MainForm.TB_weapon.Text = "";
                        traveler.backpack.Remove(weapon.name, 1);
                    }
                    MainForm.ReflashBackpackList();
                };

                weapons.Add(name, prefab);
            }
            foreach (string v in armor)
            {
                var traveler = Form1.traveler;
                string[] str = v.Split();
                string name = str[0];
                double defense = Convert.ToDouble(str[1]);
                double maxdurability = Convert.ToDouble(str[2]);
                Armor prefab = new Armor(name,defense,maxdurability);

                prefab.usefn1 = (Prefab who) => {
                    var armor = (Armor) who;
                    traveler.SetArmor(armor);
                    traveler.backpack.Remove(armor.name, 1);
                    MainForm.TB_armor.Text = armor.name;
                    MainForm.ReflashBackpackList();
                };

                armors.Add(name, prefab);              
            }
            foreach(string v in hat)
            {
                var traveler = Form1.traveler;
                string[] str = v.Split();
                string name = str[0];
                double sanitybonus = Convert.ToDouble(str[1]);
                int usetime = Convert.ToInt32(str[2]);
                Hat prefab = new Hat(name, sanitybonus, usetime);
                prefab.usefn1 = (Prefab who) => {
                    var hat = (Hat) who;
                    traveler.SetHat(hat);
                    traveler.backpack.Remove(who.name, 1);
                    MainForm.TB_hat.Text = who.name;
                    MainForm.timer_baseupdata.Enabled = true;
                };
                hats.Add(name, prefab);
            }
            foreach (string v in food) //简单的烤
            {
                var traveler = Form1.traveler;
                string[] str = v.Split();
                string name = str[0];
                if (name == "end") { continue; }
                double hungerdelta = Convert.ToDouble(str[1]);
                double healthdelta = Convert.ToDouble(str[2]);
                double sanitydelta = Convert.ToDouble(str[3]);
                Food prefab = new Food(name, hungerdelta, healthdelta, sanitydelta);
                foods.Add(prefab.name, prefab);
                prefab.usefn1 = (Prefab who) => {
                    traveler.Eat((Food) who);
                    traveler.backpack.Remove(who.name, 1);
                    MainForm.ReflashBasedata();
                };

                if (str.Length>4) 
                {
                    double cookhungerdelta = Convert.ToDouble(str[4]);
                    double cookhealthdelta = Convert.ToDouble(str[5]);
                    double cooksanitydelta = Convert.ToDouble(str[6]);

                    Food cookprefab = new Food("烤" + name, cookhungerdelta, cookhealthdelta, cooksanitydelta);
                    cookprefab.recipe.Add(prefab.name, 1);
                    prefab.SetCookfood(cookprefab);

                    foods.Add(cookprefab.name, cookprefab);
                    MainForm.AddCookButton(cookprefab,str[7]);  //强迫加入图片
                    cookprefab.usefn1 = (Prefab who) => {
                        traveler.Eat((Food) who);
                        traveler.backpack.Remove(who.name, 1);
                        MainForm.ReflashBasedata();
                    };
                }
            }

            foreach(string name in goods)
            {
                if(prefabs.ContainsKey(name))
                {
                    prefabs[name].AddTag("tradeable");
                }
                else if(tools.ContainsKey(name))
                {
                    tools[name].AddTag("tradeable");
                }
                else if (weapons.ContainsKey(name))
                {
                    weapons[name].AddTag("tradeable");
                }
                else if (armors.ContainsKey(name))
                {
                    armors[name].AddTag("tradeable");
                }
                else if (foods.ContainsKey(name))
                {
                    foods[name].AddTag("tradeable");
                }
                else
                {
                    Prefab newprefab = new Prefab(name);
                    newprefab.AddTag("tradeable");
                    prefabs.Add(name, newprefab);
                }
            }
        }

        public static void AddPrefab(Prefab prefab)
        {
            if(!prefabs.ContainsKey(prefab.name))
                 prefabs.Add(prefab.name, new Prefab(prefab)); //要自己存一份
        }
        public static void AddPrefab(Tool prefab)
        {
            if (!tools.ContainsKey(prefab.name))
                tools.Add(prefab.name, new Tool(prefab));
        }
        public static void AddPrefab(Weapon prefab)
        {
            if (!weapons.ContainsKey(prefab.name))
                weapons.Add(prefab.name, new Weapon(prefab));
        }
        public static void AddPrefab(Armor prefab)
        {
            if (!armors.ContainsKey(prefab.name))
                armors.Add(prefab.name, new Armor(prefab));
        }
        public static void AddPrefab(Food prefab)
        {
            if (!foods.ContainsKey(prefab.name))
                foods.Add(prefab.name, new Food(prefab));
        }
        public static void AddPrefab(Hat prefab)
        {
            if (!hats.ContainsKey(prefab.name))
                hats.Add(prefab.name, new Hat(prefab));
        }
        public static void RemovePrefab(string name)
        {
            if (prefabs.ContainsKey(name))
                prefabs.Remove(name);
            else if (tools.ContainsKey(name))
                tools.Remove(name);
            else if (weapons.ContainsKey(name))
                weapons.Remove(name);
            else if (armors.ContainsKey(name))
                armors.Remove(name);
            else if (foods.ContainsKey(name))
                foods.Remove(name);
            else if (hats.ContainsKey(name))
                hats.Remove(name);
          
        }
        public static void RemovePrefab(Prefab prefab)
        {
            prefabs.Remove(prefab.name);
        }
        public static void RemovePrefab(Tool prefab)
        {
            tools.Remove(prefab.name);
        }
        public static void RemovePrefab(Weapon prefab)
        {
            weapons.Remove(prefab.name);
        }
        public static void RemovePrefab(Armor prefab)
        {
            armors.Remove(prefab.name);
        }
        public static void RemovePrefab(Food prefab)
        {
            foods.Remove(prefab.name);
        }
        public static void RemovePrefab(Hat prefab)
        {
            hats.Remove(prefab.name);
        }
        public static Prefab GetPrefab(string name)
        {
            if (prefabs.ContainsKey(name))
                return new Prefab(prefabs[name]);
            else if (tools.ContainsKey(name))
                return new Tool(tools[name]);
            else if (weapons.ContainsKey(name))
                return new Weapon(weapons[name]);
            else if (armors.ContainsKey(name))
                return new Armor(armors[name]);
            else if (foods.ContainsKey(name))
                return new Food(foods[name]);
            else if (hats.ContainsKey(name))
                return new Hat(hats[name]);
            return null;
        }
        public static bool IsExistPrefab(string name)
        {
            return prefabs.ContainsKey(name) || tools.ContainsKey(name) || weapons.ContainsKey(name) || armors.ContainsKey(name) || foods.ContainsKey(name) || hats.ContainsKey(name);
        }

        //使用以及烹饪
        private static void SetToolUSe(Tool tool)
        { //"斧头","鹤嘴锄","火把","篝火","石篝火","钓鱼竿"
            Random rand = new Random();
            var traveler = Form1.traveler;
            if(tool.name=="斧头")
            {//Tool tool,string tag, Dictionary<string,uint> input;
             //string spoil,string action,string unit="个")
                Dictionary<string, uint> input = new Dictionary<string, uint>();
                input.Add("木材 砍树 个", 10);
                GeneralToolfn(tool, "canuseaxe",input);
            }
            else if(tool.name== "鹤嘴锄")
            {
                Dictionary<string, uint> input = new Dictionary<string, uint>();
                input.Add("金块 挖矿 个", 2);
                input.Add("石头 挖矿 个", 8);
                input.Add("燧石 挖矿 个", 6);
                GeneralToolfn(tool, "canusepickaxe", input);              
            }
            else if (tool.name == "火把")
            {
                tool.SetUsefn((Prefab who) => {
                    if(Worldtime.GetState()=="晚上")
                    {
                        traveler.AddTag("light"); //判断是否能在黑暗中行走
                        traveler.backpack.Remove(who.name, 1);
                        MainForm.Print(traveler.name + "点上了火把。");
                    }
                });
            }
            else if (tool.name == "篝火" || tool.name=="石篝火")
            {
                tool.SetUsefn((Prefab who) => {
                    traveler.AddTag("light"); //判断是否能在黑暗中行走
                    traveler.AddTag("cookable");
                    if(tool.name == "篝火")
                         traveler.backpack.Remove(who.name, 1);
                    else
                    {
                        var my = (Tool) who;
                        my.curusetime--;
                        if (my.IsBroken())
                        {
                            traveler.backpack.Remove(my.name, 1);
                        }
                    }
                    MainForm.Print(traveler.name + "生起了" + tool.name + "。");
                    MainForm.EnCookMenu();
                    MainForm.ReflashBackpackList();
                });
            }
            else if (tool.name == "钓鱼竿")
            {
                Dictionary<string, uint> input = new Dictionary<string, uint>();
                input.Add("鱼 钓鱼 条", 4);
                GeneralToolfn(tool, "canfish", input);
            }
            else if(tool.name=="烹饪锅")
            {
                tool.SetUsefn((Prefab who) => {
                    traveler.AddTag("cookable");
                    var my = (Tool) who;
                    my.curusetime--;
                    if (my.IsBroken())
                    {
                        traveler.backpack.Remove(my.name, 1);
                    }
                    MainForm.Print(traveler.name + "架起来" + tool.name + "。");
                    MainForm.EnCookRecipe();
                    MainForm.ReflashBackpackList();
                });
            }
            else
            {

            }
        }
        private static void SetWeaponSkills(Weapon weapon)
        { //"长矛","火腿棒","猎枪","回力标"
            var traveler = Form1.traveler;
            if (weapon.name == "长矛")
            {//traveler.name + skills + target.name
                //
                weapon.AddFightskill("恶狠狠地刺向了", 52);
                weapon.AddFightskill("脚底一滑，扑向了", 10);
                weapon.AddSarskill("在","面前虚晃一枪");
                weapon.AddMisskill("摆出格挡姿势，格挡了");
            }
            else if (weapon.name == "火腿棒")
            {
                weapon.AddFightskill("反手就是一棒，劈向了",60 );
                weapon.AddFightskill("用火腿棒最软的地方，劈向了", 10);
                weapon.AddSarskill("在", "面前吃着火腿");
                weapon.AddMisskill("摆出格挡姿势，格挡了");
            }
            else
            {

            }
        }
        private static void GeneralToolfn(Tool tool,string tag,Dictionary<string,uint>input)
        {
            tool.SetUsefn((Prefab who) => {
                Random rand = new Random();
                var traveler = Form1.traveler;
                char[] separator = { ' ' };
                StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries;
                if (traveler.HasTag(tag))
                {
                    if ( Worldtime.GetState()=="晚上" && !traveler.HasTag("light"))
                    {
                        // MainForm.Print(traveler.name + "迷失在了黑暗当中");
                        g_baseresult.LostInDark();
                        return;
                    }
                    MainForm.SleepTime();
                    TimeSpan passtime = Worldtime.GetTimeNowToNext();
                    string hour = Convert.ToString((int) ( passtime.TotalHours * 10 ) / 10.0); //保留一位小数 
                    //--------------------------
                    var my = (Tool) who;
                    my.curusetime--;
                    if (my.IsBroken())
                    {
                        traveler.backpack.Remove(my.name, 1);
                    }
                    //----------------------------
                    Queue<string> output = new Queue<string>();
                    foreach (var v in input)
                    {
                        int num = (int) ( passtime.TotalMinutes*0.01 * v.Value ); // (12-6)*60*0.01 = 3.6
                        num = rand.Next(num-1)+1;
                        
                        string[] str = v.Key.Split(separator,options);
                        if (str.Length != 3) { return; }
                        string spoil = str[0], action = str[1], unit = str[2];
                        if (traveler.backpack.IsFull() && !traveler.backpack.IsExistObject(spoil))
                        {
                            output.Enqueue(traveler.name + "花费了" + hour + "小时来" + action + ",掉落了" + num + unit + spoil + ",却因背包满而带不回来。");
                        }
                        else
                        {
                            traveler.backpack.AddObject(spoil, (uint)num );
                            output.Enqueue(traveler.name + "花费" + hour + "小时来" + action + ",得到了"  + (uint)num + unit + spoil + "。");
                        }
                    }
                    //-------------------
                    MainForm.Print(output);
                   // MainForm.Awake(passtime);
                    MainForm.ReflashBackpackList();
                    Worldtime.GotoNextState();
                    MainForm.Awake();
                }
            });
        }
    }
}
