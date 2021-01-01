using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    public class Tool:Prefab
    {
        public Tool(string name="",int usetime = 1):base(name)
        {
            this.maxusetime = curusetime = usetime;
            this.tags.Add("tool");
            this.tags.Add("reusable");
            this.getdurabilityfn = GetDurability;
        }
        public Tool(Tool other):base(other)
        {
            this.maxusetime = other.maxusetime;
            this.curusetime = other.curusetime;         
        }
        public void SetUsefn(Prefab.UseTypev_v usefn)
        {
            this.usefn1 = usefn;
        }
        public double  GetDurability(Prefab who)
        {
            var my = (Tool) who;
            return (double) my.curusetime / my.maxusetime;
        }
        public bool IsBroken()
        {
            return curusetime <= 0;
        }
        public int maxusetime,curusetime;
    }
}
