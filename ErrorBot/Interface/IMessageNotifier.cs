using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorBot.Interface
{
    public interface IMessageNotifier
    {
        Task NotifyAsync(string message, CancellationToken cancellationToken);
    }
}
