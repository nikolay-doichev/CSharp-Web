using System;
using System.Text;

namespace SIS.HTTP
{
    public class ResponseCookie : Cookie
    {
        public ResponseCookie(string name, string value)
            : base(name, value)
        {
            this.Path = "/";
            this.SameSite = SameSiteType.None;
            this.Expires = DateTime.UtcNow.AddDays(30);
        }
        public string Domain { get; set; }
        public string Path { get; set; }
        public DateTime? Expires { get; set; }
        public long? MaxAge { get; set; }
        public bool Secure { get; set; }
        public bool HttpOnly { get; set; }
        public SameSiteType SameSite { get; set; }

        public override string ToString()
        {
            StringBuilder coockieBuilder = new StringBuilder();
            coockieBuilder.Append($"{this.Name}={this.Value}");
            if (this.MaxAge.HasValue)
            {
                coockieBuilder.Append($"; Max-Age" + this.MaxAge.Value.ToString());
            }
            else if (this.Expires.HasValue)
            {
                coockieBuilder.Append($"; Expires=" + this.Expires.Value.ToString("R"));
            }
            if (!string.IsNullOrWhiteSpace(this.Domain))
            {
                coockieBuilder.Append($"; Domain=" + this.Domain);
            }

            if (!string.IsNullOrWhiteSpace(this.Path))
            {
                coockieBuilder.Append($"; Path=" + this.Path);
            }

            if (this.Secure)
            {
                coockieBuilder.Append("; Secure");
            }

            if (this.HttpOnly)
            {
                coockieBuilder.Append("; HttpOnly");
            }

            coockieBuilder.Append("; SameSite" + this.SameSite.ToString());

            return coockieBuilder.ToString();
        }
    }
}
