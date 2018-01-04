using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Xml.Linq;
using CoreServiceUtils.Interfaces;
using CoreServiceUtils.Models;
using CoreServiceUtils.Tridion;
using CoreServiceUtils.Tridion.Services;
using Tridion.ContentManager.CoreService.Client;

namespace CoreServiceUtils.Services
{
    public class TridionClientService : BaseService, ITridionClientService
    {
        private readonly ServerConfiguration _settings;
        private ITridionCoreServiceClient _client;


        private static readonly XNamespace Rdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
        private static readonly XNamespace Rdfs = "http://www.w3.org/2000/01/rdf-schema#";
        private static readonly XNamespace Tcmt = "http://www.tridion.com/ContentManager/5.2/Taxonomies#";
        private static readonly XNamespace Owl = "http://www.w3.org/2002/07/owl#";

        public TridionClientService(ServerConfiguration settings)
            : base(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType?.ToString())
        {
            _settings = settings;
        }

        public void Open()
        {
            string endPoint = _settings.Server;

            try
            {
                
                if (_settings.Type == ConnectionType.NetTcp)
                {
                    _client = new TridionSessionAwareCoreServiceClient();
                        
                }
                else
                {
                    endPoint = (_settings.Type == ConnectionType.BasicHttps) ? $"https://{_settings.Server}" : $"http://{_settings.Server}";
                        
                    _client = new TridionCoreServiceClient();
                }

                System.Net.NetworkCredential credential = null;

                if (!string.IsNullOrEmpty(_settings.Username))
                {
                    credential =
                        new System.Net.NetworkCredential(_settings.Username, _settings.Password);
                }

                _client.Open(endPoint, credential);
                
            }
            catch (EndpointNotFoundException e)
            {
                Log.Error("CreateCoreService", e);
                throw;
            }
            catch (Exception e)
            {
                Log.Error("CreateCoreService", e);
                throw;
            }
            
        }

        public void Close()
        {
            _client?.Close();
            _client = null;
        }

        public void Abort()
        {
            _client?.Abort();
            _client = null;
        }

        public T Get<T>(string id, ReadOptions readOptions = null) where T : class
        {
            if (_client == null)
            {
                return default(T);
            }

            object obj = null;

            try
            {
                if (readOptions == null) readOptions = new ReadOptions { LoadFlags = LoadFlags.Expanded };
                obj = _client.Read(id, readOptions);
            }
            catch (Exception e)
            {
                Log.Error("Get<" + typeof(T).Name + ">", e);
                throw;
            }


            return obj as T;
        }

