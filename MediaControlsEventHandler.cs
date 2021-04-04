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

    }
}
