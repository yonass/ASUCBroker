using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Attachment
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class Attachment:EntityBase<Attachment> {
        public static List<Attachment> GetByDocumentTypeIDAndDocumentID(int documentTypeID, int documentID)
        {
            return Table.Where(a => a.DocumentID == documentID && a.DocumentTypeID == documentTypeID).ToList();
        }
    }
}
