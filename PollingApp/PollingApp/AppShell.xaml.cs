using PollingApp.Pages;

namespace PollingApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(SecureStorage.GetAsync("Device-Id").Result))
            {
                //var device = await _httpClient.GetFromJsonAsync<DeviceDto>("api/device/id");

                SecureStorage.SetAsync("Device-Id", "android").Wait();
            }
            Routing.RegisterRoute(nameof(PollListPage), typeof(PollListPage));
            Routing.RegisterRoute(nameof(PollDetailPage), typeof(PollDetailPage));
            Routing.RegisterRoute(nameof(PollResultPage), typeof(PollResultPage));

        }
    }
}
