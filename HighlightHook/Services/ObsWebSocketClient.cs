using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace HighlightHook.Services
{
    public sealed class ObsWebSocketClient : IDisposable
    {
        private readonly Uri _uri;
        private readonly ClientWebSocket _socket = new();
        private readonly SemaphoreSlim _ioLock = new(1, 1);
        private bool _connected;
        private int _rpcVersion = 1;

        public ObsWebSocketClient(string host, int port)
        {
            if (host.StartsWith("ws://", StringComparison.OrdinalIgnoreCase) || host.StartsWith("wss://", StringComparison.OrdinalIgnoreCase))
            {
                _uri = new Uri(host);
            }
            else
            {
                _uri = new Uri($"ws://{host}:{port}");
            }
        }

        public async Task ConnectAsync(string password, CancellationToken cancellationToken = default)
        {
            if (_connected)
            {
                return;
            }

            _socket.Options.AddSubProtocol("obswebsocket.json");
            await _socket.ConnectAsync(_uri, cancellationToken).ConfigureAwait(false);

            var hello = await ReceiveMessageAsync(cancellationToken).ConfigureAwait(false);
            ValidateMessageOp(hello, 0, "Hello");

            var helloData = hello.GetProperty("d");
            _rpcVersion = helloData.TryGetProperty("rpcVersion", out var rpcVersionElement)
                ? rpcVersionElement.GetInt32()
                : 1;

            JsonElement? authentication = null;
            if (helloData.TryGetProperty("authentication", out var authElement))
            {
                authentication = authElement;
            }

            string? authString = null;
            if (authentication.HasValue)
            {
                if (string.IsNullOrWhiteSpace(password))
                {
                    throw new InvalidOperationException("OBS WebSocket authentication is enabled but no password was provided.");
                }

                var challenge = authentication.Value.GetProperty("challenge").GetString() ?? string.Empty;
                var salt = authentication.Value.GetProperty("salt").GetString() ?? string.Empty;
                authString = CreateAuthenticationString(password, salt, challenge);
            }

            var identifyPayload = new Dictionary<string, object?>
            {
                ["rpcVersion"] = _rpcVersion
            };

            if (!string.IsNullOrWhiteSpace(authString))
            {
                identifyPayload["authentication"] = authString;
            }

            var identify = new
            {
                op = 1,
                d = identifyPayload
            };

            await SendJsonAsync(identify, cancellationToken).ConfigureAwait(false);

            var identified = await ReceiveMessageAsync(cancellationToken).ConfigureAwait(false);
            ValidateMessageOp(identified, 2, "Identified");

            _connected = true;
        }

        public async Task SaveReplayBufferAsync(CancellationToken cancellationToken = default)
        {
            EnsureConnected();

            var requestId = Guid.NewGuid().ToString();
            var request = new
            {
                op = 6,
                d = new
                {
                    requestType = "SaveReplayBuffer",
                    requestId,
                    requestData = new { }
                }
            };

            await _ioLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                await SendJsonAsync(request, cancellationToken).ConfigureAwait(false);

                while (true)
                {
                    var response = await ReceiveMessageAsync(cancellationToken).ConfigureAwait(false);
                    if (!response.TryGetProperty("op", out var opElement))
                    {
                        continue;
                    }

                    if (opElement.GetInt32() != 7)
                    {
                        continue;
                    }

                    var responseData = response.GetProperty("d");
                    var responseRequestId = responseData.GetProperty("requestId").GetString();
                    if (!string.Equals(responseRequestId, requestId, StringComparison.Ordinal))
                    {
                        continue;
                    }

                    var requestStatus = responseData.GetProperty("requestStatus");
                    var success = requestStatus.GetProperty("result").GetBoolean();
                    if (!success)
                    {
                        var comment = requestStatus.TryGetProperty("comment", out var commentElement)
                            ? commentElement.GetString()
                            : null;
                        throw new InvalidOperationException(string.IsNullOrWhiteSpace(comment)
                            ? "OBS failed to save replay buffer."
                            : $"OBS failed to save replay buffer: {comment}");
                    }

                    return;
                }
            }
            finally
            {
                _ioLock.Release();
            }
        }

        private async Task SendJsonAsync(object payload, CancellationToken cancellationToken)
        {
            var json = JsonSerializer.Serialize(payload);
            var bytes = Encoding.UTF8.GetBytes(json);
            await _socket.SendAsync(bytes, WebSocketMessageType.Text, true, cancellationToken).ConfigureAwait(false);
        }

        private async Task<JsonElement> ReceiveMessageAsync(CancellationToken cancellationToken)
        {
            var buffer = new byte[8192];
            using var stream = new MemoryStream();

            while (true)
            {
                var result = await _socket.ReceiveAsync(buffer, cancellationToken).ConfigureAwait(false);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _connected = false;
                    throw new InvalidOperationException("OBS WebSocket closed unexpectedly.");
                }

                stream.Write(buffer, 0, result.Count);
                if (result.EndOfMessage)
                {
                    break;
                }
            }

            stream.Position = 0;
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken).ConfigureAwait(false);
            return document.RootElement.Clone();
        }

        private static void ValidateMessageOp(JsonElement message, int expectedOp, string name)
        {
            if (!message.TryGetProperty("op", out var opElement) || opElement.GetInt32() != expectedOp)
            {
                throw new InvalidOperationException($"Unexpected OBS WebSocket message while waiting for {name}.");
            }
        }

        private static string CreateAuthenticationString(string password, string salt, string challenge)
        {
            using var sha256 = SHA256.Create();

            var secret = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            var secretBase64 = Convert.ToBase64String(secret);
            var auth = sha256.ComputeHash(Encoding.UTF8.GetBytes(secretBase64 + challenge));
            return Convert.ToBase64String(auth);
        }

        private void EnsureConnected()
        {
            if (!_connected || _socket.State != WebSocketState.Open)
            {
                throw new InvalidOperationException("OBS WebSocket is not connected.");
            }
        }

        public void Dispose()
        {
            try
            {
                _connected = false;
                _socket.Dispose();
                _ioLock.Dispose();
            }
            catch
            {
            }
        }
    }
}
