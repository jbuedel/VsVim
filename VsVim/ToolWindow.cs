using System;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace VsVim
{
    [Guid(GuidList.VsVimToolWindowGuidString)]
    public sealed class ToolWindow : ToolWindowPane
    {
        private readonly DockPanel _dockPanel;
        private ITextView _textView;

        public ToolWindow()
        {
            _dockPanel = new DockPanel();

            // Set the window title reading it from the resources.
            Caption = Resources.ToolWindowTitle;

            // Set the image that will appear on the tab of the window frame
            // when docked with an other window
            // The resource ID correspond to the one defined in the resx file
            // while the Index is the offset in the bitmap strip. Each image in
            // the strip being 16x16.
            BitmapResourceID = 301;
            BitmapIndex = 1;

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on 
            // the object returned by the Content property.

            Content = _dockPanel;
        }

        protected override void Initialize()
        {
            base.Initialize();

            var serviceProvider = this;
            var componentModel = serviceProvider.GetService<SComponentModel, IComponentModel>();

            // First create the ITextBuffer with a content type of text.  One day we may want to switch it
            // to use a Vim specific content type but for now just stick with text.  This is a general purpose
            // window
            var textBufferFactoryService = componentModel.DefaultExportProvider.GetExport<ITextBufferFactoryService>().Value;
            var textBuffer = textBufferFactoryService.CreateTextBuffer(textBufferFactoryService.TextContentType);
            textBuffer.Insert(0, "VsVim Scratch Pad");

            // Now create the containing view.  Want a simple document here but no outlining 
            var textEditorFactoryService = componentModel.DefaultExportProvider.GetExport<ITextEditorFactoryService>().Value;
            var textViewRoleSet = textEditorFactoryService.CreateTextViewRoleSet(
                PredefinedTextViewRoles.PrimaryDocument,
                PredefinedTextViewRoles.Document,
                PredefinedTextViewRoles.Editable,
                PredefinedTextViewRoles.Interactive);
            var textView = textEditorFactoryService.CreateTextView(textBuffer, textViewRoleSet);

            // Now update the display to be the document
            var textViewHost = textEditorFactoryService.CreateTextViewHost(textView, setFocus: false);
            _dockPanel.Children.Add(textViewHost.HostControl);
        }
    }
}
