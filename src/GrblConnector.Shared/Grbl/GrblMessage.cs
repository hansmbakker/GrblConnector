//
// Copyright (c) 2014 Morten Nielsen
//
// Licensed under the Microsoft Public License (Ms-PL) (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://opensource.org/licenses/Ms-PL.html
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using GrblConnector.Grbl.PushMessages;
using GrblConnector.Grbl.ResponseMessages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GrblConnector.Grbl
{
    /// <summary>
    /// Grbl Message base class.
    /// </summary>
    public abstract class GrblMessage
    {
        /// <summary>
        /// Parses the specified Grbl message.
        /// </summary>
        /// <param name="message">The Grbl message string.</param>
        /// <returns></returns>
        public static GrblMessage Parse(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException("message");

            GrblMessage msg = null;
            if (message == "ok")
            {
                msg = new OkMessage();
            }
            else if (message.StartsWith("error:"))
            {
                msg = new ErrorMessage();
            }
            else if (message.StartsWith("<") && message.EndsWith(">"))
            {
                msg = new StatusReportMessage();
            }
            else if (message.StartsWith("Grbl "))
            {
                msg = new WelcomeMessage();
            }
            else if (message.StartsWith("ALARM:"))
            {
                msg = new AlarmMessage();
            }
            else if (message.StartsWith("$"))
            {
                msg = new SettingsPrintoutMessage();
            }
            else if (message.StartsWith("[MSG:"))
            {
                msg = new NonQueriedFeedbackMessage();
            }
            else if (message.StartsWith("[GC:"))
            {
                msg = new GcodeParserStateMessage();
            }
            else if (message.StartsWith("[HLP:"))
            {
                msg = new HelpMessage();
            }
            else if (message.StartsWith("[G5")
                || message.StartsWith("[G28:")
                || message.StartsWith("[G30:")
                || message.StartsWith("[G92:"))
            {
                msg = new CoordinateSettingMessage();
            }
            else if (message.StartsWith("[PRB:"))
            {
                msg = new ProbeSettingMessage();
            }
            else if (message.StartsWith("[TLO:"))
            {
                msg = new ToolLengthOffsetMessage();
            }
            else if (message.StartsWith("[VER:"))
            {
                msg = new VersionMessage();
            }
            else if (message.StartsWith("[OPT:"))
            {
                msg = new CompileOptionPrintoutMessage();
            }
            else if (message.StartsWith("[echo:"))
            {
                msg = new EchoMessage();
            }
            else if (message.StartsWith(">"))
            {
                msg = new StartupLineResponseMessage();
            }
            else
            {
                msg = new UnknownMessage();
            }
            msg.MessageBody = message;
            msg.OnLoadMessage(message);
            return msg;
        }

        /// <summary>
        /// Gets the original Grbl message.
        /// </summary>
        public string MessageBody { get; private set; }

        /// <summary>
        /// Called when the message is being loaded.
        /// </summary>
        /// <param name="message">The Grbl message.</param>
        /// <remarks>
        /// Implement this method to create a custom Grbl message.
        /// </remarks>
        protected virtual void OnLoadMessage(string message) { MessageBody = message; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "${0}", MessageBody);
        }
    }
}
