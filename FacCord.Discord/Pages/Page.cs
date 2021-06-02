using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace IsekaiTechnologies.FacCord.Discord.Pages
{
    public abstract class Page
    {
        public PageManager Manager { get; set; }

        public virtual Task OnNavigatedTo(Page sourcePage, PageManager manager, object args) { Manager = manager; return Task.CompletedTask; }
        public virtual Task OnNavigatedFrom(Page destinationPage, PageManager manager, object args) { return Task.CompletedTask; }
        public virtual Task LoadView(IMessageChannel channel) { return Task.CompletedTask; }
    }
}