using System;
using System.Collections.Generic;
using Checkpoint.Crm.Core.Models.Cards;
using Resto.Front.Api.CheckpointPlugin.Properties;
using Resto.Front.Api.V5;
using Resto.Front.Api.V5.Attributes;
using Resto.Front.Api.V5.Extensions;
using Resto.Front.Api.V5.UI;

namespace Resto.Front.Api.CheckpointPlugin
{
    [PluginLicenseModuleId(21016318)]
    public sealed class CheckpointPlugin : IFrontPlugin
    {
        private IExternalPaymentProcessor _processor;

        private bool _disposed;
        private List<IDisposable> _disposables = new List<IDisposable>();

        public CheckpointPlugin()
        {
            PluginContext.Log.Info("Инициализируется CheckpoingPlugin");
            
            _processor = new CheckpointPaymentProcessor();

            PluginContext.Log.Info("Регистрируется тип оплаты");
            _disposables.Add(PluginContext.Operations.RegisterPaymentSystem(_processor));
        }
        
        ~CheckpointPlugin() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            PluginContext.Log.Info("CheckpointPlugin.Dispose");

            if (_disposed) return;

            if (disposing)
            {
                _disposables.ForEach(d => d.Dispose());
                _disposables.Clear();
            }

            _disposed = true;
        }
    }
}
