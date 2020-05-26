using System;

namespace new_GenericDBMS.project.domain {
    public class TableException : Exception {
        public TableException() {}
        
        public TableException(string message) : base(message) {}
        
        public TableException(string message, Exception cause) : base(message, cause) {}
    }
}