using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    public class Hat:Prefab
    {
        public Hat(string name="",double sanitybonus=1,int usetime=100):base(name)  //每六秒刷新一次
        {
            this.tags.Add("hat");
            this.usetime = usetime; 
            this.sanitybonus = sanitybonus;
        }
        public Hat(Hat other):base(other)
        {
            this.usetime = other.usetime;
            this.sanitybonus = other.sanitybonus;
        }
        public void Remove()
        {
            Form1.traveler.sanitybonus = 0;
        }
        public int usetime;
        public double sanitybonus;
    }
}
