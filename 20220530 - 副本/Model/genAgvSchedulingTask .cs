using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Dispatch.Model
{
    public class genAgvSchedulingTask
    {
        private string reqCode;
        private string reqTime;
        private string clientCode;
        private string tokenCode;
        private string taskTyp;
        private string ctnrTyp;
        private string ctnrCode;
        private string webCode;
        private List<positionCodePath> positionCodePath;
        private string podCode;
        private string podDir;
        private string podTyp;
        private string materialLot;
        private string priority;
        private string agvCode;
        private string taskCode;
        private string data;




        public string ReqCode { get => reqCode; set => reqCode = value; }
        public string ReqTime { get => reqTime; set => reqTime = value; }
        public string ClientCode { get => clientCode; set => clientCode = value; }
        public string TokenCode { get => tokenCode; set => tokenCode = value; }
        public string TaskTyp { get => taskTyp; set => taskTyp = value; }
        public string CtnrTyp { get => ctnrTyp; set => ctnrTyp = value; }
        public string CtnrCode { get => ctnrCode; set => ctnrCode = value; }
        public string WebCode { get => webCode; set => webCode = value; }
        public List<positionCodePath> PositionCodePath { get => positionCodePath; set => positionCodePath = value; }
        public string PodCode { get => podCode; set => podCode = value; }
        public string PodDir { get => podDir; set => podDir = value; }
        public string PodTyp { get => podTyp; set => podTyp = value; }
        public string MaterialLot { get => materialLot; set => materialLot = value; }
        public string Priority { get => priority; set => priority = value; }
        public string AgvCode { get => agvCode; set => agvCode = value; }
        public string TaskCode { get => taskCode; set => taskCode = value; }
        public string Data { get => data; set => data = value; }

    }

    public class positionCodePath 
    {
        private string positionCode;
        private string type;

        public string PositionCode { get => positionCode; set => positionCode = value; }
        public string Type { get => type; set => type = value; }
    }

}
