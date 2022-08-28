using System.Net.Http;
using System.Threading.Tasks;

namespace signup_example.Services
{
    public interface IApiService
    {
        public Task<HttpResponseMessage> Post(string path, object payload);
    }
}
