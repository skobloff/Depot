using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depot
{
    internal abstract class DepotData
    {
        abstract internal DataTypes DataType { get; }

        internal virtual Boolean IsIndexsable
        {
            get
            {
                return true;
            }
        }

        abstract internal int Lenght { get; }
        abstract internal byte[] GetBytes();

        abstract internal string String { get; }

        internal static DepotData Construct(Guid data) //Guid
        {
            return new DepotDataGuid(data);
        }

        // Enum
        internal static DepotData Construct(short data) 
        {
            return new DepotDataEnum(data);
        }

        // Reference = 3
        internal static DepotData Construct(EntityId data) 
        {
            return new DepotDataEntityId(data);
        }

        //Int = 4
        internal static DepotData Construct(long data)
        {
            return new DepotDataInt(data);
        }

        //Real = 5
        internal static DepotData Construct(double data)
        {
            return new DepotDataReal(data);
        }

        //String = 6
        internal static DepotData Construct(string data)
        {
            return new DepotDataString(data);
        }

        //DateTime = 7
        internal static DepotData Construct(DateTime data)
        {
            return new DepotDataDateTime(data);
        }

        //Memo = 8
        internal static DepotData ConstructMemo(string data)
        {
            return new DepotDataMemo(data);
        }
    }
}
