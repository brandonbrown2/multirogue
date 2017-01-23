using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDemo1.Map
{
    public interface IDeterministicCellAppearance : SadConsole.ICellAppearance
    {
        void Randomize(Random r);
    }
}
