using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    public class Prefab
    {

        public Prefab(string name = "")
        {
            this.name = name;
            this.tags = new List<string>();
        }
        public Prefab(Prefab other) //虚函数拷贝不来...
        {
            this.name = other.name;
            this.tags = new List<string>(other.tags);
            this.usefn1 = other.usefn1;
            this.usefn2 = other.usefn2;
            this.usefn3 = other.usefn3;
            this.usefn4 = other.usefn4;
            this.getdurabilityfn = other.getdurabilityfn;
        }
        public bool HasTag(string tag)
        {
            return tags.Contains(tag);
        }
        public void AddTag(string tag)
        {
            if (!tags.Contains(tag))
                tags.Add(tag);
        }
        public void RemoveTag(string tag)
        {
            tags.Remove(tag);
        }
        public delegate void UseTypev_v(Prefab who);
        public delegate void UseTypev_p(Prefab who,Prefab target);
        public delegate void UseTypev_a(Prefab who,Address address);
        public delegate void UseTypev_a_p(Prefab who,Address address, Prefab target);
        public delegate double GetTyped_v(Prefab who);
        public UseTypev_v usefn1 = (Prefab who) => { };
        public UseTypev_p usefn2 = (Prefab who, Prefab target) => { };
        public UseTypev_a usefn3 = (Prefab who, Address address) => { };
        public UseTypev_a_p usefn4 = (Prefab who, Address address, Prefab target) => { };
        public GetTyped_v getdurabilityfn = (Prefab who)=>{return 1; };
        public void Use() { usefn1(this); }
        public void Use(Prefab target) { usefn2(this,target); }
        public void Use(Address address) { usefn3(this,address); }
        public void Use(Address address,Prefab target) { usefn4(this,address, target); }
        public double GetDurability() { return getdurabilityfn(this); }
      
        public string name;
        public List<string> tags;
    }
}
