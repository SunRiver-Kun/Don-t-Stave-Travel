using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    public class Address
    {
        public Address(string name,Scane firstscane =null)
        {
            this.name = name;
            this.enterorder = 0; //第i次进入，可以随便改变
            this.entertime = 0; //总的进入次数，如果不跳剧情，不应该随便改变
            this.scanes = new List<Scane>();
            if(firstscane!=null)
            this.scanes.Add(firstscane);
        }
        public void AddScane(Scane scane)
        {
            scane.SetAddress(this); //这里用到了 scanes.Last，顺序不能变
            this.scanes.Add(scane);           
        }
        public void AddScane(int enterorder,Scane scane)
        {
            if(scanes.Count<=0)
            {
                while(scanes.Count< enterorder)
                {
                    scanes.Add(scane);
                }
            }
            else
            {
                Scane last = scanes.Last();
                while(scanes.Count<enterorder-1)
                {
                    scanes.Add(last);
                }
                AddScane(scane);
            }
        }
        public Scane GetScane()
        {
            if (enterorder > scanes.Count)
                return scanes.Last();
            else
                return scanes.ElementAt(enterorder - 1);
        }
        public Scane GetScane(int enterorder)
        {
            if (enterorder > 0 && enterorder <= scanes.Count)
                return scanes.ElementAt(enterorder - 1);
            else 
                return null;
        }
        public Scane GetLastScane() //果然不能只通过enterorder来给上一个LastCane
        {
            if (lastcane != null)
                return lastcane;
            else
                return null;
        }
        public void SetLastScane(Scane scane)
        {
            lastcane = scane;
        }
        public void RemoveScane(int enterorder)
        {
            if (enterorder > scanes.Count) { return; }
            scanes.RemoveAt(enterorder - 1);
        }
        public void RemoveLastScane() //移除最后一个场景
        {
            lastcane = null;
        }
        public List<Scane> scanes; //理论上，最后一个加入Scane的enterorder等于scanes,Count
        public string name;
        public int enterorder,entertime;
        public Scane lastcane = null;
    }
}
