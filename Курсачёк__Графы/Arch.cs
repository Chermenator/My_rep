using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсачёк__Графы
{
    public partial class Arch
    {
        public int source;
        public int target;
        public int weight;

        public Arch(int source, int target, int weight)
        {
            this.source = source;
            this.weight = weight;
            this.target = target;

            
        }

        public override string ToString()
        {
            return "(" + (source+1) + "; " + (target+1) + ")";
        }

        public override bool Equals(object obj)//метод возвращает true если выбрана одна и та же дуга
        {
            return obj is Arch arch &&
                   source == arch.source &&
                   target == arch.target;
                  
        }

        public override int GetHashCode()
        {
            int hashCode = -1943342112;
            hashCode = hashCode * -1521134295 + source.GetHashCode();
            hashCode = hashCode * -1521134295 + target.GetHashCode();
            return hashCode;
        }
    }

    public partial class Contour
    {
        public string contour;
        public List<int> versh_of_cont = new List<int>();
        public List<Arch> archs = new List<Arch>();

        public Contour(List<int> versh_of_cont, List<Arch> archs)
        {
            foreach (int v in versh_of_cont)
            {
                this.versh_of_cont.Add(v);
            }
            foreach (Arch a in archs)
            {
                this.archs.Add(a);
            }
        }
        
        //метод для проверки вхождения дуги графа в дугу контура
        public bool Contains(Arch a)
        {
            return archs.Contains(a);
        }
        //поиск веса максимальной дуги в контуре
        public int Max_Arch()
        {
            int max = archs[0].weight;
            foreach (Arch a in archs)
            {
                if (a.weight > max) max = a.weight;
            }
            return max;
        }
        public override string ToString()
        {
            
            string str = archs[0].ToString();
            for (int i = 1; i < archs.Count; i++)
            {
                str += "-"+archs[i].ToString() ;
            }
           // str += (versh_of_cont[0] + 1);

            str += "\t Record = " + Max_Arch();

            return str;
          
        }

    }

    
}
