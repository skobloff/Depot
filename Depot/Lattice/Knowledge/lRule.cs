using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depot
{
    /// <summary>
    /// Предок всех записей о правилах
    /// </summary>
    abstract class lRule : Lattice
    {
        /// <summary>
        /// Дата начала действия правила
        /// </summary>
        DateTime startDate;
        /// <summary>
        /// Дата окончания действия правила
        /// </summary>
        DateTime endDate;
    }
}
