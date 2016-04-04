using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depot
{
    /// <summary>
    /// Реализует trait - минимальную единицу хранения данных в системе.
    /// </summary>
    internal abstract class Trait
    {
        internal TraitTypes TraitType { get; set; }
        internal EntityId EntityId { get; set; } //EntityId есть всегда, потому что Trait это по сути отражение отношения Entity с чем-либо
        internal LatticeId LatticeId { get; set; } //LatticeId есть всегда, потому что Trait всегда принадлежит к какой-либо проводке

        internal Trait (TraitTypes _traitType, EntityId _entityId, LatticeId _latticeId)
        {
            TraitType = _traitType;
            EntityId = _entityId;
            LatticeId = _latticeId;
        }

        /// <summary>
        /// Создает пустой трэйт для случая индексации пустой страницы
        /// </summary>
        /// <returns></returns>
        internal abstract Trait GetZeroTrait();
        internal abstract int getValueLenght();
        internal abstract byte getValueBytes(int _i);
        internal abstract int compareValue(Trait _trait);

        /// <summary>
        /// Подсчитывает количество ushort необходимых для хранения данного trait.
        /// ВАЖНО!!! Нулевым ushort идет тип самого trait.
        /// </summary>
        /// <returns>Количество ushort</returns>
        internal ushort Lenght()
        {
            return (ushort)(9 + getValueLenght());
        }

        /// <summary>
        /// Возвращает данные.
        /// </summary>
        /// <param name="_i">Номер</param>
        /// <returns>Данные</returns>
        internal abstract byte Data(int _i);

        /// <summary>
        /// Сравнивает текущий и переданный трэйты. Трэйты должны быть строго одного типа.
        /// </summary>
        /// <param name="_trait">Трэйт переданный для сравнения</param>
        /// <returns>-1 если текущий трэйт меньше, 0 если равны, +1 если переданный трэйт меньше</returns>
        internal abstract int Compare(Trait _trait);

        /// <summary>
        /// Возвражает данные хранимые в trait если таковые есть.
        /// </summary>
        /// <returns>Данные</returns>
        internal abstract ValueType getValue();
        internal abstract String getStrValue();


    }

    #region Data & Mirror

    internal abstract class TraitData : Trait // LatticeId - EntityId - Value
    {
        internal TraitData(TraitTypes _traitType, EntityId _entityId, LatticeId _latticeId)
            : base(_traitType, _entityId, _latticeId) { }

        internal override int Compare(Trait _trait)
        {
            if (LatticeId.data < _trait.LatticeId.data)
                return -1;

            if (LatticeId.data > _trait.LatticeId.data)
                return +1;

            if (EntityId.data < _trait.EntityId.data)
                return -1;

            if (EntityId.data > _trait.EntityId.data)
                return +1;

            return compareValue(_trait);
        }

        internal override byte Data(int _i)
        {
            if (_i <= 1)
                return BitConverter.GetBytes((ushort)TraitType)[_i];

            if (_i <= 9)
                return LatticeId.bytes[_i - 2];

            if (_i <= 17)
                return EntityId.bytes[_i - 9];

            if (_i <= (17 + getValueLenght()))
                return getValueBytes(_i - 17);
            return 0;
        }
    }

    internal abstract class TraitIndex : Trait // EntityId - Value - LatticeId
    {
        internal TraitIndex(TraitTypes _traitType, EntityId _entityId, LatticeId _latticeId)
            : base(_traitType, _entityId, _latticeId)
        { }

        internal override int Compare(Trait _trait)
        {
            if (EntityId.data < _trait.EntityId.data)
                return -1;

            if (EntityId.data > _trait.EntityId.data)
                return +1;

            int i = compareValue(_trait);
            if (i != 0)
                return i;

            if (LatticeId.data < _trait.LatticeId.data)
                return -1;

            if (LatticeId.data > _trait.LatticeId.data)
                return +1;

            return 0;
        }

        internal override byte Data(int _i)
        {
            if (_i <= 1)
                return BitConverter.GetBytes((ushort)TraitType)[_i];

            if (_i <= 9)
                return EntityId.bytes[_i - 2];

            if (_i <= (9 + getValueLenght()))
                return getValueBytes(_i - 9);

            if (_i <= (17 + getValueLenght()))
                return LatticeId.bytes[_i - (9 + getValueLenght())];

            return 0;
        }
    }

    #endregion
    #region LE-EL
    internal class TraitLE : TraitData
    {
        internal TraitLE(EntityId _entityId, LatticeId _latticeId)
            : base(TraitTypes.LE, _entityId, _latticeId) { }

        internal override int compareValue(Trait _trait)
        {
            return 0;
        }

        internal override ValueType getValue()
        {
            return 0;
        }

        internal override int getValueLenght()
        {
            return 0;
        }

        internal override byte getValueBytes(int _i)
        {
            return 0;
        }

        internal override string getStrValue()
        {
            return "";
        }

        internal override Trait GetZeroTrait()
        {
            return new TraitLE(new EntityId(0), new LatticeId(0));
        }
    }

    internal class TraitEL : TraitIndex
    {
        internal TraitEL(EntityId _entityId, LatticeId _latticeId)
            : base(TraitTypes.EL, _entityId, _latticeId)
        {
            TraitType = TraitTypes.EL;
        }

        internal override int compareValue(Trait _trait)
        {
            return 0;
        }

        internal override ValueType getValue()
        {
            return 0;
        }

        internal override int getValueLenght()
        {
            return 0;
        }

        internal override byte getValueBytes(int _i)
        {
            return 0;
        }

        internal override string getStrValue()
        {
            return "";
        }

        internal override Trait GetZeroTrait()
        {
            return new TraitLE(new EntityId(0), new LatticeId(0));
        }
    }
    #endregion

    #region EGL-LEG
    internal class TraitLEG : TraitData
    {
        internal Guid Soul { get; set; }

        internal TraitLEG(EntityId _entityId, LatticeId _latticeId, Guid _guid)
            : base(TraitTypes.LEG, _entityId, _latticeId)
        {
            Soul = _guid;
        }

        internal override ValueType getValue()
        {
            return Soul;
        }

        internal override int getValueLenght()
        {
            return 8;
        }

        internal override byte getValueBytes(int _i)
        {
            byte[] soulBuf = Soul.ToByteArray();
            return soulBuf[_i];
        }

        internal override int compareValue(Trait _trait)
        {
            Guid buf = (Guid)_trait.getValue();
            return Soul.CompareTo(buf);
        }

        internal override string getStrValue()
        {
            return Soul.ToString();
        }

        internal override Trait GetZeroTrait()
        {
            return new TraitLEG(new EntityId(0), new LatticeId(0), Guid.Empty);
        }
    }

    internal class TraitEGL : TraitIndex
    {
        internal Guid Soul { get; set; }

        internal TraitEGL(EntityId _entityId, LatticeId _latticeId, Guid guid)
            : base(TraitTypes.EGL, _entityId, _latticeId)
        {
            Soul = guid;
        }

        internal override ValueType getValue()
        {
            return Soul;
        }

        internal override int getValueLenght()
        {
            return 8;
        }

        internal override byte getValueBytes(int _i)
        {
            byte[] buf = Soul.ToByteArray();
            return buf[_i];
        }

        internal override int compareValue(Trait _trait)
        {
            Guid buf = (Guid)_trait.getValue();
            return Soul.CompareTo(buf);
        }

        internal override string getStrValue()
        {
            return Soul.ToString();
        }

        internal override Trait GetZeroTrait()
        {
            return new TraitEGL(new EntityId(0), new LatticeId(0), Guid.Empty);
        }
    }
    #endregion

    #region LEEnum-EEnumL
    internal class TraitLEEnum : TraitData
    {
        internal ushort enumData { get; private set; }

        internal TraitLEEnum(EntityId _entityId, LatticeId _latticeId, ushort _enum)
            : base(TraitTypes.LEEnum, _entityId, _latticeId)
        {
            enumData = _enum;
        }

        internal override int getValueLenght()
        {
            return 1;
        }

        internal override byte getValueBytes(int _i)
        {
            return BitConverter.GetBytes(enumData)[_i];
        }

        internal override int compareValue(Trait _trait)
        {
            ushort buf = (ushort)_trait.getValue();

            if (enumData < buf)
                return -1;

            if (enumData > buf)
                return 1;

            return 0;
        }

        internal override ValueType getValue()
        {
            return enumData;
        }

        internal override string getStrValue()
        {
            return enumData.ToString();
        }

        internal override Trait GetZeroTrait()
        {
            return new TraitLEEnum(new EntityId(0), new LatticeId(0), 0);
        }
    }

    internal class TraitEEnumL : TraitIndex
    {
        internal ushort enumData { get; private set; }

        internal TraitEEnumL(EntityId _entityId, LatticeId _latticeId, ushort _enum)
            : base(TraitTypes.EEnumL, _entityId, _latticeId)
        {
            enumData = _enum;
        }

        internal override int getValueLenght()
        {
            return 1;
        }

        internal override byte getValueBytes(int _i)
        {
            return BitConverter.GetBytes(enumData)[_i];
        }

        internal override int compareValue(Trait _trait)
        {
            ushort buf = (ushort)_trait.getValue();

            if (enumData < buf)
                return -1;

            if (enumData > buf)
                return 1;

            return 0;
        }

        internal override ValueType getValue()
        {
            return enumData;
        }

        internal override string getStrValue()
        {
            return enumData.ToString();
        }

        internal override Trait GetZeroTrait()
        {
            return new TraitEEnumL(new EntityId(0), new LatticeId(0), 0);
        }
    }
    #endregion

    #region LEStr-EStrL
    internal class TraitLEStr : TraitData
    {
        internal String strData { get; private set; }
        Encoding u16LE = Encoding.Unicode;

        internal TraitLEStr(EntityId _entityId, LatticeId _latticeId, String _str)
            : base(TraitTypes.LEStr, _entityId, _latticeId)
        {
            strData = _str;
        }

        internal override int getValueLenght()
        {
            return u16LE.GetByteCount(strData);
        }

        internal override byte getValueBytes(int _i)
        {
             return u16LE.GetBytes(strData)[_i];
        }

        internal override int compareValue(Trait _trait)
        {
            return strData.CompareTo(_trait.getStrValue());
        }

        internal override ValueType getValue()
        {
            return 0;
        }

        internal override string getStrValue()
        {
            return strData;
        }

        internal override Trait GetZeroTrait()
        {
            return new TraitLEStr(new EntityId(0), new LatticeId(0), "");
        }
    }

    internal class TraitEStrL  : TraitIndex
    {
        internal String strData { get; private set; }
        Encoding u16LE = Encoding.Unicode;

        internal TraitEStrL(EntityId _entityId, LatticeId _latticeId, String _str)
            : base(TraitTypes.LEStr, _entityId, _latticeId)
        {
            strData = _str;
        }

        internal override int getValueLenght()
        {
            return u16LE.GetByteCount(strData);
        }

        internal override byte getValueBytes(int _i)
        {
            return u16LE.GetBytes(strData)[_i];
        }

        internal override int compareValue(Trait _trait)
        {
            return strData.CompareTo(_trait.getValue());
        }

        internal override ValueType getValue()
        {
            return 0;
        }

        internal override string getStrValue()
        {
            return strData;
        }

        internal override Trait GetZeroTrait()
        {
            return new TraitEStrL(new EntityId(0), new LatticeId(0), "");
        }
    }
    #endregion

    #region LEInt-EIntL
    internal class TraitLEInt : TraitData
    {
        internal long Soul { get; set; }

        internal TraitLEInt(EntityId _entityId, LatticeId _latticeId, long _long)
            : base(TraitTypes.LEInt, _entityId, _latticeId)
        {
            Soul = _long;
        }

        internal override int getValueLenght()
        {
            return 8;
        }

        internal override byte getValueBytes(int _i)
        {
            return BitConverter.GetBytes(Soul)[_i];
        }

        internal override int compareValue(Trait _trait)
        {
            long buf = (long)_trait.getValue();

            if (Soul < buf)
                return -1;

            if (Soul > buf)
                return 1;

            return 0;
        }

        internal override ValueType getValue()
        {
            return Soul;
        }

        internal override string getStrValue()
        {
            return Soul.ToString();
        }

        internal override Trait GetZeroTrait()
        {
            return new TraitLEInt(new EntityId(0), new LatticeId(0), 0);
        }
    }

    internal class TraitEIntL : TraitIndex
    {
        internal long Soul { get; set; }

        internal TraitEIntL(EntityId _entityId, LatticeId _latticeId, long _long)
            : base(TraitTypes.EIntL, _entityId, _latticeId)
        {
            Soul = _long;
        }

        internal override int getValueLenght()
        {
            return 8;
        }

        internal override byte getValueBytes(int _i)
        {
            return BitConverter.GetBytes(Soul)[_i];
        }

        internal override int compareValue(Trait _trait)
        {
            long buf = (long)_trait.getValue();

            if (Soul < buf)
                return -1;

            if (Soul > buf)
                return 1;

            return 0;
        }

        internal override ValueType getValue()
        {
            return Soul;
        }

        internal override string getStrValue()
        {
            return Soul.ToString();
        }

        internal override Trait GetZeroTrait()
        {
            return new TraitEIntL(new EntityId(0), new LatticeId(0), 0);
        }
    }
    #endregion

    #region LEReal-ERealL
    internal class TraitLEReal : TraitData
    {
        internal double Soul { get; private set; }

        internal TraitLEReal(EntityId _entityId, LatticeId _latticeId, double _double)
            : base(TraitTypes.LEReal, _entityId, _latticeId)
        {
            Soul = _double;
        }

        internal override int getValueLenght()
        {
            return 8;
        }

        internal override byte getValueBytes(int _i)
        {
            return BitConverter.GetBytes(Soul)[_i];
        }

        internal override int compareValue(Trait _trait)
        {
            double buf = (double)_trait.getValue();

            if (Soul < buf)
                return -1;

            if (Soul > buf)
                return 1;

            return 0;
        }

        internal override ValueType getValue()
        {
            return Soul;
        }

        internal override string getStrValue()
        {
            return Soul.ToString();
        }

        internal override Trait GetZeroTrait()
        {
            return new TraitLEReal(new EntityId(0), new LatticeId(0), 0);
        }
    }

    internal class TraitERealL : TraitIndex
    {
        internal double Soul { get; private set; }

        internal TraitERealL(EntityId _entityId, LatticeId _latticeId, double _double)
            : base(TraitTypes.ERealL, _entityId, _latticeId)
        {
            Soul = _double;
        }

        internal override int getValueLenght()
        {
            return 8;
        }

        internal override byte getValueBytes(int _i)
        {
            return BitConverter.GetBytes(Soul)[_i];
        }

        internal override int compareValue(Trait _trait)
        {
            double buf = (double)_trait.getValue();

            if (Soul < buf)
                return -1;

            if (Soul > buf)
                return 1;

            return 0;
        }

        internal override ValueType getValue()
        {
            return Soul;
        }

        internal override string getStrValue()
        {
            return Soul.ToString();
        }

        internal override Trait GetZeroTrait()
        {
            return new TraitERealL(new EntityId(0), new LatticeId(0), 0.0);
        }
    }
    #endregion

    #region LEDate-EdateL
    internal class TraitLEDate : TraitData
    {
        internal DateTime Soul { get; set; }

        internal TraitLEDate(EntityId _entityId, LatticeId _latticeId, DateTime _DateTime)
            : base(TraitTypes.LEDateTime, _entityId, _latticeId)
        {
            Soul = _DateTime;
        }

        internal override int getValueLenght()
        {
            return 8;
        }

        internal override byte getValueBytes(int _i)
        {
            long buf = Soul.ToBinary();
            return BitConverter.GetBytes(buf)[_i];
        }

        internal override int compareValue(Trait _trait)
        {
            DateTime buf = (DateTime)_trait.getValue();
            return Soul.CompareTo(buf);
        }

        internal override ValueType getValue()
        {
            return Soul;
        }

        internal override string getStrValue()
        {
            return Soul.ToString();
        }

        internal override Trait GetZeroTrait()
        {
            return new TraitLEDate(new EntityId(0), new LatticeId(0), DateTime.MinValue);
        }
    }

    internal class TraitEDateL : TraitIndex
    {
        internal DateTime Soul { get; private set; }

        internal TraitEDateL(EntityId _entityId, LatticeId _latticeId, DateTime _DateTime)
            : base(TraitTypes.EDateTimeL, _entityId, _latticeId)
        {
            Soul = _DateTime;
        }

        internal override int getValueLenght()
        {
            return 8;
        }

        internal override byte getValueBytes(int _i)
        {
            long buf = Soul.ToBinary();
            return BitConverter.GetBytes(buf)[_i];
        }

        internal override int compareValue(Trait _trait)
        {
            DateTime buf = (DateTime)_trait.getValue();
            return Soul.CompareTo(buf);
        }

        internal override ValueType getValue()
        {
            return Soul;
        }

        internal override string getStrValue()
        {
            return Soul.ToString();
        }

        internal override Trait GetZeroTrait()
        {
            return new TraitEDateL(new EntityId(0), new LatticeId(0), DateTime.MinValue);
        }
    }
    #endregion
}
