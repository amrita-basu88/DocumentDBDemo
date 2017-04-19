using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using System.Net;
using DocumentDBCollections.Models;

namespace DocumentDBCollections
{
    class Program
    { 

        static void Main(string[] args)
        {
            try
            {
                //Initialization
                DocumentDB documentDB = new DocumentDB
                {
                    AuthorizationKey = "h6H3BgbraykUTr2yP3UEzgqXRqkaiixaliJI9UFgGEOU5JazdciFehayZgrJERwXQHwHOknVaH6FGygXBNVCeA==",
                    EndpointUrl = "https://abasu1.documents.azure.com:443/"
                };
                DocumentClient client = new DocumentClient(new Uri(documentDB.EndpointUrl), documentDB.AuthorizationKey);
                DocumentDBOperations dbOperations = new DocumentDBOperations(documentDB.EndpointUrl, documentDB.AuthorizationKey, client);
                Program p = new Program();
                //p.CreateDB(dbOperations).Wait();
                p.CreateCollections(dbOperations).Wait();
                p.CreateDocuments(dbOperations).Wait();
                
                
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }
            finally
            {
                Console.WriteLine("End of demo, press any key to exit.");
                Console.ReadKey();
            }
        }
        
        private async Task CreateDB(DocumentDBOperations dbOperations)
        { 
            //Create DB
            await dbOperations.CreateDocumentDB("FamilyDB");
        }

        private async Task CreateCollections(DocumentDBOperations dbOperations)
        {
            //Create Collections
            await dbOperations.CreateDocumentCollections("FamilyDB", "FamilyCollection");
        }

        private async Task CreateDocuments(DocumentDBOperations dbOperations)
        {
            //Create Collections
            Family andersenFamily = new Family
            {
                Id = "Andersen.1",
                LastName = "Andersen",
                Parents = new Parent[]
                    {
                new Parent { FirstName = "Thomas" },
                new Parent { FirstName = "Mary Kay" }
                    },
                Children = new Child[]
                    {
                new Child
                {
                        FirstName = "Henriette Thaulow",
                        Gender = "female",
                        Grade = 5,
                        Pets = new Pet[]
                        {
                                new Pet { GivenName = "Fluffy" }
                        }
                }
                    },
                Address = new Address { State = "WA", County = "King", City = "Seattle" },
                IsRegistered = true
            };

            await dbOperations.InsertDocuments(andersenFamily);

            Family wakefieldFamily = new Family
            {
                Id = "Wakefield.7",
                LastName = "Wakefield",
                Parents = new Parent[]
                    {
                new Parent { FamilyName = "Wakefield", FirstName = "Robin" },
                new Parent { FamilyName = "Miller", FirstName = "Ben" }
                    },
                Children = new Child[]
                    {
                new Child
                {
                        FamilyName = "Merriam",
                        FirstName = "Jesse",
                        Gender = "female",
                        Grade = 8,
                        Pets = new Pet[]
                        {
                                new Pet { GivenName = "Goofy" },
                                new Pet { GivenName = "Shadow" }
                        }
                },
                new Child
                {
                        FamilyName = "Miller",
                        FirstName = "Lisa",
                        Gender = "female",
                        Grade = 1
                }
                    },
                Address = new Address { State = "NY", County = "Manhattan", City = "NY" },
                IsRegistered = false
            };
            await dbOperations.InsertDocuments(wakefieldFamily);
        } 

        private async Task ExceuteQuery(DocumentDBOperations dbOperations)
        {
            IQueryable query = dbOperations.ExecuteSimpleQuery(); 
            foreach (Family family in query)
            {
                Console.WriteLine("\tRead {0}", family);
            }
        }
    }
}
