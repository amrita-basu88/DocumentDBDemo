using DocumentDBCollections.Interfaces;
using DocumentDBCollections.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDBCollections
{
    public class DocumentDBOperations : IDocument
    {
        public string _endpointUrl { get; set; }
        public string _authorizationKey { get; set; }
        public DocumentClient _client { get; set; }
        public DocumentDBOperations(string EndpointUrl, string AuthorizationKey, DocumentClient client)
        {
            this._authorizationKey = AuthorizationKey;
            this._endpointUrl = EndpointUrl;
            this._client = client;
        }

        public async Task CreateDocumentDB(string dbName)
        {
            await this._client.CreateDatabaseIfNotExistsAsync(new Database { Id = dbName });
        }
        public async Task CreateDocumentCollections(string dbName, string collectionId)
        {
            await this._client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(dbName), new DocumentCollection { Id = collectionId });
        }

        public async Task InsertDocuments(Family family)
        {
            await this.CreateFamilyDocumentIfNotExists("FamilyDB", "FamilyCollection", family);
        }

        public async Task CreateFamilyDocumentIfNotExists(string databaseName, string collectionName, Family family)
        {
            try
            {
                await this._client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, family.Id));
                this.WriteToConsoleAndPromptToContinue("Found {0}", family.Id);
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await this._client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), family);
                    this.WriteToConsoleAndPromptToContinue("Created Family {0}", family.Id);
                }
                else
                {
                    throw;
                }
            }
        }

        private void WriteToConsoleAndPromptToContinue(string format, params object[] args)
        {
            Console.WriteLine(format, args);
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }



       public IQueryable ExecuteSimpleQuery()  
        {
            // Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Here we find the Andersen family via its LastName
            IQueryable<Family> familyQuery = this._client.CreateDocumentQuery<Family>(
                    UriFactory.CreateDocumentCollectionUri("FamilyDB", "FamilyCollection"), queryOptions)
                    .Where(f => f.LastName == "Andersen");

             

            //// Now execute the same query via direct SQL
            //IQueryable<Family> familyQueryInSql = this.client.CreateDocumentQuery<Family>(
            //        UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
            //        "SELECT * FROM Family WHERE Family.LastName = 'Andersen'",
            //        queryOptions);

            return familyQuery;
        }

        public Task DeleteDocument(string databaseName, string collectionName, string documentName)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }
    }
}
