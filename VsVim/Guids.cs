
using System;
namespace VsVim
{
    internal static class GuidList
    {
        internal const string VsVimPackageGuidString = "1286dc8b-8d05-45df-bf41-d50275f3d985";
        internal const string VsVimCommandSetGuidString = "c0481abe-bb84-42c9-8b25-0801a7b55cb7";
        internal const string VsVimToolWindowGuidString = "4869721e-9926-4ead-8dea-10c351f08358";
        internal static readonly Guid VsVimCommandSetGuid = new Guid(VsVimCommandSetGuidString);
    };
}
