using Microsoft.Toolkit.Uwp.Notifications;
using System.Net.Http.Json;

class SendToast
{
    struct Status
    {
        public DateTime time { get; set; }
        public string ip { get; set; }
        public bool isComing { get; set; }
        public string who { get; set; }
    }

    static readonly HttpClient client = new();
    static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No server provided.");
            return;
        }
        string url = args[0] + "/query";
        Status last = new();
        for (; ; )
        {
            Thread.Sleep(100);
            try
            {
                var now = await client.GetFromJsonAsync<Status>(url);
                if (now.isComing == last.isComing && now.who == last.who)
                    continue;
                if (now.isComing)
                {
                    new ToastContentBuilder().AddText("Wrong Answer")
                        .AddText($"{now.who} {now.time.ToShortTimeString()} from {now.ip}")
                        .Show();
                }
                else
                {
                    new ToastContentBuilder().AddText("Accepted")
                        .AddText($"{now.who} {now.time.ToShortTimeString()} from {now.ip}")
                        .Show();
                }
                last = now;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}