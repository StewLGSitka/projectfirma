﻿/*-----------------------------------------------------------------------
<copyright file="Reference.cs" company="Tahoe Regional Planning Agency and Sitka Technology Group">
Copyright (c) Tahoe Regional Planning Agency and Sitka Technology Group. All rights reserved.
<author>Sitka Technology Group</author>
</copyright>

<license>
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License <http://www.gnu.org/licenses/> for more details.

Source code is available upon request via <support@sitkatech.com>.
</license>
-----------------------------------------------------------------------*/
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectFirma.Web.KeystoneDataService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Organization", Namespace="http://schemas.datacontract.org/2004/07/Keystone.Web.DataService")]
    [System.SerializableAttribute()]
    public partial class Organization : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FullNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsActiveField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid OrganizationGuidField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ShortNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string URLField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime UpdateDateField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FullName {
            get {
                return this.FullNameField;
            }
            set {
                if ((object.ReferenceEquals(this.FullNameField, value) != true)) {
                    this.FullNameField = value;
                    this.RaisePropertyChanged("FullName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsActive {
            get {
                return this.IsActiveField;
            }
            set {
                if ((this.IsActiveField.Equals(value) != true)) {
                    this.IsActiveField = value;
                    this.RaisePropertyChanged("IsActive");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid OrganizationGuid {
            get {
                return this.OrganizationGuidField;
            }
            set {
                if ((this.OrganizationGuidField.Equals(value) != true)) {
                    this.OrganizationGuidField = value;
                    this.RaisePropertyChanged("OrganizationGuid");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ShortName {
            get {
                return this.ShortNameField;
            }
            set {
                if ((object.ReferenceEquals(this.ShortNameField, value) != true)) {
                    this.ShortNameField = value;
                    this.RaisePropertyChanged("ShortName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string URL {
            get {
                return this.URLField;
            }
            set {
                if ((object.ReferenceEquals(this.URLField, value) != true)) {
                    this.URLField = value;
                    this.RaisePropertyChanged("URL");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime UpdateDate {
            get {
                return this.UpdateDateField;
            }
            set {
                if ((this.UpdateDateField.Equals(value) != true)) {
                    this.UpdateDateField = value;
                    this.RaisePropertyChanged("UpdateDate");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="UserProfile", Namespace="http://schemas.datacontract.org/2004/07/Keystone.Web.DataService")]
    [System.SerializableAttribute()]
    public partial class UserProfile : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Address1Field;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Address2Field;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BiographyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CityField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CountryField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailAlternateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FacebookURLField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FacultyURLField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FirstNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string JobTitleField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LastNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LinkedInURLField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LoginNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NamePrefixField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameSuffixField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.Guid> OrganizationGuidField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PersonalURLField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PhotoURLField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PostalCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PrimaryPhoneField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PrimaryPhoneExtensionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PublicationsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TimezoneField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TwitterURLField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid UserGuidField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Address1 {
            get {
                return this.Address1Field;
            }
            set {
                if ((object.ReferenceEquals(this.Address1Field, value) != true)) {
                    this.Address1Field = value;
                    this.RaisePropertyChanged("Address1");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Address2 {
            get {
                return this.Address2Field;
            }
            set {
                if ((object.ReferenceEquals(this.Address2Field, value) != true)) {
                    this.Address2Field = value;
                    this.RaisePropertyChanged("Address2");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Biography {
            get {
                return this.BiographyField;
            }
            set {
                if ((object.ReferenceEquals(this.BiographyField, value) != true)) {
                    this.BiographyField = value;
                    this.RaisePropertyChanged("Biography");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string City {
            get {
                return this.CityField;
            }
            set {
                if ((object.ReferenceEquals(this.CityField, value) != true)) {
                    this.CityField = value;
                    this.RaisePropertyChanged("City");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Country {
            get {
                return this.CountryField;
            }
            set {
                if ((object.ReferenceEquals(this.CountryField, value) != true)) {
                    this.CountryField = value;
                    this.RaisePropertyChanged("Country");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Email {
            get {
                return this.EmailField;
            }
            set {
                if ((object.ReferenceEquals(this.EmailField, value) != true)) {
                    this.EmailField = value;
                    this.RaisePropertyChanged("Email");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EmailAlternate {
            get {
                return this.EmailAlternateField;
            }
            set {
                if ((object.ReferenceEquals(this.EmailAlternateField, value) != true)) {
                    this.EmailAlternateField = value;
                    this.RaisePropertyChanged("EmailAlternate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FacebookURL {
            get {
                return this.FacebookURLField;
            }
            set {
                if ((object.ReferenceEquals(this.FacebookURLField, value) != true)) {
                    this.FacebookURLField = value;
                    this.RaisePropertyChanged("FacebookURL");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FacultyURL {
            get {
                return this.FacultyURLField;
            }
            set {
                if ((object.ReferenceEquals(this.FacultyURLField, value) != true)) {
                    this.FacultyURLField = value;
                    this.RaisePropertyChanged("FacultyURL");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FirstName {
            get {
                return this.FirstNameField;
            }
            set {
                if ((object.ReferenceEquals(this.FirstNameField, value) != true)) {
                    this.FirstNameField = value;
                    this.RaisePropertyChanged("FirstName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string JobTitle {
            get {
                return this.JobTitleField;
            }
            set {
                if ((object.ReferenceEquals(this.JobTitleField, value) != true)) {
                    this.JobTitleField = value;
                    this.RaisePropertyChanged("JobTitle");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LastName {
            get {
                return this.LastNameField;
            }
            set {
                if ((object.ReferenceEquals(this.LastNameField, value) != true)) {
                    this.LastNameField = value;
                    this.RaisePropertyChanged("LastName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LinkedInURL {
            get {
                return this.LinkedInURLField;
            }
            set {
                if ((object.ReferenceEquals(this.LinkedInURLField, value) != true)) {
                    this.LinkedInURLField = value;
                    this.RaisePropertyChanged("LinkedInURL");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LoginName {
            get {
                return this.LoginNameField;
            }
            set {
                if ((object.ReferenceEquals(this.LoginNameField, value) != true)) {
                    this.LoginNameField = value;
                    this.RaisePropertyChanged("LoginName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NamePrefix {
            get {
                return this.NamePrefixField;
            }
            set {
                if ((object.ReferenceEquals(this.NamePrefixField, value) != true)) {
                    this.NamePrefixField = value;
                    this.RaisePropertyChanged("NamePrefix");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NameSuffix {
            get {
                return this.NameSuffixField;
            }
            set {
                if ((object.ReferenceEquals(this.NameSuffixField, value) != true)) {
                    this.NameSuffixField = value;
                    this.RaisePropertyChanged("NameSuffix");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.Guid> OrganizationGuid {
            get {
                return this.OrganizationGuidField;
            }
            set {
                if ((this.OrganizationGuidField.Equals(value) != true)) {
                    this.OrganizationGuidField = value;
                    this.RaisePropertyChanged("OrganizationGuid");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PersonalURL {
            get {
                return this.PersonalURLField;
            }
            set {
                if ((object.ReferenceEquals(this.PersonalURLField, value) != true)) {
                    this.PersonalURLField = value;
                    this.RaisePropertyChanged("PersonalURL");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PhotoURL {
            get {
                return this.PhotoURLField;
            }
            set {
                if ((object.ReferenceEquals(this.PhotoURLField, value) != true)) {
                    this.PhotoURLField = value;
                    this.RaisePropertyChanged("PhotoURL");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PostalCode {
            get {
                return this.PostalCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.PostalCodeField, value) != true)) {
                    this.PostalCodeField = value;
                    this.RaisePropertyChanged("PostalCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PrimaryPhone {
            get {
                return this.PrimaryPhoneField;
            }
            set {
                if ((object.ReferenceEquals(this.PrimaryPhoneField, value) != true)) {
                    this.PrimaryPhoneField = value;
                    this.RaisePropertyChanged("PrimaryPhone");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PrimaryPhoneExtension {
            get {
                return this.PrimaryPhoneExtensionField;
            }
            set {
                if ((object.ReferenceEquals(this.PrimaryPhoneExtensionField, value) != true)) {
                    this.PrimaryPhoneExtensionField = value;
                    this.RaisePropertyChanged("PrimaryPhoneExtension");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Publications {
            get {
                return this.PublicationsField;
            }
            set {
                if ((object.ReferenceEquals(this.PublicationsField, value) != true)) {
                    this.PublicationsField = value;
                    this.RaisePropertyChanged("Publications");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string State {
            get {
                return this.StateField;
            }
            set {
                if ((object.ReferenceEquals(this.StateField, value) != true)) {
                    this.StateField = value;
                    this.RaisePropertyChanged("State");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Timezone {
            get {
                return this.TimezoneField;
            }
            set {
                if ((object.ReferenceEquals(this.TimezoneField, value) != true)) {
                    this.TimezoneField = value;
                    this.RaisePropertyChanged("Timezone");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TwitterURL {
            get {
                return this.TwitterURLField;
            }
            set {
                if ((object.ReferenceEquals(this.TwitterURLField, value) != true)) {
                    this.TwitterURLField = value;
                    this.RaisePropertyChanged("TwitterURL");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid UserGuid {
            get {
                return this.UserGuidField;
            }
            set {
                if ((this.UserGuidField.Equals(value) != true)) {
                    this.UserGuidField = value;
                    this.RaisePropertyChanged("UserGuid");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="KeystoneDataService.IKeystoneData")]
    public interface IKeystoneData {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKeystoneData/GetOrganizations", ReplyAction="http://tempuri.org/IKeystoneData/GetOrganizationsResponse")]
        ProjectFirma.Web.KeystoneDataService.Organization[] GetOrganizations(System.Guid applicationGuid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKeystoneData/GetOrganization", ReplyAction="http://tempuri.org/IKeystoneData/GetOrganizationResponse")]
        ProjectFirma.Web.KeystoneDataService.Organization GetOrganization(System.Guid organizationIdentifier);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKeystoneData/GetUserProfile", ReplyAction="http://tempuri.org/IKeystoneData/GetUserProfileResponse")]
        ProjectFirma.Web.KeystoneDataService.UserProfile GetUserProfile(System.Guid userIdentifier);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKeystoneData/GetUserProfileByUsername", ReplyAction="http://tempuri.org/IKeystoneData/GetUserProfileByUsernameResponse")]
        ProjectFirma.Web.KeystoneDataService.UserProfile GetUserProfileByUsername(System.Guid applicationGuid, string username);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IKeystoneDataChannel : ProjectFirma.Web.KeystoneDataService.IKeystoneData, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class KeystoneDataClient : System.ServiceModel.ClientBase<ProjectFirma.Web.KeystoneDataService.IKeystoneData>, ProjectFirma.Web.KeystoneDataService.IKeystoneData {
        
        public KeystoneDataClient() {
        }
        
        public KeystoneDataClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public KeystoneDataClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public KeystoneDataClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public KeystoneDataClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public ProjectFirma.Web.KeystoneDataService.Organization[] GetOrganizations(System.Guid applicationGuid) {
            return base.Channel.GetOrganizations(applicationGuid);
        }
        
        public ProjectFirma.Web.KeystoneDataService.Organization GetOrganization(System.Guid organizationIdentifier) {
            return base.Channel.GetOrganization(organizationIdentifier);
        }
        
        public ProjectFirma.Web.KeystoneDataService.UserProfile GetUserProfile(System.Guid userIdentifier) {
            return base.Channel.GetUserProfile(userIdentifier);
        }
        
        public ProjectFirma.Web.KeystoneDataService.UserProfile GetUserProfileByUsername(System.Guid applicationGuid, string username) {
            return base.Channel.GetUserProfileByUsername(applicationGuid, username);
        }
    }
}
