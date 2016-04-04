using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depot
{
    /// <summary>
    /// Holder кэширует страницы в памяти, при обращении к страницу подчитывает ее в кэш.
    /// Механизм учитывает популярность страниц, и освобождает память в первую очередь от непопулярных страниц.
    /// Holder ведет учет изменений в страницах и управляет их записью на диск.
    /// </summary>
    public class Holder
    {
        // Поток для доступа к файлу
        private FileStream file;
        // Хранит перечень всех страниц в индексе
        private SortedList<Guid, int> filePageIndex;

        // Guid базы данных
        private Guid baseGuid;

        // Текущий буфер со страницами
        private SortedList<Guid, Page> pages;
        // Индекс использования страниц
        private List<Guid> usedIndex;
        // Текущее количество буферизованных страниц
        private const int maxCountPages = 100;
        // Ссылка на Sorter чтобы извещать его о новых страницах
        internal Sorter sorter { get; private set; }


        /// <summary>
        /// Init new instance of Holder
        /// </summary>
        /// <param name="filename">true - It is a name of file</param>
        /// <param name="isCreate">true - create new base, false - open older base</param>
        public Holder(string filename, Boolean isCreate)
        {
            try
            {
                if (isCreate)
                {
                    file = File.Create(filename);
                    baseGuid = Guid.NewGuid();
                }
                else
                {
                    file = File.Open(filename, FileMode.Open);
                }
                
            }
            catch(IOException e)
            {
                Info.Instance.Add("Error create new data base!");
                Info.Instance.Add(e.Message);
                file = null;
            }

            if ((file.Length % (128*1024)) > 0)
            {
                Info.Instance.Add("Page count is not round in file!");
                Info.Instance.Add("File not open.");
                file.Dispose();
                return;
            }

            filePageIndex = new SortedList<Guid, int>();
            pages = new SortedList<Guid, Page>();
            usedIndex = new List<Guid>();
        }

        #region internal index routines
        // Добавляет информацию о странице в буфере в текущие индексы и буферы
        private void IndexAddPage(Page _page)
        {
            pages.Add(_page.data.pageGUID, _page);
            usedIndex.Insert(0, _page.data.pageGUID);
            //pageCount++;   
        }

        // Удаляет информацию о странице из текущих буферов и индексов
        private void IndexDelPage(Page _page)
        {
            usedIndex.Remove(_page.data.pageGUID);
            pages.Remove(_page.data.pageGUID);
            //pageCount--;
        }

        // Вызывается при использовании страницы которая уже есть в памяти, номер этой страницы ставится первым
        // Таким образом usedIndex хранит Guid страниц в порядке обратном их последнему использованию
        private void IndexPageUp(Page _page)
        {
            usedIndex.Remove(_page.data.pageGUID);
            usedIndex.Insert(0, _page.data.pageGUID);
        }
        #endregion


        #region page routines

        private Page LoadPage(int _numPageInFile)
        {
            PushPlace();
            Page page = new Page(_numPageInFile);
            page.Load(file);
            IndexAddPage(page);
            return page;
        }

        private void PushPlace()
        {
            if (usedIndex.Count == maxCountPages)
            {
                Page page = pages[usedIndex.Last()];
                freePage(page);
                IndexDelPage(page);
            }
        }

        private void freePage(Page page)
        {
            if (page.needSave)
            {
                //сохраняем здесь данные в файл.
                page.Write(file);
            }
        }

        #endregion

        #region external interface
        internal Page getPage(Guid guid)
        {
            //ищем страницу в буфере
            Page page = pages[guid];
            if (page != null)
                return page;
            //читаем искомую страницу в память и возвращаем
            PushPlace();
            int numPageInFile = filePageIndex[guid];
            page = new Page(numPageInFile);
            page.Load(file);
            IndexAddPage(page);
            return page;
        }

        internal void CreateSorter()
        {
            sorter = new Sorter();
            long pageCount = file.Length / (128 * 1024);
            for (int i = 0; i < pageCount; i++)
            {
                PushPlace();
                Page page = LoadPage(i);
                if (i == 0)
                    baseGuid = page.data.baseGUID;
                filePageIndex.Add(page.data.pageGUID, i);
                IndexAddPage(page);
                sorter.AddPage(page);
            }
        }

        internal Page CreateNewPage(TraitTypes _traitType)
        {
            Page page = new Page(pages.Count,_traitType,baseGuid,Guid.NewGuid());
            return page;
        }
        #endregion
    }
}
