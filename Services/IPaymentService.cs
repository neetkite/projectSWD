using BeanlancerAPI.Services;
using BeanlancerAPI2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeanlancerAPI2.Services
{
    public interface IPaymentService : IBaseService<Payment, int>
    {
        Payment CreatePayment(Payment payment);

        IEnumerable<Payment> GetPaymentByRequest(string request);

        IEnumerable<Payment> DeletePaymentsByIdRequest(string id);

        IEnumerable<Payment> GetAllPayment();
    }

    public class PaymentService : BaseService<Payment, int>, IPaymentService
    {
        public PaymentService(BeanlancersContext context) : base(context)
        {
        }

        public Payment CreatePayment(Payment payment)
        {
            DateTime time = DateTime.Now;
            payment.Time = time;
            return Create(payment);
        }

        public IEnumerable<Payment> DeletePaymentsByIdRequest(string id)
        {
            var list = GetPaymentByRequest(id);
            if(list != null)
            {
                for (int i = 0; i < list.Count(); i++)
                {
                    DeleteByEntity(list.ElementAt(i));
                }
                Commit();
                return GetPaymentByRequest(id);
            }
            return null;
        }

        public IEnumerable<Payment> GetAllPayment()
        {
            return GetAllNonPaging();
        }

        public IEnumerable<Payment> GetPaymentByRequest(string request)
        {
            return GetAllNonPaging(a => a.IdRequest == request);
        }
    }
}
