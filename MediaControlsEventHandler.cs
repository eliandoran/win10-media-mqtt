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
            mediaControls.PlaybackStatusChanged += MediaControls_PlaybackStatusChanged;
        }        

        private void MediaControls_MediaInfoChanged(object sender, MediaControls.MediaInfoChangedEventArgs e)
        {
            var mediaInfo = e.MediaInfo;
            mqttClient.Publish("title", mediaInfo.Title);
            mqttClient.Publish("artist", mediaInfo.Artist);
        }

        private void MediaControls_PlaybackStatusChanged(object sender, MediaControls.PlaybackStatusEventArgs e)
        {
            var playbackStatus = e.PlaybackStatus;
            mqttClient.Publish("playing", SerializeBoolean(playbackStatus.IsPlaying));
        }

        private string SerializeBoolean(bool boolValue)
        {
            return boolValue.ToString().ToLower();
        }

    }
}
