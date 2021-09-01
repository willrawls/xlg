using System.Threading.Tasks;
using MetX.Standard.Five;

namespace MetX.Standard.Interfaces
{
    public interface IProcess
    {
        public ProcessingResult Go();
        public Task<ProcessingResult> GoAsync();
    }
}