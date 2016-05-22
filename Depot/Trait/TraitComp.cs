using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depot
{
    /// <summary>
    /// Realise IComparer for compare two traits.
    /// </summary>
    internal class TraitComp : IComparer<Trait>
    {
        /// <summary>
        /// Сравнивает два трэйта
        /// </summary>
        /// <param name="x">Первый трэйт для сравнения</param>
        /// <param name="y">Второй трэйт для сравнения</param>
        /// <returns>-1,0,+1 в зависимости от результата сравнения</returns>
        public int Compare(Trait x, Trait y)
        {
            if (x.TraitType < y.TraitType)
                return -1;
            if (x.TraitType > y.TraitType)
                return 1;
            return x.Compare(y);   
        }
    }
}
