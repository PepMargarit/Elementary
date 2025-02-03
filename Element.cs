using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Elementary
{
    public class Element
    {
        public int AtomicNumber { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public double AtomicWeight { get; set; }        
        public double? IonicEnergy { get; set; }
        public double ElectroNeg {  get; set; }
        public string ElectroConfig { get; set; }
        public int[] OxidationStates { get; set; }
        public double MeltingPoint { get; set; }        
        public int Row { get; set; }
        public int Column { get; set; }
        public string Category { get; set; }
        public SolidColorBrush CategoryColor { get; set; }

    }

    public class ElementTranslation
    {        
        public string Name { get; set; }
        public string Category { get; set; }
    }
}
