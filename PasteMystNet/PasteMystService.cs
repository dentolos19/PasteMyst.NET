﻿using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace PasteMystNet
{
    
    public static class PasteMystService
    {

        public static PasteMystInfo Post(PasteMystForm form)
        {
            // todo
        }

        private static PasteMystInfoJson PostJson(PasteMystFormJson form)
        {
            var json = JsonConvert.SerializeObject(form);
            var request = WebRequest.Create("https://paste.myst.rs/paste");
            request.ContentType = "application/json";
            request.Method = "POST";
            using var writer = new StreamWriter(request.GetRequestStream());
            writer.Write(json);
            var response = (HttpWebResponse)request.GetResponse();
            using var reader = new StreamReader(response.GetResponseStream()!);
            var data = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<PasteMystInfoJson>(data);
        }

        public static PasteMystInfo Get(string id)
        {
            var data = GetJson(id);
            var info = new PasteMystInfo();
            // todo
            return info;
        }

        private static PasteMystInfoJson GetJson(string id)
        {
            // todo
        }

    }
    
}