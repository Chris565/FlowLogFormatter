using FlowLogFormatter;
using FlowLogFormatter.Configuration;
using Newtonsoft.Json.Linq;

var settingsFile = JObject.Parse(File.ReadAllText("Configuration\\app-settings.json"));
var appSettings = settingsFile.ToObject<AppSettings>();

var userApp = new UserApp(appSettings);
userApp.Run();