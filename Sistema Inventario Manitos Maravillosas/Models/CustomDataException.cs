using System.Runtime.Serialization;

namespace Sistema_Inventario_Manitos_Maravillosas.Models
{
    [Serializable]
    internal class CustomDataException : Exception
    {
        public CustomDataException()
        {
        }

        public CustomDataException(string? message) : base(message)
        {
        }

        public CustomDataException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CustomDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}