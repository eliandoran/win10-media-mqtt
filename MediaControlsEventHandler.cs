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
            // Subscribe to local media events.
            mediaControls.MediaInfoChanged += MediaControls_MediaInfoChanged;
            mediaControls.PlaybackStatusChanged += MediaControls_PlaybackStatusChanged;

            // Subscribe to MQTT command events.
            mqttClient.MessageReceived += MqttClient_MessageReceived;
        }        

        private void MediaControls_MediaInfoChanged(object sender, MediaControls.MediaInfoChangedEventArgs e)
        {
            var mediaInfo = e.MediaInfo;
            mqttClient.Publish("title", mediaInfo.Title);
            mqttClient.Publish("artist", mediaInfo.Artist);
            mqttClient.Publish("thumbnail", mediaInfo.Thumbnail);
        }

        private void MediaControls_PlaybackStatusChanged(object sender, MediaControls.PlaybackStatusEventArgs e)
        {
            var playbackStatus = e.PlaybackStatus;
            mqttClient.Publish("playing", SerializeBoolean(playbackStatus.IsPlaying));
        }

        private void MqttClient_MessageReceived(object sender, MqttClient.MessageReceivedEventArgs e)
        {
            var command = e.Payload;
            Console.WriteLine("Got command: " + command);

            switch (command)
            {
                case "pause":
                    mediaControls.Pause();
                    break;
                case "play":
                    mediaControls.Play();
                    break;
                case "previous":
                    mediaControls.SkipPrevious();
                    break;
                case "next":
                    mediaControls.SkipNext();
                    break;
            }
        }

        private string SerializeBoolean(bool boolValue)
        {
            return boolValue.ToString().ToLower();
        }

    }
}
