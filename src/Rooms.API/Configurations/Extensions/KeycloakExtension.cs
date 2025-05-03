﻿namespace Rooms.API.Configurations.Extensions
{
    public class KeycloakExtension
    {
        public string AuthorizationUrl { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string MetadataAddress { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string TokenUrl { get; set; } = string.Empty;
        public string CurrentRealm { get; set; } = string.Empty;
    }
}