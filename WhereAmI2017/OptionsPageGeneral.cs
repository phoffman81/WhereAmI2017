﻿using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using WhereAmI2017.Converters;

namespace WhereAmI2017
{
    /// <summary>
    // Extends the standard dialog functionality for implementing ToolsOptions pages, 
    // with support for the Visual Studio automation model, Windows Forms, and state 
    // persistence through the Visual Studio settings mechanism.
    /// </summary>
    [Guid(Constants.GuidPageGeneral)]
    public class OptionsPageGeneral : Microsoft.VisualStudio.Shell.DialogPage
    {
        IWhereAmISettings settings
        {
            get
            {
                var componentModel = (IComponentModel)(Site.GetService(typeof(SComponentModel)));
                IWhereAmISettings s = componentModel.DefaultExportProvider.GetExportedValue<IWhereAmISettings>();

                return s;
            }
        }

        #region Constructors

        public OptionsPageGeneral()
        {
            FilenameSize = 60;
            FoldersSize = ProjectSize = 52;

            FilenameColor = Color.FromArgb(234, 234, 234);
            FoldersColor = ProjectColor = Color.FromArgb(243, 243, 243);

            ViewFilename = ViewFolders = ViewProject = true;

            Opacity = 1;
        }

        #endregion

        #region Properties

        [Browsable(true)]
        [Category("File name")]
        [Description("The color of the filename part")]
        [DisplayName("Filename color")]
        public Color FilenameColor { get; set; }

        [Category("File name")]
        [Description("Indicate to show the filename part or not")]
        [DisplayName("Show")]
        public bool ViewFilename { get; set; }

        [Browsable(true)]
        [Category("Folder")]
        [Description("The color of the folder part")]
        [DisplayName("Folder color")]
        public Color FoldersColor { get; set; }

        [Category("Folder")]
        [Description("Indicate to show the folder part or not")]
        [DisplayName("Show")]
        public bool ViewFolders { get; set; }

        [Browsable(true)]
        [Category("Project")]
        [Description("The color of the project part")]
        [DisplayName("Project color")]
        public Color ProjectColor { get; set; }

        [Category("Project")]
        [Description("Indicate to show the project part or not")]
        [DisplayName("Show")]
        public bool ViewProject { get; set; }

        [Category("File name")]
        [Description("The size of the filename part")]
        [DisplayName("Filename size")]
        public double FilenameSize { get; set; }

        [Category("Folder")]
        [Description("The size of the folder part")]
        [DisplayName("Folder size")]
        public double FoldersSize { get; set; }

        [Category("Project")]
        [Description("The size of the project part")]
        [DisplayName("Project size")]
        public double ProjectSize { get; set; }

        [Category("Appearance")]
        [DisplayName("Opacity")]
        [Description("Opacity of the text. Insert a value between 0 and 1.")]
        [TypeConverter(typeof(PercentageConverter))]
        public double Opacity { get; set; }

        [Category("Appearance")]
        [DisplayName("Position")]
        [Description("The position in the view of the text block")]
        [Browsable(true)]
        public AdornmentPositions Position { get; set; }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Handles "activate" messages from the Visual Studio environment.
        /// </summary>
        /// <devdoc>
        /// This method is called when Visual Studio wants to activate this page.  
        /// </devdoc>
        /// <remarks>If this handler sets e.Cancel to true, the activation will not occur.</remarks>
        protected override void OnActivate(CancelEventArgs e)
        {
            base.OnActivate(e);

            BindSettings();
        }

        /// <summary>
        /// Handles "apply" messages from the Visual Studio environment.
        /// </summary>
        /// <devdoc>
        /// This method is called when VS wants to save the user's 
        /// changes (for example, when the user clicks OK in the dialog).
        /// </devdoc>
        protected override void OnApply(PageApplyEventArgs e)
        {
            // TODO: calculate the Hash of the stored settings and of the changed to see if there's a change and ask the confirmation:
            int result = VsShellUtilities.ShowMessageBox(Site, Resources.MessageOnApplyEntered, Resources.Confirm, OLEMSGICON.OLEMSGICON_QUERY, OLEMSGBUTTON.OLEMSGBUTTON_OKCANCEL, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

            if (result == (int)VSConstants.MessageBoxResult.IDCANCEL)
            {
                e.ApplyBehavior = ApplyKind.Cancel;
            }
            else
            {
                if (e.ApplyBehavior == ApplyKind.Apply)
                {
                    settings.FilenameColor = FilenameColor;
                    settings.FoldersColor = FoldersColor;
                    settings.ProjectColor = ProjectColor;

                    settings.FilenameSize = FilenameSize;
                    settings.FoldersSize = FoldersSize;
                    settings.ProjectSize = ProjectSize;

                    settings.ViewFilename = ViewFilename;
                    settings.ViewFolders = ViewFolders;
                    settings.ViewProject = ViewProject;

                    settings.Position = Position;
                    settings.Opacity = Opacity;

                    settings.Store();
                }

                base.OnApply(e);
            }
        }

        #endregion Event Handlers

        private void BindSettings()
        {
            FilenameColor = settings.FilenameColor;
            FoldersColor = settings.FoldersColor;
            ProjectColor = settings.ProjectColor;

            FilenameSize = settings.FilenameSize;
            FoldersSize = settings.FoldersSize;
            ProjectSize = settings.ProjectSize;

            ViewFilename = settings.ViewFilename;
            ViewFolders = settings.ViewFolders;
            ViewProject = settings.ViewProject;

            Position = settings.Position;
            Opacity = settings.Opacity;
        }
    }
}
