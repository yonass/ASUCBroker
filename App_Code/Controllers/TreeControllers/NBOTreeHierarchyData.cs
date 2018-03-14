using System;
using System.ComponentModel;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Broker.DataAccess;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Broker.Controllers.Tree {

    /// <summary>
    /// Klasa za specificiranje na hierarhijata na podatocite vo dinamickoto drvo
    /// (nasleduva od klasite: IHierarchyData i ICustomTypeDescriptor)
    /// </summary>
    public class NBOTreeHierarchyData : IHierarchyData, ICustomTypeDescriptor {

        private INBOTreeNode treeItem = null;
        private int userId;
        private string typeName;

        /// <summary>
        /// Konstruktor na klasata koj se povikuva so eden parametar
        /// </summary>
        /// <param name="obj"></param>
        public NBOTreeHierarchyData(INBOTreeNode obj) {
            treeItem = obj;
        }

        /// <summary>
        /// Konstruktor na klasata koj se povikuva so dva parametri
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="userId"></param>
        public NBOTreeHierarchyData(INBOTreeNode obj, int userId) {
            treeItem = obj;
            this.userId = userId;
        }
        
        /// <summary>
        /// Konstruktor na klasata koj se povikuva so tri parametri
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="userId"></param>
        /// <param name="typeName"></param>
        public NBOTreeHierarchyData(INBOTreeNode obj, int userId, string typeName) {
            treeItem = obj;
            this.userId = userId;
            this.typeName = typeName;
        }

        /// <summary>
        /// Metod koj vrsi override na funkcijata ToString, taka sto 
        /// taa ke vraka naslov na elementot treeItem od drvoto
        /// </summary>
        /// <returns>string</returns>
        public override string ToString() {
            return treeItem.Title;
        }

        /// <summary>
        /// Interfejs za promenlivata HasChildren koja vraka
        /// dali elementot treeItem od drvoto ima deca za specificiran korisnik (userID)
        /// </summary>
        public bool HasChildren {
            get {
                return treeItem.HasChildren(userId);
            }

        }

        /// <summary>
        /// Interfejs za promenlivata Path koja go vraka
        /// ID-to na elementot treeItem od drvoto kako string
        /// </summary>
        public string Path {
            get {
                return treeItem.ID.ToString();
            }
        }

        /// <summary>
        /// Interfejs za objektot Item pri sto se vraka
        /// objekt od tip element na drvoto - treeItem
        /// </summary>
        public object Item {
            get {
                return treeItem;
            }
        }

        /// <summary>
        /// Interfejs za promenlivata Type pri sto se vraka
        /// promenlivata typeName
        /// </summary>
        public string Type {
            get {
                return typeName;
            }
        }

        /// <summary>
        /// Metod koj go polni i vraka mnozestvoto NBOTreeHierarchicalEnumerable
        /// so elementi - deca na konkreten korisnik (userID)
        /// </summary>
        /// <returns>IHierarchicalEnumerable</returns>
        public IHierarchicalEnumerable GetChildren() {
            NBOTreeHierarchicalEnumerable children = new NBOTreeHierarchicalEnumerable();
            foreach(INBOTreeNode item in treeItem.GetChildren(userId)) {
                children.Add(item);
            }
            return children;
        }

        /// <summary>
        /// Metod koj vraka nova instanca od objektot NBOTreeHierarchyData
        /// napolneta so roditeli
        /// </summary>
        /// <returns>IHierarchyData</returns>
        public IHierarchyData GetParent() {
            NBOTreeHierarchicalEnumerable parentContainer = new NBOTreeHierarchicalEnumerable();
            return new NBOTreeHierarchyData(treeItem.GetParent());
        }

        /// <summary>
        /// Metod koj gi vraka svojstvata na sekoj element od drvoto
        /// </summary>
        /// <returns>PropertyDescriptorCollection</returns>
        public PropertyDescriptorCollection GetProperties() {
            List<PropertyDescriptor> props = new List<PropertyDescriptor>();
            foreach(PropertyInfo p in treeItem.GetType().GetProperties()) {
                props.Add(new NBOTreeWebPageDescriptor(p.Name));
            }
            return new PropertyDescriptorCollection(props.ToArray());
        }


        // The following properties and methods are required by the
        // ICustomTypeDescriptor interface but are not implemented

        public System.ComponentModel.AttributeCollection GetAttributes() {
            throw new Exception("Not implemented.");
        }

        public string GetClassName() {
            throw new Exception("Not implemented.");
        }

        public string GetComponentName() {
            throw new Exception("Not implemented.");
        }

        public TypeConverter GetConverter() {
            throw new Exception("Not implemented.");
        }

        public EventDescriptor GetDefaultEvent() {
            throw new Exception("Not implemented.");
        }

        public PropertyDescriptor GetDefaultProperty() {
            throw new Exception("Not implemented.");
        }

        public object GetEditor(Type editorBaseType) {
            throw new Exception("Not implemented.");
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes) {
            throw new Exception("Not implemented.");
        }

        public EventDescriptorCollection GetEvents() {
            throw new Exception("Not implemented.");
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes) {
            throw new Exception("Not implemented.");
        }

        public object GetPropertyOwner(PropertyDescriptor pd) {
            throw new Exception("Not implemented.");
        }
    }
}