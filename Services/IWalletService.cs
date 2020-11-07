using BeanlancerAPI.Services;
using BeanlancerAPI2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BeanlancerAPI2.Services{

    public interface IWalletService : IBaseService<Wallet, int>
    {
         Wallet CreateWallet(Wallet wallet);

         Wallet UpdateWallet(int id, Wallet wallet);

         Wallet DeleteWallet(int id);

         Wallet GetWalletById(int id);
         IEnumerable<Wallet> GetAllWallet();

         Wallet GetWalletByUsername(int username);
            
    }


    public class WalletSerice : BaseService<Wallet, int>, IWalletService
    {
        public WalletSerice(BeanlancersContext context) : base(context)
        {
        }

        public Wallet CreateWallet(Wallet wallet)
        {
            wallet.BeanAmout = 0;
            return Create(wallet);
        }

        public Wallet DeleteWallet(int id)
        {
            return DeleteById(id);
        }

        public IEnumerable<Wallet> GetAllWallet()
        {
            return GetAllNonPaging();
        }

        public Wallet GetWalletById(int id)
        {
            return GetWalletById(id);
        }

        public Wallet GetWalletByUsername(int username)
        {
            return GetByExpress(a => a.Username == username);
        }

        public Wallet UpdateWallet(int id, Wallet wallet)
        {
            return Update(id,wallet);
        }
    }
}