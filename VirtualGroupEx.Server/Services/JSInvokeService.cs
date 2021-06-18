using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace VirtualGroupEx.Server.Services
{
    public class JSInvokeService
    {
        private readonly IJSRuntime jSRuntime;

        public JSInvokeService(IJSRuntime jSRuntime)
        {
            this.jSRuntime = jSRuntime;
        }

        public async Task ResetButtonAsync()
        {
            await jSRuntime.InvokeVoidAsync("resetButtons");
        }

        public async Task<bool> Confirm(string message)
        {
            return await jSRuntime.InvokeAsync<bool>("confirm", message);
        }

        public async Task<string> PromptString(string message)
        {
            return await jSRuntime.InvokeAsync<string>("prompt", message);
        }

        public async Task HideModalsAsync()
        {
            await jSRuntime.InvokeVoidAsync("hideModals");
        }
    }
}
