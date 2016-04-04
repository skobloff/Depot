using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depot
{
    /// <summary>
    /// Предок всех записей о фактах
    /// Факт определяется синтаксисом:
    /// The [name] is a [definition].
    /// </summary>
    abstract class lrFact : lRule
    {
        /// <summary>
        /// Наименование факта
        /// </summary>
        String name;
        /// <summary>
        /// Возвращает UPCASE-строку содержащую определение факта
        /// </summary>
        /// <returns>Определение</returns>
        abstract protected String getDefinition();
    }
}
