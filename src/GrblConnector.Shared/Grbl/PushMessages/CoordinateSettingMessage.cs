using System;
using System.Collections.Generic;
using System.Text;

namespace GrblConnector.Grbl.PushMessages
{
    public class CoordinateSettingMessage : GrblMessage
    {
        protected override void OnLoadMessage(string message)
        {
            message = message.Trim(new char[] { '[', ']' });
            var messageParts = message.Split(':');
            SettingForCommand = messageParts[0];

            var coordinateStrings = messageParts[1].Split(',');
            X = float.Parse(coordinateStrings[0]);
            Y = float.Parse(coordinateStrings[1]);
            Z = float.Parse(coordinateStrings[2]);
        }

        public string SettingForCommand { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }
    }
}
