﻿using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Dexter.PeerReview
{
    [Export(typeof(IWpfTextViewMarginProvider))]
    [Name("PReviewMargin")]
    [Order(After = PredefinedMarginNames.OverviewChangeTracking, Before = PredefinedMarginNames.OverviewMark)]
    [MarginContainer(PredefinedMarginNames.VerticalScrollBar)]
    [ContentType("text")]               
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    internal sealed class PeerReviewMarginProvider : IWpfTextViewMarginProvider
    {
        public IWpfTextViewMargin CreateMargin(IWpfTextViewHost wpfTextViewHost, IWpfTextViewMargin marginContainer)
        {
            return new PReviewMargin(wpfTextViewHost.TextView, marginContainer as IVerticalScrollBar);
        }
    }
}
