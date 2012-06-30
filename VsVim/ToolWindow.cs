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

            Caption = Resources.ToolWindowTitle;
            BitmapResourceID = 301;
            BitmapIndex = 1;
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
            _textView = textView;
        }
    }
}
