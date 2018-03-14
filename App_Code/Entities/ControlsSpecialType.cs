using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ControlsSpecialType
/// </summary>
namespace Broker.DataAccess{
public partial class ControlsSpecialType:EntityBase<ControlsSpecialType>
    {

    public static List<ControlsSpecialType> GetForFiscal(int specialFieldTypeID) {
       return  Table.Where(c=>c.SpecialFieldTypeID==specialFieldTypeID ).ToList();
       
    }

    public static bool IsForFiscal(int controlID) {
        SpecialFieldType sft = SpecialFieldType.GetByCode(SpecialFieldType.FISCAL);
        List<ControlsSpecialType> cstList = Table.Where(c => c.SpecialFieldTypeID == sft.ID && c.ControlID == controlID && c.IsActive == true).ToList();
        return (cstList.Count != 0);
    }



    }
}
