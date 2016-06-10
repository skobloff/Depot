using System;
using System.IO.MemoryMappedFiles;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <concept>
/// Depot - NoSQL database
/// Depot реализует хранение и управление бизнес-сущностями на основании правил. 
/// Depot реализует следующие бизнес-сущности, верхнего уровня:
///  * правила
///  * справочники
///  * документы
///  * учеты
///  * календари
/// Правило состоит из предикатов и выводов.
/// Справочник состоит из элементов справочника.
/// У документов верхний уровень называется - тип документа и состоит из собсвтенно документов.
/// Учет состоит из учетных проводок
/// Календарь состоит из дней
/// </concept>
namespace Depot
{
    /// <summary>
    /// Предок классов реализующих саму базу и DataSource для прикладных приложений
    /// </summary>
    public class Depot
    {
        /// <summary>
        /// возвращает статус - открыта БД или нет
        /// </summary>
        public bool IsOpen { get; }

        /// <summary>
        /// Возвращает описание базы данных
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            return "";
        }

        private Sorter sorter;
        private Holder holder;
        private SortedList<Entity> entities;

        /// <summary>
        /// Создает новый файл с базой данных и переводит механизм в готовность исполения команд
        /// </summary>
        /// <param name="_fileName">Имя нового файла для базы данных</param>
        public void CreateNewBase(string _fileName)
        {
            if (IsOpen)
            {
                Info.Instance.Add("Data base is olready open!");
                return;
            }

            holder = new Holder(_fileName, true);
            holder.CreateSorter();
            sorter = holder.sorter;
        }

        /// <summary>
        /// Открывает существующий файл с базой данных и переводит механизм в готовность исполения команд
        /// </summary>
        /// <param name="_fileName">Имя файла с базой данных</param>
        public void OpenExistsBase(string _fileName)
        {
            if (IsOpen)
            {
                Info.Instance.Add("Data base is olready open!");
                return;
            }
            holder = new Holder(_fileName, false);
            sorter = holder.sorter;
        }
    }
}
