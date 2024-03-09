using AutoMapper;
using Learn.Net.Helper;
using Learn.Net.Modal;
using Learn.Net.Repository;
using Learn.Net.Repository.Models;
using Learn.Net.Services;
using Microsoft.EntityFrameworkCore;

namespace Learn.Net.Container
{
    public class CustomerService : ICustomerService
    {
        private readonly Repository.LearnContext _context;
        //inject IMapper
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;
        public CustomerService(Repository.LearnContext context,IMapper mapper1, ILogger<CustomerService> logger)
        {
            _context = context;
            _mapper = mapper1;
            _logger = logger;
        }
        //Async method (we use Task and await)
        public async Task<List<CustomerModal>> GetAll()
        {
            List<CustomerModal> customerModals = new List<CustomerModal>();
            
           var data = await _context.TblCustomers.ToListAsync();
            if (data!=null)
            {
                customerModals = _mapper.Map<List<TblCustomer>,List<CustomerModal>>(data);
            }
            return customerModals;
        }

        public async Task<CustomerModal> GetByCode(string code)
        {
            CustomerModal customerModal = new CustomerModal();

            var data = await _context.TblCustomers.FindAsync(code);
            if (data != null)
            {
                customerModal = _mapper.Map<TblCustomer, CustomerModal>(data);
            }
            return customerModal;
        }

        public async Task<APIResponse> Add(CustomerModal customerModal)
        {
            APIResponse response = new APIResponse();
            try
            {
                _logger.LogInformation("Add method called");
                TblCustomer tblCustomer = _mapper.Map<CustomerModal, TblCustomer>(customerModal);
                await _context.TblCustomers.AddAsync(tblCustomer);
                await _context.SaveChangesAsync();
                response.ResponseCode = 201; //Created
                response.Result = customerModal.Code;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 400; //Bad Request
                response.ErrorMessage = ex.Message;
                _logger.LogError(ex.Message); //capture the error
            }
            return response;
        }

        public async Task<APIResponse> Update(CustomerModal customerModal, string code)
        {
            APIResponse response = new APIResponse();
            try
            {
                //check if we have it in the database
                var data = await _context.TblCustomers.FindAsync(code);
                //check if not equal to null
                if (data != null)
                {
                    data.Name = customerModal.Name;
                    data.Email = customerModal.Email;
                    data.Phone = customerModal.Phone;
                    data.Creditlimit = customerModal.Creditlimit;
                    data.IsActive = customerModal.IsActive;
                    data.Taxcode = customerModal.Taxcode;
                    _context.TblCustomers.Update(data); //maybe??
                    await _context.SaveChangesAsync();
                    response.ResponseCode = 200; //Created
                    response.Result = code;
                }
                else
                {
                    response.ResponseCode = 404; //Not Found
                    response.ErrorMessage = "Not found!";
                }

            }
            catch (Exception ex)
            {
                response.ResponseCode = 400; //Bad Request
                response.ErrorMessage = ex.Message;
            }
            return response;
  
        }

        public async Task<APIResponse> Delete(string code)
        {
            APIResponse response = new APIResponse();
            try
            {
                //check if we have it in the database
                var data = await _context.TblCustomers.FindAsync(code);
                //check if not equal to null
                if (data != null)
                {
                    _context.TblCustomers.Remove(data);
                    await _context.SaveChangesAsync();
                    response.ResponseCode = 200; //Created
                    response.Result = code;
                }
                else
                {
                    response.ResponseCode = 404; //Not Found
                    response.ErrorMessage="Not found!";
                }
                
            }
            catch (Exception ex)
            {
                response.ResponseCode = 400; //Bad Request
                response.ErrorMessage = ex.Message;
            }
            return response;
        }
    }
}
