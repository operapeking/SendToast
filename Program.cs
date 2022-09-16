using Microsoft.Toolkit.Uwp.Notifications;
using System.Net.Http.Json;

class SendToast
{
    struct Status
    {
        public DateTime time { get; set; }
        public string ip { get; set; }
        public bool isComing { get; set; }
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
        int gone = 0;
        for (; ; )
        {
            try
            {
                var result = await client.GetFromJsonAsync<Status>(url);
                if (result.isComing && gone != 2)
                {
                    new ToastContentBuilder().AddText("Wrong Answer")
                        .AddText($"{result.time.ToShortTimeString()} from {result.ip}")
                        .Show();
                    gone = 2;
                }
                else if (!result.isComing && gone != 1)
                {
                    new ToastContentBuilder().AddText("Accepted")
                        .AddText($"{result.time.ToShortTimeString()} from {result.ip}")
                        .Show();
                    gone = 1;
                }
                Thread.Sleep(50);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}