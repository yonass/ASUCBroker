using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for VariableType
/// </summary>
namespace Broker.DataAccess{
    public partial class VariableType : EntityBase<VariableType> {


        public static ValidationDataType GetForVariableType(int variableTypeID) {

            VariableType vt = Table.Where(c => c.ID == variableTypeID).SingleOrDefault();
            if (vt.CSintakse == "int") {
                return ValidationDataType.Integer;
            }
            if (vt.CSintakse == "decimal") {
                return ValidationDataType.Double;
            }
            if (vt.CSintakse == "float") {
                return ValidationDataType.Double;
            }
            if (vt.CSintakse == "DateTime") {
                return ValidationDataType.Date;
            }
            return ValidationDataType.String;
        }
    }
    
}
