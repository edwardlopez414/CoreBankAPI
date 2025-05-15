using CoreBankAPI.CoreDbContext;
using CoreBankAPI.Data;
using CoreBankAPI.Logic.Interfaces;
using CoreBankAPI.Models;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CoreBankAPI.Logic
{
    public class AccountManager : IAccountManager
    {
        CoreDb db;
        private static readonly Random _random = new();
        public AccountManager(CoreDb db) 
        {
            this.db = db;
        }
        public (bool, ErrorModel, BalanceResponse) balance(BalanceDto model)
        {
            ErrorModel error = new ErrorModel();
            var account = db.AccountDta
                .Where(a => a.identifier == model.identifier)
                .Select(a => a)
                .FirstOrDefault();

            BalanceResponse response = new BalanceResponse();

            if (account != null)
            {
                response = new BalanceResponse
                {
                    identifier = model.identifier,
                    UserId = account.UserId,
                    Balance = account.Balance,
                    Currency = account.Currency,
                    Isactive = account.Isactive,
                    Registered = account.Registered
                };

                return (false, error, response);
            }
            else
            {
                error = new ErrorModel
                {
                    status = "error in balance query",
                    text = "Please validate the account and try again."
                };
                return (true, error, response);
            }
        }

        public (bool, ErrorModel, AccountResponse) insert(AccountDto model)
        {
            AccountResponse response = new AccountResponse();

            (bool validatereq,var error) = validateModel(model);
            if (validatereq) return (true, error , response);

            AccoutDta account = new AccoutDta 
            {
                identifier = Generate(),
                UserId = model.UserId,
                Currency = "NIO",
                Balance = model.InitialBalance,
                Registered = DateTime.Now,
                Isactive = true,
            };

            db.AccountDta.Add(account);
            db.SaveChanges();

            response = new AccountResponse 
            {
                identifier = account.identifier,
                balance = account.Balance
            };
            
            return (false, error, response);

        }

        public (bool, ErrorModel) validateModel(AccountDto model) 
        {
            ErrorModel error = new ErrorModel();

            if (model.InitialBalance < 0)
            {
                error.status = "required field";
                error.text = "the field InitialBalance must be greater than zero";
                return (true, error);
            }
            return (false, error);
        }
        public static string Generate()
        {
            char firstLetter = (char)_random.Next('A', 'Z' + 1);
            int number = _random.Next(10000, 99999); // 5 dígitos
            char lastLetter = (char)_random.Next('A', 'Z' + 1);

            return $"{firstLetter}{number}{lastLetter}";
        }
    }
}
