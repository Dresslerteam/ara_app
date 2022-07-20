using System;
using UnityEngine;

    public enum instructionTypes
    {
        Assemble,
        Disassemble,
        Misc
    }
    [Serializable]
    public class pdfData
    {
        public string title;
        public string url;
        public instructionTypes typeOfInstruction;
    }
    