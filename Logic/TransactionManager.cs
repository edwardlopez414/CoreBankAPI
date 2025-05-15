using CoreBankAPI.CoreDbContext;
using CoreBankAPI.Data;
using CoreBankAPI.Logic.Interfaces;
using CoreBankAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreBankAPI.Logic
{
    public class TransactionManager : ITransacionManager
    {
        CoreDb db;
        public TransactionManager(CoreDb db) 
        {
            this.db = db;
        }
        public (bool, ErrorModel, TransactionResponse) deposit(TransactionDto model)
        {
            TransactionResponse response = new TransactionResponse();
            (bool iserror, var error) = validateModel(model);
            if (iserror) return (true, error, response);

            //agregamos la cantidad al monto actual 
            var account = db.AccountDta.Where(a => a.identifier == model.Identifier).Select(b => b).FirstOrDefault();

            account.Balance += model.Amount;

            db.AccountDta.Update(account);

            //registramos la transaccion
            TransactionDta transaction = new TransactionDta 
            {
                AccountId = account.Id,
                Type = "DEPOSIT",
                Amount = model.Amount,
                Description = model.Description,
                Registered = DateTime.Now,
                Isreversed = false,
            };
            db.TransactionDta.Add(transaction);
            db.SaveChanges();

            response = new TransactionResponse 
            {
                Status = "completed",
                Account = account.identifier,
                BalanceAccount = account.Balance,
                TransactionAmount = model.Amount
            };

            return (false, error, response);
            
        }

        public (bool, ErrorModel, HistoryResponse) history(TransactionHistoryDto model)
        {
            HistoryResponse response = new HistoryResponse();
            response.Details = new List<HistoryInfo>();
            (bool iserror, var error) = validateModel(model);
            if (iserror) return (true, error, response);

            //agregamos la cantidad al monto actual 
            var account = db.AccountDta.Where(a => a.identifier == model.Identifier).Select(b => b).FirstOrDefault();

            var History = db.TransactionDta.Where(a => a.AccountId == account.Id).Select(b => b).ToList();

            if (History != null)
            {

                foreach (var item in History)
                {
                    response.Details.Add(new HistoryInfo
                    {
                        Account = model.Identifier,
                        Type = item.Type,
                        TransactionAmount = item.Amount
                    });
                }
                response.AccountBalance = account.Balance;

                return (false, error, response);
            }
            else 
            {
                //el cliente no tiene transacciones
                return (false, error, response);
            }
        }
    

        public (bool, ErrorModel, TransactionResponse) withdrawals(TransactionDto model)
        {
            TransactionResponse response = new TransactionResponse();
            (bool iserror, var error) = validateModel(model);
            if (iserror) return (true, error, response);

            //agregamos la cantidad al monto actual 
            var account = db.AccountDta.Where(a => a.identifier == model.Identifier).Select(b => b).FirstOrDefault();

            account.Balance -= model.Amount;

            db.AccountDta.Update(account);

            //registramos la transaccion
            TransactionDta transaction = new TransactionDta
            {
                AccountId = account.Id,
                Type = "WITHDRAWALS",
                Amount = model.Amount,
                Description = model.Description,
                Registered = DateTime.Now,
                Isreversed = false,
            };
            db.TransactionDta.Add(transaction);
            db.SaveChanges();

            response = new TransactionResponse
            {
                Status = "completed",
                Account = account.identifier,
                BalanceAccount = account.Balance,
                TransactionAmount = model.Amount
            };

            return (false, error, response);
        }
        public (bool, ErrorModel) validateModel(TransactionDto model) 
        {
            ErrorModel error = new ErrorModel();

            if (string.IsNullOrEmpty(model.Identifier))
            {
                error.status = "required field";
                error.text = "the field identifier is required";
                return (true, error);
            }
            if(model.Amount < 0)
            {
                error.status = "required field";
                error.text = "the field amount must be greater than zero";
                return (true, error);
            }
            return (false, error);
        }
        public (bool, ErrorModel) validateModel(TransactionHistoryDto model)
        {
            ErrorModel error = new ErrorModel();

            if (string.IsNullOrEmpty(model.Identifier))
            {
                error.status = "required field";
                error.text = "the field identifier is required";
                return (true, error);
            }
            return (false, error);
        }
    }
}
