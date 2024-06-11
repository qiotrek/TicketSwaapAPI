using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSwaapAPI.StoreModels;

namespace TicketSwaapAPI.Services.Infrastructure
{
    public class FireStoreService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<FireStoreService> _logger;
        private readonly FirestoreClient client;
        private readonly FirestoreDb db;
        private readonly string dbName;

        public FireStoreService(IConfiguration config, ILogger<FireStoreService> logger, FirestoreTableNamesConfig tableNamesConfig)
        {
            _config = config;
            _logger = logger;
            dbName = _config["FireStore_GCPDatabaseName"];
            client = GetFirestoreClient();
            db = GetFirestoreDb();
        }

        private FirestoreDb GetFirestoreDb()
        {
            return FirestoreDb.Create(dbName, client);
        }
        private FirestoreClient GetFirestoreClient()
        {
            //var config = Path.Combine(filePath, "bq-secrets.json");
            //using (var jsonStream = new FileStream(config, FileMode.Open, FileAccess.Read, FileShare.Read))
            //   credential = GoogleCredential.FromStream(jsonStream);

            var gCPCredentialsDict = _config.GetSection("FireStore_GCPCredentials")?.GetChildren()?.ToDictionary(x => x.Key, x => x.Value);
            if (gCPCredentialsDict == null || gCPCredentialsDict.Count == 0)
            {
                return FirestoreClient.Create();
            }
            else
            {
                var gCPCredentialsDictJson = System.Text.Json.JsonSerializer.Serialize(gCPCredentialsDict);
                // GoogleCredential credential = GoogleCredential.FromJson(gCPCredentialsDictJson);
                var clientBuilder = new FirestoreClientBuilder();
                clientBuilder.JsonCredentials = gCPCredentialsDictJson;
                return clientBuilder.Build();
            }
        }


        public async Task<Timestamp> Set<T>(string collectionName, string documentId, T documentObject)
        {
            CollectionReference collection = db.Collection(collectionName);
            try
            {
                var documentReference = collection.Document(documentId);
                var result = await documentReference.SetAsync(documentObject, SetOptions.MergeAll);
                //if (documentReference != null)
                //{ 
                //}
                return result.UpdateTime;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Set Error");
                throw;
            }
        }
        public async Task<Timestamp> Delete(string collectionName, string documentId)
        {
            CollectionReference collection = db.Collection(collectionName);
            try
            {
                var result = await collection.Document(documentId).DeleteAsync();
                return result.UpdateTime;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete Error");
                throw;
            }
        }
        public async Task<Timestamp> DeleteCollection(string collectionName)
        {
            CollectionReference collection = db.Collection(collectionName);

            try
            {
                QuerySnapshot querySnapshot = await collection.GetSnapshotAsync();
                foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
                {
                    await documentSnapshot.Reference.DeleteAsync();
                }
                return Timestamp.FromDateTime(DateTime.UtcNow);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteCollection Error");
                throw;
            }
        }

        public async Task<Timestamp> Merge<T>(string collectionName, string documentId, IDictionary<string, object> update)
        {
            CollectionReference collection = db.Collection(collectionName);
            try
            {
                var result = await collection.Document(documentId).SetAsync(update, SetOptions.MergeAll);
                return result.UpdateTime;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Merge Error");
                throw;
            }
        }

        public async Task<Timestamp> RemoveField<T>(string collectionName, string documentId, string propertyId)
        {
            CollectionReference collection = db.Collection(collectionName);
            try
            {
                var update = new Dictionary<FieldPath, object>() { { new FieldPath(propertyId), FieldValue.Delete } };
                var result = await collection.Document(documentId).UpdateAsync(update);
                return result.UpdateTime;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RemoveField Error");
                throw;
            }
        }

        internal async Task<T> Get<T>(string collectionName, string documentId)
        {
            try
            {
                CollectionReference collection = db.Collection(collectionName);
                var snapshot = await collection.Document(documentId).GetSnapshotAsync();
                if (snapshot.Exists)
                {
                    T obj = snapshot.ConvertTo<T>();
                    return obj;
                }
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get Error");
                throw;
            }
        }


