﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectFirma.Web.Views.Shared.ProjectDocument
{
    public class EditProjectDocumentsViewModel
    {
        [Required]
        [DisplayName("Display Name")]
        [MaxLength(Models.ProjectDocument.FieldLengths.DisplayName)]
        public string DisplayName { get; set; }
        
        [DisplayName("Description")]
        [MaxLength(Models.ProjectDocument.FieldLengths.Description)]
        public string Description { get; set; }

        public EditProjectDocumentsViewModel()
        {
        }

        public EditProjectDocumentsViewModel(Models.ProjectDocument projectDocument)
        {
            DisplayName = projectDocument.DisplayName;
            Description = projectDocument.Description;
        }

        public EditProjectDocumentsViewModel(Models.ProjectDocumentUpdate projectDocumentUpdate)
        {
            DisplayName = projectDocumentUpdate.DisplayName;
            Description = projectDocumentUpdate.Description;
        }

        public void UpdateModel(Models.ProjectDocument projectDocument)
        {
            projectDocument.DisplayName = DisplayName;
            projectDocument.Description = Description;
        }

        public void UpdateModel(Models.ProjectDocumentUpdate projectDocumentUpdate)
        {
            projectDocumentUpdate.DisplayName = DisplayName;
            projectDocumentUpdate.Description = Description;
        }
    }
}