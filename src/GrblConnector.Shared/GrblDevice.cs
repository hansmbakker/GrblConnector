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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace GrblConnector
{
    /// <summary>
    /// A generic abstract Grbl device
    /// </summary>
    public abstract class GrblDevice : IDisposable
	{
		private object m_lockObject = new object();
		private string m_message = "";
		private Stream m_stream;
		System.Threading.CancellationTokenSource m_cts;
		TaskCompletionSource<bool> closeTask;

		/// <summary>
		/// Initializes a new instance of the <see cref="GrblDevice"/> class.
		/// </summary>
		protected GrblDevice()
		{
		}
		/// <summary>
		/// Opens the device connection.
		/// </summary>
		/// <returns></returns>
		public async Task OpenAsync()
		{
			lock (m_lockObject)
			{
				if (IsOpen) return;
				IsOpen = true;
			}
			m_cts = new System.Threading.CancellationTokenSource();
			m_stream = await OpenStreamAsync();
			StartParser();
			MultiPartMessageCache.Clear();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "_")]
		private void StartParser()
		{
			var token = m_cts.Token;
			System.Diagnostics.Debug.WriteLine("Starting parser...");
			var _ = Task.Run(async () =>
			{
				var stream = m_stream;
				byte[] buffer = new byte[1024];
				while (!token.IsCancellationRequested)
				{
					int readCount = 0;
					try
					{
						readCount = await stream.ReadAsync(buffer, 0, 1024, token).ConfigureAwait(false);
					}
					catch { }
					if (token.IsCancellationRequested)
						break;
					if (readCount > 0)
					{
						OnData(buffer.Take(readCount).ToArray());
					}
					await Task.Delay(50);
				}
				if (closeTask != null)
					closeTask.SetResult(true);
			});
		}

		/// <summary>
		/// Creates the stream the GrblDevice is working on top off.
		/// </summary>
		/// <returns></returns>
		protected abstract Task<Stream> OpenStreamAsync();
		/// <summary>
		/// Closes the device.
		/// </summary>
		/// <returns></returns>
		public async Task CloseAsync()
		{
			if (m_cts != null)
			{
				closeTask = new TaskCompletionSource<bool>();
				if (m_cts != null)
					m_cts.Cancel();
				m_cts = null;
			}
			await closeTask.Task;
			await CloseStreamAsync(m_stream);
			MultiPartMessageCache.Clear();
			m_stream = null;
			lock (m_lockObject)
				IsOpen = false;
		}
		/// <summary>
		/// Closes the stream the GrblDevice is working on top off.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <returns></returns>
		protected abstract Task CloseStreamAsync(Stream stream);

		private void OnData(byte[] data)
		{
			var grbl = System.Text.Encoding.UTF8.GetString(data, 0, data.Length);
			List<string> lines = new List<string>();
			lock (m_lockObject)
			{
				m_message += grbl;

				var lineEnd = m_message.IndexOf("\n", StringComparison.Ordinal);
				while (lineEnd > -1)
				{
					string line = m_message.Substring(0, lineEnd).Trim();
					m_message = m_message.Substring(lineEnd + 1);
					if (!string.IsNullOrEmpty(line))
						lines.Add(line);
					lineEnd = m_message.IndexOf("\n", StringComparison.Ordinal);
				}
			}
			foreach(var line in lines)
				ProcessMessage(line);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification="Must silently handle invalid/corrupt input")]
		private void ProcessMessage(string p)
		{
			try
			{
				var msg = GrblConnector.Grbl.GrblMessage.Parse(p);
				if (msg != null)
					OnMessageReceived(msg);
			}
			catch { }
		}

		private void OnMessageReceived(Grbl.GrblMessage msg)
		{
			var args = new GrblMessageReceivedEventArgs(msg);

            MessageReceived?.Invoke(this, args);
        }

		private Dictionary<string, Dictionary<int, Grbl.GrblMessage>> MultiPartMessageCache
			= new Dictionary<string,Dictionary<int, Grbl.GrblMessage>>();

		/// <summary>
		/// Occurs when a Grbl message is received.
		/// </summary>
		public event EventHandler<GrblMessageReceivedEventArgs> MessageReceived;

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (m_stream != null)
			{
				if (m_cts != null)
				{
					m_cts.Cancel();
					m_cts = null;
				}
				CloseStreamAsync(m_stream);
				if (disposing && m_stream != null)
					m_stream.Dispose();
				m_stream = null;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this device is open.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is open; otherwise, <c>false</c>.
		/// </value>
		public bool IsOpen { get; private set; }
	}

	/// <summary>
	/// Event argument for the <see cref="GrblDevice.MessageReceived" />
	/// </summary>
	public sealed class GrblMessageReceivedEventArgs : EventArgs
	{
		internal GrblMessageReceivedEventArgs(Grbl.GrblMessage message) {
			Message = message;
		}
		/// <summary>
		/// Gets the Grbl message.
		/// </summary>
		/// <value>
		/// The Grbl message.
		/// </value>
		public Grbl.GrblMessage Message { get; private set; }		
	}
}
