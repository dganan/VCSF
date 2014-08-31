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
namespace VCS.SLORepositoryService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SLORepositoryService.ISLORepositoryService")]
    public interface ISLORepositoryService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ISLORepositoryService/GetAvailableSLOs", ReplyAction="http://tempuri.org/ISLORepositoryService/GetAvailableSLOsResponse")]
        System.IAsyncResult BeginGetAvailableSLOs(string userId, System.AsyncCallback callback, object asyncState);
        
        System.Collections.ObjectModel.ObservableCollection<VCS.SLODescriptor> EndGetAvailableSLOs(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ISLORepositoryService/GetSLOById", ReplyAction="http://tempuri.org/ISLORepositoryService/GetSLOByIdResponse")]
        System.IAsyncResult BeginGetSLOById(string resId, string userId, System.AsyncCallback callback, object asyncState);
        
        VCS.SLO EndGetSLOById(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ISLORepositoryService/CreateSLOFromCollaborativeSession", ReplyAction="http://tempuri.org/ISLORepositoryService/CreateSLOFromCollaborativeSessionRespons" +
            "e")]
        System.IAsyncResult BeginCreateSLOFromCollaborativeSession(string csid, string csthread, string source, string name, bool automaticCategorization, string userId, System.AsyncCallback callback, object asyncState);
        
        string EndCreateSLOFromCollaborativeSession(out string error, System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ISLORepositoryService/InsertSLO", ReplyAction="http://tempuri.org/ISLORepositoryService/InsertSLOResponse")]
        System.IAsyncResult BeginInsertSLO(VCS.SLO slo, string userId, System.AsyncCallback callback, object asyncState);
        
        [return: System.ServiceModel.MessageParameterAttribute(Name="error")]
        string EndInsertSLO(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ISLORepositoryService/UpdateSLO", ReplyAction="http://tempuri.org/ISLORepositoryService/UpdateSLOResponse")]
        System.IAsyncResult BeginUpdateSLO(VCS.SLO slo, string userId, System.AsyncCallback callback, object asyncState);
        
        [return: System.ServiceModel.MessageParameterAttribute(Name="error")]
        string EndUpdateSLO(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISLORepositoryServiceChannel : VCS.SLORepositoryService.ISLORepositoryService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetAvailableSLOsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetAvailableSLOsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Collections.ObjectModel.ObservableCollection<VCS.SLODescriptor> Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Collections.ObjectModel.ObservableCollection<VCS.SLODescriptor>)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetSLOByIdCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetSLOByIdCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public VCS.SLO Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((VCS.SLO)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CreateSLOFromCollaborativeSessionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public CreateSLOFromCollaborativeSessionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string error {
            get {
                base.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
        
        public string Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class InsertSLOCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public InsertSLOCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UpdateSLOCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public UpdateSLOCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SLORepositoryServiceClient : System.ServiceModel.ClientBase<VCS.SLORepositoryService.ISLORepositoryService>, VCS.SLORepositoryService.ISLORepositoryService {
        
        private BeginOperationDelegate onBeginGetAvailableSLOsDelegate;
        
        private EndOperationDelegate onEndGetAvailableSLOsDelegate;
        
        private System.Threading.SendOrPostCallback onGetAvailableSLOsCompletedDelegate;
        
        private BeginOperationDelegate onBeginGetSLOByIdDelegate;
        
        private EndOperationDelegate onEndGetSLOByIdDelegate;
        
        private System.Threading.SendOrPostCallback onGetSLOByIdCompletedDelegate;
        
        private BeginOperationDelegate onBeginCreateSLOFromCollaborativeSessionDelegate;
        
        private EndOperationDelegate onEndCreateSLOFromCollaborativeSessionDelegate;
        
        private System.Threading.SendOrPostCallback onCreateSLOFromCollaborativeSessionCompletedDelegate;
        
        private BeginOperationDelegate onBeginInsertSLODelegate;
        
        private EndOperationDelegate onEndInsertSLODelegate;
        
        private System.Threading.SendOrPostCallback onInsertSLOCompletedDelegate;
        
        private BeginOperationDelegate onBeginUpdateSLODelegate;
        
        private EndOperationDelegate onEndUpdateSLODelegate;
        
        private System.Threading.SendOrPostCallback onUpdateSLOCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public SLORepositoryServiceClient() {
        }
        
        public SLORepositoryServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SLORepositoryServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SLORepositoryServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SLORepositoryServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
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
        
        public event System.EventHandler<GetAvailableSLOsCompletedEventArgs> GetAvailableSLOsCompleted;
        
        public event System.EventHandler<GetSLOByIdCompletedEventArgs> GetSLOByIdCompleted;
        
        public event System.EventHandler<CreateSLOFromCollaborativeSessionCompletedEventArgs> CreateSLOFromCollaborativeSessionCompleted;
        
        public event System.EventHandler<InsertSLOCompletedEventArgs> InsertSLOCompleted;
        
        public event System.EventHandler<UpdateSLOCompletedEventArgs> UpdateSLOCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult VCS.SLORepositoryService.ISLORepositoryService.BeginGetAvailableSLOs(string userId, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetAvailableSLOs(userId, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Collections.ObjectModel.ObservableCollection<VCS.SLODescriptor> VCS.SLORepositoryService.ISLORepositoryService.EndGetAvailableSLOs(System.IAsyncResult result) {
            return base.Channel.EndGetAvailableSLOs(result);
        }
        
        private System.IAsyncResult OnBeginGetAvailableSLOs(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string userId = ((string)(inValues[0]));
            return ((VCS.SLORepositoryService.ISLORepositoryService)(this)).BeginGetAvailableSLOs(userId, callback, asyncState);
        }
        
        private object[] OnEndGetAvailableSLOs(System.IAsyncResult result) {
            System.Collections.ObjectModel.ObservableCollection<VCS.SLODescriptor> retVal = ((VCS.SLORepositoryService.ISLORepositoryService)(this)).EndGetAvailableSLOs(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetAvailableSLOsCompleted(object state) {
            if ((this.GetAvailableSLOsCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetAvailableSLOsCompleted(this, new GetAvailableSLOsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetAvailableSLOsAsync(string userId) {
            this.GetAvailableSLOsAsync(userId, null);
        }
        
        public void GetAvailableSLOsAsync(string userId, object userState) {
            if ((this.onBeginGetAvailableSLOsDelegate == null)) {
                this.onBeginGetAvailableSLOsDelegate = new BeginOperationDelegate(this.OnBeginGetAvailableSLOs);
            }
            if ((this.onEndGetAvailableSLOsDelegate == null)) {
                this.onEndGetAvailableSLOsDelegate = new EndOperationDelegate(this.OnEndGetAvailableSLOs);
            }
            if ((this.onGetAvailableSLOsCompletedDelegate == null)) {
                this.onGetAvailableSLOsCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetAvailableSLOsCompleted);
            }
            base.InvokeAsync(this.onBeginGetAvailableSLOsDelegate, new object[] {
                        userId}, this.onEndGetAvailableSLOsDelegate, this.onGetAvailableSLOsCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult VCS.SLORepositoryService.ISLORepositoryService.BeginGetSLOById(string resId, string userId, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetSLOById(resId, userId, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        VCS.SLO VCS.SLORepositoryService.ISLORepositoryService.EndGetSLOById(System.IAsyncResult result) {
            return base.Channel.EndGetSLOById(result);
        }
        
        private System.IAsyncResult OnBeginGetSLOById(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string resId = ((string)(inValues[0]));
            string userId = ((string)(inValues[1]));
            return ((VCS.SLORepositoryService.ISLORepositoryService)(this)).BeginGetSLOById(resId, userId, callback, asyncState);
        }
        
        private object[] OnEndGetSLOById(System.IAsyncResult result) {
            VCS.SLO retVal = ((VCS.SLORepositoryService.ISLORepositoryService)(this)).EndGetSLOById(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetSLOByIdCompleted(object state) {
            if ((this.GetSLOByIdCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetSLOByIdCompleted(this, new GetSLOByIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetSLOByIdAsync(string resId, string userId) {
            this.GetSLOByIdAsync(resId, userId, null);
        }
        
        public void GetSLOByIdAsync(string resId, string userId, object userState) {
            if ((this.onBeginGetSLOByIdDelegate == null)) {
                this.onBeginGetSLOByIdDelegate = new BeginOperationDelegate(this.OnBeginGetSLOById);
            }
            if ((this.onEndGetSLOByIdDelegate == null)) {
                this.onEndGetSLOByIdDelegate = new EndOperationDelegate(this.OnEndGetSLOById);
            }
            if ((this.onGetSLOByIdCompletedDelegate == null)) {
                this.onGetSLOByIdCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetSLOByIdCompleted);
            }
            base.InvokeAsync(this.onBeginGetSLOByIdDelegate, new object[] {
                        resId,
                        userId}, this.onEndGetSLOByIdDelegate, this.onGetSLOByIdCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult VCS.SLORepositoryService.ISLORepositoryService.BeginCreateSLOFromCollaborativeSession(string csid, string csthread, string source, string name, bool automaticCategorization, string userId, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginCreateSLOFromCollaborativeSession(csid, csthread, source, name, automaticCategorization, userId, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        string VCS.SLORepositoryService.ISLORepositoryService.EndCreateSLOFromCollaborativeSession(out string error, System.IAsyncResult result) {
            return base.Channel.EndCreateSLOFromCollaborativeSession(out error, result);
        }
        
        private System.IAsyncResult OnBeginCreateSLOFromCollaborativeSession(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string csid = ((string)(inValues[0]));
            string csthread = ((string)(inValues[1]));
            string source = ((string)(inValues[2]));
            string name = ((string)(inValues[3]));
            bool automaticCategorization = ((bool)(inValues[4]));
            string userId = ((string)(inValues[5]));
            return ((VCS.SLORepositoryService.ISLORepositoryService)(this)).BeginCreateSLOFromCollaborativeSession(csid, csthread, source, name, automaticCategorization, userId, callback, asyncState);
        }
        
        private object[] OnEndCreateSLOFromCollaborativeSession(System.IAsyncResult result) {
            string error = this.GetDefaultValueForInitialization<string>();
            string retVal = ((VCS.SLORepositoryService.ISLORepositoryService)(this)).EndCreateSLOFromCollaborativeSession(out error, result);
            return new object[] {
                    error,
                    retVal};
        }
        
        private void OnCreateSLOFromCollaborativeSessionCompleted(object state) {
            if ((this.CreateSLOFromCollaborativeSessionCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CreateSLOFromCollaborativeSessionCompleted(this, new CreateSLOFromCollaborativeSessionCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CreateSLOFromCollaborativeSessionAsync(string csid, string csthread, string source, string name, bool automaticCategorization, string userId) {
            this.CreateSLOFromCollaborativeSessionAsync(csid, csthread, source, name, automaticCategorization, userId, null);
        }
        
        public void CreateSLOFromCollaborativeSessionAsync(string csid, string csthread, string source, string name, bool automaticCategorization, string userId, object userState) {
            if ((this.onBeginCreateSLOFromCollaborativeSessionDelegate == null)) {
                this.onBeginCreateSLOFromCollaborativeSessionDelegate = new BeginOperationDelegate(this.OnBeginCreateSLOFromCollaborativeSession);
            }
            if ((this.onEndCreateSLOFromCollaborativeSessionDelegate == null)) {
                this.onEndCreateSLOFromCollaborativeSessionDelegate = new EndOperationDelegate(this.OnEndCreateSLOFromCollaborativeSession);
            }
            if ((this.onCreateSLOFromCollaborativeSessionCompletedDelegate == null)) {
                this.onCreateSLOFromCollaborativeSessionCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCreateSLOFromCollaborativeSessionCompleted);
            }
            base.InvokeAsync(this.onBeginCreateSLOFromCollaborativeSessionDelegate, new object[] {
                        csid,
                        csthread,
                        source,
                        name,
                        automaticCategorization,
                        userId}, this.onEndCreateSLOFromCollaborativeSessionDelegate, this.onCreateSLOFromCollaborativeSessionCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult VCS.SLORepositoryService.ISLORepositoryService.BeginInsertSLO(VCS.SLO slo, string userId, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginInsertSLO(slo, userId, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        string VCS.SLORepositoryService.ISLORepositoryService.EndInsertSLO(System.IAsyncResult result) {
            return base.Channel.EndInsertSLO(result);
        }
        
        private System.IAsyncResult OnBeginInsertSLO(object[] inValues, System.AsyncCallback callback, object asyncState) {
            VCS.SLO slo = ((VCS.SLO)(inValues[0]));
            string userId = ((string)(inValues[1]));
            return ((VCS.SLORepositoryService.ISLORepositoryService)(this)).BeginInsertSLO(slo, userId, callback, asyncState);
        }
        
        private object[] OnEndInsertSLO(System.IAsyncResult result) {
            string retVal = ((VCS.SLORepositoryService.ISLORepositoryService)(this)).EndInsertSLO(result);
            return new object[] {
                    retVal};
        }
        
        private void OnInsertSLOCompleted(object state) {
            if ((this.InsertSLOCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.InsertSLOCompleted(this, new InsertSLOCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void InsertSLOAsync(VCS.SLO slo, string userId) {
            this.InsertSLOAsync(slo, userId, null);
        }
        
        public void InsertSLOAsync(VCS.SLO slo, string userId, object userState) {
            if ((this.onBeginInsertSLODelegate == null)) {
                this.onBeginInsertSLODelegate = new BeginOperationDelegate(this.OnBeginInsertSLO);
            }
            if ((this.onEndInsertSLODelegate == null)) {
                this.onEndInsertSLODelegate = new EndOperationDelegate(this.OnEndInsertSLO);
            }
            if ((this.onInsertSLOCompletedDelegate == null)) {
                this.onInsertSLOCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnInsertSLOCompleted);
            }
            base.InvokeAsync(this.onBeginInsertSLODelegate, new object[] {
                        slo,
                        userId}, this.onEndInsertSLODelegate, this.onInsertSLOCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult VCS.SLORepositoryService.ISLORepositoryService.BeginUpdateSLO(VCS.SLO slo, string userId, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginUpdateSLO(slo, userId, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        string VCS.SLORepositoryService.ISLORepositoryService.EndUpdateSLO(System.IAsyncResult result) {
            return base.Channel.EndUpdateSLO(result);
        }
        
        private System.IAsyncResult OnBeginUpdateSLO(object[] inValues, System.AsyncCallback callback, object asyncState) {
            VCS.SLO slo = ((VCS.SLO)(inValues[0]));
            string userId = ((string)(inValues[1]));
            return ((VCS.SLORepositoryService.ISLORepositoryService)(this)).BeginUpdateSLO(slo, userId, callback, asyncState);
        }
        
        private object[] OnEndUpdateSLO(System.IAsyncResult result) {
            string retVal = ((VCS.SLORepositoryService.ISLORepositoryService)(this)).EndUpdateSLO(result);
            return new object[] {
                    retVal};
        }
        
        private void OnUpdateSLOCompleted(object state) {
            if ((this.UpdateSLOCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.UpdateSLOCompleted(this, new UpdateSLOCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void UpdateSLOAsync(VCS.SLO slo, string userId) {
            this.UpdateSLOAsync(slo, userId, null);
        }
        
        public void UpdateSLOAsync(VCS.SLO slo, string userId, object userState) {
            if ((this.onBeginUpdateSLODelegate == null)) {
                this.onBeginUpdateSLODelegate = new BeginOperationDelegate(this.OnBeginUpdateSLO);
            }
            if ((this.onEndUpdateSLODelegate == null)) {
                this.onEndUpdateSLODelegate = new EndOperationDelegate(this.OnEndUpdateSLO);
            }
            if ((this.onUpdateSLOCompletedDelegate == null)) {
                this.onUpdateSLOCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnUpdateSLOCompleted);
            }
            base.InvokeAsync(this.onBeginUpdateSLODelegate, new object[] {
                        slo,
                        userId}, this.onEndUpdateSLODelegate, this.onUpdateSLOCompletedDelegate, userState);
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
        
        protected override VCS.SLORepositoryService.ISLORepositoryService CreateChannel() {
            return new SLORepositoryServiceClientChannel(this);
        }
        
        private class SLORepositoryServiceClientChannel : ChannelBase<VCS.SLORepositoryService.ISLORepositoryService>, VCS.SLORepositoryService.ISLORepositoryService {
            
            public SLORepositoryServiceClientChannel(System.ServiceModel.ClientBase<VCS.SLORepositoryService.ISLORepositoryService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginGetAvailableSLOs(string userId, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = userId;
                System.IAsyncResult _result = base.BeginInvoke("GetAvailableSLOs", _args, callback, asyncState);
                return _result;
            }
            
            public System.Collections.ObjectModel.ObservableCollection<VCS.SLODescriptor> EndGetAvailableSLOs(System.IAsyncResult result) {
                object[] _args = new object[0];
                System.Collections.ObjectModel.ObservableCollection<VCS.SLODescriptor> _result = ((System.Collections.ObjectModel.ObservableCollection<VCS.SLODescriptor>)(base.EndInvoke("GetAvailableSLOs", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginGetSLOById(string resId, string userId, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[2];
                _args[0] = resId;
                _args[1] = userId;
                System.IAsyncResult _result = base.BeginInvoke("GetSLOById", _args, callback, asyncState);
                return _result;
            }
            
            public VCS.SLO EndGetSLOById(System.IAsyncResult result) {
                object[] _args = new object[0];
                VCS.SLO _result = ((VCS.SLO)(base.EndInvoke("GetSLOById", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginCreateSLOFromCollaborativeSession(string csid, string csthread, string source, string name, bool automaticCategorization, string userId, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[6];
                _args[0] = csid;
                _args[1] = csthread;
                _args[2] = source;
                _args[3] = name;
                _args[4] = automaticCategorization;
                _args[5] = userId;
                System.IAsyncResult _result = base.BeginInvoke("CreateSLOFromCollaborativeSession", _args, callback, asyncState);
                return _result;
            }
            
            public string EndCreateSLOFromCollaborativeSession(out string error, System.IAsyncResult result) {
                object[] _args = new object[1];
                string _result = ((string)(base.EndInvoke("CreateSLOFromCollaborativeSession", _args, result)));
                error = ((string)(_args[0]));
                return _result;
            }
            
            public System.IAsyncResult BeginInsertSLO(VCS.SLO slo, string userId, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[2];
                _args[0] = slo;
                _args[1] = userId;
                System.IAsyncResult _result = base.BeginInvoke("InsertSLO", _args, callback, asyncState);
                return _result;
            }
            
            public string EndInsertSLO(System.IAsyncResult result) {
                object[] _args = new object[0];
                string _result = ((string)(base.EndInvoke("InsertSLO", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginUpdateSLO(VCS.SLO slo, string userId, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[2];
                _args[0] = slo;
                _args[1] = userId;
                System.IAsyncResult _result = base.BeginInvoke("UpdateSLO", _args, callback, asyncState);
                return _result;
            }
            
            public string EndUpdateSLO(System.IAsyncResult result) {
                object[] _args = new object[0];
                string _result = ((string)(base.EndInvoke("UpdateSLO", _args, result)));
                return _result;
            }
        }
    }
}
