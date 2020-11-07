﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PasteMystNet.Internals;

namespace PasteMystNet
{

    public class PasteMystPaste
    {

        private const string GetPasteEndpoint = "https://paste.myst.rs/api/v2/paste/{0}";
        private const string DeletePasteEndpoint = "https://paste.myst.rs/api/v2/paste/{0}";

        [JsonProperty(PropertyName = "createdAt")] private readonly long _createdAt;
        [JsonProperty(PropertyName = "expiresIn")] private readonly string _expiresIn;
        [JsonProperty(PropertyName = "deletesAt")] private readonly long _deletesAt;

        [JsonProperty(PropertyName = "_id")] public string Id { get; private set; }
        [JsonProperty(PropertyName = "ownerId")] public string OwnerId { get; private set; }
        [JsonProperty(PropertyName = "title")] public string Title { get; private set; }
        [JsonProperty(PropertyName = "stars")] public uint Stars { get; private set; }
        [JsonProperty(PropertyName = "isPrivate")] public bool IsPrivate { get; private set; }
        [JsonProperty(PropertyName = "isPublic")] public bool IsPublic { get; private set; }
        [JsonProperty(PropertyName = "tags")] public string[] Tags { get; private set; }
        [JsonProperty(PropertyName = "pasties")] public PasteMystPasty[] Pasties { get; private set; }
        [JsonProperty(PropertyName = "edits")] public PasteMystEdit[] Edits { get; private set; }
        [JsonIgnore] public bool HasOwner => !string.IsNullOrEmpty(OwnerId);
        [JsonIgnore] public DateTime CreationTime => DateTimeOffset.FromUnixTimeSeconds(_createdAt).DateTime;
        [JsonIgnore] public PasteMystExpiration ExpireDuration => ParseExpiration(_expiresIn);
        [JsonIgnore] public DateTime DeletionTime => DateTimeOffset.FromUnixTimeSeconds(_deletesAt).DateTime;

        private PasteMystExpiration ParseExpiration(string expiration)
        {
            return Enum.GetValues(typeof(PasteMystExpiration)).Cast<PasteMystExpiration>().FirstOrDefault(item => item.GetStringRepresentation() == expiration);
        }

        public static async Task<PasteMystPaste> GetPasteAsync(string id, PasteMystAuth auth = null)
        {
            using var client = new HttpClient();
            if (auth != null)
                client.DefaultRequestHeaders.Authorization = auth.CreateAuthorization();
            var response = await client.GetAsync(string.Format(GetPasteEndpoint, id));
            if (response.StatusCode != HttpStatusCode.OK)
                return null;
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PasteMystPaste>(content);
        }

        public static async Task<bool> DeletePasteAsync(string id, PasteMystAuth auth)
        {
            if (auth == null)
                throw new ArgumentNullException(nameof(auth));
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = auth.CreateAuthorization();
            var response = await client.DeleteAsync(string.Format(DeletePasteEndpoint, id));
            return response.StatusCode == HttpStatusCode.OK;
        }
        
    }

}