using System;

using System.ComponentModel;

using System.Configuration;

using System.Reflection;

using System.Web;

using System.Web.UI;

using System.Web.UI.WebControls;



/// <summary>
/// Summary description for FakeBinder
/// </summary>
/// 
namespace Broker.Utility {

    /// <summary>Class that helps to simulate a fake binding context</summary>

    public class FakeBinder {

        protected Page _page;
        protected Control _control;
        protected Delegate _originalDataBind;


        protected FakeBinder(Page page, Control control) {
            _page = page;
            _control = control;
        }

        protected void Init() {
            Type controlType = typeof(Control);
            // get the Events property of the Control class
            PropertyInfo eventsProp = controlType.GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
            EventHandlerList events = eventsProp.GetValue(_control, null) as EventHandlerList;
            // get the static EventDataBinding object of the Control class
            FieldInfo eventDataBindingField = controlType.GetField("EventDataBinding", BindingFlags.NonPublic | BindingFlags.Static);
            object eventDataBinding = eventDataBindingField.GetValue(null);

            // get the delegate used for the DataBinding event by the control
            Delegate del = events[eventDataBinding];
            // find the generated DataBinding method and remove it
            foreach (Delegate currentDelegate in del.GetInvocationList()) {
                // the generated method has a fixed prefix
                if (currentDelegate.Method.Name.StartsWith("__DataBinding__control")) {
                    _originalDataBind = currentDelegate;
                    events.RemoveHandler(eventDataBinding, currentDelegate);
                }
            }

            // adds another handler to the DataBinding event
           _control.DataBinding += new EventHandler(control_DataBinding);
        }



        protected void control_DataBinding(object sender, EventArgs e) {

            // get the pushDataBindingContext method
            Type pageType = typeof(Page);
            MethodInfo pushDataBindingContextMethod = pageType.GetMethod("PushDataBindingContext", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);


            // call the pushDataBindingContextMethod with a null BindingContext
            

            try
            {
                pushDataBindingContextMethod.Invoke(_page, new object[] { null });
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.InnerException.Message);
            }

            // call the old handler. The handler will not fail because there is a BindingContext but as
            // the GetDataItem method returns null it will not try to bind the values to the control

            _originalDataBind.DynamicInvoke(sender, e);
            // remove the BindingContext
            System.Reflection.MethodInfo popDataBindingContextMethod = pageType.GetMethod("PopDataBindingContext", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            try
            {
                popDataBindingContextMethod.Invoke(_page, null);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.InnerException.Message);
            }
          
        }

        public static void SimulateBindingContext(Page page, Control control) {
            FakeBinder binder = new FakeBinder(page, control);
            binder.Init();
        }
    }
}
