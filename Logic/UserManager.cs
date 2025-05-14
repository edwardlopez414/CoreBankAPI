using CoreBankAPI.CoreDbContext;
using CoreBankAPI.Data;
using CoreBankAPI.Logic.Interfaces;
using CoreBankAPI.Models;

namespace CoreBankAPI.Logic
{
    public class UserManager : IUserManager
    {
        CoreDb db;
        public UserManager(CoreDb db) 
        {
            this.db = db;
        }
        public (bool,ErrorModel, UserResponse) Insert(UserDto model)
        {
            UserResponse response = new UserResponse();

            (bool iserror, var error) = ValidateModel(model);
            if (iserror) return (true, error, response);

            UserDta user = new UserDta
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                LastName2 = model.LastName2,
                Idtype = model.Idtype,
                Idnumber = model.Idnumber,
                Birthdate = model.Birthdate,
                Gender = model.Gender,
                Income = model.Income,
                Registered = DateTime.Now,
                Isactive = true,
            };

            db.UserDta.Add(user);
            db.SaveChanges();

            var userId = db.UserDta
                .Where(c => c.Idnumber == model.Idnumber && c.Idtype == model.Idtype)
                .Select(a => a.Id)
                .FirstOrDefault();

             response = new UserResponse
            {
                id = userId,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                LastName2 = model.LastName2
            };

            return (false, error, response);
        }

        public (bool, ErrorModel) ValidateModel(UserDto model)
        {
            ErrorModel error = new ErrorModel();
            if (string.IsNullOrEmpty(model.FirstName)) 
            {
                error.status = "required field";
                error.text = "the field FirstName is required";
                return (true, error);
            }
            if (string.IsNullOrEmpty(model.MiddleName)) 
            {
                error.status = "required field";
                error.text = "the field MiddleName is required";
                return (true, error);
            }
            if (string.IsNullOrEmpty(model.LastName))
            {
                error.status = "required field";
                error.text = "the field LastName is required";
                return (true, error);
            }
            if (string.IsNullOrEmpty(model.LastName2))
            {
                error.status = "required field";
                error.text = "the field LastName2 is required";
                return (true, error);
            }
            if (string.IsNullOrEmpty(model.Idtype))
            {
                error.status = "required field";
                error.text = "the field Idtype is required";
                return (true, error);
            }
            if (string.IsNullOrEmpty(model.Idnumber))
            {
                error.status = "required field";
                error.text = "the field Idnumber is required";
                return (true, error);
            }
            if (string.IsNullOrEmpty(model.Gender))
            {
                error.status = "required field";
                error.text = "the field Gender is required";
                return (true, error);
            }
            if (model.Income < 0)
            {
                error.status = "required field";
                error.text = "the field Income must be greater than zero";
                return (true, error);
            }


            return (false, error);
        }
    }
}
