using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
namespace DST
{
    public class g_address
    {

        public static Form1 MainForm;
        public static void SetMainForm(Form1 form) { MainForm = form; Scane.MainForm = form; }
        public static Address GetAddress(int index)
        {
            if (index >= addresses.Count || index < 0)
                return addresses.Last();
            else
                return addresses.ElementAt(index);
        }
        public static Address GetAddress(string name)
        {
            for(int i=addresses.Count-1;i>=0;--i)
            {
                if (addresses[i].name == name)
                    return addresses[i];
            }
            int index;
            if (int.TryParse(name, out index)) //也有可能传了个数字进来
                return GetAddress(index);
            else
                return addresses.Last();
        }
        public static void GotoAddress(int index)
        {
            while (Form1.TEXT.Count > 0) {; } //哪怕是死循环也不给你过！，去你的线程
            MainForm.AddAddress(GetAddress(index));
            MainForm.GotoNextScane();
        }
        public static void GotoAddress(int index,int order)
        {
            while (Form1.TEXT.Count > 0) {; }
            Address address = GetAddress(index);
            address.enterorder = order - 1; //运行的时候会先 +1，所以要在这先减一
            MainForm.AddAddress(address);
            MainForm.GotoNextScane();
        }
        public static void GotoAddress(string name)
        {
            while (Form1.TEXT.Count > 0) {; }
            //if (Form1.TEXT.Count > 0) { return; }
            MainForm.AddAddress(GetAddress(name));
            MainForm.GotoNextScane();
        }
        public static void GotoAddress(String name,int order)
        {
            while (Form1.TEXT.Count > 0) {; }
            Address address = GetAddress(name);
            address.enterorder = order - 1; //运行的时候会先 +1，所以要在这先减一
            MainForm.AddAddress(address);
            MainForm.GotoNextScane();
        }
        public static bool IsExistAddress(int index)
        {
            return index < addresses.Count;
        }
        private static List<Address> addresses = new List<Address>();
        //------------------------------------
        public static void Initaddress()
        {
            int ENDLESS = 999999;
            Scane scane;
            Address address;
            string name = Form1.traveler.name;
            //string name, Form1.Fn fn, int usetime = 1, bool CanLostInDark = true, bool CanLostCook = true


            //***************************************************************************************************
            /*!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!注意事项!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                在SetEnterfn时如果用到scane的信息请加入参数：Scane who
                !!要打印东西和GotoAddress的，把GotoAddress塞Print里面去！！
            */
            //***************************************************************************************************
            //*----------------梦开始的地方 0  //后者是index  
            #region
            address = new Address("梦开始的地方");
            scane = new Scane();
            address.AddScane(scane);
            g_address.addresses.Add(address);
            NORESTATR.Add(0); //把index加进去，不给重置
            scane.SetEnterText("*请输入旅行者的名字：");
            scane.SetEnterfn(() => {
                ReStart();
                MainForm.DisableAllButton();
                MainForm.TB_creature.ReadOnly = false; //可以输入名字了
                MainForm.SleepAllTime();
            });

            scane.AddButton("开始游戏", () => {
                cls();
                MainForm.MainPicture.Visible = false;
                MainForm.TB_Dialogue.Visible = true;
                GotoAddress("夏湾的小木屋");
            }, ENDLESS, false,true, "critter_kitten_builder");
            scane.AddButton("查看死亡记录", () => {
                cls();
                MainForm.MainPicture.Visible = false;
                MainForm.TB_Dialogue.Visible = true;
                string path = g_baseresult.FILEPATH;
                if (File.Exists(path))
                {
                    string[] strs = File.ReadAllLines(path);
                    if (strs.Length > 0)
                        MainForm.Print(strs);
                    else
                        MainForm.Print("***无死亡记录，暂时的***");
                }
                else
                    MainForm.Print("!!!results.txt，文件不存在!!!");
            }, ENDLESS, false,true, "featherpencil");
            scane.AddButton("退出游戏", () => {
                System.Environment.Exit(0);
            }, ENDLESS, false,true, "deerclops_eyeball");
            scane = new Scane( "欢迎回家，" + name + "╮(╯▽╰)╭" , true, true, false); //复制按钮 是不能新加按钮（省内存） 不复制enterfn和leavefn 
            address.AddScane(scane);
            scane.SetEnterfn(() => {
                ReStart();
            });
            scane = new Scane( "欢迎回家，" + name + "↖(^ω^)↗" , true, true, true);
            address.AddScane(scane);
            scane = new Scane( "欢迎回家，" + name + "♪(´ε｀)" , true, true, true);
            address.AddScane(scane);
            scane = new Scane( "欢迎回家，" + name + "└(^o^)┘" , true, true, true);
            address.AddScane(scane);
            scane = new Scane( "欢迎回家，" + name + "⊙﹏⊙" , true, true, true);
            address.AddScane(scane);
            scane = new Scane( "欢迎回家，" + name + "(ˉ﹃ˉ)" , true, true, true);
            address.AddScane(scane);
            scane = new Scane( "欢迎回家，" + name + "o(︶︿︶)o 唉" , true, true, true);
            address.AddScane(scane);
            scane = new Scane( "欢迎回家，" + name + "(￣▽￣)～■干杯□～(￣▽￣)" , true, true, true);
            address.AddScane(scane);
            scane = new Scane( "欢迎回家，" + name + "o(￣▽￣)ｄ" , true, true, true);
            address.AddScane(scane);
            scane = new Scane( "欢迎回家，" + name + "︿(￣︶￣)︿" , true, true, true);
            address.AddScane(scane);
            scane = new Scane( "再这样按下去就出事了" , true, true, true);
            address.AddScane(14, scane);
            scane = new Scane("", true, true, false); //15
            address.AddScane(scane);
            scane.picturepath = address.name + address.scanes.Count;
            scane.SetEnterfn((Scane who) => {
                cls();
                string str = "";
                for (int i = 0; i < who.entertext[0].Length; ++i) //去掉默认加的line
                {
                    if (who.entertext[0][i] == '-')
                        break;
                    else
                        str += who.entertext[0][i];
                }
                MainForm.PicturePrint(who.picturepath, str);
                ReStart();
            });
            scane = new Scane("出发去远方吧!", true, true, true,false); 
            address.AddScane(20, scane);
            scane.picturepath = address.name + address.scanes.Count;
            scane = new Scane("偶尔也应该坐下来休息休息~", true, true, true,false);
            address.AddScane(25, scane);
            scane.picturepath = address.name + address.scanes.Count;
            scane = new Scane("", true, true, true,false);
            address.AddScane(30, scane);
            scane.picturepath = address.name + address.scanes.Count;
            scane = new Scane("我想我需要个画师2333", true, true, true, false);
            address.AddScane(35, scane);
            scane.picturepath = address.name + address.scanes.Count;
            scane = new Scane("***故事的终章***", true, true, true, false);
            address.AddScane(40, scane);
            scane.picturepath = address.name + address.scanes.Count;
            scane = new Scane("", true, true, true, false);
            address.AddScane(scane);
            scane.picturepath = address.name + address.scanes.Count;
            scane = new Scane("", true, true, true, false);
            address.AddScane(scane);
            scane.picturepath = address.name + address.scanes.Count;
            //*----------------夏湾的小木屋 1
            address = new Address("夏湾的小木屋");
            g_address.addresses.Add(address);
            scane = new Scane();
            address.AddScane(scane);

            scane.SetEnterfn(() => {
                Form1.traveler.AddTag("light");
                MainForm.EnCookMenu();
                MainForm.EnCookRecipe();
            });
            scane.SetLeavefn(() => {
                Form1.traveler.RemoveTag("light");
                MainForm.DeCookMenu();
                MainForm.DeCookRecipe();
            });
            scane.AddButton("接受并离开", () => {
                Form1.traveler.backpack.AddObject("树枝", 4);
                Form1.traveler.backpack.AddObject("干草", 4);
                Form1.traveler.backpack.AddObject("燧石", 2);
                Form1.traveler.backpack.AddObject("火把", 1);
                MainForm.ReflashBackpackList();
                GotoAddress("神秘森林");
            }, 1, false,true, "gift_large2");
            scane.AddButton("直接离开", () => {
                GotoAddress("神秘森林");
            }, ENDLESS, false,true, "fence_gate_item");
            scane.AddButton("我要回家", () => {
                cls();
                GotoAddress(0);
            }, ENDLESS, false,true, "homesign");

            scane = new Scane( "欢迎再次回到夏湾的小木屋，" + name + "↖(^ω^)↗" , true, false,true);
            address.AddScane(scane);
            scane.buttons.RemoveAt(0); //结束并离开
            scane.buttons.RemoveAt(1);//我要回家
            scane.AddButton("休息", g_basebuttonfn.Sleep, ENDLESS, "tent");
            scane.AddButton("使用", g_basebuttonfn.UseItem, ENDLESS, "bonestew");

            scane = new Scane("欢迎再次回到夏湾的小木屋，" + name + "?_?", true, true, true);
            address.AddScane(scane);
            scane = new Scane("欢迎再次回到夏湾的小木屋，" + name + "#^_^#", true, true, true);
            address.AddScane(scane);
            scane = new Scane("欢迎再次回到夏湾的小木屋，" + name + "(*^▽^*)", true, true, true);
            address.AddScane(scane);
            scane = new Scane("欢迎再次回到夏湾的小木屋，" + name + "<(￣︶￣)>", true, true, true);
            address.AddScane(scane);
            scane = new Scane("欢迎再次回到夏湾的小木屋，" + name + "ヾ(≧▽≦*)o", true, true, true);
            address.AddScane(scane);
            scane = new Scane("欢迎再次回到夏湾的小木屋，" + name + "o(*≧▽≦)ツ", true, true, true);
            address.AddScane(scane);
            scane = new Scane("欢迎再次回到夏湾的小木屋，" + name + "(￣▽￣)～■干杯□～(￣▽￣)", true, true, true);
            address.AddScane(scane);
            scane = new Scane("欢迎再次回到夏湾的小木屋，" + name + "都回来了那么多次不试试检查一下这个房间吗？", true, false, true);
            address.AddScane(scane);
            scane.AddButton("检查", () => {
                if (Form1.traveler.backpack.IsFull())
                    MainForm.Print(name+"试图找了找有什么好东西，结果什么也没找到"+endline);
                else
                {
                    Form1.traveler.backpack.AddObject("猎枪",1);
                    MainForm.Print(name+"找到一把猎枪"+endline,MainForm.ReflashBackpackList);
                }
            }, 3, false, false, "backpack_smallbird");
            scane = new Scane("", false, false,false);
            address.AddScane(scane);
            scane.SetEnterfn(()=> {
                MainForm.Print("现在你已经可以独自出去闯荡了，夏湾的小木屋为你关闭",()=> { g_address.GotoAddress(2); });
            });
            //*********************  神秘森林 2  *********************

            address = new Address("神秘森林");
            g_address.addresses.Add(address);
            scane = new Scane();
            address.AddScane(scane);

            scane.AddButton("四处逛逛", (Scane who) => {
                int usedtime = who.GetButton("四处逛逛").UsedTime;
                if (usedtime == 1) //从1开始
                {
                    Queue<string> str = who.GetButtonText(1,"四处逛逛",1);
                    if (Worldtime.GetNextState() == "晚上")
                        str.Enqueue("时间不早了，回去吧");
                    else
                        str.Enqueue("回去吧");
                    GetRadomItem(new string[] {"树枝", "浆果" },new int[] {10,15 },str);
                    MainForm.Print(str, () => {
                        MainForm.ReflashBackpackList();
                    });
                }
                else if (usedtime == 2)
                {
                    Queue<string> str = who.GetButtonText(1,"四处逛逛", 2);
                    if (Worldtime.GetNextState() == "晚上")
                        str.Enqueue("时间不早了，回去吧");
                    else
                        str.Enqueue("回去吧");
                    GetRadomItem(new string[] { "干草", "浆果" ,"胡萝卜","树枝"}, new int[] { 15, 20,16,4 },str);
                    MainForm.Print(str, () => {
                        MainForm.ReflashBackpackList();
                    });
                }
                else
                {
                    Queue<string> str = who.GetButtonText(1,"四处逛逛", 3);
                    if (Worldtime.GetNextState() == "晚上")
                        str.Enqueue("时间不早了，回去吧\r\n"+ endline);
                    else
                        str.Enqueue("回去吧\r\n"+ endline);
                    MainForm.Print(str);
                }
            }, 3,true,true, "cartographydesk", true);
            scane.AddButton("进入小木屋", () => {
                GotoAddress("夏湾的小木屋");
            }, ENDLESS, false,true, "homesign",false);
            //------------------判断，是否逛完了
            scane = new Scane("");
            address.AddScane(scane);
            scane.SetEnterfn((Scane who) => {
                Scane firstscane = who.address.GetScane(1);
                if (firstscane == null || firstscane.GetButton("四处逛逛") == null) { GotoAddress("神秘森林", 3); return; } //避免代码跳剧情卡死
                Scane.Button button = firstscane.GetButton("四处逛逛");
                if (button.usetime > 0)
                {
                    GotoAddress("神秘森林", 1); //回到第一个场景，还没逛完呢
                }
                else
                {
                    MainForm.Print("~是时候准备出发去远方了~\r\n"+endline,()=> {
                        g_address.GotoAddress("神秘森林", 3);  //给我塞里面去！！
                    });
                }
            });

            scane = new Scane("",false,false,false);
            address.AddScane(scane);
            scane.AddButton("先西走", (Scane who) => {
                int usedtime = who.GetButton("先西走").UsedTime;
                if (usedtime == 1) //从1开始
                {;
                    Queue<string> str = who.GetButtonText(3, "先西走", 1);
                    MainForm.Print(str, () => {
                        g_address.GotoAddress("破损之桥");
                    });
                }
                else
                {
                    g_address.GotoAddress("破损之桥");
                }
            },ENDLESS,true,true, "blowdart_lava",true);
            scane.AddButton("先北走", (Scane who) => {
                int usedtime = who.GetButton("先北走").UsedTime;
                if (usedtime == 1) //从1开始
                {
                    Queue<string> str = who.GetButtonText(3, "先北走", 1);
                    MainForm.Print(str, () => {
                        GotoAddress("山峦聚");
                    });
                }
                else
                {
                    GotoAddress("山峦聚");
                }
            }, ENDLESS,true,true, "mole", true);
            scane.AddButton("先东走", (Scane who) => {
                int usedtime = who.GetButton("先东走").UsedTime;
                if (usedtime == 1) //从1开始
                {
                    Queue<string> str = who.GetButtonText(3, "先东走", 1);
                    MainForm.Print(str, () => {
                        GotoAddress("绿野仙踪");
                    });
                }
                else
                {
                    GotoAddress("绿野仙踪");
                }
            }, ENDLESS,true,true,"chester_eyebone_snow", true);
            scane.AddButton("进入小木屋", () => {
                GotoAddress("夏湾的小木屋");
            }, ENDLESS,false,false, "homesign", true);

            //*----------------破损之桥 3
            address = new Address("破损之桥");
            g_address.addresses.Add(address);
            scane = new Scane();
            address.AddScane(scane);
            scane.AddButton("另寻他路", (Scane who) => {
                int usetime = who.GetButton("修桥").UsedTime;
                if (usetime == 0)
                {
                    Queue<string> str = who.GetButtonText(1, "另寻他路", 1);
                    MainForm.Print(str,()=> {
                        g_address.GotoAddress("破损之桥", 2);//迷路
                    });
                }
                else
                {
                    Queue<string> str = who.GetButtonText(1, "另寻他路", 2);
                    MainForm.Print(str, () => {
                        g_address.GotoAddress("破损之桥", 2);//迷路
                    });
                }
            }, 1, true, true, "canary", true);
            scane.AddButton("修桥", (Scane who) => {
                Dictionary<string, uint> recipe = new Dictionary<string, uint>();
                recipe.Add("木材", 12); recipe.Add("干草", 6); recipe.Add("燧石", 2);
                if (Form1.traveler.backpack.IsMatrialsEnough(recipe))
                {
                    GetAddress("神秘森林").GetScane(3).buttons[0].fn = (object sender, EventArgs e) => {
                        GotoAddress("鹤之桥");
                    };          
                    Form1.traveler.backpack.Remove(recipe);
                    Queue<string> str = who.GetButtonText(1, "修桥", 1);
                    MainForm.Print(str, ()=> {
                        g_address.GotoAddress("鹤之桥");  //想了想还是不改名字了，直接建个新地点吧
                    });                 
                }
                else
                {
                    MainForm.Print("虽然旅行者很想修复这桥，不过奈何手头没有足够的材料\r\n" +
                        "也许我们应该去弄点木材、干草和燧石过来\r\n"+endline);
                }
            }, ENDLESS, false, false, "boards", true); //改变了先西走
            scane.AddButton("使用", g_basebuttonfn.UseItem, ENDLESS, "torch_shadow_alt");
            scane.AddButton("回到小木屋", () => {
                GotoAddress("神秘森林");
            }, ENDLESS, true, true, "homesign", true);

            //------------------判断，是否修桥了
            scane = new Scane("");
            address.AddScane(scane);
            scane.SetEnterfn((Scane who) => {
                Scane firstscane = who.address.GetScane(1);
                if (firstscane == null || firstscane.GetButton("另寻他路") == null) { GotoAddress("破损之桥", 3); return; } //避免代码跳剧情卡死
                Scane.Button button = firstscane.GetButton("另寻他路");
                if (button.UsedTime == 0)
                {
                    GotoAddress("破损之桥", 1); //回到第一个场景，还没修路呢
                }
                else
                {
                    GotoAddress("破损之桥", 3); //迷路了
                }
            });

            scane = new Scane(); //迷路 2
            address.AddScane(scane);
            scane.AddButton("继续前进", (Scane who) => {
                Queue<string> str = who.GetButtonText(2, "继续前进", 1);
                MainForm.Print(str,()=> {
                    g_address.GotoAddress("荒漠之丘"); //新地形走起
                });
            },1, true,true,"cane", true);
            scane.AddButton("休息", g_basebuttonfn.Sleep, 2, "canary_poisoned");
            scane.AddButton("使用", g_basebuttonfn.UseItem, ENDLESS, "bonestew");

            //*------------------ 鹤之桥 4
            address = new Address("鹤之桥");
            g_address.addresses.Add(address);
            scane = new Scane();
            address.AddScane(scane);

            scane.AddButton("过桥",()=> {
                GotoAddress("桦树林");
            },ENDLESS,false,true, "deciduous");
            scane.AddButton("回到小木屋", () => {
                GotoAddress("神秘森林");
            }, ENDLESS, true, true, "homesign", true);

            //------------------ 桦树林
            address = new Address("桦树林");
            g_address.addresses.Add(address);
            scane = new Scane();
            address.AddScane(scane);
            scane.SetEnterfn(()=> {
                Form1.traveler.AddTag("canuseaxe");
            });
            scane.SetLeavefn(()=> {
                Form1.traveler.RemoveTag("canuseaxe");
            });

            scane.AddButton("四处逛逛", (Scane who) => {
                int usetime = who.GetButton("四处逛逛").usetime;
                switch (usetime)
                {
                    case 1:
                        {

                        }
                        break;
                    case 2:
                        {

                        }
                        break;
                    case 3:
                        {

                        }
                        break;
                    case 4:
                        {

                        }
                        break;
                    default:
                        break;
                }
            }, 4,true,true, "cartographydesk", true);
            scane.AddButton("使用", g_basebuttonfn.UseItem, ENDLESS,"axe");
            scane.AddButton("回到小木屋", () => {
                GotoAddress("神秘森林");
            }, ENDLESS, true, true, "homesign", true);
            //*----------------山峦聚 
            address = new Address("山峦聚");
            g_address.addresses.Add(address);
            scane = new Scane();
            address.AddScane(scane);
            scane.SetEnterfn(() => {
                Form1.traveler.AddTag("canusepickaxe");
            });
            scane.SetLeavefn(() => {
                Form1.traveler.RemoveTag("canusepickaxe");
            });

            scane.AddButton("使用", g_basebuttonfn.UseItem, ENDLESS, "pickaxe");
            scane.AddButton("回到小木屋", () => {
                GotoAddress("神秘森林");
            }, ENDLESS, true, true, "homesign", true);
            //*----------------绿野仙踪 
            address = new Address("绿野仙踪");
            g_address.addresses.Add(address);
            scane = new Scane();
            address.AddScane(scane);
            scane.SetEnterfn(()=> {
                Form1.traveler.AddTag("canuseaxe");
            });
            scane.SetLeavefn(()=> {
                Form1.traveler.RemoveTag("canuseaxe");
            });

            scane.AddButton("收集资源", () => {
                string[] items = new string[] { "干草", "浆果", "胡萝卜" };
                int[] maxnums = new int[] { 12,5,5}; 
                Queue<string> result  = GetRadomItem(items,maxnums);
                MainForm.Print(result,MainForm.ReflashBackpackList);
            }, 4, true, true, "cutgrass", true);
            scane.AddButton("使用", g_basebuttonfn.UseItem, ENDLESS, "axe");
            scane.AddButton("回到小木屋", () => {
                GotoAddress("神秘森林");
            }, ENDLESS, true, true, "homesign", true);

            //*********************** 荒漠之丘 ***********************
            address = new Address("荒漠之丘");
            g_address.addresses.Add(address);
            scane = new Scane();
            address.AddScane(scane);

            scane.AddButton("冲上去", () => {
                string str = "义无反顾地冲了上去，";
                if ( Form1.traveler.weapon!=null && Form1.traveler.weapon.name == "猎枪")
                {
                    str += "拿出手里的猎枪，给那怪物来了致命一击\r\n并大喊到：“大人，时代变了！！”\r\n"+endline+"还没写完";
                    MainForm.Print(str, () => {
                        g_address.GotoAddress("0");
                    });
                }
                else
                {
                    str += "然后被无情的杀害了";
                    MainForm.Print(str, () => {
                        g_baseresult.GotoResult(Form1.traveler.name + "死于愚蠢");
                    });                  
                }
               
            },1,false,true,"fight_lose");
            scane.AddButton("躲好",()=> {
                string str = "躲到了旁边的大石头后面";
                MainForm.Print(str, () => {
                    g_address.GotoAddress("荒漠之丘");
                });
            },ENDLESS, false, false, "chester_eyebone_closed");

            scane = new Scane("");
            address.AddScane(scane);
            scane.AddButton("探头偷看", (Scane who) => {
                int usetime = who.GetButton("继续躲藏").UsedTime;
                if(usetime<=3)
                {
                    MainForm.Print("旅行者试图偷看，结果被怪物发现并吃掉了\r\n", () => {
                        g_baseresult.GotoResult(Form1.traveler.name + "死于神秘怪物");
                    });
                }
                else 
                {
                    GotoAddress("荒漠之丘");
                }
            },1,false,false, "chester_eyebone_snow");
            scane.AddButton("继续躲藏", (Scane who) => {
                MainForm.Print("旅行者继续躲藏~", () => {
                    int usedtime = who.GetButton("继续躲藏").UsedTime;
                    if (usedtime > 2)
                        g_baseresult.GotoResult(Form1.traveler.name + "在等待中老去");
                });

            },ENDLESS,false,false, "chester_eyebone_closed_shadow");

            scane = new Scane("旅行者发现那怪物已经离开了，那么接下来干什么呢？");
            address.AddScane(scane);
            scane.AddButton("过去看看", () => {
                MainForm.Print("等等等等，还没写完呢",()=> {
                    GotoAddress(0);
                });
            },1,true,true, "chester_eyebone_shadow");
            scane.AddButton("再等等",(Scane who)=> {
                MainForm.Print("再等等~",()=>{
                    int usedtime = who.GetButton("再等等").UsedTime;
                    if (usedtime > 2)
                        g_baseresult.GotoResult(Form1.traveler.name + "在等待中老去");
                });               
            },ENDLESS,false,false, "chester_eyebone_closed_snow");

            #endregion
            /*
            address = new Address("荒漠之丘");
            g_address.addresses.Add(address);
            scane = new Scane();
            address.AddScane(scane);

            scane = new Scane();
            address.AddScane(scane);
            */
        }
        private static readonly string endline = Scane.endline;
        private static List<int> NORESTATR = new List<int>(); //把不重置的加这里面,index
        public static void ReStart()
        {
            //var traveler = Form1.traveler; //这样只是本地赋值
            //traveler = new Traveler(traveler.name, 100, 100, 100);
            Form1.traveler.ReSet();
            MainForm.ReflashBackpackList();
            MainForm.ReflashBasedata();
            MainForm.bakcepackName.Text = "小背包";

            for (int i = 0; i < addresses.Count; ++i)
            {
                if (NORESTATR.Contains(i)) { continue; } //跳过
                Address address = addresses.ElementAt(i);
                address.enterorder = 0;
                for (int j = 0; j < address.scanes.Count; ++j)
                {
                    var buttons = address.scanes.ElementAt(j).buttons;
                    for (int k = 0; k < buttons.Count; ++k)
                        buttons.ElementAt(k).usetime = buttons.ElementAt(k).maxusetime;
                }
            }
        }
        public static void cls()
        {
            MainForm.TB_Dialogue.Text = "";
        }
        public static Queue<string> GetRadomItem(string[] name, int[] maxnum)
        {
            Queue<string> result = new Queue<string>();
            Random random = new Random();
            for (int i=0;i<name.Length;++i)
            {
                int num = random.Next(maxnum[i]-1) + 1;
                if(Form1.traveler.backpack.AddObject(name[i],num))
                {
                    result.Enqueue("获得" + name[i] +" X " + num);
                }
                else
                {
                    result.Enqueue("由于背包已满，未获得" + name[i] + " X " + num);
                }
            }
            result.Enqueue(endline);
            return result;
        }
        public static void GetRadomItem(string[] name, int[] maxnum,Queue<string> output)
        {
            if (output == null) { return; }
            Queue<string> result = GetRadomItem(name, maxnum);
            foreach (var v in result)
                output.Enqueue(v);
        }
    }
}
