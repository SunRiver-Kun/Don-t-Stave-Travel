using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    public class Food:Prefab
    {
        public double healthdelta, sanitydelta, hungerdelta;
        public Food cookfood; //简单的烤
        public Dictionary<string, uint> recipe; //简单的烤的
        public List<Dictionary<string, uint>> cookrecipes; //这个是用烹饪锅煮的
        public Food(string name="", double hungerdelta = 10, double healthdelta = 0 , double sanitydelta =0 ):base(name)
        {
            this.recipe = new Dictionary<string, uint>();
            this.cookrecipes = new List<Dictionary<string, uint>>();
            this.cookfood = null;
            this.healthdelta = healthdelta;
            this.sanitydelta = sanitydelta;
            this.hungerdelta = hungerdelta;
            this.usefn1 = Use;
            this.tags.Add("food");
            this.tags.Add("eatable");
        }
        public Food(Food other):base(other)
        {
            this.recipe = new Dictionary<string, uint>(other.recipe);
            this.cookrecipes = new List<Dictionary<string, uint>>(other.cookrecipes);
            this.cookfood = other.cookfood;
            this.healthdelta = other.healthdelta;
            this.sanitydelta = other.sanitydelta;
            this.hungerdelta = other.hungerdelta;
        }
        public bool IsCookable()
        {
            return cookfood != null;
        }
        public new void Use()
        {
            Form1.traveler.Eat(this);
            Form1.traveler.backpack.Remove(this.name);
        }
       public void SetRecipe(Dictionary<string,uint> recipe)
        {          
            this.recipe = new Dictionary<string, uint>(recipe);
        }
        public void SetCookfood(Food cookfood)
        {
            this.AddTag("cookable");
            this.cookfood = new Food(cookfood);
        }
    }
}
