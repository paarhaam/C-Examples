using System;
using System.Collections.Generic;
using System.Linq;

namespace Purdue.Agrawal.Distillation
{
    /// <summary>
    /// Logical key, label, data type or range, and other documentation about a parameter
    /// </summary>
    public class ParamKey
    {
        public ParamKey(string key, string label, bool nullable)
        {
            this.Key = key;
            this.UserLabel = label;
            this.Nullable = nullable;
        }

        /// <summary> Equal key and label </summary>
        public ParamKey(string key) 
            : this(key,key, nullable: false)
        {
        }

        /// <summary> May be used in Matlab structs </summary>
        public string Key { get; private set; }

        /// <summary> May be used in GUI </summary>
        public string UserLabel { get; private set; }

        public object Default { get; set; }

        public bool Nullable { get; set; }

        public override string ToString()
        {
            return Key;
        }
    }
}
