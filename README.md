# Tridion Core Service Template

This is a simple console program template that allows easy access to the Tridion Core Service and will let you get down to coding with no faffing.

1.  Add your 'Tridion.ContentManager.CoreService.Client.dll' to the DLLs directory

2. Setup up your server connection configuraiton file. A sample of this file can be found in 'Samples/server-configuration.json' and has the below format.
```json
{
  "server": "{ServerUrl}",
  "type": "BasicHttps",
  "username": "{Username}",
  "password": "{Password}"
}
```
Options for the connection Type: 
* BasicHttps
* BasicHttp
* NetTcp

3. Update the Run() method in Program.cs to exceute your custom code.

4. Update 'CoreServiceUtils' project properties > Debug > Start Options to use your configuration file. e.g.

```
-c "Samples/server-configuration.json"  -p
```

Progrma usage options:
```
  -c, --configuration    Required. Path to the application configuration file.

  -p, --prompt           Prompt before exiting

  --help                 Display this help screen.
```

## Currently Exposed Functions from the Core Service

```csharp
var component = service.Get<ComponentData>("{TcmId or Webdav Path}");

var componentList = service.GetList<ComponentData>("{TcmId or Webdav Path}");

var schemaFields = service.ReadSchemaFields("{TcmId or Webdav Path}");

var componentDefaultData = service.GetDefaultData<ComponentData>(ItemType.Component, "{TcmId or Webdav Path}");

var checkedInComponent = service.CheckIn<VersionedItemData>("{TcmId or Webdav Path}");

var savedComponent = service.Save(component);

var checkedOutComponent = service.CheckOut<VersionedItemData>("{TcmId or Webdav Path}", true);

var existing = service.IsExistingObject("{TcmId or Webdav Path}");

var keywordListFromKey = service.FindKeywordFromKey("keyword-key", "{TcmId or Webdav Path}");

var keywordListFromTitle = service.FindKeywordFromTitle("Keyword Title", "{TcmId or Webdav Path}");

service.Delete("{TcmId or Webdav Path}");

```