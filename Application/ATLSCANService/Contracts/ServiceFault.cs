using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ATLSCANService.Contracts
{
    [DataContract]
    public class ServiceFault
    {
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string Details { get; set; }
    }
}