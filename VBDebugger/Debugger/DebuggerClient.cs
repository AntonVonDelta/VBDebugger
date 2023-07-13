using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using NetModels;
using Google.FlatBuffers;

namespace VBDebugger.Debugger
{
    class DebuggerClient : IDisposable
    {
        private NetworkStream _stream;
        private bool disposedValue;
        private readonly TcpClient _client = new TcpClient();
        private readonly IPEndPoint _address;
        private readonly Action<string> _logger;


        private bool Attached => _client.Connected;


        public DebuggerClient(IPEndPoint address, Action<string> logger)
        {
            _logger = logger;
        }


        public async Task<bool> Attach()
        {
            try
            {
                await _client.ConnectAsync(_address.Address, _address.Port);
                _stream = _client.GetStream();

                EnableKeepAlive();

                await sendPacketModel(new DebuggerInfoT() { Name = "Testing" });
                await readPacketModel<DebuggerAttachedT>();

                return true;
            }
            catch (Exception ex)
            {
                _logger(ex.ToString());
            }

            return false;
        }

        private async Task<T> readPacketModel<T>() where T : class
        {
            byte[] packetData;
            T packet;

            try
            {
                packetData = await readPacket();
            }
            catch (Exception ex)
            {
                _logger($"Could not read packet for type {typeof(T)}, {ex}");
                return null;
            }

            packet = DeserializeModel<T>(packetData);

            if (packet == null)
            {
                _logger($"Received packet malformed, size {packetData.Length}");

                _client.Close();
            }

            return packet;
        }
        private async Task<byte[]> readPacket()
        {
            byte[] packetSizeHeader = await readAllAsync(4);
            int packetSize = BitConverter.ToInt32(packetSizeHeader, 0);
            return await readAllAsync(packetSize);
        }
        private async Task<byte[]> readAllAsync(int len)
        {
            byte[] data = new byte[len];
            int total_sent_bytes = 0;

            do
            {
                int sent_bytes = await _stream.ReadAsync(data, total_sent_bytes, len - total_sent_bytes);

                total_sent_bytes += sent_bytes;
            } while (total_sent_bytes != len);

            return data;
        }
        private T DeserializeModel<T>(byte[] packetData) where T : class
        {
            Verifier _verifier = new Verifier(new ByteBuffer(packetData), new Options());

            if (typeof(T) == typeof(DebuggerAttachedT))
            {
                if (!DebuggerAttachedVerify.Verify(_verifier, 0)) return null;
                return (T)(object)DebuggerAttachedT.DeserializeFromBinary(packetData);
            }
            //else if (typeof(T) == typeof(DebuggerAttachedT))
            //{
            //    if (!DebuggerAttachedVerify.Verify(_verifier, 0)) return null;
            //    return (T)(object)DebuggerAttachedT.DeserializeFromBinary(packetData);
            //}

            return null;
        }

        private async Task<bool> sendPacketModel(object model)
        {
            byte[] data;
            int packetSize;

            if (!SerializeModel(model, out data))
            {
                _logger($"No packet handler registered for given packet type {model}");
                _client.Close();
                return false;
            }

            packetSize = data.Length;

            try
            {
                await _stream.WriteAsync(BitConverter.GetBytes(packetSize), 0, 4);
                await _stream.WriteAsync(data, 0, packetSize);

                return true;
            }
            catch (Exception ex)
            {
                _logger($"Could not send packet: {model}, {ex}");
            }

            return false;
        }
        private bool SerializeModel(object model, out byte[] data)
        {
            data = null;

            if (model is DebuggerInfoT castedModel1) data = castedModel1.SerializeToBinary();
            else if (model is DebuggerInfoT castedModel2) data = castedModel2.SerializeToBinary();
            else return false;

            return true;
        }

        private void EnableKeepAlive()
        {
            var data = new byte[12];
            
            Array.Copy(BitConverter.GetBytes(1), 0, data, 0, 4);
            Array.Copy(BitConverter.GetBytes(5000), 0, data, 4, 4);
            Array.Copy(BitConverter.GetBytes(5000), 0, data, 8, 4);

            _client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            _client.Client.IOControl(IOControlCode.KeepAliveValues, data, new byte[0] { });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    _client.Dispose();

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
