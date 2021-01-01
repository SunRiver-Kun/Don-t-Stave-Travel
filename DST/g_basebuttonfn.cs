using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace DST
{
    public class g_basebuttonfn
    {
        public static Form1 MainForm;
        public static void SetMainForm(Form1 form) { MainForm = form; }
        public static void UseItem(object sender, EventArgs e) //自己使用,只使用第一个
        {
            var traveler = Form1.traveler;
            var items = MainForm.backpackList.CheckedItems;
            if (items.Count <= 0) { return; }
            string[] str = Convert.ToString(items[0]).Split();
            string name = str[0];
            Prefab prefab = traveler.backpack.GetObject(name);
            prefab.Use();
            prefab.Use(MainForm.GetNowAddress());
            MainForm.ReflashBackpackList();
            MainForm.ReflashBasedata();
        }
        public static void Fight(object sender,EventArgs e)
        {
            var traveler = Form1.traveler;
            var meet = Form1.meatcreature;
            if(meet.name!="" && meet.HasTag("fightable"))
            {
                Queue<string> process = Creature.Fight(traveler, meet);
                MainForm.Print(process,()=> {
                    if(traveler.IsDead())
                    {
                        g_baseresult.GotoResult(traveler.name + "死于" + meet.name);
                    }
                });
            }
        }
        public static void Sleep(object sender, EventArgs e)
        {
            ( (Button) sender ).Enabled = false;
            MainForm.Print("休息中……");
            Worldtime.GotoNextState(0.3);
            MainForm.ReflashBasedata();
            ( (Button) sender ).Enabled = true;
        }
    }
}
