using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzHttpAPI.Models
{
    /// <summary>
    /// The Response Objet that will be Common for response 
    /// generated from each HTTP Method Request
    /// </summary>
    public class ResponseObject<T>
    {
        public IEnumerable<T> Records { get; set; }
        public T Record { get; set; }
        public int StatucCode { get; set; }
        public string? StatusMessage { get; set; }
    }
}
