using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    public class Backpack
    {
        public class Pair<FirstType,SecondType>
        {
            public Pair(FirstType first, SecondType second)
            {
                this.first = first;
                this.second = second;
            }
            public FirstType first;
            public SecondType second;
        }
        public Backpack(uint capacity = 0)
        {
            this.capacity = capacity;
            this.objects = new Dictionary<string, Pair<uint, Prefab>>();
        }
        ~Backpack()
        {
            this.objects.Clear();
            this.capacity = 0;
        }

        public bool IsFull()
        {
            return this.objects.Count >= this.capacity;
        }
        public bool IsMatrialsEnough(Dictionary<string,uint> recipe)
        {
            foreach(var item in recipe)
            {
                if (!( this.objects.ContainsKey(item.Key) && this.objects[item.Key].first >= item.Value ))
                    return false;
            }
            return true;
        }
        private bool IsCanRemoveSomeMatrials(Dictionary<string,uint> recipe)
        {
            //make sure matrials is enough before use this function
            foreach(var item in recipe)
            {
                if (this.objects[item.Key].first == item.Value)
                    return true;
            }
            return false;
        }
        public bool IsCanMake(Dictionary<string,uint> recipe)
        {
            if (!IsMatrialsEnough(recipe)) { return false; }
            return IsFull() ? IsCanRemoveSomeMatrials(recipe) : true;
        }
        public uint GetMaxMakeNum(Dictionary<string, uint> recipe)
        {
            if (recipe.Count <= 0 || !IsMatrialsEnough(recipe)) { return 0; }
            uint maxnum = GetObjectNum(recipe.ElementAt(0).Key)/recipe.ElementAt(0).Value;
            foreach(var v in recipe)
            {
                uint num = GetObjectNum(v.Key) / v.Value;
                if (num < maxnum)  
                    maxnum = num;
            }
            return maxnum;
        }
        public bool AddObject(string name,Prefab prefab,uint num = 1)
        {
            if (num == 0) { return false; }
            if (this.objects.ContainsKey(name))  //背包中有同样的物品
            {
                this.objects[name].first += num;
                return true;
            }
            else if (!IsFull())
            {
                this.objects.Add(name,new Pair<uint,Prefab>(num,prefab));
                if (!g_prefabs.IsExistPrefab(name))
                    g_prefabs.AddPrefab(prefab);
                return true;
            }
            return false;
        }
        public bool AddObject(string name,uint num=1)
        {
            if (num == 0) { return false; }
            if (this.objects.ContainsKey(name))  //背包中有同样的物品
            {
                 this.objects[name].first += num;
                return true;
            }
            else if (!IsFull())
            {
                var prefab = g_prefabs.GetPrefab(name);
                if(prefab!=null)
                {
                    Pair<uint, Prefab> obj = new Pair<uint, Prefab>(num, prefab);
                    this.objects.Add(name, obj);
                    return true;
                }
            }
            return false;
        }
        public bool AddObject(string name,int num=1)
        {
            if (num <= 0) { return false ; }
            return AddObject(name, (uint) num);
        }
        public void Remove(string name ,uint num=0)
        {  //默认全部删除
            if(num!=0)
            {
                if (this.objects.ContainsKey(name))
                {
                    if (this.objects[name].first > num)
                        this.objects[name].first -= num;
                    else
                        this.objects.Remove(name);
                }
            }
            else
            {
                this.objects.Remove(name);
            }
        }
        public void Remove(Dictionary<string,uint> recipe,uint num=1)
        {
            foreach(var item in recipe)
            {
                Remove(item.Key, item.Value*num);
            }
        }
        public void Clear()
        {
            this.objects.Clear();
        }
        public string[] GetOutputString()
        {
            string[] strs = new string[this.objects.Count];
            int index = 0;
            foreach (var v in this.objects)
            {
                if (v.Value.second.HasTag("stackable"))  //tool weapon armor
                    strs[index++] = v.Key + " X " + v.Value.first;             
                else
                {
                    double durability = v.Value.first - 1 + v.Value.second.GetDurability();
                    string str = Convert.ToString((int) ( durability * 10 ) / 10.0); //保留一位小数
                    strs[index++] = v.Key + " X " + str;
                 }
            }
            return strs;
        }
        public void Resize(int size)
        {
            this.capacity = (uint)(size<0 ? -size : size);  
        }
        public bool IsExistObject(string name)
        {
            return objects.ContainsKey(name);
        }
        public Prefab GetObject(string name)
        {
            if (IsExistObject(name))
                return objects[name].second;
            else
                return null;
        }
        public uint GetObjectNum(string name)
        {
            if (IsExistObject(name))
                return objects[name].first;
            else
                return 0;
        }

        public int Capacity { get { return (int) this.capacity; } }
        private Dictionary<string, Pair<uint,Prefab> >objects;
        private uint capacity;
        //private g_prefabs prefabs = new g_prefabs();
    }
}
