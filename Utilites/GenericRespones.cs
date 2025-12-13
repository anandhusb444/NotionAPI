using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Identity.Client;

namespace NotionAPI.Utilites
{
    public class GenericRespones<T>
    {
        public string Message { get; set; }
        public string? Error { get; set; }
        public int StatusCode { get; set; }
        public T Data { get; set; }
        public bool Status { get; set; }


        public GenericRespones(string message, string error,int statusCode,T data,bool status)
        {
            Message = message;
            Error = error;
            StatusCode = statusCode;
            Data = data;
            Status = status;
        }
    }
}
