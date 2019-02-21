using Bloop.Plugin;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Controls;

namespace Bloop.TeamViewer
{
    public class TeamViewerPlugin : IPlugin, ISettingProvider
    {
        private static readonly JsonSerializer Serializer = new JsonSerializer();

        private PluginInitContext _context;
        private List<Device> _devices;

        public Control CreateSettingPanel()
        {
            return new TeamViewerPluginSettings();
        }

        public void Init(PluginInitContext context)
        {
            _context = context;
            _devices = new List<Device>();
            PluginSettings.LoadSettings(Path.Combine(context.CurrentPluginMetadata.PluginDirectory, "settings.json"));
            PluginSettings.Instance.SettingsChanged = Refresh;

            Refresh();
        }

        public List<Result> Query(Query query)
        {
            if(!_devices.Any())
            {
                return OpenSettingsResult();
            }

            return _devices.Where(d => d.Alias.ToLower().Contains(query.Search.ToLower())).Select(d =>
                new Result
                {
                    Action = ctx => 
                    {
                        var id = d.RemoteControlId.TrimStart('r');
                        System.Diagnostics.Process.Start($"teamviewer10://control?device={id}&authori-zation=password");
                        return true;
                    },
                    Title = d.Alias,
                    SubTitle = d.Description,
                    IcoPath = "images\\icon.png"
                }).ToList();
        }

        private async void Refresh()
        {
            var token = PluginSettings.Instance.Token;
            if(string.IsNullOrEmpty(token))
            {
                return;
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var resp = await client.GetAsync("https://webapi.teamviewer.com/api/v1/devices");
                var content = await resp.Content.ReadAsStringAsync();
                _devices = JsonConvert.DeserializeObject<DevicesResponse>(content).Devices;
            }              
        }

        private List<Result> OpenSettingsResult()
        {
            return new List<Result> { new Result
                {
                    Action = ctx => { _context.API.OpenSettingDialog("plugins"); return true; },
                    Title = "Open Settings",
                    SubTitle = "Configure TeamViewer Plugin",
                    IcoPath = "images\\icon.png"
                }}.ToList();
        }
    }
}
