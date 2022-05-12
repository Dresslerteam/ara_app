/* Copyright(c) Tim Watts, Box of Clicks - All Rights Reserved */

using System.IO;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace BOC.BTagged.Shared
{
    public static class BTaggedSharedUtils
    {
        public static string GetASMDEFDirectory(string asmdef)
        {
            return Path.GetDirectoryName(CompilationPipeline.GetAssemblyDefinitionFilePathFromAssemblyName(asmdef));
        }
    }
}
