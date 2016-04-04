using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depot
{
    internal class TraitFactory
    {
        public static TraitFactory Instance = new TraitFactory();

        private List<Trait> _storage = new List<Trait>();
   
        private TraitFactory()
        {
            // NoType = 0,
            _storage.Add(null);
            // EL = 1, 
            _storage.Add(new TraitEL(new EntityId(0), new LatticeId(0)));
            // LE = 2,        
            _storage.Add(new TraitLE(new EntityId(0), new LatticeId(0)));
            // LEG = 3, 
            _storage.Add(new TraitLEG(new EntityId(0), new LatticeId(0), Guid.Empty));
            // EGL = 4,
            _storage.Add(new TraitEGL(new EntityId(0), new LatticeId(0), Guid.Empty));
            // LEEnum = 5,
            _storage.Add(new TraitLEEnum(new EntityId(0), new LatticeId(0), 0));
            // EEnumL = 6, 
            _storage.Add(new TraitEEnumL(new EntityId(0), new LatticeId(0), 0));
            // LEStr = 7,
            _storage.Add(new TraitLEStr(new EntityId(0), new LatticeId(0), ""));
            // EStrL = 8,
            _storage.Add(new TraitEStrL(new EntityId(0), new LatticeId(0), ""));
            // LEDateTime = 9,
            _storage.Add(new TraitLEDate(new EntityId(0), new LatticeId(0), DateTime.MinValue));
            // EDateTimeL = 10,
            _storage.Add(new TraitEDateL(new EntityId(0), new LatticeId(0), DateTime.MinValue));
            // LEInt = 11,
            _storage.Add(new TraitLEInt(new EntityId(0), new LatticeId(0), 0));
            // EIntL = 12,
            _storage.Add(new TraitEIntL(new EntityId(0), new LatticeId(0), 0));
            // LEReal = 13,
            _storage.Add(new TraitLEReal(new EntityId(0), new LatticeId(0), 0));
            // ERealL = 14,
            _storage.Add(new TraitERealL(new EntityId(0), new LatticeId(0), 0));
            // LEMemo = 15
        }

        public Trait GetZeroTrait(TraitTypes _type)
        {
            return _storage[(int)_type].GetZeroTrait();
        }

    }
}