        internal async Task<List<T>> GetList<T>(string collectionName, List<FilterLine> filters = null)
        {
            try
            {
                CollectionReference collection = db.Collection(collectionName);

                Query capitalQuery = db.Collection(collectionName);
                List<T> list = new List<T> { };

                if (filters != null)
                {
                    foreach (var filter in filters)
                    {
                        FieldPath filterColumnName = FieldPath.DocumentId;
                        if (filter.ColumnName != "FieldPath.DocumentId")
                        {
                            filterColumnName = new FieldPath(filter.ColumnName.Split("."));
                        }
                        else
                        {
                            DocumentReference docRef = collection.Document(filter.Value[0].ToString());
                            filter.Value[0] = docRef;

                        }


                        capitalQuery = filter.FilterType switch
                        {
                            FireStoreQueryFilterType.WhereEqualTo => capitalQuery.WhereEqualTo(filterColumnName, filter.Value[0]),
                            FireStoreQueryFilterType.WhereGreaterThanOrEqualTo => capitalQuery.WhereGreaterThanOrEqualTo(filterColumnName, filter.Value[0]),
                            FireStoreQueryFilterType.WhereLessThanOrEqualTo => capitalQuery.WhereLessThanOrEqualTo(filterColumnName, filter.Value[0]),
                            FireStoreQueryFilterType.WhereLessThan => capitalQuery.WhereLessThan(filterColumnName, filter.Value[0]),
                            FireStoreQueryFilterType.WhereGreaterThan => capitalQuery.WhereGreaterThan(filterColumnName, filter.Value[0]),
                            FireStoreQueryFilterType.WhereNotEqualTo => capitalQuery.WhereNotEqualTo(filterColumnName, filter.Value[0]),
                            FireStoreQueryFilterType.WhereIn => capitalQuery.WhereIn(filterColumnName, filter.Value),
                            FireStoreQueryFilterType.WhereNotIn => capitalQuery.WhereNotIn(filterColumnName, filter.Value),
                            FireStoreQueryFilterType.WhereArrayContains => capitalQuery.WhereArrayContains(filterColumnName, filter.Value[0]),
                            FireStoreQueryFilterType.WhereArrayContainsAny => capitalQuery.WhereArrayContainsAny(filterColumnName, filter.Value),
                            _ => throw new ArgumentOutOfRangeException()
                        };

                        /*switch (filter.FilterType)
                        {
                            case FireStoreQueryFilterType.WhereEqualTo:
                                capitalQuery = capitalQuery.WhereEqualTo(filter.ColumnName, filter.Value[0]);
                                break;
                            case FireStoreQueryFilterType.WhereGreaterThanOrEqualTo:
                                capitalQuery = capitalQuery.WhereGreaterThanOrEqualTo(filter.ColumnName, filter.Value[0]);
                                break;
                            case FireStoreQueryFilterType.WhereLessThanOrEqualTo:
                                capitalQuery = capitalQuery.WhereLessThanOrEqualTo(filter.ColumnName, filter.Value[0]);
                                break;
                            case FireStoreQueryFilterType.WhereLessThan:
                                capitalQuery = capitalQuery.WhereLessThan(filter.ColumnName, filter.Value[0]);
                                break;
                            case FireStoreQueryFilterType.WhereGreaterThan:
                                capitalQuery = capitalQuery.WhereGreaterThan(filter.ColumnName, filter.Value[0]);
                                break;
                            case FireStoreQueryFilterType.WhereNotEqualTo:
                                capitalQuery = capitalQuery.WhereNotEqualTo(filter.ColumnName, filter.Value[0]);
                                break;
                            case FireStoreQueryFilterType.WhereIn:
                                capitalQuery = capitalQuery.WhereIn(filter.ColumnName, filter.Value);
                                break;
                            case FireStoreQueryFilterType.WhereNotIn:
                                capitalQuery = capitalQuery.WhereNotIn(filter.ColumnName, filter.Value);
                                break;
                            case FireStoreQueryFilterType.WhereArrayContains:
                                capitalQuery = capitalQuery.WhereArrayContains(filter.ColumnName, filter.Value[0]);
                                break;
                            case FireStoreQueryFilterType.WhereArrayContainsAny:
                                capitalQuery = capitalQuery.WhereArrayContainsAny(filter.ColumnName, filter.Value);
                                break;
                            default:
                                break;
                        }*/
                    }
                }
                QuerySnapshot snapshot = await capitalQuery.GetSnapshotAsync();
                foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
                {
                    T obj = documentSnapshot.ConvertTo<T>();
                    list.Add(obj);
                    _logger.LogInformation($"GetList: {documentSnapshot.Id}");

                }
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetList Error");
                throw;
            }
        }


        internal enum FireStoreQueryFilterType
        {
            WhereEqualTo,
            WhereNotEqualTo,
            WhereGreaterThanOrEqualTo,
            WhereGreaterThan,
            WhereLessThanOrEqualTo,
            WhereLessThan,
            WhereIn,
            WhereNotIn,
            WhereArrayContains,
            WhereArrayContainsAny
        }
        internal class FilterLine
        {
            public FilterLine(string ColumnName, List<object> Value, FireStoreQueryFilterType FilterType)
            {
                this.ColumnName = ColumnName;
                this.Value = Value;
                this.FilterType = FilterType;
            }
            public string ColumnName { get; private set; }
            public List<object> Value { get; private set; }
            public FireStoreQueryFilterType FilterType { get; private set; }
        }

    }
}
