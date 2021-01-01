using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DST
{
    // delegate void ClickFn();
    public partial class Form1 : Form
    {
        public delegate void Fn();
        public static Traveler traveler = new Traveler("旅行者", 100, 100, 100);
        public static Creature meatcreature = new Creature();
        //----------------------------------------------控制剧情发展的参数
        public static Queue<string> TEXT = new Queue<string>(); //为了实现print里套gotoaddress，不得不开放这个     

        private static Fn TEXTFN = null; //一般函数
        private static Scane.ElFN ENTERFN = null; //enterfn 
        private static Scane WHO = null;
        private static string PICTUREPATH = null;
        private static Queue<Address> addresses = new Queue<Address>(); //会自己根据enterorder调用
        //-----------------------------------------------------------
        public Form1()
        {
            InitializeComponent();
        }
        //初始化数据
        private void Form1_Load(object sender, EventArgs e)
        {
            //初始化时间
            Worldtime.InitData();
            //初始化按钮函数，通用类
            g_basebuttonfn.SetMainForm(this);
            //初始化所有预设物
            g_prefabs.SetMainForm(this);
            g_prefabs.InitPrefabs();
            //初始化结局
            g_baseresult.SetMainForm(this);
            //初始化recipe
            g_recipes.SetMainForm(this);
            g_recipes.InitRecipes();
            //初始化场景
            g_address.SetMainForm(this);
            g_address.Initaddress();

            //绑定Timer
            timer_baseupdata.Tick += Form1.traveler.UpdataSanity;
            timer_baseupdata.Tick += Form1.traveler.UpdataHunger;
            timer_baseupdata.Tick += ReflashBasedata;
            //载入主界面
            AddAddress(g_address.GetAddress(0));
            AddAddress(g_address.GetAddress(0));
            GotoNextScane();
            //------------------debug
            Debug();
            //-------------------debug
        }
        //刷新
        public void ReflashBackpackList()
        {

            backpackList.Items.Clear();
            string[] strs = traveler.backpack.GetOutputString();
            foreach (var v in strs)
            {
                if (v != null)
                    backpackList.Items.Add(v, false);
            }
        }
        public void ReflashBasedata()
        {
            TB_health.Text = Convert.ToString((int) Form1.traveler.currenthealth);
            TB_sanity.Text = Convert.ToString((int) Form1.traveler.currentsanity);
            TB_hunger.Text = Convert.ToString((int) Form1.traveler.currenthunger);
        }
        public void ReflashBasedata(object sender, EventArgs e)
        {
            ReflashBasedata();
        }

        public void Print(string str,Fn fn=null,Scane.ElFN efn = null,Scane who=null)
        {
            TB_Dialogue.Visible = true;
            MainPicture.Visible = false;
            TEXT.Enqueue(str);
            if (fn != null)
            {
                TEXTFN = () => {
                    if (TEXT.Count > 0) { return; }
                    else
                    {
                        fn();
                        TEXTFN = null;
                    }
                };
                //TEXTFN = fn;
            }
            if (efn != null && who != null)
            {
                WHO = who;
                ENTERFN = (Scane) => {
                    if (TEXT.Count > 0) { return; }
                    else
                    {
                        efn(WHO);
                        ENTERFN = null;
                        WHO = null;
                    }
                };
            }
            DisableAllButton();
            if (!timer_Dialogue.Enabled)
                timer_Dialogue.Enabled = true;
        }
        public void Print(string[] strs,Fn fn=null,Scane.ElFN efn=null,Scane who=null)
        {
            TB_Dialogue.Visible = true;
            MainPicture.Visible = false;
            if (strs.Length <= 0) { return; }
            Queue<string> que = new Queue<string>();
            for (int i = 0; i < strs.Length; ++i)
                que.Enqueue(strs[i]);
            Print(que, fn,efn,who);
        }
        public void Print(Queue<string> strs,Fn fn=null,Scane.ElFN efn=null,Scane who=null)
        {
            TB_Dialogue.Visible = true;
            MainPicture.Visible = false;
            //载入数据
            while (strs.Count > 0)
            {
                TEXT.Enqueue(strs.Dequeue());
            }
            if (fn != null)
            {
                TEXTFN = () => {
                    if (TEXT.Count > 0) { return; }
                    else
                    {
                        fn();
                        TEXTFN = null;
                    }
                };
                //TEXTFN = fn;
            }
            if (efn != null && who != null)
            {
                WHO = who;
                ENTERFN = (Scane) => {
                    if (TEXT.Count > 0) { return; }
                    else
                    {
                        efn(WHO);
                        ENTERFN = null;
                    }
                };
            }
            DisableAllButton();
            if (!timer_Dialogue.Enabled)
                timer_Dialogue.Enabled = true;
        }
        public void PicturePrint(string path,string str,Fn fn=null,Scane.ElFN efn = null,Scane who=null)
        {
            if (path == null || !LoadMainPictureImage(path)) { return; }

            MainPicture.Tag = true; //判断加载成功

            TB_Dialogue.Visible = false;
            MainPicture.Visible = true;
            TEXT.Enqueue(str);

            if (fn != null)
            {
                TEXTFN = () => {
                    if (TEXT.Count > 0) { return; }
                    else
                    {
                        fn();
                        TEXTFN = null;
                    }
                };
                //TEXTFN = fn;
            }
            if (efn != null && who != null)
            {
                WHO = who;
                ENTERFN = (Scane) => {
                    if (TEXT.Count > 0) { return; }
                    else
                    {
                        efn(WHO);
                        ENTERFN = null;
                        WHO = null;
                    }
                };
            }
            DisableAllButton();
            timer_Dialogue.Tick += PicturePrint; //随便打印图片上的
            if (!timer_Dialogue.Enabled)
                timer_Dialogue.Enabled = true;
        }
        public void PicturePrint(string path,string[] strs, Fn fn = null, Scane.ElFN efn = null, Scane who = null)
        {
            if (path == null || !LoadMainPictureImage(path)) { return; }
            MainPicture.Tag = true; //判断加载成功

            TB_Dialogue.Visible = false;
            MainPicture.Visible = true;
            if (strs.Length <= 0) { return; }
            Queue<string> que = new Queue<string>();
            for (int i = 0; i < strs.Length; ++i)
                que.Enqueue(strs[i]);
            PicturePrint(path,que, fn, efn, who);
        }
        public void PicturePrint(string path,Queue<string> strs, Fn fn = null, Scane.ElFN efn = null, Scane who = null)
        {
            if (path == null || !LoadMainPictureImage(path)) { return; }
            MainPicture.Tag = true; //判断加载成功

            TB_Dialogue.Visible = false;
            MainPicture.Visible = true;
            //载入数据
            while (strs.Count > 0)
            {
                TEXT.Enqueue(strs.Dequeue());
            }
            if (path != null) { PICTUREPATH = path; }
            if (fn != null)
            {
                TEXTFN = () => {
                    if (TEXT.Count > 0) { return; }
                    else
                    {
                        fn();
                        TEXTFN = null;
                    }
                };
                //TEXTFN = fn;
            }
            if (efn != null && who != null)
            {
                WHO = who;
                ENTERFN = (Scane) => {
                    if (TEXT.Count > 0) { return; }
                    else
                    {
                        efn(WHO);
                        ENTERFN = null;
                        WHO = null;
                    }
                };
            }
            DisableAllButton();
            timer_Dialogue.Tick += PicturePrint;
            if (!timer_Dialogue.Enabled)
                timer_Dialogue.Enabled = true;
        }
        //--------------------------------------------------------------------------------------------------------
        public void SleepTime()//timer_Time
        {
            timer_Time.Enabled = false;
            TB_time.Text = "";
        }
        public void SleepAllTime() // 除了timer_Time,其他都可以通过按钮开启
        {
            timer_Dialogue.Enabled = false; //这个输入对话以后会自己启动，用完自己关闭
            timer_baseupdata.Enabled = false;
            timer_Time.Enabled = false;
        }
        public void Awake()//timer_Time
        {
            timer_Time.Enabled = true;
            TB_time.Text = Worldtime.GetTime().ToString()+" "+Worldtime.GetState();
        }
        public void Awake(TimeSpan passtime)
        {
            Worldtime.AddTime(passtime);
            timer_Time.Enabled = true;
        }
        //-----------------------菜单栏
        public void EnCookMenu()
        {
            traveler.AddTag("cookable");
            食物.Enabled = true;
            烹饪.Enabled = true;
        }
        public void DeCookMenu()
        {
            traveler.RemoveTag("cookable");
            烹饪.Enabled = false;
        }
        public void EnCookRecipe()
        {
            traveler.AddTag("cookable");
            食物.Enabled = true;
            菜肴.Enabled = true;
        }
        public void DeCookRecipe()
        {
            traveler.RemoveTag("cookable");
            菜肴.Enabled = false;
        }
        //烤类
        private void cookallfn(object sender, EventArgs e)
        {
            string name = ( (ToolStripMenuItem) sender ).Text.Split()[1];
            Food food = (Food) ( g_prefabs.GetPrefab(name) );
            uint num = traveler.backpack.GetMaxMakeNum(food.recipe);
            if (num > 0)
            {
                traveler.backpack.Remove(food.recipe, num);
                traveler.backpack.AddObject(food.name, num);
                ReflashBackpackList();
            }
            else
            {
                MessageBox.Show("材料不够", "", MessageBoxButtons.OK);
            }
        }
        private void cookonefn(object sender, EventArgs e)
        {
            string name = ( (ToolStripMenuItem) sender ).Text.Split()[1];
            Food food = (Food) ( g_prefabs.GetPrefab(name) );
            if (food.recipe.Count > 0)
                MakeObject(food.name, food.recipe);
        }
        public void AddCookButton(Food food,string prefabname=null)
        {
            var newitem = new ToolStripMenuItem(food.name); //名字唯一
            var cookall = new ToolStripMenuItem("制作全部 " + food.name);
            var cookone = new ToolStripMenuItem("制作一次 " + food.name);
            if (prefabname != null)
            {
                newitem.Image = cookall.Image = cookone.Image = GetPrefabImage(prefabname);     
            }
            cookall.Click += cookallfn;
            cookone.Click += cookonefn;
            newitem.DropDownItems.AddRange(new ToolStripItem[] { cookall, cookone });
            烹饪.DropDownItems.Add(newitem);
        }
        //-------------------场景设置
        public Address GetNowAddress()
        {
            return addresses.First();
        }
        public void AddAddress(Address address)
        {
            if(address!=null)
            addresses.Enqueue(address);
        }
        public  void GotoNextScane() //下一幕
        {
            if (addresses.Count <= 1) { return; } //第一个存放上一个场景   

            Scane last = addresses.Dequeue().GetLastScane();
            if(last!=null)
            {
                for (int i = 0; i < last.buttons.Count; ++i)
                    last.buttons[i].Remove();
                if (last.leavefn != null)
                    last.leavefn(last);
            }

            Address address = addresses.First();
            address.enterorder++; //必须先这步
            address.entertime++; //总的进入次数
            Scane scane = address.GetScane();
            address.SetLastScane(scane);
            if (scane == null) { return; }
            var buttons = buttonGroup.Controls;
            TB_address.Text = scane.address==null?"":scane.address.name; //我们来到了个新场景

            //DisableAllButton();

            for (int i = 0; i < buttons.Count; ++i)
            {
                Button button = (Button) buttons[i];
                if(scane.buttons==null || i>=scane.buttons.Count)
                {
                    button.Enabled = true;
                    button.Visible = false;
                }
                else 
                {
                    scane.buttons[i].SetLinkButton(button);
                    button.Visible = true;
                }
            }

            if (scane.entertext != null)
            {
                Print(scane.entertext,null ,scane.enterfn,scane);
            }
            else if(scane.enterfn!=null && scane.enterfn!=ENTERFN)
            {
                scane.enterfn(scane);
            }

        }
        public void DisableAllButton()
        {
            ////存数据
            //buttonGroup.Tag = buttonGroup.Enabled ;
            //makeButton.Tag = makeButton.Enabled;
            //discardButton.Tag =  discardButton.Enabled ;
            //TB_input.Tag = TB_input.Enabled; 忘了这个我拿来存控制台代码了

            //禁用所有按钮
            buttonGroup.Enabled = false;
            makeButton.Enabled = false;
            discardButton.Enabled = false;
            //禁用控制台
            TB_input.Enabled = false;
            //---------------
            //把菜单栏收起
            groupMake.Visible = false;
            //改变关注点
            basedata.Focus();
        }

        public void EnableAllButton()
        {
            //buttonGroup.Enabled = (bool) buttonGroup.Tag;
            //makeButton.Enabled = (bool) makeButton.Tag;
            //discardButton.Enabled = (bool) discardButton.Tag;
            buttonGroup.Enabled = true;
            makeButton.Enabled = true;
            discardButton.Enabled = true;
            TB_input.Enabled = true;           
        }

        public bool LoadMainPictureImage(string filename) //直接给MainPicture的
        {
            Image image = GetImage(filename);
            if (image == null) { return false; }
            MainPicture.Image = image;
            return true;     
        }
        public Image GetImage(string filename)
        {
            string path = Scane.baseimagepath + '\\' + filename;
            string[] suffixes = new string[] { ".jpeg", ".png", "bmp", ".jpg" };
            foreach (var suffix in suffixes) //不加后缀的相对路径
            {
                if (File.Exists(path)) //必须把这个放前面，虽然比较次数多，不过绝对路径优先
                {
                    return Image.FromFile(path);
                  
                }
                else if (File.Exists(path + suffix))
                {
                    return Image.FromFile(path + suffix);
                }
            }
            return null;
        }
        public Image GetPrefabImage(string prefabname)
        {
            string path = Scane.baseimagepath + @"\prefabs\" + prefabname + ".png";
            if (File.Exists(path))
                return Image.FromFile(path);
            else
                return null;
        }
        public bool LoadPrefabImage(Image image,string prefabname)
        {
           Image imagex = GetPrefabImage(prefabname);
            if (imagex != null)
            {
                image = imagex;
                return true;
            }
            else
                return false;
        }
        //----------------------------------------------------------------------------private
        //正常刷新时间,加秒
        private void ReflashTime(object sender, EventArgs e)
        {// gg yyyy/MM/dd hh:mm:ss dddd
            Worldtime.AddTime(new TimeSpan(0, 0, 15)); //1秒
            var time = Worldtime.GetTime();
            string state = Worldtime.GetState();
            TB_time.Text = time.ToString() + " " + state;
        }

        //显示/隐藏制作界面
        private void makeButton_Click(object sender, EventArgs e)
        {
            groupMake.Visible = !groupMake.Visible;
        }
        //制作物品
        private void MakeObject( string name,Dictionary<string,uint> recipe,uint num = 1)
        {
            var traveler = Form1.traveler;
            if (!traveler.backpack.IsMatrialsEnough(recipe))
            {
                MessageBox.Show("材料不够", "", MessageBoxButtons.OK);
            }
            else if (traveler.backpack.IsExistObject(name) || traveler.backpack.IsCanMake(recipe))
            {
                traveler.backpack.Remove(recipe);
                traveler.backpack.AddObject(name, num);
                ReflashBackpackList();
            }
            else
            {
                MessageBox.Show("背包已满，请先清理背包", "", MessageBoxButtons.OK);
            }
        }
        //丢弃物品
        private void discardButton_Click(object sender, EventArgs e)
        {
           var items =  backpackList.CheckedItems;
            foreach(var v in items)
            {
                String []key = v.ToString().Split();
                if(key[0]!=null)
                {
                    traveler.backpack.Remove(key[0]);
                }
            }
            ReflashBackpackList();
        }
        //找到对应材料然后转为Dictionary
        private Dictionary<string, uint> MakeRecipe(String[,] recipe, int index)
        {
            Dictionary<string, uint> rec = new Dictionary<string, uint>();
            string[] names = recipe[index,1].Split();
            string[] nums = recipe[index,2].Split();
            uint num = new uint();
            for (int i=0;i<names.Length;++i)
            {
                if(!(names[i]=="" || names[i] == null || names[i]==" "))
                {
                    num = 1;
                    if (i < nums.Length && !( nums[i] == "" || names[i] == null || nums[i]==" " ))
                        num = Convert.ToUInt32(nums[i]);
                    rec.Add(names[i], num);
                }
            }
            return rec;
        }
        //************************************************************************************
        private void Print(object sender,EventArgs e) //time_Dialogue开始给TB_Dialogue整文字
        {

            if(TEXT.Count>0)
            {
                TB_Dialogue.Text += (TEXT.Dequeue()+"\r\n");  //出列并输出
                TB_Dialogue.Select(TB_Dialogue.TextLength,0); //光标移到最后
                TB_Dialogue.ScrollToCaret(); //滑动条移动到光标
            }
            else
            {//到头了该停了，函数会跳进程，所以要判断
                EnableAllButton();
                timer_Dialogue.Enabled = false;
                if(TEXTFN!=null)
                {
                    TEXTFN();
                  //  TEXTFN = null;

                }//我也是醉了，载入 g_address.GotoAddress，直接给我无视条件来运行
                if (ENTERFN != null &&( MainPicture.Tag==null || (bool)MainPicture.Tag==false ) ) 
                {
                    ENTERFN(WHO);
                }
            }
           
        }
        private void PicturePrint(object sender,EventArgs e)
        {
            if (MainPicture.Tag == null || (bool) MainPicture.Tag == false) { return; }

            if (TEXT.Count >= 0) //与Print的相差一位
            {
                //画在picture上
                int x = MainPicture.Location.X;
                int y = MainPicture.Location.Y;
                int h = MainPicture.Height;
                int w = MainPicture.Width;
                Graphics g = Graphics.FromImage(MainPicture.Image);
                SolidBrush mybrush;
                mybrush = new SolidBrush(Color.Lime);  //设置默认画刷颜色
                Font myfont;
                myfont = new Font("黑体", 14);         //设置默认字体格式
                g.DrawString(TB_Dialogue.Text, myfont, mybrush, new Rectangle(x, y, h, w));
                MainPicture.Refresh(); //刷新图片
                
                if (TEXT.Count==0) //最后
                {
                    timer_Dialogue.Tick -= PicturePrint;
                    MainPicture.Tag = false;
                    if(ENTERFN!=null)
                    {
                        ENTERFN(WHO);
                    }
                }
            }
        }
        //***********************************************************************************
        //控制台
        private void TB_input_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Up)
            {
                string str = (string)TB_input.Tag;
                TB_input.Text = str;
                
            }
            else if (e.KeyCode == Keys.Enter)
            {//char[] separator, int count, StringSplitOptions options
                char[] separator = { ' ', '(', ')', ',', ';', '"','（', '）', '；','“','”' };
                StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries;
                string[] str = TB_input.Text.Split(separator, options);
                if (str.Length <= 0) { return; }

                if(str.Length>1) //含有参数的
                {
                    if ( str[0] == "c_give" || str[0] == "给我" )
                    {
                        if(str[1]=="普通背包"||str[1]=="背包"||str[1]=="猪背包")
                        {
                            traveler.backpack.Resize(str[1] == "猪背包" ? 16 : 12);
                            bakcepackName.Text = str[1];
                        }

                        if (str.Length == 2) { traveler.backpack.AddObject(str[1],1); }
                        else
                        {
                            uint num;
                            if (uint.TryParse(str[2], out num))
                                traveler.backpack.AddObject(str[1], num);
                        }
                        ReflashBackpackList();
                    }
                    else if ( str[0] == "printf" || str[0] == "print" || str[0] == "打印" ) 
                    {
                        string output = "";
                        for (int i = 1; i < str.Length; ++i)
                            output += " " + str[i];
                        Print(output);
                    }
                    else if(str[0]== "WorldState"||str[0]=="时间")
                    {
                        timer_Time.Enabled = false;
                        Worldtime.SetState(str[1]);
                        timer_Time.Enabled = true;
                    }
                    else if(str[0]=="scane"||str[0]=="场景")
                    {
                        int enterorder;
                        if (str.Length > 2 && int.TryParse(str[2], out enterorder))
                        { if (enterorder <= 0)
                                enterorder = 0;
                            else
                                enterorder = enterorder - 1;
                        }
                        else { enterorder = 0; }
                        Address address = g_address.GetAddress(str[1]);
                        address.enterorder = enterorder;
                        this.AddAddress(address);
                        this.GotoNextScane();
                    }
                    else if(str[0]=="health"||str[0]=="健康")
                    {
                        int num;
                        if (Int32.TryParse(str[1], out num))
                            Form1.traveler.currenthealth = num;
                        ReflashBasedata();
                    }
                    else if (str[0] == "sanity" || str[0] == "精神")
                    {
                        int num;
                        if (Int32.TryParse(str[1], out num))
                            Form1.traveler.currentsanity = num;
                        ReflashBasedata();
                    }
                    else if (str[0] == "hunger" || str[0] == "饥饿")
                    {
                        int num;
                        if (Int32.TryParse(str[1], out num))
                            Form1.traveler.currenthunger = num;
                        ReflashBasedata();
                    }

                }
                else//不含参数的
                {
                    if (str[0] == "cls" || str[0] == "清屏")
                    {
                        TB_Dialogue.Text = "";
                    }
                    else if(str[0]== "NextWorldState" || str[0]=="下一段")
                    {
                        timer_Time.Enabled = false;
                        Worldtime.AddTime(Worldtime.GetTimeNowToNext());
                        timer_Time.Enabled = true;
                    }
                   
                }
                
                TB_input.Tag = TB_input.Text;
                TB_input.Text = "";
                TB_input.Focus();
            }
        }

        private void TB_creature_KeyDown(object sender, KeyEventArgs e) //输入名字
        {
            if (e.KeyCode == Keys.Enter)
            {
                EnableAllButton();
                traveler.name = TB_creature.Text;
                Print(traveler.name + "加入游戏!!");
                TB_creature.ReadOnly = true;
                timer_Time.Enabled = true;
                timer_baseupdata.Enabled = true;
            }
        }

        private void Debug()
        {
            //Image x = GetPrefabImage("axe");
           
        }
    }
}
