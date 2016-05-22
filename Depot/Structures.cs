using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Data;

namespace Depot
{
    /// <summary>
    /// Метатип trait
    /// </summary>
    public enum MetaTypes : byte
    {
        /// <summary>
        /// Метатип не задан
        /// </summary>
        None = 0,
        /// <summary>
        /// Знания-факты
        /// </summary>
        Fact = 1
    }

    /// <summary>
    /// Метатип данных
    /// </summary>
    public enum DataTypes : byte
    {
        /// <summary>
        /// Тип данных не известен
        /// </summary>
        None = 0,

        /// <summary>
        /// Тип данных GUID
        /// </summary>
        GUID = 1,

        /// <summary>
        /// Перечислимый тип
        /// </summary>
        ENUM = 2,

        /// <summary>
        /// Ссылка на другие данные
        /// </summary>
        EntityId = 3,

        /// <summary>
        /// Целочисленное значение (реальный тип Int64)
        /// </summary>
        Int = 4,

        /// <summary>
        /// Данные с плавающей запятой
        /// </summary>
        Real = 5,

        /// <summary>
        /// Строковые данные
        /// </summary>
        String = 6,

        /// <summary>
        /// Дата и время
        /// </summary>
        DateTime = 7,

        /// <summary>
        /// Не индексируемая строка
        /// </summary>
        Memo = 8
    }

    /// <summary>
    /// Тип трэйта определяет его физическую структуру.
    /// </summary>
    public enum TraitTypes : ushort
    {
        /// <summary>
        /// Тип не задан, используется тоько для страниц. Все трэйты на этой странице должны содержать тип.
        /// </summary>
        NoType  = 0,

        /// <summary>
        /// Трэйт из двух значений: 8 байт код сущности и 8 байт код lattice
        /// </summary>
        LE = 1, 

        /// <summary>
        /// Трэйт из двух значений: 8 байт код lattice и 8 байт код сущности
        /// </summary>
        EL = 2,        
        
        /// <summary>
        /// Трэйт из двух значений: 8 байт код сущности и 32 байта GUID
        /// </summary>
        LEG = 3, 

        /// <summary>
        /// Трэйт из двух значений: 32 байта GUID и 8 байт код сущности
        /// </summary>
        EGL = 4,
        
        /// <summary>
        /// Трэйт из трех значений: 8 байт код lattice, 8 байт код сущности и 2 байта Enum-значение сущности
        /// </summary>
        LEEnum = 5,

        /// <summary>
        /// Трэйт из трех значений: 8 байт код сущности, 2 байта Enum-значение сущности и 8 байт код lattice
        /// </summary>
        EEnumL = 6, 

        /// <summary>
        /// Трэйт из трех значений: 8 байт код lattice, 8 байт код сущности и строка (2 байта длинна и затем данные)
        /// </summary>
        LEStr = 7,

        /// <summary>
        /// Трэйт из трех значений: 8 байт код сущности, строка (2 байта длинна и затем данные) и 8 байт код lattice
        /// </summary>
        EStrL = 8,

        /// <summary>
        /// Трэйт из трех значений: 8 байт код lattice, 8 байт код сущности и DateTime (8 байт)
        /// </summary>
        LEDateTime = 9,

        /// <summary>
        /// Трэйт из трех значений: 8 байт код сущности, DateTime (8 байт) и 8 байт код lattice
        /// </summary>
        EDateTimeL = 10,

        /// <summary>
        /// Трэйт из трех значений: 8 байт код lattice, 8 байт код сущности и Int64 (8 байт)
        /// </summary>
        LEInt = 11,

        /// <summary>
        /// Трэйт из трех значений: 8 байт код сущности, Int64 (8 байт) и 8 байт код lattice
        /// </summary>
        EIntL = 12,

        /// <summary>
        /// Трэйт из трех значений: 8 байт код lattice, 8 байт код сущности и Real (? байт)
        /// </summary>
        LEReal = 13,

        /// <summary>
        /// Трэйт из трех значений: 8 байт код сущности, Real (? байт) и 8 байт код lattice
        /// </summary>
        ERealL = 14,

        /// <summary>
        /// Трэйт из трех значений: 8 байт код lattice, 8 байт код сущности и Real (? байт)
        /// </summary>
        LEMemo = 15

    }

    /// <summary>
    /// Структура для элемента trait, хранит код сущности - код правила, предиката, вывода, элемента справочника, поля, документа, и т.д.
    /// </summary>
    internal struct EntityId
    {
        /// <summary>
        /// Данные
        /// </summary>
        internal ulong data { get; set; }

        internal EntityId (ulong data)
            : this ()
        {
            this.data = data;
        }
    }

    /// <summary>
    /// Уникальный идентификатор trait в базе данных.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct LatticeId
    {
        /// <summary>
        /// Код
        /// </summary>
        [FieldOffset(0)]
        internal ulong data;
        [FieldOffset(0), MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        internal byte[] bytes;

        internal LatticeId(ulong data)
                : this()
        {
            this.data = data;
        }
    }


    /// <summary>
    /// 128килобайтная структура со страницей данных внутри. 
    /// Сделана чтобы в одну операцию читать всю страницу в память и выгружать ее на диск также в одну операцию со всеми описывающими переменными.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct PageData
    {
        internal const int pageSize = 131072;
        internal const int dataSize = 109196;
        internal const int maxTraitCount = 10920;
        /// <summary>
        /// Guid базы данных к которой принадлежит страница.
        /// </summary>
        [FieldOffset(0)]
        internal Guid baseGUID;
        /// <summary>
        /// Guid страницы.
        /// </summary>
        [FieldOffset(16)]
        internal Guid pageGUID;
        /// <summary>
        /// Тип trait's которые хранит страница.
        /// </summary>
        [FieldOffset(32)]
        internal ushort type;
        /// <summary>
        /// Количество traits уже сохраненных в странице.
        /// </summary>
        [FieldOffset(34)]
        internal ushort count;
        /// <summary>
        /// Массив со смещениями всех traits сохраненных на странице
        /// </summary>
        [FieldOffset(36), MarshalAs(UnmanagedType.ByValArray, SizeConst = maxTraitCount)]
        internal ushort[] index;
        /// <summary>
        /// Массив с собственно traits
        /// </summary>
        [FieldOffset(21876), MarshalAs(UnmanagedType.ByValArray, SizeConst = dataSize)]
        internal byte heap;
    }

    /// <summary>
    /// Структура для индекса страниц
    /// </summary>
    public unsafe struct PageIndexElement
    {
        /// <summary>
        /// Guid страницы
        /// </summary>
        public Guid pageGUID;
        /// <summary>
        /// Буфер с первым trait страницы
        /// </summary>
        public fixed ushort buffer[64];
    }
}
