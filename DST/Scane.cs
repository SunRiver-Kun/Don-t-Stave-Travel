using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DST
{
    public class Scane
    {
        //System.EventHandler;
        public static Form1 MainForm;
        public static void SetMainForm(Form1 form) { MainForm = form; }
        public static readonly string basetextpath =  @"texts";  //等价为 Application.StartupPath+'\\'+ basetextpath
        public static readonly string baseimagepath =  @"images";
        public static readonly string endline = "----------------------------------------------";
        public delegate void ElFN(Scane who);
        //----------------------------------------------------------------------------
        public class Button
        {
            public Button(string name, Form1.Fn fn, int usetime = 1, bool CanLostInDark = true, bool CanLostCook = true,string prefabname=null,bool GotoNextState = false,double rate=0.5)
            {
                this.name = name;
                this.maxusetime = this.usetime = usetime;
                this.prefabname = prefabname;
                this.fn = (object sender, EventArgs e) => {
                    if (CanLostInDark && Worldtime.GetState() == "晚上" && !Form1.traveler.HasTag("light"))
                    {
                        g_baseresult.LostInDark();
                    }
                    else
                    {
                        if (this.usetime > 0) //注意不能直接用 usetime--  !!!，因为它是指传入的参数
                        {                            
                            --this.usetime;
                            if (fn != null) { fn(); }
                            if (this.usetime <= 0)
                            {
                                if (linkbutton != null)
                                    linkbutton.Enabled = false;
                            }
                        }
                        if (CanLostCook)
                        {
                            MainForm.DeCookMenu();
                        }
                    }
                    if (GotoNextState)
                        Worldtime.GotoNextState(rate);
                };
            }
            public Button(string name, EventHandler fn, int usetime = 1, string prefabname = null, bool GotoNextState = false,double rate=0.5)
            {
                this.name = name;
                this.maxusetime = this.usetime = usetime;
                this.prefabname = prefabname;
                this.fn = (object sender,EventArgs e)=> {
                    if (GotoNextState)
                        Worldtime.GotoNextState(rate);
                    fn(sender, e);
                };
            }
            public Button(string name,ElFN fn,int usetime = 1, bool CanLostInDark = true, bool CanLostCook = true, string prefabname = null, bool GotoNextState = false,double rate = 0.5)
            {
                this.name = name;
                this.maxusetime = this.usetime = usetime;
                this.prefabname = prefabname;
                this.fn = (object sender, EventArgs e) => {
                    if (CanLostInDark && Worldtime.GetState() == "晚上" && !Form1.traveler.HasTag("light"))
                    {
                        g_baseresult.LostInDark();
                    }
                    else
                    {
                        if (this.usetime > 0) //注意不能直接用 usetime--  !!!，因为它是指传入的参数
                        {                           
                            --this.usetime;
                            if (fn != null) { fn(this.scane); }
                            if (this.usetime <= 0)
                            {
                                if (linkbutton != null)
                                    linkbutton.Enabled = false;
                            }
                        }
                        if (CanLostCook)
                        {
                            MainForm.DeCookMenu();
                        }
                    }
                    if (GotoNextState)
                        Worldtime.GotoNextState(rate);
                };
            }
            public void SetLinkButton(System.Windows.Forms.Button linkbutton) //在Form1里面用来绑定
            {
                if (linkbutton == null) { return; }
                this.linkbutton = linkbutton;
                this.linkbutton.Click += this.fn;
                this.linkbutton.Enabled = this.usetime > 0;
                this.linkbutton.Text = this.name;
                if (prefabname != null) { this.linkbutton.Image = MainForm.GetPrefabImage(prefabname); }
            }
            public void Remove()
            {
                if (this.linkbutton == null) { return; }
                this.linkbutton.Click -= this.fn;
                if (this.linkbutton.Image != null) { this.linkbutton.Image = null; }
                this.linkbutton = null;

            }
            public string name;
            public int UsedTime
            {
                get
                {
                    return maxusetime - usetime;
                }
            }
            public EventHandler fn;
            public int usetime;  //小于等于0就禁用
            public readonly int maxusetime; //这个是以后初始化用
            public System.Windows.Forms.Button linkbutton = null;
            public Scane scane = null;
            public string prefabname; //只是个简单的图片位置
        }
        //-----------------------------------------------------------------------
        //public Scane(string[] entertext, bool Copylastbotton = false, bool IsReference = true, bool IsCopyFnEL = false,bool IsCopyPicture=false)
        //{
        //    this.Copylastbotton = Copylastbotton;
        //    this.IsReference = IsReference;
        //    this.IsCopyFnEL = IsCopyFnEL;
        //    this.IsCopyPicture = IsCopyPicture;
        //    this.entertext = entertext;           
        //    if (!Copylastbotton)
        //        this.buttons = new List<Button>();
            
        //}
        public Scane(string entertext = null, bool Copylastbotton = false, bool IsReference = true, bool IsCopyFnEL = false,bool IsCopyPicture= false)
        {
            this.Copylastbotton = Copylastbotton;
            this.IsReference = IsReference;
            this.IsCopyFnEL = IsCopyFnEL;
            this.IsCopyPicture = IsCopyPicture;
            if (entertext == "") { this.entertext = new string[] { entertext }; }//不给其加endline
            else if (entertext != null)
            { this.entertext = new string[] { entertext + "\r\n" + endline }; }

            if (!Copylastbotton)
                this.buttons = new List<Button>();
        }
        public void SetAddress(Address address)
        {
            this.address = address;
            if (this.entertext == null)
                this.entertext = GetText();

            if (address.scanes.Count <=0) { return; } //第一个建立的就没有复制
            Scane last = address.scanes.Last();
            if (Copylastbotton) //复制按键
            {               
                if (IsReference)
                    this.buttons = last.buttons; //引用，不要添加新按键，占内存小
                else
                {
                    this.buttons = new List<Button>(last.buttons); //扩建，可以添加新按键，占内存大
                }
            }

            if (IsCopyFnEL)
            {
                this.enterfn = last.enterfn;
                this.leavefn = last.leavefn;
            }

            if (IsCopyPicture)
                this.picturepath = last.picturepath;
        }
        public void SetEnterfn(Form1.Fn fn)
        {
            this.enterfn  = (Scane who)=>{ fn(); };
        }
        public void SetEnterfn(ElFN fn)
        {
            this.enterfn = fn;
        }
        public void SetLeavefn(Form1.Fn fn)
        {
            this.leavefn = (Scane who) => { fn(); };
        }
        public void SetLeavefn(ElFN fn)
        {
            this.leavefn = fn;
        }
        public void AddButton(Button button)
        {
            if(this.buttons.Count<4) // 我只预设了4个按钮
            {
                this.buttons.Add(button);
                button.scane = this;
            }
            
        }
        public void AddButton(string name, Form1.Fn fn, int usetime = 1, bool CanLostInDark = true, bool CanLostCook = true,string prefabname=null,bool GotoNextState=false)
        {
            Button button = new Button(name, fn, usetime, CanLostInDark, CanLostCook,prefabname, GotoNextState);
            AddButton(button);
        }
        public void AddButton(string name,EventHandler fn,int usetime=1, string prefabname = null, bool GotoNextState = false)
        {
            Button button = new Button(name, fn, usetime,prefabname, GotoNextState);
            AddButton(button);
        }
        public void AddButton(string name,ElFN fn, int usetime = 1, bool CanLostInDark = true, bool CanLostCook = true, string prefabname = null, bool GotoNextState = false)
        {
            Button button = new Button(name, fn, usetime, CanLostInDark, CanLostCook,prefabname, GotoNextState);
            AddButton(button);
        }
        public void SetEnterText(string[] text)
        {
            if (text != null)
                this.entertext = text;
        }
        public void SetEnterText(string text)
        {
            if(text!=null)
                this.entertext = new string[]{ text};
        }
        public Button GetButton(int index)
        {
            if (index >= buttons.Count)
                return null;
            else
                return buttons.ElementAt(index);
        }
        public Button GetButton(string name)
        {
            foreach(var v in buttons)
            {
                if (v.name == name)
                    return v;
            }
            return null;
        }
        public string[] GetText()
        {
            string path = basetextpath + '\\' + address.name  + (address.scanes.Count+1); // 夏湾的小木屋_1 从1开始
            if (File.Exists(path)) //不加后缀
                return File.ReadAllLines(path);
            else if (File.Exists(path + ".txt")) //加后缀
                return File.ReadAllLines(path + ".txt");
            else
                return null;
        }
        public static string[] GetText(Scane who)
        {
            string path = basetextpath + '\\' + who.address.name + ( who.address.scanes.Count + 1 ); // 夏湾的小木屋_1 从1开始
            if (File.Exists(path)) //不加后缀
                return File.ReadAllLines(path);
            else if (File.Exists(path + ".txt")) //加后缀
                return File.ReadAllLines(path + ".txt");
            else
                return null;
        }
        public Queue<string> GetButtonText(int enterorder,string buttonname,int usedtime) //都是从1开始
        {
            string[] text = null;
            Queue<string> output = new Queue<string>();
            string path = basetextpath + '\\' + address.name + enterorder +'_'+ buttonname+ usedtime; // 夏湾的小木屋_1 从1开始
            if (File.Exists(path)) //不加后缀
                text =  File.ReadAllLines(path);
            else if (File.Exists(path + ".txt")) //加后缀
               text =  File.ReadAllLines(path + ".txt");
            if(text!=null)
            {
                foreach (var v in text)
                    output.Enqueue(v);
            }
            return output;
        }
        public string[] entertext = null;
        public Address address = null;
        public List<Button> buttons;
        public ElFN enterfn = null,leavefn = null;
        public string picturepath = null;
        private bool Copylastbotton , IsReference , IsCopyFnEL,IsCopyPicture;
        
    }
}
