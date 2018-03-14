using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Place
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class Place:EntityBase<Place> {

        public static List<Place> GetActivePlaces() {
            return Table.Where(p => p.IsActive == true).OrderBy(c=>c.Name).ToList();
        }

        public static List<Place> GetAllPlaces()
        {
            return Table.OrderBy(c => c.Name).ToList();
        }

        public static bool ExistCodeInPlaces(string code) {
            return (Table.Where(p => p.Code == code && p.IsActive == true).SingleOrDefault() != null);
        }

        public static bool ExistOtherCode(int codeID, string code) {
            return (Table.Where(p => p.Code == code && p.ID != codeID && p.IsActive == true).SingleOrDefault() != null);
        }

        
        public override void Validate() {

        }

        public void TestBeforeInsert() {
            if (Place.ExistCodeInPlaces(this.Code)) {
                ValidationErrors.Add("PLACE_CODE_INSERT_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public void TestBeforeUpdate() {
            if (Place.ExistOtherCode(this.ID, this.Code)) {
                ValidationErrors.Add("PLACE_CODE_UPDATE_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }
        public static Place GetFirstyMunicipality(int municipalityID) {
            List<Place> listPlacesForMunicipity = Table.Where(c => c.MunicipalityID == municipalityID).ToList();
            if (listPlacesForMunicipity.Count > 0)
            {
                return listPlacesForMunicipity.First();
            } else
            {
                return Table.First();
            }

        }

    }
}