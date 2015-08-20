using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace KitchenPC
{
    /// <summary>
    /// Represents a user identity within a KitchenPC context.
    /// This identity can be stored locally or within the KitchenPC network.
    /// </summary>  
    public class AuthIdentity : IIdentity
    {
        public Guid UserId { get; private set; }

        public string Name { get; private set; }

        public AuthIdentity()
        {
        }

        public AuthIdentity(Guid id, string name)
        {
            this.UserId = id;
            this.Name = name;
        }

        public string AuthenticationType
        {
            get
            {
                return "KPCAuth";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return UserId != Guid.Empty;
            }
        }

        public static string CreateHash(string value)
        {
            var hasher = MD5.Create();
            var bytes = hasher.ComputeHash(Encoding.Unicode.GetBytes(value));
            return Convert.ToBase64String(bytes);
        }

        public override string ToString()
        {
            return this.IsAuthenticated ? String.Format("{0} ({1})", this.Name, this.UserId) : "<Anonymous>";
        }

        public static Byte[] Serialize(AuthIdentity identity)
        {
            var userId = identity.UserId.ToByteArray();
            var name = Encoding.UTF8.GetBytes(identity.Name);

            return userId.Concat(name).ToArray();
        }

        public static AuthIdentity Deserialize(Byte[] bytes)
        {
            if (bytes.Length < 17) throw new ArgumentException("AuthIdentity must be at least 17 bytes.");

            var userId = new byte[16];
            var name = new byte[bytes.Length - 16];

            Buffer.BlockCopy(bytes, 0, userId, 0, 16);
            Buffer.BlockCopy(bytes, 16, name, 0, name.Length);

            return new AuthIdentity(new Guid(userId), Encoding.UTF8.GetString(name));
        }
    }
}