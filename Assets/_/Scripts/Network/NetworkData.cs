using System;
using System.Collections.Generic;

namespace UnityLibrary.Runtime.Network {

    [System.Serializable]
    public enum NetworkRequestType {
        GET = 0,
        POST = 1,
        PUT = 2
    }

    [System.Serializable]
    public enum NetworkResponseType {
        SUCCESS = 0,
        ERROR = 1,
        UNKNOWN = 2
    }

    [System.Serializable]
    public enum NetworkServiceType {
        NONE = -1,
        APP_LOGIN,
        APP_LICENSE,
        APP_VALIDATION
    }

    [System.Serializable]
    public enum NetworkServiceStatus {
        NONE = -1,
        DEFAULT,
        PENDING,
        COMPLETE_SUCCESSFUL,
        COMPLETE_UNSUCCESSFUL,
        CONNECTION_ERROR,
        PROTOCOL_ERROR,
        DATA_PROCESSING_ERROR
    }

    public interface IData {
        public T GetInfo<T>();
        public Type GetDataType();
    }

    public interface IDataHandler {
        public void AddData(IData data);
        public void GetData(out IData data);
    }

    public abstract class NetworkDataHandler : IDataHandler {
        public abstract void AddData(IData data);
        public abstract void GetData(out IData data);
    }

    public class NetworkMessage<T> {
        public T Content { get; set; }
    }

    public class NetworkRequest<T> : NetworkMessage<T> {
        public Uri Uri { get; set; }
        public NetworkRequestType Type { get; set; }
        public KeyValuePair<string, string> Header { get; set; }
    }

    public class NetworkResponse<T> : NetworkMessage<T> {
        public NetworkResponseType Type { get; set; }

    }
}