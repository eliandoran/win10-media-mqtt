using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpCompanion
{
    class MediaControlsEventHandler
    {
        private MediaControls mediaControls;
        private MqttClient mqttClient;

        public MediaControlsEventHandler(MediaControls mediaControls, MqttClient mqttClient)
        {                        
            this.mediaControls = mediaControls;
            this.mqttClient = mqttClient;
        }

        public void Initialize()
        {
            mediaControls.MediaInfoChanged += MediaControls_MediaInfoChanged;
        }

        private async void MediaControls_MediaInfoChanged(object sender, MediaControls.MediaInfoChangedEventArgs e)
        {
            var mediaInfo = e.mediaInfo;
            var serializedData = JsonConvert.SerializeObject(mediaInfo);
            Console.WriteLine(serializedData);
            await mqttClient.Publish("mediainfo", serializedData);
        }
    }
}
