using System.Threading.Tasks;
// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Hosting
{
    public interface IApplicationTask
    {
        string Name { get; }
        
        Task RunAsync();
    }
}