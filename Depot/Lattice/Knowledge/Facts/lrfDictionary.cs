using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <concept>
/// <head>Определение справочника</head>
/// <syntax>The [identificator] is a dictionary.</syntax>
/// <description>Эта матрица описывает  </description>
/// </concept>
namespace Depot
{
    /// <summary>
    /// Запись о факте существования справочника в базе данных
    /// </summary>
    class lrfDictionary : lrFact
    {
        override protected String getDefinition()
        {
            return "DICTIONARY";
        }
    }
}
