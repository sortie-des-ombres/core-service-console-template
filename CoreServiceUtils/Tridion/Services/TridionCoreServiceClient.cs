using System.Net;
using System.ServiceModel;
using System.Xml.Linq;
using Tridion.ContentManager.CoreService.Client;

namespace CoreServiceUtils.Tridion.Services
{
    public class TridionCoreServiceClient : ITridionCoreServiceClient
    {

        ICoreService _client;

        private ChannelFactory<ICoreService> _factory;

        public void Close()
        {
            _factory.Close();
        }

        public void Abort()
        {
            _factory.Abort();
        }

        public SchemaFieldsData ReadSchemaFields(string schemaId, bool expandEmbeddedFields, ReadOptions readOptions)
        {
            return _client.ReadSchemaFields(schemaId, expandEmbeddedFields, readOptions);
        }

        public IdentifiableObjectData Read(string id, ReadOptions readOptions)
        {
            return _client.Read(id, readOptions);
        }

        public XElement GetListXml(string id, SubjectRelatedListFilterData filter)
        {
            return _client.GetListXml(id, filter);
        }

        public IdentifiableObjectData[] GetList(string id, SubjectRelatedListFilterData filter)
        {
            return _client.GetList(id, filter);
        }

        public IdentifiableObjectData[] GetSearchResults(SearchQueryData filter)
        {
            return _client.GetSearchResults(filter);
        }

        public IdentifiableObjectData GetDefaultData(ItemType itemType, string containerId, ReadOptions readOptions)
        {
            return _client.GetDefaultData(itemType, containerId, readOptions);
        }

        public IdentifiableObjectData Save(IdentifiableObjectData deltaData, ReadOptions readBackOptions)
        {
            return _client.Save(deltaData, readBackOptions);
        }

        public VersionedItemData CheckIn(string id, ReadOptions readBackOptions)
        {
            return _client.CheckIn(id, true, null, readBackOptions);
        }

        public VersionedItemData CheckOut(string id, bool permanentLock, ReadOptions readBackOptions)
        {
            return _client.CheckOut(id, permanentLock, readBackOptions);
        }

        public bool IsExistingObject(string id)
        {
            return _client.IsExistingObject(id);
        }

        public PublishTransactionData[] Publish(string[] ids, PublishInstructionData publishInstruction, string[] targets,
            PublishPriority? priority, ReadOptions readOptions)
        {
            return _client.Publish(ids, publishInstruction, targets, priority, readOptions);
        }

        public void Delete(string id)
        {
            _client.Delete(id);
        }

        public void Open(string endPoint, NetworkCredential credentials)
        {

            EndpointAddress endpointAddress =
                            new EndpointAddress($"{endPoint}/webservices/CoreService2013.svc/basicHttp" ??
                                                "CoreService");

            if (endPoint.StartsWith("https"))
            {
                OpenHttpsConnection(credentials, endpointAddress);
            }
            else
            {
                OpenHttpConnection(credentials, endpointAddress);
            }

        }

        private void OpenHttpConnection(NetworkCredential credentials, EndpointAddress endpointAddress)
        {
            var binding = new BasicHttpBinding()
            {
                MaxBufferSize = int.MaxValue,
                MaxBufferPoolSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas()
                {
                    MaxStringContentLength = int.MaxValue,
                    MaxArrayLength = int.MaxValue,
                },
                Security = new BasicHttpSecurity()
                {
                    Mode = BasicHttpSecurityMode.TransportCredentialOnly,
                    Transport = new HttpTransportSecurity()
                    {
                        ClientCredentialType = HttpClientCredentialType.Windows,
                    }
                }
            };

            ChannelFactory<ICoreService> factory = new ChannelFactory<ICoreService>(binding, endpointAddress);

            factory.Credentials.Windows.ClientCredential = credentials;

            _client = factory.CreateChannel();
            _factory = factory;
        }

        private void OpenHttpsConnection(NetworkCredential credentials, EndpointAddress endpointAddress)
        {
            var binding = new BasicHttpsBinding()
            {
                MaxBufferSize = int.MaxValue,
                MaxBufferPoolSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas()
                {
                    MaxStringContentLength = int.MaxValue,
                    MaxArrayLength = int.MaxValue,
                },
                Security = new BasicHttpsSecurity()
                {
                    Mode = BasicHttpsSecurityMode.Transport,
                    Transport = new HttpTransportSecurity()
                    {
                        ClientCredentialType = HttpClientCredentialType.Basic
                    }
                }
            };

            ChannelFactory<ICoreService> factory = new ChannelFactory<ICoreService>(binding, endpointAddress);

            var credentialBehaviour = factory.Endpoint.Behaviors.Find<System.ServiceModel.Description.ClientCredentials>();
            credentialBehaviour.UserName.UserName = credentials.UserName;
            credentialBehaviour.UserName.Password = credentials.Password;

            _client = factory.CreateChannel();
            _factory = factory;
        }

        //public void Open(string endPoint, string endPointAddress, NetworkCredential credentials)
        //{
        //    _client = new CoreServiceClient(endPoint, endPointAddress);

        //    if (credentials != null)
        //    {
        //        _client.ChannelFactory.Credentials.Windows.ClientCredential = credentials;
        //    }
        //}
    }
}