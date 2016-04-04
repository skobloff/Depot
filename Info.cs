using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depot
{
    internal class Info
    {
        public static Info Instance = new Info();

        private List<string> _storage = new List<string>();
        private object _sybcRoot = new object();

        private Info()
        {

        }

        public IEnumerable<string> GetAllMessages()
        {
            List<string> tmp;
            lock (_sybcRoot)
            {
                tmp = _storage;
                _storage = new List<string>();
            }
            return tmp;
        }

        public void Add(string message)
        {
            lock(_sybcRoot)
            {
                _storage.Add(message);
            }
        }

    }
}
