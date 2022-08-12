using System.Threading.Tasks;
using MetX.Standard.Primary.Fimm;

namespace MetX.Standard.Primary.Interfaces
{
    public interface IProcess
    {
        public ProcessingResult Go();
        public Task<ProcessingResult> GoAsync();
    }
}