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
    public class g_recipes
    {
        public static Form1 MainForm;
        public static void SetMainForm(Form1 form) { MainForm = form; }

        public static void InitRecipes()
        {
            string[,] recipes = new string[,] {
                //工具
                {"斧头","树枝 燧石","1 1" ,"axe"},  //每个用一个空格隔开，多了不行
                {"鹤嘴锄","树枝 燧石","2 2" ,"pickaxe"},
                { "end","end","",""},
                ///照明
                {"火把","干草 树枝","2 2","torch_shadow_alt" },
                {"篝火","干草 木材","3 2" ,"campfire"},
                {"石篝火","木材 石头","2 12","firepit_stonehenge" },
                { "end","","",""},
                //生存
                {"钓鱼竿","树枝 蜘蛛丝","2 2","fishingrod" },
                { "end","","",""},
                //武器 
                {"长矛","树枝 干草 燧石","2 3 1","spear_rose" },
                {"火腿棒","猪皮 树枝 大肉","1 2 2","hambat_spiralcut" },
                { "end","","",""},
                //防具
                {"草甲","干草 树枝","10 2","lavaarena_armorlight" },
                {"木甲","木材 干草","8 6","armorwood" },
                { "橄榄球头盔","猪皮 干草","1 3","lavaarena_lightdamagerhat"},
                { "end","","",""},
                //服装
                { "花环","花瓣","12","flowerhat_healing"},
                {"草帽","干草","12","strawhat_floppy" },
                {"高礼帽","蜘蛛丝","6","tophat_festive_bell_red_firehound" },
                { "end","","",""},
            };
            string[,] backpacks = new string[,] { //这个比较特殊要单独写
                {"背包","12","干草 树枝","4 4","backpack_catcoon" }, //12格
                {"猪背包","16","猪皮 蜘蛛丝 干草","4 4 6","piggyback" }, //16格
            };
            //string[,] foods = new string[,] //不同于烤这个高级
            //{
            //    {""},

            //}
            int Length = recipes.Length / 3;//有3个字符串要除3
            int index = 0;
            char[] separator = new char[]{' '};
            StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries; //移除空的

            for (;index<Length;++index) //工具
            {                
                string name =  recipes[index, 0];
                if (name == "end") { index++ ; break; }
                Tool tool = (Tool)g_prefabs.GetPrefab(name);
                string[] prefabs = recipes[index, 1].Split(separator,options);
                string[] nums = recipes[index, 2].Split(separator, options);
                string skin = recipes[index, 3];
                Dictionary<string, uint> recipe = new Dictionary<string, uint>();
                for (int i = 0; i < prefabs.Length; ++i)
                    recipe.Add(prefabs[i], UInt32.Parse(nums[i]));
                AddTool(tool, recipe,skin);
            }
            for (; index < Length; ++index) //照明
            {
                string name = recipes[index, 0];
                if (name == "end") { index++; break; }
                Prefab light =  g_prefabs.GetPrefab(name);
                string[] prefabs = recipes[index, 1].Split(separator, options);
                string[] nums = recipes[index, 2].Split(separator, options);
                string skin = recipes[index, 3];
                Dictionary<string, uint> recipe = new Dictionary<string, uint>();
                for (int i = 0; i < prefabs.Length; ++i)
                    recipe.Add(prefabs[i], UInt32.Parse(nums[i]));
                AddLight(light, recipe,skin);
            }
            for (; index < Length; ++index) //生存
            {
                string name = recipes[index, 0];
                if (name == "end") { index++; break; }
                Prefab live = g_prefabs.GetPrefab(name);
                string[] prefabs = recipes[index, 1].Split(separator, options);
                string[] nums = recipes[index, 2].Split(separator, options);
                string skin = recipes[index, 3];
                Dictionary<string, uint> recipe = new Dictionary<string, uint>();
                for (int i = 0; i < prefabs.Length; ++i)
                    recipe.Add(prefabs[i], UInt32.Parse(nums[i]));
                AddLive(live, recipe,skin);
            }
            for (; index < Length; ++index) //武器
            {
                string name = recipes[index, 0];
                if (name == "end") { index++; break; }
                Weapon weapon = (Weapon) g_prefabs.GetPrefab(name);
                string[] prefabs = recipes[index, 1].Split(separator, options);
                string[] nums = recipes[index, 2].Split(separator, options);
                string skin = recipes[index, 3];
                Dictionary<string, uint> recipe = new Dictionary<string, uint>();
                for (int i = 0; i < prefabs.Length; ++i)
                    recipe.Add(prefabs[i], UInt32.Parse(nums[i]));
                AddFight(weapon, recipe,skin);
            }
            for (; index < Length; ++index) //防具
            {
                string name = recipes[index, 0];
                if (name == "end") { index++; break; }
                Armor armor = (Armor) g_prefabs.GetPrefab(name);
                string[] prefabs = recipes[index, 1].Split(separator, options);
                string[] nums = recipes[index, 2].Split(separator, options);
                string skin = recipes[index, 3];
                Dictionary<string, uint> recipe = new Dictionary<string, uint>();
                for (int i = 0; i < prefabs.Length; ++i)
                    recipe.Add(prefabs[i], UInt32.Parse(nums[i]));
                AddFight(armor, recipe,skin);
            }
            for (; index < Length; ++index) //服装，没时间写了，所以就简单写了个帽子
            {
                string name = recipes[index, 0];
                if (name == "end") { index++; break; }
                Hat hat = (Hat) g_prefabs.GetPrefab(name);
                string[] prefabs = recipes[index, 1].Split(separator, options);
                string[] nums = recipes[index, 2].Split(separator, options);
                string skin = recipes[index, 3];
                Dictionary<string, uint> recipe = new Dictionary<string, uint>();
                for (int i = 0; i < prefabs.Length; ++i)
                    recipe.Add(prefabs[i], UInt32.Parse(nums[i]));
                AddClothes(hat, recipe,skin);
            }

            for(int i=0;i<backpacks.Length/4;++i) //不要复制粘贴！！！
            {
                string name = backpacks[i, 0];
                uint capacity = UInt32.Parse(backpacks[i, 1]);
                Dictionary<string, uint> recipe = new Dictionary<string, uint>();
                string[] prefabs = backpacks[i, 2].Split(separator, options);
                string[] nums = backpacks[i, 3].Split(separator, options);
                string skin = backpacks[i, 4];
                for (int j = 0; j < prefabs.Length; ++j)
                    recipe.Add(prefabs[j], UInt32.Parse(nums[j]));
                AddBackPack(name,recipe,capacity, skin);
            }
        }
    
        public static bool AddTool(Tool tool,Dictionary<string,uint> recipe,string prefabname=null,uint getnum = 1) //因为我名字用了中文，所以这里就加个prefabname吧
        {
            if (tools.Contains(tool.name)) { return false; }
            tools.Add(tool.name);
            ToolStripMenuItem TOOL = (ToolStripMenuItem) MainForm.menuStrip.Items["工具"];
            var item = TOOL.DropDownItems.Add(tool.name);
            item.ToolTipText = GetToolTipText(recipe);
            item.Click += (object sender, EventArgs e) => {
                MakePrefab(tool, recipe, getnum);
            };
            if (prefabname != null) { item.Image = MainForm.GetPrefabImage(prefabname); }
            if (!g_prefabs.IsExistPrefab(tool.name)) { g_prefabs.AddPrefab(tool); }
            return true;
        }
        public static bool AddLight(Prefab light, Dictionary<string, uint> recipe, string prefabname = null,uint getnum=1) //因为我名字用了中文，所以这里就加个prefabname吧
        {
            if (lights.Contains(light.name)) { return false; }
            lights.Add(light.name);
            ToolStripMenuItem LIGHT = (ToolStripMenuItem) MainForm.menuStrip.Items["照明"];
            var item = LIGHT.DropDownItems.Add(light.name);
            item.ToolTipText = GetToolTipText(recipe);
            item.Click += (object sender, EventArgs e) => {
                MakePrefab(light, recipe, getnum);
            };
            if (prefabname != null) { item.Image = MainForm.GetPrefabImage(prefabname); }
            if (!g_prefabs.IsExistPrefab(light.name)) { g_prefabs.AddPrefab(light); }
            return true;
        }
        public static bool AddFood(Food food, Dictionary<string, uint> recipe, string prefabname = null, uint getnum = 1) //因为我名字用了中文，所以这里就加个prefabname吧
        {
            if (foods.Contains(food.name)) { return false; }
            var newitem = new ToolStripMenuItem(food.name); //名字唯一
            var cookall = new ToolStripMenuItem("制作全部 " +"烤" +food.name);
            var cookone = new ToolStripMenuItem("制作一次 " +"烤" +food.name);
            cookall.Click += cookallfn;
            cookone.Click += cookonefn;
            newitem.DropDownItems.AddRange(new ToolStripItem[] { cookall, cookone });
            MainForm.烹饪.DropDownItems.Add(newitem);
            return true;
        }
        public static bool AddLive(Prefab live, Dictionary<string, uint> recipe, string prefabname = null, uint getnum = 1) //因为我名字用了中文，所以这里就加个prefabname吧
        {
            if (lives.Contains(live.name)) { return false; }
            lives.Add(live.name);
            ToolStripMenuItem LIVE = (ToolStripMenuItem) MainForm.menuStrip.Items["生存"];
            var item = LIVE.DropDownItems.Add(live.name);
            item.ToolTipText = GetToolTipText(recipe);
            item.Click += (object sender, EventArgs e) => {
                ToolStripMenuItem x = (ToolStripMenuItem) sender;
                ;
                MakePrefab(live, recipe, getnum);
            };
            if (prefabname != null) { item.Image = MainForm.GetPrefabImage(prefabname); }
            if (!g_prefabs.IsExistPrefab(live.name)) { g_prefabs.AddPrefab(live); }
            return true;
        }
        public static bool AddFight(Weapon weapon, Dictionary<string, uint> recipe, string prefabname = null, uint getnum = 1) //因为我名字用了中文，所以这里就加个prefabname吧
        {
            if (fights.Contains(weapon.name)) { return false; }
            fights.Add(weapon.name);
            ToolStripMenuItem FIGHT = (ToolStripMenuItem) MainForm.menuStrip.Items["战斗"];
            var item = FIGHT.DropDownItems.Add(weapon.name);
            item.ToolTipText = GetToolTipText(recipe);
            item.Click += (object sender, EventArgs e) => {
                ToolStripMenuItem x = (ToolStripMenuItem) sender;
                ;
                MakePrefab(weapon, recipe, getnum);
            };
            if (prefabname != null) { item.Image = MainForm.GetPrefabImage(prefabname); }
            if (!g_prefabs.IsExistPrefab(weapon.name)) { g_prefabs.AddPrefab(weapon); }
            return true;
        }
        public static bool AddFight(Armor armor, Dictionary<string, uint> recipe, string prefabname = null, uint getnum = 1) //因为我名字用了中文，所以这里就加个prefabname吧
        {
            if (fights.Contains(armor.name)) { return false; }
            fights.Add(armor.name);
            ToolStripMenuItem ARMOR = (ToolStripMenuItem) MainForm.menuStrip.Items["战斗"];
            var item = ARMOR.DropDownItems.Add(armor.name);
            item.ToolTipText = GetToolTipText(recipe);
            item.Click += (object sender, EventArgs e) => {
                ToolStripMenuItem x = (ToolStripMenuItem) sender;
                ;
                MakePrefab(armor, recipe, getnum);
            };
            if (prefabname != null) { item.Image = MainForm.GetPrefabImage(prefabname); }
            if (!g_prefabs.IsExistPrefab(armor.name)) { g_prefabs.AddPrefab(armor); }
            return true;
        }
        public static bool AddClothes(Prefab clothes, Dictionary<string, uint> recipe, string prefabname = null, uint getnum = 1) //因为我名字用了中文，所以这里就加个prefabname吧
        {
            if (g_recipes.clothes.Contains(clothes.name)) { return false; }
            g_recipes.clothes.Add(clothes.name);
            ToolStripMenuItem CLOTHES = (ToolStripMenuItem) MainForm.menuStrip.Items["服装"];
            var item = CLOTHES.DropDownItems.Add(clothes.name);
            item.ToolTipText = GetToolTipText(recipe);
            item.Click += (object sender, EventArgs e) => {
                ToolStripMenuItem x = (ToolStripMenuItem) sender;
                ;
                MakePrefab(clothes, recipe, getnum);
            };
            if (prefabname != null) { item.Image = MainForm.GetPrefabImage(prefabname); }
            if (!g_prefabs.IsExistPrefab(clothes.name)) { g_prefabs.AddPrefab(clothes); }
            return true;
        }
        //*********************************************************************************背包类
        public static void AddBackPack(string name, Dictionary<string, uint> recipe, uint capacity, string prefabname = null)
        {
            ToolStripMenuItem LIVE = (ToolStripMenuItem) MainForm.menuStrip.Items["生存"];
            var traveler = Form1.traveler;
            var item = LIVE.DropDownItems.Add(name);
            item.ToolTipText = GetToolTipText(recipe);
            item.Click += (object sender, EventArgs e) => {
                if (!traveler.backpack.IsMatrialsEnough(recipe))
                {
                    MessageBox.Show("材料不够", "", MessageBoxButtons.OK);
                }
                else
                {
                    traveler.backpack.Remove(recipe);
                    traveler.backpack.Resize((int) capacity);
                    MainForm.bakcepackName.Text = name;
                    MainForm.ReflashBackpackList();
                }
            };
            if (prefabname != null) { item.Image = MainForm.GetPrefabImage(prefabname); }
        }
        //*********************************************************************************
        private static void MakePrefab(Prefab prefab, Dictionary<string, uint> recipe, uint num = 1)
        {
            var traveler = Form1.traveler;
            string name = prefab.name;
            if (!traveler.backpack.IsMatrialsEnough(recipe))
            {
                MessageBox.Show("材料不够", "", MessageBoxButtons.OK);
            }
            else if (traveler.backpack.IsExistObject(name) || traveler.backpack.IsCanMake(recipe))
            {
                traveler.backpack.Remove(recipe);
                traveler.backpack.AddObject(name, num);
                MainForm.ReflashBackpackList();
            }
            else  
            {
                MessageBox.Show("背包已满，请先清理背包", "", MessageBoxButtons.OK);
            }
        }
        private static string GetToolTipText(Dictionary<string,uint> recipe)
        {
            if (recipe.Count <= 0) { return ""; }
            string text = "材料：";
            for(int i=0;i<recipe.Count-1;++i)
            {
                var v = recipe.ElementAt(i);
                text += v.Key + " " + v.Value + "，";
            }
            text += recipe.Last().Key + " " + recipe.Last().Value;
            return text;
        }
        private static void cookonefn(object sender, EventArgs e)
        {
            string name = ( (ToolStripMenuItem) sender ).Text.Split()[1];
            Food food = (Food) ( g_prefabs.GetPrefab(name) );
            if (food.recipe.Count > 0)
                MakePrefab(food, food.recipe);
        }
        private static void cookallfn(object sender, EventArgs e)
        {
            var traveler = Form1.traveler;
            string name = ( (ToolStripMenuItem) sender ).Text.Split()[1];
            Food food = (Food) ( g_prefabs.GetPrefab(name) );
            uint num = traveler.backpack.GetMaxMakeNum(food.recipe);
            if (num > 0)
            {
                traveler.backpack.Remove(food.recipe, num);
                traveler.backpack.AddObject(food.name, num);
                MainForm.ReflashBackpackList();
            }
            else
            {
                MessageBox.Show("材料不够", "", MessageBoxButtons.OK);
            }
        }
        public void AddCookButton(Food food)
        {
            var newitem = new ToolStripMenuItem(food.name); //名字唯一
            var cookall = new ToolStripMenuItem("制作全部 " + food.name);
            var cookone = new ToolStripMenuItem("制作一次 " + food.name);
            cookall.Click += cookallfn;
            cookone.Click += cookonefn;
            newitem.DropDownItems.AddRange(new ToolStripItem[] { cookall, cookone });
            MainForm.烹饪.DropDownItems.Add(newitem);
        }

        private static List<string> tools = new List<string>();
        private static List<string> lights = new List<string>();
        private static List<string> foods = new List<string>();
        private static List<string> lives = new List<string>();
        private static List<string> fights = new List<string>();
        private static List<string> clothes = new List<string>();

    }
}
