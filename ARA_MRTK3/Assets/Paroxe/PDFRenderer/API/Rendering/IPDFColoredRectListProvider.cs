﻿using System.Collections.Generic;

namespace Paroxe.PdfRenderer
{
    /// <summary>
    /// This interface allow to implement custom colored rects provider. For example, PDFViewer inherits
    /// this class and implements it to provide colored rects during a search session to the renderer.
    /// </summary>
    public interface IPDFColoredRectListProvider
    {
        IList<PDFColoredRect> GetBackgroundColoredRectList(PDFPage page);
    }
}