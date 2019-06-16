using System.Collections.Generic;

namespace VariableLengthRouting.Models
{
    public class Result
    {
        public string Controller { get; set; }

        public string Action { get; set; }

        public IDictionary<string, object> Data { get; } = new Dictionary<string, object>();
    }
}