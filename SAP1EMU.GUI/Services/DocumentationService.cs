using SAP1EMU.GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAP1EMU.GUI.Services
{   public interface IDocumentationService 
    {
        public DocumentationModel GetDocs();
    }
    public class DocumentationService : IDocumentationService
    {
        string DocuFile = "docuFile.json";
        public DocumentationModel GetDocs()
        {
            return null;
        }
    }
}
