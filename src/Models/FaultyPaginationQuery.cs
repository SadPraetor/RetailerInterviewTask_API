using System;
using System.Runtime.Serialization;

namespace API.Models {
    [Serializable]
    internal class FaultyPaginationQuery : Exception {
        public FaultyPaginationQuery() {
        }

        public FaultyPaginationQuery( string message ) : base( message ) {
        }

        public FaultyPaginationQuery( string message, Exception innerException ) : base( message, innerException ) {
        }

        protected FaultyPaginationQuery( SerializationInfo info, StreamingContext context ) : base( info, context ) {
        }
    }
}