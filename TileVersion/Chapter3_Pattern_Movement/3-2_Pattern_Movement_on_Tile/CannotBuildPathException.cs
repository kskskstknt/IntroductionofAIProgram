using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternMovementForTile
{
    /// <summary>
    /// entityがpathを何らかの理由で作れなかった場合に発生する例外
    /// </summary>
    class CannotBuildPathException : Exception
    {
        public CannotBuildPathException() : base() { }
        public CannotBuildPathException(string message) : base(message) { }
        public CannotBuildPathException(string message, Exception innerException) : base(message, innerException) { }
        public CannotBuildPathException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string ToString()
        {
            return Message;
        }
    }
}
