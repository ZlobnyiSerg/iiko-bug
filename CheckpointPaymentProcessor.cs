using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Checkpoint.Crm.Core.Exceptions;
using Checkpoint.Crm.Core.Models.Cards;
using Resto.Front.Api.CheckpointPlugin.Properties;
using Resto.Front.Api.V5;
using Resto.Front.Api.V5.Data.Orders;
using Resto.Front.Api.V5.Data.Organization;
using Resto.Front.Api.V5.Data.Payments;
using Resto.Front.Api.V5.Data.Security;
using Resto.Front.Api.V5.Exceptions;
using Resto.Front.Api.V5.Extensions;
using Resto.Front.Api.V5.UI;

namespace Resto.Front.Api.CheckpointPlugin
{
    public sealed class CheckpointPaymentProcessor : MarshalByRefObject, IExternalPaymentProcessor, IDisposable
    {
        private bool _disposed;
        private List<IDisposable> _disposables = new List<IDisposable>();

        public string PaymentSystemKey => "Checkpoint";
        public string PaymentSystemName => "Checkpoint";

        internal CheckpointPaymentProcessor()
        {
            _disposables.Add(PluginContext.Notifications.OrderChanged.Subscribe(new OrderChangeObserver(this)));
        }

        public void CollectData(Guid orderId, Guid paymentTypeId, IUser cashier, IReceiptPrinter printer, IViewManager viewManager, IPaymentDataContext context,
            IProgressBar progressBar)
        {
            PluginContext.Log.Info($"=== CollectData");
            var bonusAction = viewManager.ShowChooserPopup("Logus CRM", new List<string>
            {
                $"Проверка платёжного ср-ва"
            });
        }

        public void OnPaymentAdded(IOrder order, IPaymentItem paymentItem, IUser cashier, IOperationService operations, IReceiptPrinter printer,
            IViewManager viewManager, IPaymentDataContext context, IProgressBar progressBar)
        {
            PluginContext.Log.Info($"=== OnPaymentAdded");
        }

        public bool OnPreliminaryPaymentEditing(IOrder order, IPaymentItem paymentItem, IUser cashier, IOperationService operationService,
            IReceiptPrinter printer, IViewManager viewManager, IPaymentDataContext context, IProgressBar progressBar)
        {
            PluginContext.Log.Info($"=== OnPreliminaryPaymentEditing");
            return true;
        }

        public void Pay(decimal sum, Guid? orderId, Guid paymentTypeId, Guid transactionId, IPointOfSale pointOfSale, IUser cashier, IReceiptPrinter printer,
            IViewManager viewManager, IPaymentDataContext context, IProgressBar progressBar)
        {
            PluginContext.Log.Info($"=== Pay {sum}");
        }

        public void EmergencyCancelPayment(decimal sum, Guid? orderId, Guid paymentTypeId, Guid transactionId, IPointOfSale pointOfSale, IUser cashier,
            IReceiptPrinter printer, IViewManager viewManager, IPaymentDataContext context, IProgressBar progressBar)
        {
            PluginContext.Log.Info($"=== EmergencyCancelPayment {sum}");
        }

        public void ReturnPayment(decimal sum, Guid? orderId, Guid paymentTypeId, Guid transactionId, IPointOfSale pointOfSale, IUser cashier,
            IReceiptPrinter printer, IViewManager viewManager, IPaymentDataContext context, IProgressBar progressBar)
        {
            PluginContext.Log.Info($"=== ReturnPayment {sum}");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _disposables.ForEach(d => d.Dispose());
                _disposables.Clear();
            }

            _disposed = true;
        }

        protected class OrderChangeObserver : IObserver<IOrder>
        {
            private readonly CheckpointPaymentProcessor _checkpointPaymentProcessor;

            public OrderChangeObserver(CheckpointPaymentProcessor checkpointPaymentProcessor)
            {
                _checkpointPaymentProcessor = checkpointPaymentProcessor;
            }

            public void OnNext(IOrder order)
            {
                PluginContext.Log.Info("OnNext");
            }

            public void OnError(Exception error)
            {
                PluginContext.Log.Error("Error on order change", error);
            }

            public void OnCompleted()
            {
                PluginContext.Log.Info("Order observer completed");
            }
        }
    }
}