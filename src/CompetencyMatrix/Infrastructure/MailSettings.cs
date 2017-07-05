using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.Infrastructure
{
    public class MailSettings
    {
        public MailSettings(IEnumerable<IConfigurationSection> configuration)
        {
            foreach (var item in configuration)
            {
                switch (item.Key)
                {
                    case "Host":
                        Host = item.Value;
                        break;
                    case "Port":
                        Port = int.Parse(item.Value);
                        break;
                    case "Login":
                        Login = item.Value;
                        break;
                    case "Password":
                        Password = item.Value;
                        break;

                    default:
                        break;
                }
            }
        }

        public string Host { get; private set; }
        public int Port { get; private set; }

        public string Login { get; private set; }

        public string Password { get; private set; }
    }
}
