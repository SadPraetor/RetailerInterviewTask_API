using System;
using System.Runtime.Serialization;

namespace API.Models {
    [Serializable]
    public class DescriptionTooLongException : Exception {
        public DescriptionTooLongException() {
        }

        public DescriptionTooLongException(int limit) : base($"Description length is limited to {limit} characters") {

        }

        public DescriptionTooLongException( string message ) : base( message ) {
        }

        public DescriptionTooLongException( string message, Exception innerException ) : base( message, innerException ) {
        }

        protected DescriptionTooLongException( SerializationInfo info, StreamingContext context ) : base( info, context ) {
        }
    }
}