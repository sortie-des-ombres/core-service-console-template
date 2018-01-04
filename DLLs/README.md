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