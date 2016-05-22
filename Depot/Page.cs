using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depot
{
    /// <summary>
    /// Класс-держатель отсортированного массива traits в пямяти.
    /// </summary>
    internal class Page
    {
        internal bool needSave { get; private set; }

        private PageData _data;
        internal PageData data { get { return _data; } private set { _data = value; } }
        
        internal int numPageInFile { get; private set; } 

        public Page (int _numPageInFile)
        {
            numPageInFile = _numPageInFile;
        }

        public Page(
            int _numPageInFile, 
            TraitTypes _pageTraitType,
            Guid _baseGuid,
            Guid _pageGuid)
        {
            numPageInFile = _numPageInFile;
            _data.type = (ushort)_pageTraitType;
            _data.pageGUID = _pageGuid;
            _data.baseGUID = _baseGuid;
        }

        /// <summary>
        /// Определяет отношение trait к странице
        /// Поисковые методы возможно будут не в страницах, понимание где они будут появится позже
        /// </summary>
        /// <param name="_trait">Искомый trait</param>
        /// <returns>
        /// -1 trait меньше начального в странице;
        /// 0 trait попадает в страницу но его нет;
        /// 1 trait попадает в страницу и такой есть;
        /// 2 trait больше конечного на странице
        /// </returns>
        internal int find(Trait _trait)
        {
            return 0;
        }

        internal void Load(FileStream fs)
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(PageData))];
            fs.Read(buffer, numPageInFile * PageData.pageSize, PageData.pageSize);
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            Marshal.PtrToStructure(handle.AddrOfPinnedObject(), _data);
            handle.Free();
        }

        internal void Write(FileStream fs)
        {
            byte[] rawdata = new byte[PageData.pageSize];
            GCHandle handle = GCHandle.Alloc(rawdata, GCHandleType.Pinned);
            Marshal.StructureToPtr(_data, handle.AddrOfPinnedObject(), false);
            fs.Write(rawdata, numPageInFile * PageData.pageSize, PageData.pageSize);
            handle.Free();
        }
        
        /// <summary>
        /// Возвращает первый трэйт, если страница пуста то возвращает особый нулевой трэйт
        /// </summary>
        /// <returns></returns>
        internal Trait getFirstTrait()
        {
            return new TraitLE(new EntityId(0), new LatticeId(0));
        }
    }

}
