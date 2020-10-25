using System;
using System.Collections.Generic;
using System.Linq;

namespace Pashua
{
    /// <summary>
    /// Indicates one or more errors in setting up the controls.
    /// </summary>
    public sealed class PashuaScriptException : Exception
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="errors">The validation errors.</param>
        public PashuaScriptException(IEnumerable<string> errors) 
            : base("The following errors were found in setting up the script:" + errors.Select(e=>$"\n* {e}"))
        {
        }
    }
}
