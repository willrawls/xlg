using System;
using System.Diagnostics;
using System.IO;
using MetX.Standard.Generators.Support;

namespace MetX.Standard.Generators
{
    public class BaseRoslynCodeGenerator : IDisposable
    {
        public AppDomain ShadowDomain { get; set; }
        public IAmAContextForGeneration ShadowRunContext { get; set; }
        public string ShadowDomainName { get; set; }
        public string PathToActual { get; set; }
        public string FullNameOfActual { get; set; }

        protected BaseRoslynCodeGenerator(string pathToActual, string fullNameOfActual)
        {
            PathToActual = pathToActual;
            FullNameOfActual = fullNameOfActual;
        }

        public bool InitializeShadowRunContext()
        {
            if (!File.Exists(PathToActual))
            {
                Debug.WriteLine($"InitializeShadowRunContext: Actual generator DLL not found: {PathToActual}");
                return false;
            }

            ShadowDomain = null;
            ShadowRunContext = null;
            ShadowDomainName = Guid.NewGuid().ToString("N");
            try
            {
                ShadowDomain = AppDomain.CreateDomain(ShadowDomainName);
                var actualCodeGeneratorAssembly = ShadowDomain.Load(PathToActual);
                var actualCodeGenerator =
                    actualCodeGeneratorAssembly.CreateInstance(FullNameOfActual, true) as IAmAContextForGeneration;
                if (actualCodeGenerator == null)
                {
                    Debug.WriteLine("InitializeShadowRunContext: Create instance failed");
                }
                else
                {
                    ShadowRunContext = actualCodeGenerator;
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return false;
        }

        public void InitializeContextIfNeeded()
        {
            if (ShadowRunContext == null)
                InitializeShadowRunContext();
        }

        public void Cleanup()
        {
            ShadowRunContext = null;
            if (ShadowDomain == null) return;

            try
            {
                AppDomain.Unload(ShadowDomain);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            ShadowDomain = null;
        }

        public void Dispose()
        {
            Cleanup();
        }



    }
}