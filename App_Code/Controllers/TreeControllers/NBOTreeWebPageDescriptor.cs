using System;
using System.ComponentModel;
using System.Data;
using Broker.DataAccess;

namespace Broker.Controllers.Tree {
    /// <summary>
    /// Klasa koja dava opis na web-stranite od dinamickoto drvo
    /// (nasleduva od klasata PropertyDescriptor)
    /// </summary>
    public class NBOTreeWebPageDescriptor : PropertyDescriptor {

        /// <summary>
        /// Konstruktor na klasata
        /// </summary>
        /// <param name="name"></param>
        public NBOTreeWebPageDescriptor(string name) : base(name, null) {
        }

        /// <summary>
        /// Metod koj vrsi override na funkcijata GetValue pri sto
        /// vlezniot parametar component se kastira vo objekt od tip
        /// NBOTreeHierarchyData i se vraka vrednosta na svojstvoto
        /// - GetPropertyValue za nego
        /// </summary>
        /// <param name="component"></param>
        /// <returns>object</returns>
        public override object GetValue(object component) {
            NBOTreeHierarchyData data= (NBOTreeHierarchyData)component;
            return ((INBOTreeNode)data.Item).GetPropertyValue(this.Name);
        }


        public override bool CanResetValue(object component) {
            throw new Exception("Not implemented.");
        }

        public override Type ComponentType {
            get { throw new Exception("Not implemented."); }
        }

        public override bool IsReadOnly {
            get { throw new Exception("Not implemented."); }
        }

        public override Type PropertyType {
            get { throw new Exception("Not implemented."); }
        }

        public override void ResetValue(object component) {
            throw new Exception("Not implemented.");
        }

        public override void SetValue(object component, object value) {
            throw new Exception("Not implemented.");
        }

        public override bool ShouldSerializeValue(object component) {
            throw new Exception("Not implemented.");
        }
    }
}