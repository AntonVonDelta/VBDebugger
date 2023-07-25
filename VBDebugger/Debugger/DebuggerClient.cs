using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using NetModels;
using Google.FlatBuffers;

namespace VBDebugger.Debugger {
    class DebuggerClient : IDisposable {
        private NetworkStream _stream;
        private bool disposedValue;
        private readonly TcpClient _client = new TcpClient();
        private readonly IPEndPoint _address;
        private readonly Action<string> _logger;
        private StackDumpT _currentStackDump;
        private InstructionException _currentException;

        public bool Attached => _client.Connected;
        public StackDumpT CurrentStackDump => _currentStackDump;
        public InstructionException CurrentException => _currentException;

        public DebuggerClient(IPEndPoint address, Action<string> logger) {
            _address = address;
            _logger = logger;
        }


        public async Task<bool> Attach() {
            try {
                await _client.ConnectAsync(_address.Address, _address.Port);
                _stream = _client.GetStream();

                EnableKeepAlive();

                if (!(await SendPacketModel(new DebuggerInfoT() { Name = "Testing" })))
                    return false;

                if (await ReadPacketModel<DebuggerAttachedT>() == null)
                    return false;

                return true;
            } catch (Exception ex) {
                _logger(ex.ToString());
            }

            return false;
        }

        public async Task<bool> Pause() {
            bool result;
            StackDumpT stackDump;

            result = await SendPacketModel(new DebugCommandT() { CommandType = CommandType.Pause });
            if (!result) return false;

            stackDump = await ReadPacketModel<StackDumpT>();
            ProcessStackDump(stackDump);

            return stackDump != null;
        }
        public async Task<bool> Resume() {
            ProcessStackDump(null);

            return await SendPacketModel(new DebugCommandT() { CommandType = CommandType.Resume });
        }
        public async Task<bool> StepOver() {
            bool result;
            StackDumpT stackDump;

            result = await SendPacketModel(new DebugCommandT() { CommandType = CommandType.NextInstruction });
            if (!result) return false;

            stackDump = await ReadPacketModel<StackDumpT>();
            ProcessStackDump(stackDump);

            return stackDump != null;
        }


        private void ProcessStackDump(StackDumpT stackDump) {
            _currentStackDump = stackDump;
            _currentException = null;

            if (stackDump == null || stackDump.Frames.Count == 0)
                return;

            var errNumber = stackDump.Frames.Last().Locals.Where(el => el.Name == "ErrNumber").FirstOrDefault();
            var errSource = stackDump.Frames.Last().Locals.Where(el => el.Name == "ErrSource").FirstOrDefault();
            var errDescription = stackDump.Frames.Last().Locals.Where(el => el.Name == "ErrDescription").FirstOrDefault();

            if (errNumber != null) {
                _currentException = new InstructionException() {
                    Number = long.Parse(errNumber.Value),
                    Source = errSource.Value,
                    Description = errDescription.Value
                };
            }
        }


        private async Task<T> ReadPacketModel<T>() where T : class {
            byte[] packetData;
            T packet;

            try {
                packetData = await ReadPacket();
            } catch (Exception ex) {
                _logger($"Could not read packet for type {typeof(T)}, {ex}");
                return null;
            }

            packet = DeserializeModel<T>(packetData);

            if (packet == null) {
                _logger($"Received packet malformed, size {packetData.Length}");

                _client.Close();
            }

            return packet;
        }
        private async Task<byte[]> ReadPacket() {
            byte[] packetSizeHeader = await ReadAllAsync(4);
            int packetSize = BitConverter.ToInt32(packetSizeHeader, 0);
            return await ReadAllAsync(packetSize);
        }
        private async Task<byte[]> ReadAllAsync(int len) {
            byte[] data = new byte[len];
            int total_sent_bytes = 0;

            do {
                int sent_bytes = await _stream.ReadAsync(data, total_sent_bytes, len - total_sent_bytes);

                if (sent_bytes == 0) throw new Exception("Other endpoint signaled the end of stream");

                total_sent_bytes += sent_bytes;
            } while (total_sent_bytes != len);

            return data;
        }
        private T DeserializeModel<T>(byte[] packetData) where T : class {
            var byteBuffer = new ByteBuffer(packetData);

            if (typeof(T) == typeof(DebuggerAttachedT)) {
                if (!DebuggerAttached.VerifyDebuggerAttached(byteBuffer)) return null;
                return (T)(object)DebuggerAttachedT.DeserializeFromBinary(packetData);
            } else if (typeof(T) == typeof(StackDumpT)) {
                if (!StackDump.VerifyStackDump(byteBuffer)) return null;
                return (T)(object)StackDumpT.DeserializeFromBinary(packetData);
            }

            return null;
        }

        private async Task<bool> SendPacketModel(object model) {
            byte[] data;
            int packetSize;

            if (!SerializeModel(model, out data)) {
                _logger($"No packet handler registered for given packet type {model}");
                _client.Close();
                return false;
            }

            packetSize = data.Length;

            try {
                await _stream.WriteAsync(BitConverter.GetBytes(packetSize), 0, 4);
                await _stream.WriteAsync(data, 0, packetSize);

                return true;
            } catch (Exception ex) {
                _logger($"Could not send packet: {model}, {ex}");
            }

            return false;
        }
        private bool SerializeModel(object model, out byte[] data) {
            data = null;

            if (model is DebuggerInfoT castedModel1) data = castedModel1.SerializeToBinary();
            else if (model is DebugCommandT castedModel2) data = castedModel2.SerializeToBinary();
            else return false;

            return true;
        }

        private void EnableKeepAlive() {
            var data = new byte[12];

            Array.Copy(BitConverter.GetBytes(1), 0, data, 0, 4);
            Array.Copy(BitConverter.GetBytes(5000), 0, data, 4, 4);
            Array.Copy(BitConverter.GetBytes(5000), 0, data, 8, 4);

            _client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            _client.Client.IOControl(IOControlCode.KeepAliveValues, data, new byte[0] { });
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing)
                    _client.Dispose();

                disposedValue = true;
            }
        }
        public void Dispose() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
