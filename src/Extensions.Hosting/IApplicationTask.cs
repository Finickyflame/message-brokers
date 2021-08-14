using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting
{
    public interface IApplicationTask
    {
        string Name { get; }
        
        Task RunAsync();
    }
}