using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using ATLSCANService.Contracts;
using System.ServiceModel.Web;
using System.Text;

namespace ATLSCANService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IZipService
    {

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        string ProcessZipManual(string zipFileName);

        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        string ProcessZipBatch();

        [OperationContract]
        List<string> SearchFile(string fileName);

        [OperationContract]
        byte[] DownloadFile(string relativePath);
    }

}
