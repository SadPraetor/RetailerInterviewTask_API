using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models {
    public class ExceptionDto {

        public string Exception { get; set; }
        public string Message { get; set; }

        public ExceptionDto(string exception, string message) {
            Exception = exception;
            Message = message;
        }

        public ExceptionDto(System.Exception exception) {
            Exception = exception.GetType().Name;
            Message = exception.Message;
        }
    }
}
