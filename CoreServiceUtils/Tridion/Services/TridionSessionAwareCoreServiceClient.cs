using System.Net;
using System.ServiceModel;
using System.Xml;
using System.Xml.Linq;
using Tridion.ContentManager.CoreService.Client;

namespace CoreServiceUtils.Tridion.Services
{
    public class TridionSessionAwareCoreServiceClient : ITridionCoreServiceClient
    {

        ISessionAwareCoreService _client;
        private ChannelFactory<ISessionAwareCoreService> _factory;


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
                            new EndpointAddress($"net.tcp://{endPoint}:2660/CoreService/2013/netTcp" ??
                                                "CoreService");
            var netTcpBinding = new NetTcpBinding
            {
                Name = "netTcp_2013",
                TransactionFlow = true,
                TransactionProtocol = TransactionProtocol.OleTransactions,
                MaxReceivedMessageSize = 10485760,
                ReaderQuotas = new XmlDictionaryReaderQuotas
                {
                    MaxStringContentLength = 10485760,
                    MaxArrayLength = 10485760
                }
            };

            //_client = new SessionAwareCoreServiceClient(netTcpBinding, endpointAddress);

            //if (credentials != null)
            //{
            //    _client.ChannelFactory.Credentials.Windows.ClientCredential = credentials;
            //}

            ChannelFactory<ISessionAwareCoreService> factory = new ChannelFactory<ISessionAwareCoreService>(netTcpBinding, endpointAddress);

            factory.Credentials.Windows.ClientCredential = credentials; //new System.Net.NetworkCredential(username, password);

            _client = factory.CreateChannel();
            _factory = factory;
        }
        
        //public void Open(string endPoint, string endPointAddress, NetworkCredential credentials)
        //{
        //    _client = new SessionAwareCoreServiceClient(endPoint, endPointAddress);
        //    if (credentials != null)
        //    {
        //        _client.ChannelFactory.Credentials.Windows.ClientCredential = credentials;
        //    }
        //}
    }
}