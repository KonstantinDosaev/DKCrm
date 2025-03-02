using Castle.Core.Smtp;
using DKCrm.Shared.Models.UserAuth;

namespace DKCrm.Server.Services
{
    public class EmailTaskService : IHostedService, IDisposable
    {
        private Timer _timer;
        public DateTime? DateTimeInit;
        // private readonly Interfaces.IEmailSender _sender;
 
        private bool isStar;


        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (DateTimeInit != null)
            {
                isStar = true;
                var time = (int)Math.Abs(((DateTime)DateTimeInit - DateTime.Now).TotalMilliseconds);

                _timer = new Timer(DoWork!, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(time));
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            if (isStar == false)
            {
                var loginRequest = new LoginRequest()
                {
                    UserName = "DK",
                    Password = "A5945a."

                };

                using (HttpClient client = new HttpClient())
                {
                    var result = await client.PostAsJsonAsync("https://localhost:7039/api/auth/login", loginRequest);
                    if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        throw new Exception(await result.Content.ReadAsStringAsync());
                    result.EnsureSuccessStatusCode();
                    var t = await client.GetAsync($"https://localhost:7039/api/Mails/SendEmailTaskScheduler");
                }
            }
            else
            {
                isStar = false;
            }
        }
        /*public async void DoWork()
        {
            var loginRequest = new LoginRequest()
            {
                UserName = "DK",
                Password = "A5945a."

            };
            var request = new SendEmailRequest
            {

                Emails = ["dosaevk@yandex.ru"],
                Message = "Test Task", Subject = "Subj test",
            };
            using (HttpClient client = new HttpClient())
            {
                var result = await client.PostAsJsonAsync("https://localhost:7039/api/auth/login", loginRequest);
                if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
                result.EnsureSuccessStatusCode();
                var t = await client.PostAsJsonAsync($"https://localhost:7039/api/Mails/SendEmail", request);
            }
        }*/
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }


    }
}

