using System;
using System.Collections.Generic;
using System.Text;

namespace TeduCoreApp.Utilities.Dtos
{
    public class GenericResult
    {
        public object Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Error { get; set; }

        public GenericResult()
        {
        }

        public GenericResult(bool success) => Success = success;

        public GenericResult(bool success, string message) : this(success) => Message = message;

        public GenericResult(bool success, object data) : this(success) => Data = data;
    }
}
