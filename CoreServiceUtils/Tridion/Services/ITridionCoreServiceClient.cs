using System.Net;
using System.Xml.Linq;
using Tridion.ContentManager.CoreService.Client;

namespace CoreServiceUtils.Tridion.Services
{
    public interface ITridionCoreServiceClient
    {
        void Open(string endPoint, NetworkCredential credentials);
        //void Open(string endPoint, string endPointAddress, NetworkCredential credentials);

        void Close();

        void Abort();

        SchemaFieldsData ReadSchemaFields(string schemaId, bool expandEmbeddedFields, ReadOptions readOptions);

        IdentifiableObjectData Read(string id, ReadOptions readOptions);

        XElement GetListXml(string id, SubjectRelatedListFilterData filter);

        IdentifiableObjectData[] GetList(string id, SubjectRelatedListFilterData filter);

        IdentifiableObjectData[] GetSearchResults(SearchQueryData filter);

        IdentifiableObjectData GetDefaultData(ItemType itemType, string containerId, ReadOptions readOptions);

        IdentifiableObjectData Save(IdentifiableObjectData deltaData, ReadOptions readBackOptions);

        VersionedItemData CheckIn(string id, ReadOptions readBackOptions);

        VersionedItemData CheckOut(string id, bool permanentLock, ReadOptions readBackOptions);

        bool IsExistingObject(string id);

        PublishTransactionData[] Publish(string[] ids, PublishInstructionData publishInstruction, string[] targets, PublishPriority? priority, ReadOptions readOptions);


        void Delete(string id);
    }
}