using System.Collections.Generic;
using System.Xml.Linq;
using CoreServiceUtils.Models;
using Tridion.ContentManager.CoreService.Client;

namespace CoreServiceUtils.Interfaces
{
    public interface ITridionClientService
    {
        T Get<T>(string id, ReadOptions readOptions = null) where T : class;

        List<T> GetList<T>(string id, SubjectRelatedListFilterData filterData = null) where T : class;

        SchemaFieldsData ReadSchemaFields(string schemaId, bool expandEmbeddedFields = true, ReadOptions readOptions = null);

        T GetDefaultData<T>(ItemType itemType, string containerId, ReadOptions readOptions = null) where T : class;

        T Save<T>(T deltaData, ReadOptions readBackOptions = null) where T : IdentifiableObjectData;

        T CheckIn<T>(string id, ReadOptions readBackOptions = null) where T : VersionedItemData;

        T CheckOut<T>(string id, bool permanentLock, ReadOptions readBackOptions = null) where T : VersionedItemData;

        bool IsExistingObject(string id);

        void Open();

        void Close();

        void Abort();

        XElement GetCategoryTaxonomy(string categoryId);

        List<BaseKeyword> FindKeywordFromTitle(string identifier, string categoryId);

        List<BaseKeyword> FindKeywordFromKey(string key, string categoryId);

        void Delete(string id);
    }
}