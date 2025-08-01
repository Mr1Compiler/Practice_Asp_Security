﻿using Newtonsoft.Json;

namespace WebApp_UnderTheHood.Authorization;

public class JwtToken
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; } = string.Empty;
    
    [JsonProperty("expires_At")]
    public DateTime ExpiresAt { get; set; }
}
