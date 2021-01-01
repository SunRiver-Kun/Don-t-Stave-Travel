using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    public class Worldtime
    {
        public static void InitData()
        {
            allts.Add(6, "早上");
            allts.Add(12, "中午");
            allts.Add(14, "下午");
            allts.Add(18, "晚上");
        }
        public static TimeSpan GetTime()
        {
            return time;
        }
        public static TimeSpan GetTime(string state)
        {
            if (!allts.ContainsValue(state)) { return new TimeSpan(0, 0, 0); }
            int index = allts.IndexOfValue(state);
            return new TimeSpan(allts.ElementAt(index).Key, 0, 0);
        }
        public static TimeSpan GetTimeNowToNext()
        {
            string nextstate = GetNextState();
            if (nextstate == allts.First().Value)  //过来一晚上
            {
                return new TimeSpan(time.Days, 24, 0, 0) - time + GetTime(nextstate);
            }
            else
            {
                return GetTime(nextstate) - ( time - new TimeSpan(time.Days, 0, 0, 0) );
            }
        }
        public static string GetState()
        {
            return state;
        }
        public static string GetNextState()
        {
            int index = allts.IndexOfValue(state);
            if (index == allts.Count - 1)
                return allts.ElementAt(0).Value;
            else
                return allts.ElementAt(index + 1).Value;
        }
        public static TimeSpan AddTime(TimeSpan passtime)
        {
            time += passtime;
            SetState(); //刷新下state
            return time;
        }
        public static void SetState()
        {
            int hour = time.Hours;
            for(int i=0;i<allts.Count-1;++i)
            {
                if(hour>=allts.ElementAt(i).Key && hour<allts.ElementAt(i+1).Key)
                {
                    state = allts.ElementAt(i).Value;
                    return;
                }
            }
            state = allts.ElementAt(allts.Count - 1).Value;
        }
        public static void SetState(string newstate)
        {
            if (allts.ContainsValue(newstate))
            {
                int day = time.Days;
                time = new TimeSpan(day, 0, 0, 0) + GetTime(newstate);
                state = newstate;
            }
        }
        public static void GotoNextState(double rate = 1) //旅行者也扣
        {
            TimeSpan time = GetTimeNowToNext();
            Form1.traveler.UpdataHunger(time,rate);

            if (state=="下午")
            {
                SetState("晚上");               
                Form1.traveler.sanityrate -= 1; //扣san
                Form1.traveler.UpdataSanity(time,rate);
                Form1.traveler.sanityrate += 1; //数值回来
              
            }
            else if(state=="晚上") 
            {
                SetState("早上");
                bool IsHasLight = Form1.traveler.HasTag("light");
                Form1.traveler.sanityrate -= IsHasLight?1:3; //扣san
                Form1.traveler.UpdataSanity(time,rate);
                Form1.traveler.sanityrate += IsHasLight ? 1 : 3; //数值回来
                Form1.traveler.RemoveTag("light");
            }
            else
            {
                SetState(GetNextState());
                Form1.traveler.UpdataSanity(time,rate);
            }

        }
       //   6~12 早上  12~14 中午  14~18 下午  18~6 晚上
        private static TimeSpan time = new TimeSpan(6,0,0);
        private static string state = "早上";
        private static SortedList<int, string> allts = new SortedList<int, string>();
    }
}
