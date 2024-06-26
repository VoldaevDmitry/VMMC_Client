﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VMMC_Import.FileLoaderServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="FileLoaderServiceReference.IFileLoaderService")]
    public interface IFileLoaderService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFileLoaderService/LoadFile", ReplyAction="http://tempuri.org/IFileLoaderService/LoadFileResponse")]
        VMMC_Core.FileLoaderServiceReference.LoadFileResult LoadFile(VMMC_Core.FileLoaderServiceReference.LoadFileInput inputData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFileLoaderService/LoadFile", ReplyAction="http://tempuri.org/IFileLoaderService/LoadFileResponse")]
        System.Threading.Tasks.Task<VMMC_Core.FileLoaderServiceReference.LoadFileResult> LoadFileAsync(VMMC_Core.FileLoaderServiceReference.LoadFileInput inputData);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IFileLoaderServiceChannel : VMMC_Import.FileLoaderServiceReference.IFileLoaderService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FileLoaderServiceClient : System.ServiceModel.ClientBase<VMMC_Import.FileLoaderServiceReference.IFileLoaderService>, VMMC_Import.FileLoaderServiceReference.IFileLoaderService {
        
        public FileLoaderServiceClient() {
        }
        
        public FileLoaderServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public FileLoaderServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FileLoaderServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FileLoaderServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public VMMC_Core.FileLoaderServiceReference.LoadFileResult LoadFile(VMMC_Core.FileLoaderServiceReference.LoadFileInput inputData) {
            return base.Channel.LoadFile(inputData);
        }
        
        public System.Threading.Tasks.Task<VMMC_Core.FileLoaderServiceReference.LoadFileResult> LoadFileAsync(VMMC_Core.FileLoaderServiceReference.LoadFileInput inputData) {
            return base.Channel.LoadFileAsync(inputData);
        }
    }
}
