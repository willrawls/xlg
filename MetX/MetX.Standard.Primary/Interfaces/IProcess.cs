using System.Threading.Tasks;
using MetX.Standard.Primary.Five;

namespace MetX.Standard.Primary.Interfaces
{
    public interface IProcess
    {
        public ProcessingResult Go();
        public Task<ProcessingResult> GoAsync();
    }
}