using System.Linq;

namespace NotifierShared.Logic {
    public static class NetHelper {
        public static string ResolceNameToIp(string name) {
            System.Console.WriteLine("resolve " + name);
            var ip = System.Net.Dns.GetHostEntryAsync(name).Result;
            System.Console.WriteLine("resolve found " + string.Join(", ", ip.AddressList.Select(a=>a.ToString())));
            return ip.AddressList[0].ToString();
        }
    }
}
