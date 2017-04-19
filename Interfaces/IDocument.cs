using DocumentDBCollections.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDBCollections.Interfaces
{
    interface IDocument
    {

        Task CreateDocumentCollections(string dbName, string collectionId); 
        IQueryable ExecuteSimpleQuery();
        Task DeleteDocument(string databaseName, string collectionName, string documentName);
        Task DeleteDatabase(string databaseName);
    }
}
