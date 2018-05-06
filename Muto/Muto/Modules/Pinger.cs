using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Diagnostics;

namespace Muto.Modules {
    public class Pinger : ModuleBase<SocketCommandContext> {
        string[] IPREBOOT = { /*"8.31.99.161"*/ "google.com", "8.31.99.135", "8.31.99.237", "8.31.99.136",
        "8.31.99.162", "8.31.99.137","8.31.99.195", "8.31.99.138", "8.31.99.163",
        "8.31.99.144", "8.31.99.196", "8.31.99.145", "8.31.99.157", "8.31.99.146",
        "8.31.99.197", "8.31.99.158", "8.31.99.147", "8.31.99.148", "8.31.99.150",
        "8.31.99.149"};
        [Command("server"), Summary("Finds ping with lowest server on Reboot")]
        public async Task PingTest() {
            string bestServer = PingIP(IPREBOOT, 8585);
            var name = Context.User.Mention;
            await Context.Channel.SendMessageAsync($"{name}, {bestServer}");
        }
        public string PingIP(string[] IP, int portNum) {
            long[] response = new long[19];
            long responseSmallest = 9999999;
            string fastestServer = "";
            /*Ping pingSender = new Ping();
            PingOptions options = new PingOptions(); //?
            //options.DontFragment = true; //change fragment behavior

            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"; //32 bit data
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 30;
            */
            TcpClient tcpClient;
            var stopwatch = Stopwatch.StartNew();
    
            for (int i = 0; i < IP.Length; i++) {
                stopwatch.Restart();
                try {
                    tcpClient = new TcpClient();
                    tcpClient.Connect(IP[i], portNum);
                    if(tcpClient.Connected == true) {
                        stopwatch.Stop();
                        var timeElapsed = stopwatch.ElapsedMilliseconds;
                        tcpClient.GetStream().Close();
                        response[i] = timeElapsed;
                    }
                }
                catch (Exception e) {
                    Console.WriteLine($"SERVER: {i + 1} ISSUE");
                    Console.WriteLine(e.StackTrace);
                } 
            }
          

            for(int i = 0; i < response.Length; i++) {
                if (response[i] < responseSmallest && (response[i] != 0)) {
                    responseSmallest = response[i];
                    fastestServer = ($"The fastest server is Server {i + 1} ({responseSmallest} ms)");
                }
            }
            return fastestServer;
        }
    }
}

