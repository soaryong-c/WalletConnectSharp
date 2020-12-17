using System.Linq;
using System.Threading.Tasks;
using Nethereum.JsonRpc.Client;

namespace WalletConnectSharp.Client.Nethereum
{
    public class FallbackProvider : IClient
    {
        private static readonly string[] validMethods = new[]
        {
            "eth_signTransaction",
            "eth_sendTransaction"
        };
        
        private readonly IClient _fallback;
        private readonly IClient _signer;

        public FallbackProvider(IClient primary, IClient fallback)
        {
            this._signer = primary;
            this._fallback = fallback;
        }
        
        public Task SendRequestAsync(RpcRequest request, string route = null)
        {
            return validMethods.Contains(request.Method) ? _signer.SendRequestAsync(request, route) : _fallback.SendRequestAsync(request, route);
        }

        public Task SendRequestAsync(string method, string route = null, params object[] paramList)
        {
            return validMethods.Contains(method) ? _signer.SendRequestAsync(method, route, paramList) : _fallback.SendRequestAsync(method, route, paramList);
        }

        public RequestInterceptor OverridingRequestInterceptor { get; set; }
        public Task<T> SendRequestAsync<T>(RpcRequest request, string route = null)
        {
            return validMethods.Contains(request.Method) ? _signer.SendRequestAsync<T>(request, route) : _fallback.SendRequestAsync<T>(request, route);
        }

        public Task<T> SendRequestAsync<T>(string method, string route = null, params object[] paramList)
        {
            return validMethods.Contains(method) ? _signer.SendRequestAsync<T>(method, route, paramList) : _fallback.SendRequestAsync<T>(method, route, paramList);
        }
    }
}