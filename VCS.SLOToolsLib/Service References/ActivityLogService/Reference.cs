﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.261
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 4.0.60310.0
// 
namespace VCS.ActivityLogService {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Activity", Namespace="http://schemas.datacontract.org/2004/07/VCS", IsReference=true)]
    public partial class Activity : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string ActionField;
        
        private System.Collections.Generic.List<VCS.ActivityLogService.Argument> ArgumentsField;
        
        private System.DateTime DateField;
        
        private string IPField;
        
        private string UserNameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Action {
            get {
                return this.ActionField;
            }
            set {
                if ((object.ReferenceEquals(this.ActionField, value) != true)) {
                    this.ActionField = value;
                    this.RaisePropertyChanged("Action");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.List<VCS.ActivityLogService.Argument> Arguments {
            get {
                return this.ArgumentsField;
            }
            set {
                if ((object.ReferenceEquals(this.ArgumentsField, value) != true)) {
                    this.ArgumentsField = value;
                    this.RaisePropertyChanged("Arguments");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Date {
            get {
                return this.DateField;
            }
            set {
                if ((this.DateField.Equals(value) != true)) {
                    this.DateField = value;
                    this.RaisePropertyChanged("Date");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IP {
            get {
                return this.IPField;
            }
            set {
                if ((object.ReferenceEquals(this.IPField, value) != true)) {
                    this.IPField = value;
                    this.RaisePropertyChanged("IP");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UserName {
            get {
                return this.UserNameField;
            }
            set {
                if ((object.ReferenceEquals(this.UserNameField, value) != true)) {
                    this.UserNameField = value;
                    this.RaisePropertyChanged("UserName");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Argument", Namespace="http://schemas.datacontract.org/2004/07/VCS", IsReference=true)]
    public partial class Argument : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string KeyField;
        
        private string ValueField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Key {
            get {
                return this.KeyField;
            }
            set {
                if ((object.ReferenceEquals(this.KeyField, value) != true)) {
                    this.KeyField = value;
                    this.RaisePropertyChanged("Key");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Value {
            get {
                return this.ValueField;
            }
            set {
                if ((object.ReferenceEquals(this.ValueField, value) != true)) {
                    this.ValueField = value;
                    this.RaisePropertyChanged("Value");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ActivityLogService.IActivityLogService")]
    public interface IActivityLogService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IActivityLogService/LogActivity", ReplyAction="http://tempuri.org/IActivityLogService/LogActivityResponse")]
        System.IAsyncResult BeginLogActivity(VCS.ActivityLogService.Activity activity, System.AsyncCallback callback, object asyncState);
        
        void EndLogActivity(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IActivityLogServiceChannel : VCS.ActivityLogService.IActivityLogService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ActivityLogServiceClient : System.ServiceModel.ClientBase<VCS.ActivityLogService.IActivityLogService>, VCS.ActivityLogService.IActivityLogService {
        
        private BeginOperationDelegate onBeginLogActivityDelegate;
        
        private EndOperationDelegate onEndLogActivityDelegate;
        
        private System.Threading.SendOrPostCallback onLogActivityCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public ActivityLogServiceClient() {
        }
        
        public ActivityLogServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ActivityLogServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ActivityLogServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ActivityLogServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> LogActivityCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult VCS.ActivityLogService.IActivityLogService.BeginLogActivity(VCS.ActivityLogService.Activity activity, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginLogActivity(activity, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void VCS.ActivityLogService.IActivityLogService.EndLogActivity(System.IAsyncResult result) {
            base.Channel.EndLogActivity(result);
        }
        
        private System.IAsyncResult OnBeginLogActivity(object[] inValues, System.AsyncCallback callback, object asyncState) {
            VCS.ActivityLogService.Activity activity = ((VCS.ActivityLogService.Activity)(inValues[0]));
            return ((VCS.ActivityLogService.IActivityLogService)(this)).BeginLogActivity(activity, callback, asyncState);
        }
        
        private object[] OnEndLogActivity(System.IAsyncResult result) {
            ((VCS.ActivityLogService.IActivityLogService)(this)).EndLogActivity(result);
            return null;
        }
        
        private void OnLogActivityCompleted(object state) {
            if ((this.LogActivityCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.LogActivityCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void LogActivityAsync(VCS.ActivityLogService.Activity activity) {
            this.LogActivityAsync(activity, null);
        }
        
        public void LogActivityAsync(VCS.ActivityLogService.Activity activity, object userState) {
            if ((this.onBeginLogActivityDelegate == null)) {
                this.onBeginLogActivityDelegate = new BeginOperationDelegate(this.OnBeginLogActivity);
            }
            if ((this.onEndLogActivityDelegate == null)) {
                this.onEndLogActivityDelegate = new EndOperationDelegate(this.OnEndLogActivity);
            }
            if ((this.onLogActivityCompletedDelegate == null)) {
                this.onLogActivityCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnLogActivityCompleted);
            }
            base.InvokeAsync(this.onBeginLogActivityDelegate, new object[] {
                        activity}, this.onEndLogActivityDelegate, this.onLogActivityCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override VCS.ActivityLogService.IActivityLogService CreateChannel() {
            return new ActivityLogServiceClientChannel(this);
        }
        
        private class ActivityLogServiceClientChannel : ChannelBase<VCS.ActivityLogService.IActivityLogService>, VCS.ActivityLogService.IActivityLogService {
            
            public ActivityLogServiceClientChannel(System.ServiceModel.ClientBase<VCS.ActivityLogService.IActivityLogService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginLogActivity(VCS.ActivityLogService.Activity activity, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = activity;
                System.IAsyncResult _result = base.BeginInvoke("LogActivity", _args, callback, asyncState);
                return _result;
            }
            
            public void EndLogActivity(System.IAsyncResult result) {
                object[] _args = new object[0];
                base.EndInvoke("LogActivity", _args, result);
            }
        }
    }
}