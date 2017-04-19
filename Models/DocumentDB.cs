using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDBCollections.Models
{
    public class DocumentDB
    {
        public string EndpointUrl { get; set; }
        public string AuthorizationKey { get; set; } 
    }
}
