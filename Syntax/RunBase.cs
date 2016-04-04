using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Parse;

namespace Syntax
{
    /// <summary>
    /// Parent all class where do in system
    /// </summary>
    public abstract class RunBase
    {
        /// <summary>
        /// Link to match from result of script parsing.
        /// </summary>
        public Match match { get; private set; }

        /// <summary>
        /// Init class
        /// </summary>
        /// <param name="_match"></param>
        public RunBase(Match _match)
        {
            match = _match;
        }

        /// <summary>
        /// Have to contain code where check condition for method Run().
        /// </summary>
        /// <returns> Return "true" if all OK</returns>
        protected abstract bool Verified();

        /// <summary>
        /// Do functionaliti. Must be ovverriden.
        /// </summary>
        protected abstract void Run();

        /// <summary>
        /// Call this method for execute functionality
        /// </summary>
        public void Go()
        {
            if (this.Verified())
                this.Run();
        }
    }
}