        public List<T> GetList<T>(string id, SubjectRelatedListFilterData filterData = null) where T : class
        {
            if (_client == null)
            {
                return default(List<T>);
            }

            object[] obj = null;
            List<T> list = new List<T>();


            try
            {
                if (filterData == null) filterData = new SubjectRelatedListFilterData { };

                if (typeof(T) == typeof(XmlListItemData))
                {
                    XElement element = _client.GetListXml(id, filterData);

                    list = XmlListItemData.GetListOf(element) as List<T>;
                }
                else
                {
                    obj = _client.GetList(id, filterData);

                    if (obj != null && obj.Length > 0)
                    {
                        list = obj.OfType<T>().ToList();
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("GetList<" + typeof(T).Name + ">", e);
                throw;
            }

            return list;
        }


        public SchemaFieldsData ReadSchemaFields(string schemaId, bool expandEmbeddedFields = true, ReadOptions readOptions = null)
        {
            if (_client == null)
            {
                return default(SchemaFieldsData);
            }

            SchemaFieldsData data = null;


            try
            {
                if (readOptions == null) readOptions = new ReadOptions { LoadFlags = LoadFlags.Expanded };
                data = _client.ReadSchemaFields(schemaId, expandEmbeddedFields, readOptions);
            }
            catch (Exception e)
            {
                Log.Error("ReadSchemaFields", e);
                throw;
            }


            return data;
        }

        public T GetDefaultData<T>(ItemType itemType, string containerId, ReadOptions readOptions = null)
             where T : class
        {

            if (_client == null)
            {
                return default(T);
            }

            object obj = null;


            try
            {
                if (readOptions == null) readOptions = new ReadOptions { LoadFlags = LoadFlags.Expanded };
                obj = _client.GetDefaultData(itemType, containerId, readOptions);
            }
            catch (Exception e)
            {
                Log.Error("GetDefaultData<" + typeof(T).Name + ">", e);
                throw;
            }


            return obj as T;
        }

        public T Save<T>(T deltaData, ReadOptions readBackOptions = null)
            where T : IdentifiableObjectData
        {
            if (_client == null)
            {
                return default(T);
            }


            object obj = null;


            try
            {
                if (readBackOptions == null) readBackOptions = new ReadOptions { LoadFlags = LoadFlags.Expanded };
                obj = _client.Save(deltaData, readBackOptions);
            }
            catch (Exception e)
            {
                Log.Error("Save<" + typeof(T).Name + ">", e);
                throw;
            }


            return obj as T;
        }

        public T CheckIn<T>(string id, ReadOptions readBackOptions) where T : VersionedItemData
        {
            if (_client == null)
            {
                return default(T);
            }

            object obj = null;


            try
            {
                if (readBackOptions == null) readBackOptions = new ReadOptions { LoadFlags = LoadFlags.Expanded };
                obj = _client.CheckIn(id, readBackOptions);
            }
            catch (Exception e)
            {
                Log.Error("CheckIn<" + typeof(T).Name + ">", e);
                throw;
            }


            return obj as T;
        }

        public T CheckOut<T>(string id, bool permanentLock, ReadOptions readBackOptions) where T : VersionedItemData
        {
            if (_client == null)
            {
                return default(T);
            }

            object obj = null;

            try
            {
                if (readBackOptions == null) readBackOptions = new ReadOptions { LoadFlags = LoadFlags.Expanded };
                obj = _client.CheckOut(id, permanentLock, readBackOptions);
            }
            catch (Exception e)
            {
                Log.Error("CheckOut<" + typeof(T).Name + ">", e);
                throw;
            }


            return obj as T;
        }

        public bool IsExistingObject(string id)
        {
            if (_client == null)
            {
                return default(bool);
            }

            bool exists = false;


            try
            {
                exists = _client.IsExistingObject(id);
            }
            catch (Exception e)
            {
                Log.Error("IsExistingObject", e);
                throw;
            }


            return exists;
        }
        private static string GetKeywordTitle(XElement n)
        {
            return n.Parent?.Descendants(Rdfs + "label").First()?.Value;
        }

        private static string GetKeywordKey(XElement n)
        {
            return n.Parent?.Descendants(Tcmt + "key").First()?.Value;
        }

        private static string GetKeywordId(XElement n)
        {
            return n.Parent?.Attribute(Rdf + "about")?.Value;
        }

        public List<BaseKeyword> FindKeywordFromTitle(string identifier, string categoryId)
        {
            var result = new List<BaseKeyword>();

            try
            {
                XElement doc = GetCategoryTaxonomy(categoryId);

                var results = doc
                    .Descendants(Rdfs + "label")
                    .Where(n => n.Value.ToLower().Contains(identifier.ToLower()))
                    .ToList();

                if (results.Any())
                {
                    result = results.Select(n => new BaseKeyword()
                    {
                        Id = GetKeywordId(n),
                        Key = GetKeywordKey(n),
                        Title = n.Value
                    }).ToList();

                }

            }
            catch (Exception e)
            {
                Log.Error("FindKeywordFromTitle", e);
            }

            return result;
        }


        public List<BaseKeyword> FindKeywordFromKey(string key, string categoryId)
        {
            var result = new List<BaseKeyword>();

            try
            {
                XElement doc = GetCategoryTaxonomy(categoryId);

                var results = doc
                            .Descendants(Tcmt + "key")
                            .Where(n => n.Value.Equals(key))
                            .ToList();


                if (results.Any())
                {
                    result = results.Select(n => new BaseKeyword()
                    {
                        Id = GetKeywordId(n),
                        Key = n.Value,
                        Title = GetKeywordTitle(n)
                    }).ToList();

                }

            }
            catch (Exception e)
            {
                Log.Error("FindKeywordFromKey", e);
            }

            return result;
        }

        public void Delete(string id)
        {
            _client.Delete(id);
        }


        public XElement GetCategoryTaxonomy(string categoryId)
        {
            if (_client == null)
            {
                return default(XElement);
            }

            XElement tree = null;


            try
            {
                TaxonomiesOwlFilterData filter = new TaxonomiesOwlFilterData();
                filter.RootCategories = new[] { new LinkToCategoryData { IdRef = categoryId }, };
                filter.BaseColumns = ListBaseColumns.Extended;

                string publicationId = GetPublicationTcmId(categoryId);

                if (!String.IsNullOrEmpty(publicationId))
                {
                    tree = _client.GetListXml(publicationId, filter);
                }
            }
            catch (Exception e)
            {
                Log.Error("GetCategoryTree", e);
                throw;
            }


            return tree;
        }

        private string GetPublicationTcmId(string tcmId)
        {
            string result = String.Empty;

            string format = "tcm:0-{0}-1";

            string process = tcmId.Replace("tcm:", "");

            string[] parts = process.Split('-');

            if (parts.Length > 0)
            {
                result = String.Format(format, parts.First<string>());
            }

            return result;

        }



    }
}